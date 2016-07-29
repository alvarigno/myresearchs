using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;
using EAGetMail;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Security.Permissions;

namespace ConsoleAppReadFile
{
    class Program
    {

        myConnection myConn = new myConnection();

        public static int count = 0;
        public static string mailbox;
        public static String DatoContenidoMailPlain;
        public static String DatoContenidoMailHtml;
        public static int IdEmailParse;
        public static String DatoFechaMail;
        public static string format = "yyyy-MM-dd HH:MM:ss";
        public static DateTime DatoFechaFromate;
        public static String DatoAsuntoMail;
        public static String DatoHeaderMail;
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


        private static void ParseEmail(string emlFile)
        {

            Mail oMail = new Mail("TryIt");
            
            oMail.Load(emlFile, false);

            // Parse Header Mail
            DatoHeaderMail = oMail.Headers.ToString();
            DatoHeaderMail = DatoHeaderMail.Replace("'", "\'");
            //Console.WriteLine("Header: {0}", oMail.Headers.ToString());

            // Parse Mail From, Sender
            DatoRemitenteMail = oMail.From.ToString();
            //Console.WriteLine("From: {0}", oMail.From.ToString());

            //Date
            DatoFechaMail = oMail.SentDate.ToString();

            DatoFechaFromate = oMail.SentDate;

            //DatoFechaFromate = DateTime.Parse(DatoFechaMail, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            //DatoFechaFromate = Convert.ToDateTime(DatoFechaMail);

            //            string format = "yyyy-dd-MM HH:MM:ss";
            //            DatoFechaFromate.ToString(format);

            //Console.WriteLine("Date: {0}"+ DatoFechaFromate);

            // Parse Mail To, Recipient
            EAGetMail.MailAddress[] addrs = oMail.To;
            DatoDestinatariosMail = new string[addrs.Length]; 
            for (int i = 0; i < addrs.Length; i++)
            {
                DatoDestinatariosMail[i] = addrs[i].ToString();
            }
            if (addrs.Length > 0)
            {
                DataDestinatariosString = DatoDestinatariosMail.Aggregate((a, b) => Convert.ToString(a) + "," + Convert.ToString(b));
            }
            //Console.WriteLine("Destinatarios To: " + DataDestinatariosString);

            // Parse Mail CC
            EAGetMail.MailAddress[] addrs2 = oMail.Cc;
            DatoCcMail = new string[addrs2.Length];
            for (int i = 0; i < addrs2.Length; i++)
            {
                DatoCcMail[i] = addrs2[i].ToString();
            }
            if (addrs2.Length > 0)
            {
                DataCcString = DatoCcMail.Aggregate((c, d) => Convert.ToString(c) + "," + Convert.ToString(d));
            }else { DataCcString = "vacio"; }
            //Console.WriteLine("Destinatarios Cc: " + DataCcString); 

            // Parse Mail Subject
            String personalprueba = oMail.Subject;
            personalprueba =  oMail.Subject;
            personalprueba = personalprueba.Replace("(Trial Version)", "");
            DatoAsuntoMail = personalprueba.ToString();
            //Console.WriteLine("Subject: "+ personalprueba);

            // Parse Mail Text/Plain body
            DatoContenidoMailPlain = oMail.TextBody.ToString();
            //Console.WriteLine("TextBody: {0}", oMail.TextBody);

            // Parse Mail Html Body
            DatoContenidoMailHtml = oMail.HtmlBody.ToString();
            //Console.WriteLine("HtmlBody: {0}", oMail.HtmlBody);

        }

        public static void Main(string[] args)
        {

            //string curpath = Directory.GetCurrentDirectory();
            //string mailbox = String.Format("{0}\\inbox", curpath);
            mailbox = String.Format("{0}\\mails_prueba", "C:\\");
            //mailbox = String.Format("{0}\\git\\mailparser\\documentos", "D:\\");

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
            LoadDocmumentos(mailbox);
        }

