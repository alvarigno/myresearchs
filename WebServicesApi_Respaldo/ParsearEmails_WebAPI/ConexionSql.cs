﻿using System.Configuration;
using System.Data.SqlClient;

namespace ParsearEmails_WebAPI
{
    public class ConexionSql
    {
        private SqlConnection _con;


        public SqlConnection Conexion()
        {
            string strCadenaConexion = ConfigurationManager.ConnectionStrings["bdMailParser"].ToString();

            _con = new SqlConnection(strCadenaConexion);
            return _con;
        }


        public void Abrir()
        {
            _con.Open();

        }

        public void Cerrar()
        {
            _con.Close();
        }

        public string getStrCon()
        {
            return ConfigurationManager.ConnectionStrings["bdMailParser"].ToString();
        }

    }
}
