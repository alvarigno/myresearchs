using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppMake360ViewIMG
{
    class Program
    {

        public static string filename = "prova_v1.jpg";
        public static string urlsource = "C:\\Users\\Álvaro\\Desktop\\gear360pano-master\\";
        static void Main(string[] args)
        {

            Console.WriteLine("Genera img 360view");

            while (ProcesaVista360(filename) == false) {

                Console.WriteLine("Procesando...");

            }

            Console.ReadKey();

        }

        public static Boolean ProcesaVista360(string filename) {

            Boolean resultado = false;
            
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.StandardInput.WriteLine("cd C:\\Users\\Álvaro\\Desktop\\gear360pano-master\\");
            process.StandardInput.WriteLine(@".\gear360pano.cmd " + urlsource + filename);
            process.StandardInput.Close();
            process.WaitForExit();

            string namefile = Path.GetFileNameWithoutExtension(urlsource+filename);

            string curFile = urlsource + "data\\" + namefile + "_pano.jpg";

            if (File.Exists(curFile))
            {

                Console.WriteLine("\nimagen 360 creada con éxito.");
                resultado = UpLoadFile(curFile);
                
            }
            
            return resultado;

        }


        public static Boolean UpLoadFile(string fileName) {

            Boolean resultado = false;

            WebClient myWebClient = new WebClient();

            byte[] responseArray = myWebClient.UploadFile("https://ws.chileautos.cl/api-cla/Upload/auto/particular", fileName);
            
            if (responseArray != null && responseArray.Length > 0)
            {

                String prova = "";

                prova =  System.Text.Encoding.ASCII.GetString(responseArray);
                
                var test = JObject.Parse(prova);
                JArray items = (JArray)test["images"];
                string file = items.ToString().Replace("[","").Replace("]","").Replace("\n","").Replace("\r","").Replace(" ","").Replace("\"", "");
                int length = items.Count;
                Console.WriteLine("\nimagen a cargada en servidor con éxito. Url es: " + file);
                resultado = true;

            }

            return resultado;
        }

    }
}