        public static bool insertondatabase()
        {

            try
            {
                DatoContenidoMailHtml = DatoContenidoMailHtml.Replace("'", "\"").Replace("\n", "").Replace("\r", "").Replace("\t", "");
                if (DatoContenidoMailHtml.Length == 0) {
                    DatoContenidoMailHtml = DatoContenidoMailPlain.Replace("'", "\"");
                }
                else if(DatoContenidoMailHtml.Length > 7000 ) { DatoContenidoMailHtml = DatoContenidoMailPlain.Replace("'", "\""); }

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

                Console.WriteLine("INSERT INTO[dbo].[tbl_mp_email] (uid_tipo,uid_estado,uid_automotora,email,fecha_recibido,asunto,cabecera,destinatarios,remitente, cc, email_html) VALUES("+DataUidTipo+","+4+","+DataUidAutomotora+",'" + DatoContenidoMailPlain.Replace("'", "\"") + "','" + DatoFechaFromate.ToString(format) + "','" + DatoAsuntoMail + "','" + DatoHeaderMail.Replace("'", "''") + "','" + DataDestinatariosString + "','" + DatoRemitenteMail + "', '" + DataCcString + "', '" + DatoContenidoMailHtml + "');");

                SqlCommand mycommand = new SqlCommand();

                mycommand.CommandType = System.Data.CommandType.Text;
                mycommand.Connection = myConnection.GetConnection();
                mycommand.CommandText = "INSERT INTO[dbo].[tbl_mp_email] (uid_tipo,uid_estado,uid_automotora,email,fecha_recibido,asunto,cabecera,destinatarios,remitente, cc, email_html) VALUES(" + DataUidTipo + "," + 4 + "," + DataUidAutomotora + ",'" + DatoContenidoMailPlain.Replace("'", "\"") + "','" + DatoFechaFromate.ToString(format) + "','" + DatoAsuntoMail + "','" + DatoHeaderMail.Replace("'", "''") + "','" + DataDestinatariosString + "','" + DatoRemitenteMail + "', '" + DataCcString + "', '" + DatoContenidoMailHtml + "')";

                int a = mycommand.ExecuteNonQuery();
                mycommand.Connection.Close();
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
                            documentoemail.id_num = reader["id_num"].ToString();
                            documentoemail.estado = reader["estado"].ToString();
                            documentoemail.fnombre = reader["fnombre"].ToString();
                            documentoemail.sitio = reader["sitio"].ToString(); ;

                            listOfDocumentos.Add(documentoemail);

                            ParseEmail(ubicacion+"\\"+documentoemail.fnombre);

                            if (new FileInfo(ubicacion + "\\" + documentoemail.fnombre).Length > 0) {


                                if (VerificaSitio(int.Parse(documentoemail.sitio)) && VerificaAutomotora(DataDestinatariosString))
                                {

                                    Console.WriteLine("Si, existe sitio y automotora, para archivo:" + documentoemail.fnombre);
                                    if (VerificaFuente(DatoRemitenteMail, int.Parse(documentoemail.sitio)))
                                    {

                                        Console.WriteLine("Fuente Existe");
                                        if (VerificaTipo(DataUidFuente, "default"))
                                        {

                                            Console.WriteLine("Tipo Existe");

                                            Console.WriteLine("UiTipo: " + DataUidTipo + ", Estado: encola (4) , UidAutomotora: " + DataUidAutomotora);
                                            if (insertondatabase())
                                            {
                                                IndiceMasAlto();
                                                UpDateEstadoDocumento(documentoemail.fnombre, int.Parse(documentoemail.sitio));
                                                Console.WriteLine("nº: " + count + ", Datos documento: " + documentoemail.id_num + ", " + documentoemail.fnombre + ", estado: " + documentoemail.estado + ", Sitio: " + documentoemail.sitio);

                                            }
                                            else
                                            {

                                                Console.WriteLine("No logro ser procesado");

                                            }

                                        }
                                        else
                                        {

                                            Console.WriteLine("Tipo NO Existe");
                                            insertTipo(DataUidFuente);

                                        }

                                    }
                                    else
                                    {

                                        Console.WriteLine("Fuente no existe");
                                        insertFuente(DatoRemitenteMail, int.Parse(documentoemail.sitio));
                                        Console.WriteLine("Inserto fuentes" + DatoRemitenteMail);
                                    }

                                }
                                else
                                {

                                    Console.WriteLine("NO existe sitio ni automotora.");

                                }

                            }
                            else {

                                Console.WriteLine("Archivo: " + ubicacion + "\\" + documentoemail.fnombre + ", viene vacio.");

                            }
                        Console.WriteLine("nº: " + count +"\n");

                        count++;

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


        public static string ChangeEncodingFormat(string DataChangeEnconde) {

            string utf8String = DataChangeEnconde;
            string propEncodeString = string.Empty;

            byte[] utf8_Bytes = new byte[utf8String.Length];
            for (int i = 0; i < utf8String.Length; ++i)
            {
                utf8_Bytes[i] = (byte)utf8String[i];
            }

            propEncodeString = Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length);
            //Console.WriteLine("Muestra Codificacion: Default: "+propEncodeString);
            return propEncodeString;
        }


    }
}
