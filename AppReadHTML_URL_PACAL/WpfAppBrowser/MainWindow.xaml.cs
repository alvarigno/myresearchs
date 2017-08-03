using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using mshtml;
using System.Net;
using System.Security.Permissions;
using HtmlAgilityPack;
using System.Xml;
using System.IO;

namespace WpfAppBrowser
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string ContenidoPublicacion = "";

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public MainWindow()
        {

            InitializeComponent();

            myWebBrowser.Address = "http://www.macal.cl/Loteo.aspx";
           
        }

        public void LoadHTML() {
            
            var doc = new HtmlDocument();
            var web = new HtmlWeb();

            string HTML = myWebBrowser.GetSourceAsync().Result;
            doc.LoadHtml(HTML);

            List<string> ListadoImagenes = ObtieneImagenes(doc);
            List<string> ListadoIdentificadores = ObitneIdentificadorUnico(doc);
            List<string> ListadoTitulos = ObtieneTitulo(doc);
            List<string> ListadoEspcificaciones = ObtieneEspecificaciones(doc);
            List<string> ListadoEquipamiento = ObtieneEquipamiento(doc);
            ProcesaDescargas(ListadoImagenes, ListadoIdentificadores[0]);

            CargaContenido(ListadoImagenes, "listado de Imágenes", ListadoIdentificadores[0]);
            CargaContenido(ListadoTitulos, "listado de Títulos", ListadoIdentificadores[0]);
            CargaContenido(ListadoEspcificaciones, "listado de Especificaciones", ListadoIdentificadores[0]);
            CargaContenido(ListadoEquipamiento, "listado de Equipamiento", ListadoIdentificadores[0]);

            CreaArchivoTxt(ContenidoPublicacion, ListadoIdentificadores[0]);
            ContenidoPublicacion = "";
        }

        private void btncarga_Click(object sender, RoutedEventArgs e)
        {
            LoadHTML();
        }

        public List<string> ObtieneImagenes(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> listado = new List<string>();
            var query = localdoc.DocumentNode.Descendants("a")
            .Where(c => (c.Attributes["rel"] != null))
            .Select(c => c.ChildAttributes("href").FirstOrDefault().Value);

            listado = query.ToList();
            listado.RemoveAt(0);

            return listado;
        }

        public static List<string> ObitneIdentificadorUnico(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();

            var query = localdoc.DocumentNode.Descendants("a")
            .Where(c => (c.Attributes["data-contador-id"] != null) && (c.Attributes["rel"] != null) && (c.Attributes["rel"].Value != "fancybox-thumb_principal"))
            .Select(c => c.ChildAttributes("data-contador-id").First().Value);

            foreach (var d in query)
            {

                retorna.Add(d.Trim());

            }

            return retorna;
        }

        public static List<string> ObtieneTitulo(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();

            var query = localdoc.DocumentNode.Descendants("span")
            .Where(c => (c.Attributes["class"] != null) && (c.Attributes["class"].Value == "macal-general-info-minimo-veh"))
            .SelectMany(c => c.DescendantsAndSelf());

            foreach (var d in query)
            {

                retorna.Add(d.InnerText.Trim());

            }

            return retorna;
        }

        public static List<string> ObtieneEspecificaciones(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();

            var query = localdoc.DocumentNode.Descendants("div")
            .Where(c => (c.Attributes["id"] != null) && (c.Attributes["id"].Value == "divEspecificaciones"))
            .SelectMany(c => c.DescendantsAndSelf("td"));

            foreach (var d in query)
            {

                retorna.Add(d.InnerText.Trim());

            }

            return retorna;
        }

        public static List<string> ObtieneEquipamiento(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();

            var query = localdoc.DocumentNode.Descendants("div")
            .Where(c => (c.Attributes["id"] != null) && (c.Attributes["id"].Value == "divEquipamiento"))
            .SelectMany(c => c.DescendantsAndSelf("td"));

            foreach (var d in query)
            {

                retorna.Add(d.InnerText.Trim());

            }

            return retorna;
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
                else
                {

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

        private static string CrearDirectorio(string namefolder)
        {

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

            pathfile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\Descarga", directorio);
            System.IO.Directory.CreateDirectory(pathfile);

            return pathfile;
        }

        public static void CargaContenido(List<string> data, string seccion, string idpublicacion)
        {

            string bloque = "\r\n >>>>>>>>>>>>>>>>>>>>>>>>>>>>> Sección: " + seccion + "\r\n";
            ContenidoPublicacion = ContenidoPublicacion + bloque;

            data = ModificaTexto(data);

            foreach (var d in data)
            {
                string texto = "\r -> " + d.Trim() + "\r\n";
                ContenidoPublicacion = ContenidoPublicacion + texto;
            }
            data.Clear();

        }

        private static void CreaArchivoTxt(string contenido, string nombredirectorio)
        {

            string m_exePath = string.Empty;

            if (Directory.Exists(System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\Descarga" + "\\" + nombredirectorio))
            {
                m_exePath = System.IO.Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\Descarga" + "\\" + nombredirectorio;
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

        private static List<string> ModificaTexto(List<string> data)
        {

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
