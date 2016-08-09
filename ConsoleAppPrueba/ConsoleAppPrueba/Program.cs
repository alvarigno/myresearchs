using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleAppPrueba
{
    class Program
    {
        public static string mailheader;
        public static string filename = @"C:\Users\Álvaro\Desktop\Nueva carpeta (3)\mails_pruebas_27072016\prueba_format_UTF8_v4038.eml";
        //public static string filename = @"C:\Users\Álvaro\Desktop\Nueva carpeta (3)\mails_pruebas_27072016\prueba_format_UTF8_v5037.eml";

        static void Main(string[] args)
        {

            var mimeMessage = MimeMessage.Load(filename);

            var header = mimeMessage.Headers;

            var mailto = mimeMessage.To;
            var mailfrom = mimeMessage.From;
            var mailcc = mimeMessage.Cc;
            var mailsubject = mimeMessage.Subject;
            var maildate = mimeMessage.Date;
            var mailplainbody = mimeMessage.TextBody;
            //var mailplainbody = ""; //probar si sólo viene html en mensaje
            var mailhtmlbody = mimeMessage.HtmlBody;


            for (int i = 0; i < header.Count; i++) {

                string datoslocales;
                datoslocales = header[i].ToString()+ "\r\n";
                mailheader = mailheader + datoslocales;

            }

            if (string.IsNullOrEmpty(mailplainbody)) {

                mailplainbody = "Texto sin HTML: " + GetPlainTextFromHtml(mimeMessage.HtmlBody.ToString());

            }

            Console.WriteLine(
                "Muestra Resultado: \n"
                +"\n Header: "+ mailheader
                +"\n Asunto: "+ mailsubject
                +"\n Fcha: "+ maildate
                +"\n To: "+ mailto
                +"\n From: "+ mailfrom
                +"\n Cc: "+ mailcc
                +"\n Body Text: "+ mailplainbody
                +"\n Body Html: "+ mailhtmlbody
                );


            Console.Read();

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

    }
}
