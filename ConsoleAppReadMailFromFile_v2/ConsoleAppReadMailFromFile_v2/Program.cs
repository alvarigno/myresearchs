using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppReadMailFromFile_v2
{
    public class Program
    {
        

        public static void Main()
        {
            
            StreamReader reader = File.OpenText(@"C:\mails_prueba\1467129628.H461329P12217.cp01.chileautos.cl,S=4336_2,S");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split('\t');
                //int myInteger = int.Parse(items[1]); // Here's your integer.
                // Now let's find the path.

                cargamail(line);

               string path;
                foreach (string item in items)
                {   
                    if (item.StartsWith("From:") )
                    {
                        path = item;
                        Console.WriteLine("Hola"+ path);
                    
                    }

                    if (item.StartsWith("Date:"))
                    {
                        path = item;
                        Console.WriteLine("Hola" + path);

                    }

                    if (item.StartsWith("Subject:"))
                    {
                        path = item;
                        Console.WriteLine("Hola" + path);

                    }

                    if (item.StartsWith("To:"))
                    {
                        path = item;
                        Console.WriteLine("Hola" + path);

                    }

                    //if (item.StartsWith("From:\\") && item.EndsWith(".ddj"))
                    //{
                    //    path = item;
                    //}
                }

                // At this point, `myInteger` and `path` contain the values we want
                // for the current line. We can then store those values or print them,
                // or anything else we like.
            }
            
            Console.ReadLine();

        }

        public static string[] GetStringInBetween(string strBegin,
            string strEnd, string strSource,
            bool includeBegin, bool includeEnd)
        {
            string[] result = { "", "" };
            int iIndexOfBegin = strSource.IndexOf(strBegin);
            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;
                strSource = strSource.Substring(iIndexOfBegin
                    + strBegin.Length);
                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd
                            + strEnd.Length);
                }
            }
            else
                // stay where we are
                result[1] = strSource;
            return result;
        }


        public static void cargamail(string line) {

            MailAddress mio = new MailAddress(line);


    }


}
}
