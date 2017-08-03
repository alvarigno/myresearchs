
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace AppReadHTML_URL
{
    class Program
    {
        //revisión de datos de html de Pacal
        static string filePath = "http://www.macal.cl/Loteo.aspx?P_rm=";
        static string ContenidoPublicacion = "";
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        static void Main(string[] args)
        {
            Console.WriteLine("Descargar información de aviso desde http://www.macal.cl/Loteo.aspx?P_rm=. \r\n debe ingresar un código válido.");
            Console.WriteLine("Cód.:");
            string codigo = Console.ReadLine();
            Console.WriteLine("Código de aviso ingresado: "+codigo);
            // From File
            var doc = new HtmlDocument();

            //VerificaURL(filePath + codigo);



            // From Web
            var url = filePath+codigo;
            var web = new HtmlWeb();
            doc = web.Load(url);

            if (!VerificaContenido(doc))
            {
                ObtieneImagenes(doc);
                //             List<string> ListadoImagenes = ObtieneImagenes(doc);
                //               List<string> ListadoDatos = ObtieneDatos(doc);
                //               List<string> Listadotitulo = ObtieneTitulo(doc);
                //               List<string> Listadovalor = ObtienValor(doc);
                //               List<string> Listadovalortachado = ObtienValorTachado(doc);
                //               List<string> Listadovalorbono = ObtienValorBono(doc);
                //                //List<string> Listadoprueba = pruebauno(doc); 
                //
                //               //MuestraContenido(ListadoImagenes, "imágenes");
                //               ProcesaDescargas(ListadoImagenes, codigo);
                //               MuestraContenido(ListadoDatos, "listado de datos", codigo);
                //               MuestraContenido(Listadotitulo, "título de la publicación", codigo);
                //               MuestraContenido(Listadovalor, "Precio", codigo);
                //               MuestraContenido(Listadovalorbono, "Valor bonificado", codigo);
                //               MuestraContenido(Listadovalortachado, "Valor tachado", codigo);
                //
                //               CreaArchivoTxt(ContenidoPublicacion, codigo);

            }
            else {

              //  List<string> datoserror = new List<string>();
              //  Console.WriteLine("Vehículo no encontrado");
              //  datoserror.Add("-> url Ficha: " + url + ".\r\n-> Código: " + codigo + ". \r\n-> no posee información.");
              //  datoserror =  ModificaTexto(datoserror);
              //  Console.WriteLine(datoserror[0]);

            }

            Console.ReadLine();
        }

        public static List<string> ObtieneImagenes(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            var query = localdoc.DocumentNode.Descendants("a")
            .Where(c => (c.Attributes["class"] != null) && (c.Attributes["class"].Value == "fancybox-thumb"))
            .Select(c => c.ChildAttributes("href").FirstOrDefault().Value);

            return query.ToList();
        }

        public static List<string> ObtieneDatos(HtmlDocument doc) {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();

            var query = localdoc.DocumentNode.Descendants("span")
            .Where(c => (c.Attributes["itemprop"] != null) && (c.Attributes["itemprop"].Value == "description"))
            .SelectMany(c => c.Descendants());

            foreach (var d in query) {

                retorna.Add(d.InnerHtml);

            }

            return retorna;

        }

        public static List<string> ObtieneTitulo(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();

            var query = localdoc.DocumentNode.Descendants("div")
            .Where(c => (c.Attributes["class"] != null) && (c.Attributes["class"].Value == "row pagetitle"))
            .SelectMany(c => c.DescendantsAndSelf("h1"));

            foreach (var d in query)
            {

                retorna.Add(d.InnerText.Trim());

            }

            return retorna;
        }

        public static List<string> ObtienValor(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();
            var query = localdoc.DocumentNode.Descendants("div")
            .Where(c => (c.Attributes["class"] != null) && (c.Attributes["class"].Value == "tags price"))
            .SelectMany(c => c.DescendantsAndSelf());

            foreach (var d in query)
            {

                retorna.Add(d.InnerText);

            }

            return retorna;

        }

        public static List<string> ObtienValorTachado(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();
            var query = localdoc.DocumentNode.Descendants("div")
            .Where(c => (c.Attributes["class"] != null) && (c.Attributes["class"].Value == "tags price tachado"))
            .SelectMany(c => c.DescendantsAndSelf());

            foreach (var d in query)
            {

                retorna.Add(d.InnerText);

            }

            return retorna;

        }

        public static List<string> ObtienValorBono(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();
            var query = localdoc.DocumentNode.Descendants("div")
            .Where(c => (c.Attributes["class"] != null) && (c.Attributes["class"].Value == "tags bono"))
            .SelectMany(c => c.DescendantsAndSelf());
            
            foreach (var d in query)
            {
                retorna.Add(d.InnerText);
            }

            return retorna;
            
        }

        public static void MuestraContenido(List<string> data, string seccion, string idpublicacion) {

            string bloque = "\r\n >>>>>>>>>>>>>>>>>>>>>>>>>>>>> Sección: " + seccion + "\r\n";
            ContenidoPublicacion = ContenidoPublicacion + bloque;
            Console.WriteLine(bloque);

            data = ModificaTexto(data);

            foreach (var d in data)
            {
                string texto = "\r -> " + d.Trim() + "\r\n";
                Console.WriteLine(texto);
                ContenidoPublicacion = ContenidoPublicacion + texto;
            }
            data.Clear();
            
        }

        public static void ProcesaDescargas(List<string> data, string idpublicacion)
        {

            Console.WriteLine("\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Descargando Imágenes: " + idpublicacion);
            foreach (var d in data)
            {
                Console.WriteLine("-> " + d);
                if (DescargaFotografias(d, idpublicacion))
                {

                    Console.WriteLine("Descarga completa.");
                }
                else {

                    Console.WriteLine("Descarga fallida.");

                }
            }

        }

        private static Boolean DescargaFotografias(string urlfilename, string namefolder)
        {
            Boolean cargo = false;


            try
            {
                string pathfile = CrearDirectorio(namefolder);

                string filename = "";
                Uri uri = new Uri(urlfilename);

                filename = System.IO.Path.GetFileName(uri.Segments.Last(seg => seg.Contains(".")));

                using (WebClient webClient = new WebClient())
                {

                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate (object sender, DownloadProgressChangedEventArgs e)
                    {
                        Console.WriteLine("Downloaded:" + e.ProgressPercentage.ToString());
                    });

                    webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler
                        (delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                        {
                            if (e.Error == null && !e.Cancelled)
                            {
                                Console.WriteLine("Download completed!");

                            }
                        });

                    webClient.DownloadFile(new System.Uri(urlfilename), pathfile + "\\" + filename);
                    cargo = true;
                }

            }
            catch (Exception e)
            {

                Console.Write(e.Message);

                cargo = false;
            }

            return cargo;
        }

        private static string CrearDirectorio(string namefolder) {

            string pathfile = "";
            string directorio = "";
            if (!String.IsNullOrEmpty(namefolder))
            {
                directorio = namefolder;
            }
            else
            {
                directorio = "defecto";
            }

            pathfile = System.IO.Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\Descarga", directorio);
            System.IO.Directory.CreateDirectory(pathfile);

            return pathfile;
        }

        private static void CreaArchivoTxt(string contenido, string nombredirectorio)
        {

            string m_exePath = string.Empty;

            if (Directory.Exists(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\Descarga"+"\\"+nombredirectorio))
            {
                m_exePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\Descarga" + "\\" + nombredirectorio;
            }
            else
            {
                m_exePath = CrearDirectorio(nombredirectorio);
            }


            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + nombredirectorio + "_contenido.txt"))
                {
                    Log(contenido, w);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        private static void Log(string contenido, TextWriter txtWriter)
        {

            txtWriter.Write("\r\nLog Entry : ");
            txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            txtWriter.WriteLine("  :Contenido de la puclicación");
            txtWriter.WriteLine("  :{0}", contenido);
            txtWriter.WriteLine("-------------------------------");

        }

        private static Boolean VerificaURL(string url) {

            //
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            var response = (HttpWebResponse)request.GetResponse();
            Boolean success = response.StatusCode == HttpStatusCode.OK;

            return success;
        }

        private static Boolean VerificaContenido( HtmlDocument doc) {

            Boolean valido = false;

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();
            var query = localdoc.DocumentNode.Descendants("div")
            .Where(c => (c.Attributes["class"] != null) && (c.Attributes["class"].Value == "alert alert-danger"))
            .SelectMany(c => c.DescendantsAndSelf());

            foreach (var d in query) {


                if (d.InnerText == " Veh&iacute;culo no encontrado")
                {
                    
                    valido = true;

                }

            }

            return valido;

        }

        private static List<string> ModificaTexto(List<string> data) {

            for (int i = 0; i < data.Count; i++)
            {
                data[i] = data[i].Replace("&aacute;", "á");
                data[i] = data[i].Replace("&Aacute;", "Á");
                data[i] = data[i].Replace("&eacute;", "é");
                data[i] = data[i].Replace("&Eacute;", "É");
                data[i] = data[i].Replace("&iacute;", "í");
                data[i] = data[i].Replace("&Iacute;", "Í");
                data[i] = data[i].Replace("&ntilde;", "ñ");
                data[i] = data[i].Replace("&Ntilde;", "Ñ");
                data[i] = data[i].Replace("&oacute;", "ó");
                data[i] = data[i].Replace("&Oacute;", "Ó");
                data[i] = data[i].Replace("&uacute;", "ú");
                data[i] = data[i].Replace("&Uacute;", "Ú");
                data[i] = data[i].Replace("&uuml;", "ü");
                data[i] = data[i].Replace("&Uuml;", "Ü");
                data[i] = data[i].Replace("&iquest;", "¿");
                data[i] = data[i].Replace("&iexcl;", "¡");
                data[i] = data[i].Replace("&nbsp;", " ");

            }

            return data;

        }

    }
}
