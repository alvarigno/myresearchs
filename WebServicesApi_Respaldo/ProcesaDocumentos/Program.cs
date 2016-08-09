﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Security.Permissions;
using MimeKit;
using System.Text.RegularExpressions;

namespace ProcesaDocumentos
{
    public class Program
    {
        private static SqlConnection _cnx;

        string pruebadato;
        int count = 0;
        string mailbox;
        string DatoContenidoMailPlain;
        string DatoContenidoMailHtml;
        int IdEmailParse;
        string DatoFechaMail;
        string format = "yyyy-MM-dd HH:MM:ss";
        DateTime DatoFechaFromate;
        string DatoAsuntoMail;
        string DatoHeaderMail = "";
        string[] DatoDestinatariosMail = new string[100];
        string DataDestinatariosString;
        string[] DatoCcMail = new string[100];
        string DataCcString;
        string DatoRemitenteMail;
        Documentos listadocumentos = new Documentos();
        List<Documentos> listOfDocumentos = new List<Documentos>();
        List<Sitios> listOfSitios = new List<Sitios>();
        List<Fuentes> listOfFuentes = new List<Fuentes>();
        List<Tipos> listOfTipos = new List<Tipos>();
        List<Automotoras> listOfAutomotoras = new List<Automotoras>();
        List<Emails> listOfEmails = new List<Emails>();

        int DataUidFuente;
        int DataUidTipo;
        int DataUidEstado;
        int DataUidAutomotora;

        /// <summary>
        /// Parse documento divido en las partes del e-mail
        /// </summary>
        /// <returns></returns>

        public void ParseEmail(string emlFile)
        {

            var mimeMessage = MimeMessage.Load(emlFile);

            var header = mimeMessage.Headers;

            var mailto = mimeMessage.To;
            var mailfrom = mimeMessage.From;
            var mailcc = mimeMessage.Cc;
            var mailsubject = mimeMessage.Subject;
            DateTime maildate = mimeMessage.Date.UtcDateTime;
            var mailplainbody = mimeMessage.TextBody;
            var mailhtmlbody = mimeMessage.HtmlBody;

            for (int i = 0; i < header.Count; i++)
            {

                string datoslocales;
                datoslocales = header[i].ToString() + "\r\n";
                DatoHeaderMail = DatoHeaderMail + datoslocales;

            }


            // Parse Mail From, Sender
            DatoRemitenteMail = mailfrom.ToString();
            //Console.WriteLine("From: {0}", oMail.From.ToString());

            //Date
            DatoFechaFromate = maildate;
            //Console.WriteLine("Date: {0}"+ DatoFechaFromate);

            // Parse Mail To, Recipient
            DataDestinatariosString = mailto.ToString();
            //Console.WriteLine("Destinatarios To: " + DataDestinatariosString);

            // Parse Mail CC

            DataCcString = mailcc.ToString();
            //Console.WriteLine("Destinatarios Cc: " + DataCcString); 

            // Parse Mail Subject
            DatoAsuntoMail = mailsubject;
            //Console.WriteLine("Subject: "+ personalprueba);

            // Parse Mail Text/Plain body
            DatoContenidoMailPlain = mailplainbody;
            //Console.WriteLine("TextBody: {0}", oMail.TextBody);

            // Parse Mail Html Body
            DatoContenidoMailHtml = mailhtmlbody;
            //Console.WriteLine("HtmlBody: {0}", oMail.HtmlBody);

        }

        /// <summary>
        /// Método que inicia el proceso.
        /// </summary>
        /// <param name="documentoRecibido"></param>
        /// <param name="PosicionDocumento"></param>
        /// <returns></returns>

        public string Comienzaproceso(string documentoRecibido, string PosicionDocumento)
        {
            
            string hola = LoadDocmumentos(documentoRecibido, PosicionDocumento);
            return hola;
        }

        /// <summary>
        /// Método que hace el insert de los datos de parseo dentro de la base de datos.
        /// </summary>
        /// <returns></returns>

