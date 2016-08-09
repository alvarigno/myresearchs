using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using MimeKit;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace ConsoleAppReadFile
{
    class Program
    {

        myConnection myConn = new myConnection();

        public static string pruebadato;
        public static int count = 0;
        public static string PosicionDocumento;
        public static String DatoContenidoMailPlain;
        public static String DatoContenidoMailHtml;
        public static int IdEmailParse;
        public static String DatoFechaMail;
        public static string format = "yyyy-MM-dd HH:MM:ss";
        public static DateTime DatoFechaFromate;
        public static String DatoAsuntoMail;
        public static String DatoHeaderMail = "";
        public static String[] DatoDestinatariosMail = new string[100];
        public static string DataDestinatariosString;
        public static String[] DatoCcMail = new string[100];
        public static string DataCcString;
        public static String DatoRemitenteMail;
        public Documentos listadocumentos = new Documentos();
        public static List<Documentos> listOfDocumentos = new List<Documentos>();
        public static List<Sitios> listOfSitios = new List<Sitios>();
        public static List<Fuentes> listOfFuentes = new List<Fuentes>();
        public static List<Tipos> listOfTipos = new List<Tipos>();
        public static List<Automotoras> listOfAutomotoras = new List<Automotoras>();
        public static List<Emails> listOfEmails = new List<Emails>();

        public static int DataUidFuente;
        public static int DataUidTipo;
        public static int DataUidEstado;
        public static int DataUidAutomotora;


        public static void ParseEmail(string emlFile)
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

        public static void Main(string[] args)
        {

            //string curpath = Directory.GetCurrentDirectory();
            //string mailbox = String.Format("{0}\\inbox", curpath);
            PosicionDocumento = String.Format("{0}\\mails_prueba\\", "C:\\");
            //PosicionDocumento = String.Format("{0}\\git\\mailparser\\documentos", "D:\\");

            Run();

            // If the folder is not existed, create it.
            //if (!Directory.Exists(mailbox))
            //{
            //    Directory.CreateDirectory(mailbox);
            //}

            // Get all *.eml files in specified folder and parse it one by one.
            //string[] files = Directory.GetFiles(mailbox, "*.eml");
            //string[] files = Directory.GetFiles(mailbox, "*.*");




            //for (int i = 0; i < files.Length; i++)
            //{
            // Console.WriteLine("Cuenta: "+ (i+1));
            //ParseEmail(files[i]);
           // LoadDocmumentos(mailbox);
            //insertondatabase();  
            //}

            Console.ReadLine();

        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = "C:\\mails_prueba";
           // watcher.Path = "D:\\git\\mailparser\\documentos";

            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */

            watcher.NotifyFilter =
            NotifyFilters.LastAccess |
            NotifyFilters.CreationTime |
            NotifyFilters.LastWrite |
            NotifyFilters.FileName |
            NotifyFilters.DirectoryName;

            // Only watch text files.
            //watcher.Filter = "*.txt";
            //watcher.Filter = "*.*";

            // Add event handlers.
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Parsea Archivos de email y los inserta en la tabla email.");
            //while (Console.Read() != 'q');

        }

        // Define the event handlers. To Process Files.
        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            LoadDocmumentos(PosicionDocumento);
        }

        public static bool insertondatabase()
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

                SqlCommand mycommand = new SqlCommand();

                mycommand.CommandType = System.Data.CommandType.Text;
                mycommand.Connection = myConnection.GetConnection();
                mycommand.CommandText = "INSERT INTO [dbo].[tbl_mp_email] (uid_tipo,uid_estado,uid_automotora,email,fecha_recibido,asunto,cabecera,destinatarios,remitente, cc, email_html) VALUES(" + DataUidTipo + "," + 4 + "," + DataUidAutomotora + ",'" + DatoContenidoMailPlain.Replace("'", "\"") + "',CONVERT(DATETIME, '" + DatoFechaFromate.ToString(format) + "', 120),'" + DatoAsuntoMail + "','" + DatoHeaderMail.Replace("'", "''") + "','" + DataDestinatariosString + "','" + DatoRemitenteMail + "', '" + DataCcString + "', '" + DatoContenidoMailHtml + "')";

                a = mycommand.ExecuteNonQuery();
                mycommand.Connection.Close();


                //myConnection con = new myConnection();
                //_cnx = con.Conexion();


                //string queryString = "INSERT INTO [dbo].[tbl_mp_email] (uid_tipo,uid_estado,uid_automotora,email,fecha_recibido,asunto,cabecera,destinatarios,remitente, cc, email_html) VALUES(" + DataUidTipo + "," + 4 + "," + DataUidAutomotora + ",'" + DatoContenidoMailPlain.Replace("'", "\"") + "',CONVERT(DATETIME, '" + DatoFechaFromate.ToString(format) + "', 120),'" + DatoAsuntoMail + "','" + DatoHeaderMail.Replace("'", "''") + "','" + DataDestinatariosString + "','" + DatoRemitenteMail + "', '" + DataCcString + "', '" + DatoContenidoMailHtml + "')";

                //using (con.Conexion())
                //{
                //    SqlCommand cmd = new SqlCommand();
                //    SqlCommand command = new SqlCommand(queryString, con.Conexion());
                //    con.Abrir();
                //    a = command.ExecuteNonQuery();
                //    con.Cerrar();

                //}


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



        public static List<Documentos> LoadDocmumentos(string ubicacion)
        {

            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM dbo.documentos where estado = 0";
 
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

                        pruebadato = PosicionDocumento + documentoemail.fnombre + "|" + DataDestinatariosString;

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

            return listOfDocumentos;
        }

        public static void UpDateEstadoDocumento(string fnombre, int sitio) {

            using (var connection = new SqlCommand())
            {
                //Console.WriteLine("UPDATE[dbo].[documentos] SET estado = 1 WHERE fnombre = '" + fnombre + "' and sitio =" + sitio);
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "UPDATE[dbo].[documentos] SET estado = 1, idemail = "+ IdEmailParse +" WHERE fnombre = '" + fnombre + "' and sitio =" + sitio;
                connection.ExecuteNonQuery();
                connection.Connection.Close();

            }

        }

        public static bool VerificaSitio(int sitio)
        {
            bool verifica = false;
            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM dbo.tbl_mp_sitio where uid_sitio = "+sitio;

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
            }

            return verifica;
        }

        public static bool VerificaFuente(string fuente, int uidsitio)
        {

            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(fuente);
            string hostmail = address.Host; // Get domain mail
            Console.WriteLine("only domain: "+ hostmail);
            bool verifica = false;

            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM dbo.tbl_mp_fuentes where nombre = '" + hostmail + "' and uid_sitio ="+ uidsitio;

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
            }

            return verifica;
        }

        public static bool VerificaTipo(int idfuente, string tipo) {

            bool verifica = false;
            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "SELECT * FROM [Procesarmails].[dbo].[tbl_mp_tipos] where uid_fuente = "+ idfuente + " and tipo ='"+ tipo + "'";

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
            }

            return verifica;
        }

        public static bool VerificaAutomotora(string destinatario)
        {
            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(destinatario);
            string emailaddresslocal = address.Address; // Get domain mail
            Console.WriteLine("Mail automotora: " + emailaddresslocal);

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
            }

            return verifica;
        }

        public static bool IndiceMasAlto()
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
            }

            return verifica;
        }

        /** Insert Fuentes**/
        public static void insertFuente(string fuente, int uidsitio) {

            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(fuente);
            string hostmail = address.Host; // Get domain mail
            //Console.WriteLine("only domain: " + hostmail);
           
            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "INSERT INTO [Procesarmails].[dbo].[tbl_mp_fuentes] (uid_sitio, nombre) VALUES( " + uidsitio + ", '" + hostmail + "')";
                connection.ExecuteNonQuery();
                connection.Connection.Close();
                //VerificaFuente(fuente, uidsitio);
            }
        }

        public static void insertTipo(int UidFunte) {

            using (var connection = new SqlCommand())
            {
                connection.Connection = myConnection.GetConnection();
                connection.CommandText = "INSERT INTO [Procesarmails].[dbo].[tbl_mp_tipos] (uid_fuente, tipo) VALUES( " + UidFunte + ", 'default')";
                connection.ExecuteNonQuery();
                connection.Connection.Close();
                //VerificaFuente(fuente, uidsitio);
            }

        }

        public static string ChangeEncodingFormat(string DataChangeEnconde)
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

        public static string returnPath(string dato)//metodo de prueba de comunicación
        {
            string folder = Environment.CurrentDirectory;
            return folder + dato;
        }

        public static string GetPlainTextFromHtml(string htmlString)
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

        /// <summary>
        /// 
        /// Pruebas de codificación de Textos 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public static bool ContainsUnicodeCharacter(string input)
        {
            const int MaxAnsiCode = 255;

            return input.Any(c => c > MaxAnsiCode);
        }

        public static string GetASCIIvar(string input)
        {

            string inputString = input;
            string asAscii = Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                        ),
                    Encoding.UTF8.GetBytes(inputString)

                )

            );

            Console.WriteLine("Valor asAsCii: " + asAscii);
            return asAscii;
        }




        public static string decodificaTextoUTF8(string strLine)
        {

            //////////////////////////////////////////////////////////////
            var buffer = new List<byte>();
            var i = 0;
            while (i < strLine.Length)
            {
                var character = strLine[i];
                if (character == '=')
                {
                    var part = strLine.Substring(i + 1, 2);
                    Console.WriteLine("LLega aquí?, part:" + part);
                    buffer.Add(byte.Parse(part, NumberStyles.HexNumber));
                    i += 3;
                }
                else
                {
                    buffer.Add((byte)character);
                    i++;
                }
            };
            string output = Encoding.UTF8.GetString(buffer.ToArray());
            ////////////////////////////////////////////////
            return output;
        }

        public static string pruebaborrame(string input)
        {

            // Create a UTF-8 encoding.
            UTF8Encoding utf8 = new UTF8Encoding();

            // A Unicode string with two characters outside an 8-bit code range.
            String unicodeString = input;
            Console.WriteLine("Original string:");
            Console.WriteLine(unicodeString);

            // Encode the string.
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            Console.WriteLine();
            Console.WriteLine("Encoded bytes:");
            for (int ctr = 0; ctr < encodedBytes.Length; ctr++)
            {
                Console.Write("{0:X2} ", encodedBytes[ctr]);
                if ((ctr + 1) % 25 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine();

            // Decode bytes back to string.
            String decodedString = utf8.GetString(encodedBytes);
            Console.WriteLine();
            Console.WriteLine("Decoded bytes:");
            Console.WriteLine(decodedString);
            return decodedString;
        }

        public static string pruebaborrame2(string input)
        {
            //Console.WriteLine("Borrame 2 "+ input);
            var specialCharacters = "áéíóúÁÉÍÓÚñÑüÜ@%!#$%^&*()?/>";
            //var specialCharacters = @"%!@#$%^&*()?/>.<,:;'\´|}]{[_~`+=-" + "\"";
            var goodEncoding = Encoding.UTF8;
            var badEncoding = Encoding.GetEncoding(28591);
            var badStrings = specialCharacters.Select(c => badEncoding.GetString(goodEncoding.GetBytes(c.ToString())));

            var sourceText = input;
            if (badStrings.Any(s => sourceText.Contains(s)))
            {
                sourceText = goodEncoding.GetString(badEncoding.GetBytes(sourceText));
            }

            return sourceText;

        }

        public static string borrame3(string input) {

            System.Text.Encoding utf_8 = System.Text.Encoding.UTF8;
            string s_unicode = input;
            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(s_unicode);
            string s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
            return s_unicode2;

        }


        public static string GetStringformat(string data) {

            string localdata;

            //localdata = data.GetType().ToString();
            //localdata =  data.GetTypeCode().ToString();
            //localdata = data.GetHashCode().ToString();
            localdata = data.IsNormalized().ToString();

            return localdata;


        }

    }
}
