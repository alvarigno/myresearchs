using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CountNodeAvisosXML
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cuenta de nodos, comienza");


            XDocument main = XDocument.Load(@"C:\Chileautos_Desarrollo\desarrollo\integrador\pruebas\Derco\05122017\xml_20171205200735.xml");

            Console.WriteLine("Cantidad de nodos: "+ ExtraeDataXml(main));
            
            Console.ReadKey();

        }

        public static int ExtraeDataXml(XDocument main)
        {

            int revision = 0;

            if (main.Descendants("aviso").Count() > 0) {

                revision = main.Descendants("aviso").Count();
            }

            return revision;

        }

    }
}
