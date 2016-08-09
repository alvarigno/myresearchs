using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsearEmails_WebAPI.Models
{
    public class DatosEmail
    {
        public int UidEmail { get; set; }
        public string Contenido { get; set; }
        public string TipoCampo { get; set; }
        public string Nombre { get; set; }
        public bool ReglaAplicada { get; set; }

        public enum EstadoEmail : int
        {
            Parseado = 1,
            NoParseado = 2,
            Check = 3,
            EnCola = 4,
            EnProceso = 5
        }

        public enum OpcionRegla : int
        {
            TextMatchAfter = 1,
            TextMatchBefore = 2,
            AfterXLine = 3,
            AfterXCharacters = 4,
            AfterXWords = 5,
            SearchAndReplace = 6,
            BeforeXLine = 7,
            UntilTheEndOfTheLine = 8
        };

        
        public void AplicarReglas(AccionDelimitador regla, ref string cuerpoEmail, ref string[] bodyEmail, ref int position1, ref int position2, ref string nomRegla, ref int vienesDe,
                                ref bool reglaAplicada, ref int buscarEnLinea, ref int iFilaInicial, ref int IdRegla)
        {
            int start = 0;
            const int SIN_COINCIDENCIA = -1;
            int iFila = 0;
            //Array con cada una de las palabras en una línea
            string[] strPalabrasEnLinea = new string[] { };
            int ctaPalabras = 0;

            int buscaLineaTope = 0;


            switch (regla.Accion)
            {
                //dentro del if
                case (int)OpcionRegla.TextMatchAfter:
                    //Busco entre las filas, donde está texto delimitador
                    foreach (var fila in bodyEmail)
                    {
                        position1 = fila.IndexOf(regla.Delimitador, start, StringComparison.OrdinalIgnoreCase);
                        if (position1 != SIN_COINCIDENCIA)
                        {
                            buscarEnLinea = iFila;
                            break;
                        }
                        iFila++;
                    }

                    // Delimitador no existe
                    if (position1 == SIN_COINCIDENCIA)
                    {
                        reglaAplicada = false;
                    }
                    else
                    {
                        //Si después de la palabra buscada hay espacio sumar 1
                        //en caso contrario, no sumar
                        position1 += regla.Delimitador.Length + 1;
                        if (bodyEmail[buscarEnLinea].Substring(position1, 1) != " ")
                            position1--;
                        
                        reglaAplicada = true;
                    }
                    break;

                case (int)OpcionRegla.TextMatchBefore:
                    if (position1 != SIN_COINCIDENCIA)
                    {
                        if (regla.Delimitador == "")
                        {
                            position2 = bodyEmail[buscarEnLinea].Length;
                        }
                        else
                        {
                            if (buscarEnLinea != 0)
                            {
                                position2 = bodyEmail[buscarEnLinea].IndexOf(regla.Delimitador, start, StringComparison.OrdinalIgnoreCase);
                            }
                            if (position2 == SIN_COINCIDENCIA) { 
                                //Averiguo posición y fila del delimitador
                                for (int x = buscarEnLinea; x < bodyEmail.Length; x++)
                                {
                                    position2 = bodyEmail[x].IndexOf(regla.Delimitador, start, StringComparison.OrdinalIgnoreCase);
                                    if (position2 != SIN_COINCIDENCIA)
                                    {
                                        buscaLineaTope = x;
                                        break;
                                    }
                                    //iFila++;
                                }
                            }
                            if (buscarEnLinea != buscaLineaTope)
                            {
                                //Existe una línea de diferencia
                                if (buscaLineaTope - buscarEnLinea == 1)
                                {
                                    //Entonces el texto está entre position1 y el largo de la línea
                                    position2 = bodyEmail[buscarEnLinea].Length;
                                }
                                else
                                {
                                    //Caso contrario, corto desde inicio hasta la línea tope
                                    cuerpoEmail = "";
                                    for (int i = buscarEnLinea; i < buscaLineaTope; i++)
                                    {
                                        cuerpoEmail += bodyEmail[i] + "\n";
                                    }
                                    bodyEmail = Regex.Split(cuerpoEmail, "\n");
                                }
                            }
                        }
                    }
                    break;

                case (int)OpcionRegla.AfterXLine:
                    cuerpoEmail = "";
                    buscarEnLinea = int.Parse(regla.Delimitador);
                    iFilaInicial = buscarEnLinea;
                    //Cortamos el mensaje, desde la línea seleccionada hasta el final
                    for (int i = buscarEnLinea; i < bodyEmail.Length; i++)
                    {
                        cuerpoEmail += bodyEmail[i] + "\n";
                    }
                    bodyEmail = Regex.Split(cuerpoEmail, "\n");
                    vienesDe = (int)OpcionRegla.AfterXLine;
                    buscarEnLinea = 0;
                    position1 = 0;
                    position2 = -1;
                    break;

                case (int)OpcionRegla.AfterXCharacters:
                    position1 = int.Parse(regla.Delimitador);
                    cuerpoEmail = bodyEmail[buscarEnLinea].Substring(position1);
                    bodyEmail = Regex.Split(cuerpoEmail, "\n");
                    position1 = 0;
                    break;

                case (int)OpcionRegla.AfterXWords:
                    cuerpoEmail = "";

                    if ((buscarEnLinea == SIN_COINCIDENCIA) || (buscarEnLinea == 0))
                    {
                        //Cuento las palabras por cada línea del texto
                        for (int i = 0; i < bodyEmail.Length; i++)
                        {
                            if (bodyEmail[i].Trim() != "")
                            {
                                strPalabrasEnLinea = bodyEmail[i].Split(' ');
                                buscarEnLinea = i;
                                ctaPalabras += strPalabrasEnLinea.Length;

                                //Verifico si sobrepasé la cantidad de palabras en la línea en revisión
                                if (ctaPalabras > int.Parse(regla.Delimitador))
                                {
                                    string strNuevaLinea = "";
                                    if (buscarEnLinea == 0)
                                    {
                                        ctaPalabras = int.Parse(regla.Delimitador);
                                        for (int z = ctaPalabras; z < strPalabrasEnLinea.Length; z++)
                                        {
                                            strNuevaLinea += strPalabrasEnLinea[z] + ' ';
                                        }
                                        strNuevaLinea = strNuevaLinea.Substring(start, strNuevaLinea.Length - 1);
                                        bodyEmail[i] = strNuevaLinea;
                                        strPalabrasEnLinea = bodyEmail[i].Split(' ');
                                        break;
                                    }
                                    else
                                    {
                                        ctaPalabras -= strPalabrasEnLinea.Length;
                                        while (int.Parse(regla.Delimitador) != ctaPalabras)
                                        {
                                            string strNuevoTexto = "";
                                            int ParteDesde = int.Parse(regla.Delimitador) - ctaPalabras;
                                            ctaPalabras++;
                                            for (int z = ParteDesde; z < strPalabrasEnLinea.Length; z++)
                                            {
                                                strNuevoTexto += strPalabrasEnLinea[z] + ' ';
                                            }
                                            if (strNuevoTexto.Length > 0)
                                                strNuevoTexto = strNuevoTexto.Substring(start, strNuevoTexto.Length - 1);
                                            bodyEmail[i] = strNuevoTexto;
                                            strPalabrasEnLinea = bodyEmail[i].Split(' ');
                                            if (int.Parse(regla.Delimitador) == ctaPalabras) break;
                                        }
                                    }
                                }
                                else if (ctaPalabras < int.Parse(regla.Delimitador))
                                {
                                    continue;
                                }
                                else if (ctaPalabras == int.Parse(regla.Delimitador))
                                {
                                    buscarEnLinea++;
                                    while (bodyEmail[buscarEnLinea].Trim().Length == 0)
                                        buscarEnLinea++;
                                }
                                if (int.Parse(regla.Delimitador) == ctaPalabras) break;
                            }
                            else buscarEnLinea = i;
                        }

                        //Armo de nuevo el cuerpo del email, después de la cantidad de palabras seleccionadas
                        foreach (var body in bodyEmail)
                        {
                            cuerpoEmail += body + "\n";
                        }
                    }
                    vienesDe = (int)OpcionRegla.AfterXWords;

                    break;

                case (int)OpcionRegla.SearchAndReplace:
                    break;

                case (int)OpcionRegla.BeforeXLine:
                    //Cortamos el mensaje, hasta la línea seleccionada
                    if (vienesDe == (int) OpcionRegla.AfterXLine)
                    {
                        cuerpoEmail = "";
                        for (int i = buscarEnLinea; i < int.Parse(regla.Delimitador); i++)
                        {
                            cuerpoEmail += bodyEmail[i] + "\n";
                        }
                        bodyEmail = Regex.Split(cuerpoEmail, "\n");
                    }
                    else
                    {
                        cuerpoEmail = "";
                        for (int i = 0; i < int.Parse(regla.Delimitador)-1; i++)
                        {
                            cuerpoEmail += bodyEmail[i] + "\n";
                        }
                        bodyEmail = Regex.Split(cuerpoEmail, "\n");
                    }
                    vienesDe = (int)OpcionRegla.BeforeXLine;
                    break;

                case (int)OpcionRegla.UntilTheEndOfTheLine:
                    position2 = bodyEmail[buscarEnLinea].Length;
                    break;

            }
            nomRegla = regla.Nombre;
            IdRegla = regla.IdRegla;
        }

        public void CambioEstadoEmail(int uidEmail, int uidEstado)
        {
            ConexionSql con = new ConexionSql();

         
            //cambio el estado del email en tbl_mp_email
            using (SqlConnection sqlCon = new SqlConnection(con.getStrCon()))
            {
                SqlCommand cmdUpdateEstadoEmail = new SqlCommand("SP_UpdateEstadoEmail", sqlCon)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                // Añado el parametro de entrada @uidEmail establecido en SQL Server
                SqlParameter p1 = new SqlParameter("@uidEmail", uidEmail)
                {
                    Direction = System.Data.ParameterDirection.Input
                };
                cmdUpdateEstadoEmail.Parameters.Add(p1);

                //Añado el segundo parámetro @uidEstado
                SqlParameter p2 = new SqlParameter("@uidEstado", uidEstado)
                {
                    Direction = System.Data.ParameterDirection.Input
                };
                cmdUpdateEstadoEmail.Parameters.Add(p2);
                cmdUpdateEstadoEmail.Connection = sqlCon;

                sqlCon.Open();

                cmdUpdateEstadoEmail.ExecuteNonQuery();

                sqlCon.Close();
                sqlCon.Dispose();
                SqlConnection.ClearPool(sqlCon);
            }
        }

        public void CambioTipoEmail(int uidEmail, int uidTipo)
        {
            ConexionSql con = new ConexionSql();

            using (SqlConnection sqlCon = new SqlConnection(con.getStrCon()))
            {
                //cambio el tipo del email en tbl_mp_email
                SqlCommand cmdUpdateTipoEmail = new SqlCommand("SP_UpdateTipoEmail", sqlCon)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                // Añado el parametro de entrada @uidEmail establecido en SQL Server
                SqlParameter p1 = new SqlParameter("@uidEmail", uidEmail)
                {
                    Direction = System.Data.ParameterDirection.Input
                };
                cmdUpdateTipoEmail.Parameters.Add(p1);

                //Añado el segundo parámetro @uidTipo
                SqlParameter p2 = new SqlParameter("@uidTipo", uidTipo)
                {
                    Direction = System.Data.ParameterDirection.Input
                };
                cmdUpdateTipoEmail.Parameters.Add(p2);
                cmdUpdateTipoEmail.Connection = sqlCon;

                sqlCon.Open();

                cmdUpdateTipoEmail.ExecuteNonQuery();
                
                sqlCon.Close();
                sqlCon.Dispose();
                SqlConnection.ClearPool(sqlCon);
            }
        }
        public List<DatosEmail> Parsear(List<AccionDelimitador> lpasos, string cuerpoEmail, int uidEmail)
        {

            const int SIN_COINCIDENCIA = -1;

            string nomRegla = "";
            int IdRegla = 0;

            // posiciones de inicio y término para realizar la extracción 
            int position1 = 0, position2 = 0;

            //Me indica desde qué acción previa viene. Estas están en enum OpcionRegla
            int vienesDe = 0;

            // En qué línea realizar la búsqueda
            int buscarEnLinea = 0;


            bool reglaAplicada = true;
            int start = 0;

            //Aquí se guardarán los valores que se almacenarán en la tabla tbl_mp_datosEmail
            List<DatosEmail> lDatosEmails = new List<DatosEmail>();

            //Mantengo la fila de la regla AfterXLine
            int iFilaInicial = 0;

            //Un array con el contenido del email
           string[] bodyEmail = Regex.Split(cuerpoEmail, "\n");

            string strMensaje = cuerpoEmail;

            //Cuenta las reglas por tipo
            int NroDeReglas = 0;

            
            //Cambio el estado de email en tbl_mp_email, mientras se intenta parsear
            CambioEstadoEmail(uidEmail, (int)EstadoEmail.EnProceso);




            foreach (var regla in lpasos)
            {

                if ((IdRegla == regla.IdRegla) || (IdRegla == 0))
                {
                    AplicarReglas(regla, ref cuerpoEmail, ref bodyEmail, ref position1, ref position2, ref nomRegla, ref vienesDe,
                        ref reglaAplicada, ref buscarEnLinea, ref iFilaInicial, ref IdRegla);
                }
                else
                {
                    // dentro del foreach

                    if (position1 == SIN_COINCIDENCIA)
                    {
                        if (position2 == SIN_COINCIDENCIA) { 
                            // Insertar en objeto lDatosEmail
                            lDatosEmails.Add(new DatosEmail()
                            {
                                UidEmail = uidEmail,
                                Nombre = nomRegla,
                                Contenido = "-",
                                TipoCampo = "-",
                                ReglaAplicada = false
                            });

                            vienesDe = 0;
                            buscarEnLinea = 0;
                            position1 = 0;
                            position2 = -1;
                        }
                    }
                    else
                    {
                        string strContenido = "";

                        if ((position2 == 0) || (position2 == SIN_COINCIDENCIA))
                            position2 = bodyEmail[buscarEnLinea].Length;

                        strContenido = bodyEmail[buscarEnLinea].Substring(position1, position2 - position1).Trim();
                        reglaAplicada = strContenido.Trim() != "";


                        // Insertar en objeto lDatosEmail
                        lDatosEmails.Add(new DatosEmail()
                        {
                            UidEmail = uidEmail,
                            Nombre = nomRegla,
                            Contenido = strContenido,
                            TipoCampo = "-",
                            ReglaAplicada = reglaAplicada
                        });
                        vienesDe = 0;
                        buscarEnLinea = 0;
                        position1 = 0;
                        position2 = -1;
                    }

                    //Recupero el texto del mensaje original, al cambiar de regla
                    cuerpoEmail = strMensaje;
                    bodyEmail = Regex.Split(cuerpoEmail, "\n");
                    AplicarReglas(regla, ref cuerpoEmail, ref bodyEmail, ref position1, ref position2, ref nomRegla, ref vienesDe, ref reglaAplicada, ref buscarEnLinea,
                         ref iFilaInicial, ref IdRegla);
                }
            } // fin del  foreach

            if (position1 == SIN_COINCIDENCIA)
            {
                if (position2 == SIN_COINCIDENCIA)
                {
                    // Insertar en objeto lDatosEmail
                    lDatosEmails.Add(new DatosEmail()
                    {
                        UidEmail = uidEmail,
                        Nombre = nomRegla,
                        Contenido = "-",
                        TipoCampo = "-",
                        ReglaAplicada = false
                    });
                }

            }
            else
            {
                string strContenido = "";

                try
                {
                    if ((position2 == 0) || (position2 == SIN_COINCIDENCIA))
                        position2 = bodyEmail[buscarEnLinea].Length;

                    strContenido = bodyEmail[buscarEnLinea].Substring(position1, position2 - position1).Trim();
                }
                catch (IndexOutOfRangeException)
                {

                    strContenido = "";
                }

               

                if (strContenido.Trim() == "")
                {
                    reglaAplicada = false;
                }
                else
                {
                    reglaAplicada = true;

                    if (strContenido.ToLower() == strContenido.ToUpper())
                    {
                        try
                        {
                            reglaAplicada = int.Parse(strContenido) != 0;
                        }
                        catch (FormatException)
                        {
                            reglaAplicada = strContenido.IndexOf("$", start, StringComparison.Ordinal) != SIN_COINCIDENCIA;
                        }

                    }
                }

                // Insertar en objeto lDatosEmail
                lDatosEmails.Add(new DatosEmail()
                {
                    UidEmail = uidEmail,
                    Nombre = nomRegla,
                    Contenido = strContenido,
                    TipoCampo = "-",
                    ReglaAplicada = reglaAplicada
                });

            }

            NroDeReglas = lDatosEmails.Count();
            int ctaReglasAplicadas = lDatosEmails.Count(item => item.ReglaAplicada == true);
            if (ctaReglasAplicadas == 0)
                CambioEstadoEmail(uidEmail, (int) EstadoEmail.NoParseado);
            else if (ctaReglasAplicadas != NroDeReglas)
                CambioEstadoEmail(uidEmail, (int)EstadoEmail.Check);
            else
                CambioEstadoEmail(uidEmail, (int)EstadoEmail.Parseado);
            return lDatosEmails;
        }
    }
}
