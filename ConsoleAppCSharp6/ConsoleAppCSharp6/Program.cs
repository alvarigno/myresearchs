using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCSharp6
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Prova C-Sharp 6");
            int uno = 11;
            int dos = 2;
            string entrada = "Ciao";
            Console.WriteLine(GetDataresult(uno, dos, entrada));
            Console.ReadLine();

        }

        static string GetDataresult(int uno, int dos, string valorstrintin) {

            string valorstring = valorstrintin;
            double valoruno = uno;
            double valordos = dos;

            if (string.IsNullOrEmpty(valorstring)) {

                valoruno = valoruno + 2;

            }

            return $" Il dato che sei numeri di resultati {valoruno / valordos} e {valoruno * valordos} = dati originale di entrate {valoruno} / {valordos}";
        }

    }
}
