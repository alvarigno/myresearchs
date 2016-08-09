using System;
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

        myConnection myConn = new myConnection();

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


        public string Comienzaproceso(string documentoRecibido, string PosicionDocumento)
        {
            
            string hola = LoadDocmumentos(documentoRecibido, PosicionDocumento);
            return hola;
        }

        public bool insertondatabase()
        {

            try
            {
                //DatoContenidoMailHtml = DatoContenidoMailHtml.Replace("'", "\"").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                if (string.IsNullOrEmpty(DatoContenidoMailHtml) && DatoContenidoMailPlain.Length > 0)
                {
                    DatoContenidoMailHtml = DatoContenidoMailPlain.Replace("'", "\"");
                }

                if (DatoContenidoMailHtml.Length > 7000 && DatoContenidoMailPlain.Length > 0) {

                    DatoContenidoMailHtml = DatoContenidoMailPlain.Replace("'", "\""); }

                if (DatoContenidoMailHtml.Length < 7000 && string.IsNullOrEmpty(DatoContenidoMailPlain)) {

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

                SqlCommand mycommand = new SqlCommand();

                mycommand.CommandType = System.Data.CommandType.Text;
                mycommand.Connection = myConnection.GetConnection();
                mycommand.CommandText = "INSERT INTO[dbo].[tbl_mp_email] (uid_tipo,uid_estado,uid_automotora,email,fecha_recibido,asunto,cabecera,destinatarios,remitente, cc, email_html) VALUES(" + DataUidTipo + "," + 4 + "," + DataUidAutomotora + ",'" + DatoContenidoMailPlain.Replace("'", "\"") + "',CONVERT(DATETIME, '" + DatoFechaFromate.ToString(format) + "', 120),'" + DatoAsuntoMail + "','" + DatoHeaderMail.Replace("'", "''") + "','" + DataDestinatariosString + "','" + DatoRemitenteMail + "', '" + DataCcString + "', '" + DatoContenidoMailHtml + "')";

                int a = mycommand.ExecuteNonQuery();
                mycommand.Connection.Close();
                mycommand.Connection.Dispose();
                SqlConnection.ClearAllPools();

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



        public string LoadDocmumentos(string documentoRecibido, string PosicionDocumento)
        {
            string hola;
            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM dbo.documentos where fnombre = '"+ documentoRecibido + "' and estado = 0";

                using (var reader = connection.ExecuteReader())
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
                connection.Connection.Close();
            }
            hola = pruebadato;
            return hola;
        }

        public void UpDateEstadoDocumento(string fnombre, int sitio)
        {

            using (var connection = new SqlCommand())
            {
                //Console.WriteLine("UPDATE[dbo].[documentos] SET estado = 1 WHERE fnombre = '" + fnombre + "' and sitio =" + sitio);
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "UPDATE[dbo].[documentos] SET estado = 1, idemail = " + IdEmailParse + " WHERE fnombre = '" + fnombre + "' and sitio =" + sitio;
                connection.ExecuteNonQuery();
                connection.Connection.Close();
                connection.Dispose();
                SqlConnection.ClearAllPools();

            }

        }

        public bool VerificaSitio(int sitio)
        {
            bool verifica = false;
            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM dbo.tbl_mp_sitio where uid_sitio = " + sitio;

                using (var reader = connection.ExecuteReader())
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

                            // Console.WriteLine(" Datos Sitios: " + dataSitio.uid_sitio + ", Nombre sitio: " + dataSitio.sitio);

                        }

                    }

                }
                connection.Connection.Close();
                connection.Connection.Dispose();
                SqlConnection.ClearAllPools();
            }

            return verifica;
        }

        public bool VerificaFuente(string fuente, int uidsitio)
        {

            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(fuente);
            string hostmail = address.Host; // Get domain mail
            Console.WriteLine("only domain: " + hostmail);
            bool verifica = false;

            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM dbo.tbl_mp_fuentes where nombre = '" + hostmail + "' and uid_sitio =" + uidsitio;

                using (var reader = connection.ExecuteReader())
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

                            //Console.WriteLine(" Datos Fuente iud: " + dataFuente.uid_fuente + ", Uid sitio: " + dataFuente.uid_sitio + ", Nombre fuente: " + dataFuente.nombre);

                        }

                    }

                }
                connection.Connection.Close();
                connection.Connection.Dispose();
                SqlConnection.ClearAllPools();
            }

            return verifica;
        }

        public bool VerificaTipo(int idfuente, string tipo)
        {

            bool verifica = false;
            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM [Procesarmails].[dbo].[tbl_mp_tipos] where uid_fuente = " + idfuente + " and tipo ='" + tipo + "'";

                using (var reader = connection.ExecuteReader())
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

                            //Console.WriteLine(" Datos Tipo iud: " + dataTipo.uid_tipo + ", Uid fuente: " + dataTipo.uid_fuente + ", Nombre tipo: " + dataTipo.tipo);

                        }

                    }

                }
                connection.Connection.Close();
                connection.Connection.Dispose();
                SqlConnection.ClearAllPools();
            }

            return verifica;
        }

        public bool VerificaAutomotora(string destinatario)
        {
            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(destinatario);
            string emailaddresslocal = address.Address; // Get domain mail
            //Console.WriteLine("Mail automotora: " + emailaddresslocal);

            bool verifica = false;
            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM [Procesarmails].[dbo].[tbl_mp_automotoras] where email = '" + emailaddresslocal + "'";

                using (var reader = connection.ExecuteReader())
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

                            //Console.WriteLine(" Datos Automotora iud: " + dataAutomotora.uid_automotora + ", Uid sitio: " + dataAutomotora.uid_sitio + ", Codigo orig. : " + dataAutomotora.cod_original +", Nombre Automotora "+ dataAutomotora.automotora);

                        }

                    }

                }
                connection.Connection.Close();
                connection.Connection.Dispose();
                SqlConnection.ClearAllPools();
            }

            return verifica;
        }

        public bool IndiceMasAlto()
        {
            bool verifica = false;
            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT  * FROM [Procesarmails].[dbo].[tbl_mp_email] order by uid_email asc";

                using (var reader = connection.ExecuteReader())
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

                            // Console.WriteLine(" Datos Sitios: " + dataSitio.uid_sitio + ", Nombre sitio: " + dataSitio.sitio);

                        }

                    }

                }
                connection.Connection.Close();
                connection.Connection.Dispose();
                SqlConnection.ClearAllPools();
            }

            return verifica;
        }

        /** Insert Fuentes**/
        public void insertFuente(string fuente, int uidsitio)
        {

            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(fuente);
            string hostmail = address.Host; // Get domain mail
                                            //Console.WriteLine("only domain: " + hostmail);

            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "INSERT INTO [Procesarmails].[dbo].[tbl_mp_fuentes] (uid_sitio, nombre) VALUES( " + uidsitio + ", '" + hostmail + "')";
                connection.ExecuteNonQuery();
                connection.Connection.Close();
                connection.Connection.Dispose();
                SqlConnection.ClearAllPools();
                //VerificaFuente(fuente, uidsitio);
            }
        }

        public void insertTipo(int UidFunte)
        {

            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "INSERT INTO [Procesarmails].[dbo].[tbl_mp_tipos] (uid_fuente, tipo) VALUES( " + UidFunte + ", 'default')";
                connection.ExecuteNonQuery();
                connection.Connection.Close();
                connection.Connection.Dispose();
                SqlConnection.ClearAllPools();
                //VerificaFuente(fuente, uidsitio);
            }

        }

        public string ChangeEncodingFormat(string DataChangeEnconde)
        {

            //////////////////////////////Original Code//////////////////////////////////////////
            //string utf8String = DataChangeEnconde;////original
            string propEncodeString = string.Empty;
            //byte[] utf8_Bytes = new byte[utf8String.Length];
            //for (int i = 0; i < utf8String.Length; ++i)
            //{
            //    utf8_Bytes[i] = (byte)utf8String[i];
            //}

            //propEncodeString = Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length);
            //Console.WriteLine("Muestra Codificacion: Default: "+propEncodeString);

            //propEncodeString = pruebaborrame2(propEncodeString);
            //////////////////////////////End Original Code//////////////////////////////////////////

            ///////////////////////New Code ////////////////////////////////////////////
            //var specialCharacters = "áéíóúÁÉÍÓÚñÑüÜ@%!#$%^&*()?/>.<,:;'´|}]{[_~`+=-" + "\"";
            //var goodEncoding = Encoding.UTF8;
            //var badEncoding = Encoding.GetEncoding(28591);
            //var badStrings = specialCharacters.Select(c => badEncoding.GetString(goodEncoding.GetBytes(c.ToString())));
//
            //var sourceText = DataChangeEnconde;
            //if (badStrings.Any(s => sourceText.Contains(s)))
            //{
//
            //    propEncodeString = goodEncoding.GetString(badEncoding.GetBytes(sourceText));
            //    Console.WriteLine("propEncodeString: " + propEncodeString);
            //}
            ///////////////////////End New Code ////////////////////////////////////////////

            //string utf8String = DataChangeEnconde;
            //string propEncodeString = string.Empty;

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
