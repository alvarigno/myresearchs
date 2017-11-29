using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ConsoleAppReadHTMLBody
{
    class Program
    {

        public static string filePath = "http://autos.demotores.cl/dm-411265000";
        public static string prueba = @"";

        static void Main(string[] args)
        {



            var doc = new HtmlDocument();
            var url = filePath;
            var web = new HtmlWeb();
            doc = web.Load(url);

            Console.WriteLine("Carga datos HTML");

            if (VerificaContenido(doc))
            {

                Console.WriteLine("datos: " + doc);
            }
            else {

                Console.WriteLine("No cargó datos html.");

            }
            Console.ReadKey();



        }

        private static Boolean VerificaContenido(HtmlDocument doc)
        {

            Boolean valido = false;
            string contenidoerror = "";
            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();
            var query = localdoc.DocumentNode.Descendants("div")
            .Where(c => (c.Attributes["id"] != null) && (c.Attributes["id"].Value == "wrapperError"))
            .Select(n => new
                {
                    Value = n.Element("h1").InnerText,
                })
                .ToList();

            //var query = localdoc.DocumentNode.Descendants("div")
            //.Where(c => (c.Attributes["id"] != null) && (c.Attributes["id"].Value == "wrapperError"))
            //.SelectMany(c => c.DescendantsAndSelf("h1"));


            //var query = localdoc.DocumentNode.Descendants("div")
            //.Where(c => (c.Attributes["id"].Value == "wrapperError") && (c.Attributes["class"].Value == "alert alert-danger"))
            //.SelectMany(c => c.DescendantsAndSelf());
            //.SelectMany(c => c.DescendantsAndSelf("h1"));

            foreach (var d in query)
            {

                contenidoerror = contenidoerror + d.Value;

               Boolean hola =  d.Value.Contains("¡Ups!");


                if (d.Value == "¡Ups! ")
                {

                    valido = true;

                }

            }

            return valido;

        }

        public static List<string> ObtieneDatos(HtmlDocument doc)
        {

            var localdoc = new HtmlDocument();
            localdoc = doc;
            List<string> retorna = new List<string>();

            var query = localdoc.DocumentNode.Descendants("span")
            .Where(c => (c.Attributes["itemprop"] != null) && (c.Attributes["itemprop"].Value == "description"))
            .SelectMany(c => c.Descendants());


            foreach (var d in query)
            {

                retorna.Add(d.InnerHtml);

            }

            return retorna;

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
