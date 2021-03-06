﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ParsearEmails_WebAPI.Models
{
    public class Condicion
    {
        public List<MailsSinParsearModel> getTipo(List<MailsSinParsearModel> emails)
        {
            List<TiposTotalCondicionesModel> tipos = null;
            List<CondicionesFuenteModel> condiciones = null;

            //if (tipos.Length == 1)
            //    return tipos[0].uid_tipo;
            int uidFuenteActual = 0;

            foreach (var email in emails)
            {
                if (tipos == null || email.uid_fuente != uidFuenteActual)
                {
                    uidFuenteActual = email.uid_fuente;
                    tipos = getTiposTotalCond(uidFuenteActual);
                    condiciones = getCondicionesFuenteModel(uidFuenteActual);
                }

                foreach (var tipo in tipos)
                {
                    var condicionesTipo = condiciones.Where(w => w.uid_tipo == tipo.uid_tipo).ToArray();
                    int totCondCumplidas = condicionesTipo.Count(condicion => cumpleCondicion(condicion, email));

                    if (totCondCumplidas != tipo.totCondiciones) continue;
                    email.uid_tipo = tipo.uid_tipo;

                    DatosEmail correo = new DatosEmail();
                    correo.CambioTipoEmail(email.uid_email, email.uid_tipo);
                    
                }
            }


            // return emails;
            return emails.OrderBy(o => o.uid_tipo).ToList<MailsSinParsearModel>();
        }

        private bool cumpleCondicion(CondicionesFuenteModel condicion, MailsSinParsearModel email)
        {
            bool cumple = false;

            switch (condicion.uid_opCondicion)
            {
                // remitente
                case 1:
                    if (email.remitente.ToLower().IndexOf(condicion.data.ToLower()) > -1)
                        cumple = true;
                    break;

                // contiene
                case 2:
                    if (getSeccion(condicion.uid_seccion, email).IndexOf(condicion.data.ToLower()) > -1)
                        cumple = true;
                    break;

                // no contiene
                case 3:
                    if (getSeccion(condicion.uid_seccion, email).IndexOf(condicion.data.ToLower()) == -1)
                        cumple = true;
                    break;
                default:
                    break;
            }

            return cumple;
        }

        private string getSeccion(int? uidSeccion, MailsSinParsearModel email)
        {
            string seccion = "";

            switch (uidSeccion)
            {
                case 1:
                    seccion = email.asunto.ToLower();
                    break;
                case 2:
                    seccion = email.email.ToLower();
                    break;
                case 3:
                    seccion = email.destinatarios.ToLower();
                    break;
                case 4:
                    seccion = email.remitente.ToLower();
                    break;
                case 5:
                    seccion = email.cabecera.ToLower();
                    break;
                default:
                    break;
            }

            return seccion;
        }

        private List<TiposTotalCondicionesModel> getTiposTotalCond(int uidFuente)
        {
            var con = new ConexionSql();
            List<TiposTotalCondicionesModel> tipos = new List<TiposTotalCondicionesModel>();

            using (SqlConnection sqlConn = new SqlConnection(con.getStrCon()))
            {
                //Creamos el comando de tipo StoredProcedure
                SqlCommand cmd = new SqlCommand("SP_GetTiposTotalCondiciones", sqlConn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("uidFuente", uidFuente);

                cmd.Connection = sqlConn;

                //Ejecutamos el comando
                sqlConn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tipos.Add(new TiposTotalCondicionesModel
                    {
                        uid_tipo = int.Parse(reader["uid_tipo"].ToString()),
                        totCondiciones = int.Parse(reader["totCondiciones"].ToString())
                    });
                }

                sqlConn.Close();
            }

            return tipos;
        }

        private List<CondicionesFuenteModel> getCondicionesFuenteModel(int uidFuente)
        {
            List<CondicionesFuenteModel> tipos = new List<CondicionesFuenteModel>();
            var con = new ConexionSql();

            using (SqlConnection sqlConn = new SqlConnection(con.getStrCon()))
            {
                //Creamos el comando de tipo StoredProcedure
                SqlCommand cmd = new SqlCommand("SP_GetCondicionesFuente", sqlConn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("uidFuente", uidFuente);

                cmd.Connection = sqlConn;

                //Ejecutamos el comando
                sqlConn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tipos.Add(new CondicionesFuenteModel
                    {
                        uid_opCondicion = int.Parse(reader["uid_opCondicion"].ToString()),
                        uid_seccion = int.Parse(reader["uid_seccion"].ToString()),
                        uid_tipo = int.Parse(reader["uid_tipo"].ToString()),
                        condicion = reader["condicion"].ToString(),
                        data = reader["data"].ToString(),
                        opcion = reader["opcion"].ToString(),
                        seccion = reader["seccion"].ToString()
                    });
                }

                sqlConn.Close();
            }

            return tipos;
        }
    }
}