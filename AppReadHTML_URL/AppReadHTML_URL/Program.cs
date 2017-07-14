
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

        static string filePath = "http://www.julioinfante.com/ficha/49162";
        static void Main(string[] args)
        {

            // From File
            var doc = new HtmlDocument();

            // From Web
            var url = filePath;
            var web = new HtmlWeb();
            doc = web.Load(url);

            List<string> ListadoImagenes = ObtieneImagenes(doc);
            List<string> ListadoDatos = ObtieneDatos(doc);


            MuestraContenido(ListadoImagenes);
            MuestraContenido(ListadoDatos);

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
            localdoc =  doc;
            List<string> retorna = new List<string>();
            
            if (localdoc.DocumentNode.SelectNodes("//h1/span").First().Attributes["itemprop"].Value == "name") {

                var query = localdoc.DocumentNode.SelectNodes("//h1/span");
                foreach (var data in query)
                {

                    retorna.Add(data.InnerText);

                }
            }

            return retorna;

        }

        public static void MuestraContenido(List<string> data) {


            foreach (var img in data)
            {
                Console.WriteLine("Contenido: " + img);
            }

        }


    }
}
