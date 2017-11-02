using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMake360ViewIMG
{
    class Program
    {

        public static string urlsource = "C:\\Users\\Álvaro\\Desktop\\gear360pano-master\\prova_v1.jpg";
        static void Main(string[] args)
        {

            Console.WriteLine("Genera img 360view");

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.StandardInput.WriteLine("cd C:\\Users\\Álvaro\\Desktop\\gear360pano-master\\");
            process.StandardInput.WriteLine(@".\gear360pano.cmd "+ urlsource);
            process.StandardInput.Close();
            process.WaitForExit();
            //Console.WriteLine(process.StandardOutput.ReadToEnd());



            //while (!process.StandardOutput.EndOfStream)
            //{
            //    string line = process.StandardOutput.ReadLine();
            //    Console.WriteLine("salida: "+ line);
            //}

            string namefile = Path.GetFileNameWithoutExtension(urlsource);

            //string curFile = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\html\\data\\prova_v1_pano.jpg";
            string curFile = "C:\\Users\\Álvaro\\Desktop\\gear360pano-master\\html\\data\\"+ namefile + "_pano.jpg";

            if (File.Exists(curFile))
            {

                Console.WriteLine("imagen si está creada");

            }


            Console.ReadKey();


        }
    }
}
