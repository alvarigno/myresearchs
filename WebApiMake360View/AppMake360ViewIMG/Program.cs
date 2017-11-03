using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UpLoadFile;

namespace AppMake360ViewIMG
{
    public class Program
    {

        public static string filename = "prova_v1.jpg";
        public static string urlsource = "C:\\Users\\Álvaro\\Desktop\\gear360pano-master\\";
        public static void Main(string[] args)
        {

            //Console.WriteLine("Genera img 360view");
            //
            //while (ProcesaVista360(filename) == false) {
            //
            //    Console.WriteLine("Procesando...");
            //
            //}
            //
            //Console.ReadKey();

        }

        public string ProcesaVista360(string filename, string placeapp) {

            string resultado = "";

            while (resultado == "") { 

                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.StandardInput.WriteLine("cd "+placeapp);
                process.StandardInput.WriteLine(@".\gear360pano.cmd " + filename);
                process.StandardInput.Close();
                process.WaitForExit();

                string namefile = Path.GetFileNameWithoutExtension(filename);

                string curFile = placeapp + "data\\" + namefile + "_pano.jpg";

                if (File.Exists(curFile))
                {
                    Console.WriteLine("\nimagen 360 creada con éxito.");
                    CA_ImgUploadServer subearchivo = new CA_ImgUploadServer();
                    resultado = subearchivo.Uploadimage(curFile);
                }

            }

            return resultado;

        }


       

    }
}