        public bool insertondatabase()
        {
            int a = 0;
            try
            {
                //DatoContenidoMailHtml = DatoContenidoMailHtml.Replace("'", "\"").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                if (string.IsNullOrEmpty(DatoContenidoMailHtml) && DatoContenidoMailPlain.Length > 0)
                {
                    DatoContenidoMailHtml = DatoContenidoMailPlain.Replace("'", "\"");
                }

                if (DatoContenidoMailHtml.Length > 7000 && DatoContenidoMailPlain.Length > 0)
                {

                    DatoContenidoMailHtml = DatoContenidoMailPlain.Replace("'", "\"");
                }

                if (DatoContenidoMailHtml.Length < 7000 && string.IsNullOrEmpty(DatoContenidoMailPlain))
                {

                    //DatoContenidoMailPlain = DatoContenidoMailHtml; 
                    DatoContenidoMailPlain = GetPlainTextFromHtml(DatoContenidoMailHtml);
                }

                //Data before to Inster//

                DatoContenidoMailPlain = DatoContenidoMailPlain.Replace("'", "\"");
                DatoContenidoMailPlain = ChangeEncodingFormat(DatoContenidoMailPlain);

                DatoAsuntoMail = ChangeEncodingFormat(DatoAsuntoMail);

                DatoHeaderMail = DatoHeaderMail.Replace("'", "''");
                DatoHeaderMail = ChangeEncodingFormat(DatoHeaderMail);

                DataDestinatariosString = ChangeEncodingFormat(DataDestinatariosString);


                DatoRemitenteMail = ChangeEncodingFormat(DatoRemitenteMail);

                DataCcString = ChangeEncodingFormat(DataCcString);

                DatoContenidoMailHtml = ChangeEncodingFormat(DatoContenidoMailHtml);

                //Console.WriteLine("INSERT INTO[dbo].[tbl_mp_email] (uid_tipo,uid_estado,uid_automotora,email,fecha_recibido,asunto,cabecera,destinatarios,remitente, cc, email_html) VALUES(" + DataUidTipo + "," + 4 + "," + DataUidAutomotora + ",'" + DatoContenidoMailPlain.Replace("'", "\"") + "','" + DatoFechaFromate.ToString(format) + "','" + DatoAsuntoMail + "','" + DatoHeaderMail.Replace("'", "''") + "','" + DataDestinatariosString + "','" + DatoRemitenteMail + "', '" + DataCcString + "', '" + DatoContenidoMailHtml + "');");

                //SqlCommand mycommand = new SqlCommand();

                //mycommand.CommandType = System.Data.CommandType.Text;
                //mycommand.Connection = myConn.Conexion();
                //mycommand.CommandText = "INSERT INTO[dbo].[tbl_mp_email] (uid_tipo,uid_estado,uid_automotora,email,fecha_recibido,asunto,cabecera,destinatarios,remitente, cc, email_html) VALUES(" + DataUidTipo + "," + 4 + "," + DataUidAutomotora + ",'" + DatoContenidoMailPlain.Replace("'", "\"") + "','" + DatoFechaFromate.ToString(format) + "','" + DatoAsuntoMail + "','" + DatoHeaderMail.Replace("'", "''") + "','" + DataDestinatariosString + "','" + DatoRemitenteMail + "', '" + DataCcString + "', '" + DatoContenidoMailHtml + "')";

                //int a = mycommand.ExecuteNonQuery();
                //mycommand.Connection.Close();


                myConnection con = new myConnection();
                _cnx = con.Conexion();


                string queryString = "INSERT INTO [dbo].[tbl_mp_email] (uid_tipo,uid_estado,uid_automotora,email,fecha_recibido,asunto,cabecera,destinatarios,remitente, cc, email_html) VALUES(" + DataUidTipo + "," + 4 + "," + DataUidAutomotora + ",'" + DatoContenidoMailPlain.Replace("'", "\"") + "',CONVERT(DATETIME, '" + DatoFechaFromate.ToString(format) + "', 120),'" + DatoAsuntoMail + "','" + DatoHeaderMail.Replace("'", "''") + "','" + DataDestinatariosString + "','" + DatoRemitenteMail + "', '" + DataCcString + "', '" + DatoContenidoMailHtml + "')";
                
                using (con.Conexion())
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlCommand command = new SqlCommand(queryString, con.Conexion());
                    con.Abrir();
                    a = command.ExecuteNonQuery();
                    con.Cerrar();

                }


                //int a = 0;
                if (a == 0)
                {
                    //Not updated.
                    return false;
                }
                else
                {
                    //Updated.
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Not updated
                return false;
            }


        }

