using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MimeKit_ConsoleApp
{
    class Program
    {
        public static string mailheader;
        public static string filename = @"C:\Users\wizer\Desktop\emails\prueba_upload_fuera_v1.eml";

        static void Main(string[] args)
        {
            //var message = MimeMessage.Load(@"C:\Users\wizer\Desktop\email_prueba.txt");
            //var asunto = message.BodyParts;

            //Console.WriteLine("Muestra: "+ message);
            //Console.WriteLine("Asunto: "+asunto);

            var mimeMessage = MimeMessage.Load(filename);
            var header = mimeMessage.Headers;
            
            var mailto = mimeMessage.To;
            var mailfrom = mimeMessage.From;
            var mailcc = mimeMessage.Cc;
            var mailsubject = mimeMessage.Subject;
            var maildate = mimeMessage.Date;
            var mailplainBody = mimeMessage.TextBody;
            var mailhtmlBody = mimeMessage.HtmlBody;


            for (int i = 0; i < header.Count; i++) {
                string prueba;
                prueba = header[i].ToString() + "\r\n";
                mailheader = mailheader + prueba;

            }
            //Console.WriteLine("Header: " + mio);

            Console.WriteLine("Muestra 1 forma: \n "
                + "\n header: "+ mailheader
                + "\n asunto: " + mailsubject
                + "\n Fecha: " + maildate
                + "\n To: " + mailto 
                + "\n From: " + mailfrom
                + "\n Cc: " + mailcc
                + "\n Body Text: " + mailplainBody
                + "\n Body Html: " + mailhtmlBody);



            var headerList = HeaderList.Load(filename);

            var asunto = headerList[HeaderId.From];
            

            Console.WriteLine("Muestra 2: "+ asunto +"\n To: "+mailto);

            Console.Read();

        }
    }
}
