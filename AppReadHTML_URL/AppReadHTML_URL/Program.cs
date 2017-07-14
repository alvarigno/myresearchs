
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReadHTML_URL
{
    class Program
    {

        static string filePath = "http://www.julioinfante.com/ficha/";
        static void Main(string[] args)
        {

            string codigo = Console.ReadLine();
            Console.WriteLine(codigo);
            // From File
            var doc = new HtmlDocument();

            // From Web
            var url = filePath+codigo;
            var web = new HtmlWeb();
            doc = web.Load(url);

            List<string> ListadoImagenes = ObtieneImagenes(doc);
            List<string> ListadoDatos = ObtieneDatos(doc);
            List<string> Listadotitulo = ObtieneTitulo(doc);
            List<string> Listadovalortachado = ObtienValorTachado(doc);
            List<string> Listadovalorbono = ObtienValorBono(doc);
           // List<string> Listadoprueba = pruebauno(doc); 

            MuestraContenido(ListadoImagenes, "imágenes");
            MuestraContenido(ListadoDatos, "listado de datos");
            MuestraContenido(Listadotitulo, "título de la publicación");
            MuestraContenido(Listadovalorbono, "Valor bonificado");
            MuestraContenido(Listadovalortachado, "Valor tachado");

            Console.ReadLine();
        }

        public static List<string> ObtieneImagenes(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            var query = localdoc.DocumentNode.Descendants("img")
            .Where(c => c.Attributes["itemprop"] != null)
            .Select(c => c.ChildAttributes("src").FirstOrDefault().Value);

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

        public static void MuestraContenido(List<string> data, string seccion) {

            Console.WriteLine("\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Sección: "+ seccion);
            foreach (var d in data)
            {
                Console.WriteLine("-> " + d);
            }

        }


    }
}