        /// <summary>
        /// Método que procesa las validaciones de las condiciones que debe tener el documento email para ser procesado, que cuente con 
        /// - si existe fuente, si existe tipo, inserta en base de datos.
        /// - si existe fuente, y no existe tipo(inserta el tipo default), inserta en base de datos.
        /// - Si no existe fuente (inserta fuente), no existe tipo(inserta tipo), inserta en base de datos.
        /// </summary>
        /// <param name="documentoRecibido"></param>
        /// <param name="PosicionDocumento"></param>
        /// <returns></returns>

        public string LoadDocmumentos(string documentoRecibido, string PosicionDocumento)
        {
            string hola;
            myConnection con = new myConnection();
            
            string queryString = "SELECT * FROM [dbo].[documentos] where fnombre = '" + documentoRecibido + "' and estado = 0";

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        var documentoemail = new Documentos();
                        documentoemail.id_num = int.Parse(reader["id_num"].ToString());
                        documentoemail.estado = int.Parse(reader["estado"].ToString());
                        documentoemail.fnombre = reader["fnombre"].ToString();
                        documentoemail.sitio = int.Parse(reader["sitio"].ToString());

                        listOfDocumentos.Add(documentoemail);

                        ParseEmail(PosicionDocumento + documentoemail.fnombre);

                        pruebadato = PosicionDocumento + documentoemail.fnombre+"|"+ DataDestinatariosString;
                        
                        if (new FileInfo(PosicionDocumento + documentoemail.fnombre).Length > 0)
                        {

                           if (VerificaSitio(documentoemail.sitio) && VerificaAutomotora(DataDestinatariosString))
                            {

                                if (VerificaFuente(DatoRemitenteMail, documentoemail.sitio))
                                {

                                    if (VerificaTipo(DataUidFuente, "default"))
                                    {

                                        if (insertondatabase())
                                        {
                                            IndiceMasAlto();
                                            UpDateEstadoDocumento(documentoemail.fnombre, documentoemail.sitio);

                                        }
                                        else
                                        {
                                            pruebadato = "Documento no logro ser procesado";

                                        }

                                    }
                                    else
                                    {

                                        insertTipo(DataUidFuente);
                                        VerificaTipo(DataUidFuente, "default");

                                        if (insertondatabase())
                                        {
                                            IndiceMasAlto();
                                            UpDateEstadoDocumento(documentoemail.fnombre, documentoemail.sitio);
                                        }
                                        else
                                        {
                                            pruebadato = "Documento no logro ser procesado";

                                        }

                                    }

                                }
                                else
                                {

                                    insertFuente(DatoRemitenteMail, documentoemail.sitio);
                                    VerificaFuente(DatoRemitenteMail, documentoemail.sitio);
                                    insertTipo(DataUidFuente);
                                    VerificaTipo(DataUidFuente, "default");

                                    if (insertondatabase())
                                    {
                                        IndiceMasAlto();
                                        UpDateEstadoDocumento(documentoemail.fnombre, documentoemail.sitio);

                                    }
                                    else
                                    {
                                        pruebadato = "Documento no logro ser procesado";

                                    }
                                }

                            }
                            else
                            {
                                pruebadato = "Sitio y/o Automotora no existe";

                            }

                        }
                        else
                        {
                            pruebadato = "Documento no existe";

                        }

                    }
                }
                con.Cerrar();
            }
            hola = pruebadato;
            return hola;
        }

        /// <summary>
        /// Actualiza la tabla documento con el estato = 1, estado leido, estado = 0 , no liedo.
        /// </summary>
        /// <param name="fnombre"></param>
        /// <param name="sitio"></param>
        public void UpDateEstadoDocumento(string fnombre, int sitio)
        {
            myConnection con = new myConnection();

            string queryString = "UPDATE [dbo].[documentos] SET estado = 1, idemail = " + IdEmailParse + " WHERE fnombre = '" + fnombre + "' and sitio =" + sitio;

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();
                command.ExecuteNonQuery();
                con.Cerrar();

            }

        }

        /// <summary>
        /// Verifica que el sitio existe en la base de datos. ejemplo: "chileautos.cl"
        /// </summary>
        /// <param name="sitio"></param>
        /// <returns></returns>
        public bool VerificaSitio(int sitio)
        {
            bool verifica = false;

            myConnection con = new myConnection();

            string queryString = "SELECT * FROM [dbo].[tbl_mp_sitio] where uid_sitio = " + sitio;

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        verifica = true;
                        while (reader.Read())
                        {
                            var dataSitio = new Sitios();
                            dataSitio.uid_sitio = int.Parse(reader["uid_sitio"].ToString());
                            dataSitio.sitio = reader["sitio"].ToString();

                            listOfSitios.Add(dataSitio);

                        }

                    }

                }
                con.Cerrar();
            }

            return verifica;
        }

        /// <summary>
        /// Verifica la fuente existe en la base de datos. En este caso es el "from" del correo. Ejemplo: "info@chileautos.cl"
        /// </summary>
        /// <param name="fuente"></param>
        /// <param name="uidsitio"></param>
        /// <returns></returns>
        public bool VerificaFuente(string fuente, int uidsitio)
        {

            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(fuente);
            string hostmail = address.Host; // Get domain mail
            Console.WriteLine("only domain: " + hostmail);
            bool verifica = false;

            myConnection con = new myConnection();

            string queryString = "SELECT * FROM [dbo].[tbl_mp_fuentes] where nombre = '" + hostmail + "' and uid_sitio =" + uidsitio;

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        verifica = true;
                        while (reader.Read())
                        {
                            var dataFuente = new Fuentes();
                            dataFuente.uid_fuente = int.Parse(reader["uid_fuente"].ToString());
                            dataFuente.uid_sitio = int.Parse(reader["uid_sitio"].ToString());
                            dataFuente.nombre = reader["nombre"].ToString();

                            DataUidFuente = dataFuente.uid_fuente;

                            listOfFuentes.Add(dataFuente);

                        }

                    }

                }
                con.Cerrar();
            }

            return verifica;
        }

        /// <summary>
        /// Verifica que posea tipo. Ejemplo: "default"
        /// </summary>
        /// <param name="idfuente"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public bool VerificaTipo(int idfuente, string tipo)
        {

            bool verifica = false;

            myConnection con = new myConnection();

            string queryString = "SELECT * FROM [dbo].[tbl_mp_tipos] where uid_fuente = " + idfuente + " and tipo ='" + tipo + "'";

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        verifica = true;
                        while (reader.Read())
                        {
                            var dataTipo = new Tipos();
                            dataTipo.uid_tipo = int.Parse(reader["uid_tipo"].ToString());
                            dataTipo.uid_fuente = int.Parse(reader["uid_fuente"].ToString());
                            dataTipo.tipo = reader["tipo"].ToString();

                            DataUidTipo = dataTipo.uid_tipo;

                            listOfTipos.Add(dataTipo);

                        }

                    }

                }
                con.Cerrar();
            }

            return verifica;
        }

        /// <summary>
        /// Verifica que la automotora existe. En este caso es el "to", del correo:
        /// </summary>
        /// <param name="destinatario"></param>
        /// <returns></returns>
        public bool VerificaAutomotora(string destinatario)
        {
            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(destinatario);
            string emailaddresslocal = address.Address; // Get domain mail
            //Console.WriteLine("Mail automotora: " + emailaddresslocal);
            bool verifica = false;

            myConnection con = new myConnection();

            string queryString = "SELECT * FROM [dbo].[tbl_mp_automotoras] where email = '" + emailaddresslocal + "'";

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();

                using (var reader =command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        verifica = true;
                        while (reader.Read())
                        {
                            var dataAutomotora = new Automotoras();
                            dataAutomotora.uid_automotora = int.Parse(reader["uid_automotora"].ToString());
                            dataAutomotora.uid_sitio = int.Parse(reader["uid_sitio"].ToString());
                            dataAutomotora.cod_original = int.Parse(reader["cod_original"].ToString());
                            dataAutomotora.automotora = reader["automotora"].ToString();

                            DataUidAutomotora = dataAutomotora.uid_automotora;

                            listOfAutomotoras.Add(dataAutomotora);

                        }

                    }

                }
                con.Cerrar();
            }

            return verifica;
        }

        public bool IndiceMasAlto()
        {
            bool verifica = false;

            myConnection con = new myConnection();

            string queryString = "SELECT  * FROM [dbo].[tbl_mp_email] order by uid_email asc";

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        verifica = true;
                        while (reader.Read())
                        {
                            var dataEmail = new Emails();
                            dataEmail.uid_email = int.Parse(reader["uid_email"].ToString());

                            IdEmailParse = dataEmail.uid_email;

                            listOfEmails.Add(dataEmail);

                        }

                    }

                }
                con.Cerrar();
            }

            return verifica;
        }

        /** Inserts **/
        public void insertFuente(string fuente, int uidsitio)
        {

            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(fuente);
            string hostmail = address.Host;
            // Get domain mail
            //Console.WriteLine("only domain: " + hostmail);

            myConnection con = new myConnection();

            string queryString = "INSERT INTO [dbo].[tbl_mp_fuentes] (uid_sitio, nombre) VALUES( " + uidsitio + ", '" + hostmail + "')";

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();
                command.ExecuteNonQuery();
                con.Cerrar();

            }
        }

        public void insertTipo(int UidFunte)
        {
            myConnection con = new myConnection();

            string queryString = "INSERT INTO [dbo].[tbl_mp_tipos] (uid_fuente, tipo) VALUES( " + UidFunte + ", 'default')";

            using (con.Conexion())
            {

                SqlCommand cmd = new SqlCommand();
                SqlCommand command = new SqlCommand(queryString, con.Conexion());
                con.Abrir();
                command.ExecuteNonQuery();
                con.Cerrar();

            }

        }

        public string ChangeEncodingFormat(string DataChangeEnconde)
        {

            //string utf8String = DataChangeEnconde;
            string propEncodeString = string.Empty;

            //byte[] utf8_Bytes = new byte[utf8String.Length];
            //for (int i = 0; i < utf8String.Length; ++i)
            //{
            //    utf8_Bytes[i] = (byte)utf8String[i];
            //}

            //propEncodeString = Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length);

            ///////////////////////New Code v4 ////////////////////////////////////////////

            if (DataChangeEnconde.Contains("Ã"))
            {
                byte[] utf8_Bytes = new byte[DataChangeEnconde.Length];
                for (int i = 0; i < DataChangeEnconde.Length; ++i)
                {
                    utf8_Bytes[i] = (byte)DataChangeEnconde[i];
                }

                propEncodeString = Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length);
                //Console.WriteLine("trae caracter raro Ã ");
            }
            else
            {
                propEncodeString = DataChangeEnconde;
                //Console.WriteLine("NO trae caracter raro Ã ");
            }

            ///////////////////////End New Code v4 ////////////////////////////////////////////

            return propEncodeString;
        }
        
        public string returnPath(string dato)//metodo de prueba de comunicación
        {
            string folder = Environment.CurrentDirectory;
            return folder+dato;
        }

        public string GetPlainTextFromHtml(string htmlString)
        {
            htmlString = Regex.Replace(htmlString, @"</p>", "\r\n", RegexOptions.Multiline).Trim();
            htmlString = Regex.Replace(htmlString, @"<br>", "\n", RegexOptions.Multiline).Trim();
            htmlString = Regex.Replace(htmlString, @"<br />", "\n", RegexOptions.Multiline).Trim();
            htmlString = Regex.Replace(htmlString, @"</tr>", "\r", RegexOptions.Multiline).Trim();
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            //htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            //htmlString = Regex.Replace(htmlString, @"\s", " ", RegexOptions.Multiline);
            //htmlString = Regex.Replace(htmlString, @"\n", " ", RegexOptions.Multiline).Trim();
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }

    }
}