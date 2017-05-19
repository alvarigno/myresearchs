using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ReadXMLFile.Models;

namespace ReadXMLFile
{
    class Program
    {
        string img = "";
        static int count = 1;
        public List<string> imagenes = new List<string>();
        

        static void Main(string[] args)
        {

            XDocument main = XDocument.Load(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)+"\\xml\\base.xml");

            Console.WriteLine("Revisión de Avisos");

            var identificacion = main.Descendants("publicacion")
                .Descendants("identificacion")
                .Select(e => new {
                    xkey = e.Descendants("x-key").FirstOrDefault().Value
                });

            foreach (var result in identificacion)
            {
                Console.WriteLine(" x-Key de validación: {0}", result.xkey);
            }

            var query = from t in main.Descendants("aviso")
                        select new
                        {
                            idfuente = t.Attribute("id").Value,
                            patente = t.Attribute("patente").Value,
                            revision = t.Attribute("rdiccionario").Value,
                            sucursal = t.Element("vehiculo").Descendants("sucursal").Attributes("id").FirstOrDefault().Value,
                            tipo = t.Element("vehiculo").Descendants("tipo").Attributes("id").FirstOrDefault().Value,
                            txmarca = t.Element("vehiculo").Descendants("marca").FirstOrDefault().Value,
                            marca = t.Element("vehiculo").Descendants("marca").Attributes("id").FirstOrDefault().Value,
                            modelo = t.Element("vehiculo").Descendants("marca").FirstOrDefault().Value,
                            version = t.Element("vehiculo").Descendants("version").FirstOrDefault().Value,
                            carroceria = t.Element("vehiculo").Descendants("carroceria").Attributes("id").FirstOrDefault().Value,
                            ano = t.Element("vehiculo").Descendants("ano").FirstOrDefault().Value,
                            precio = t.Element("vehiculo").Descendants("precio").FirstOrDefault().Value,
                            color = t.Element("vehiculo").Descendants("color").FirstOrDefault().Value,
                            km = t.Element("vehiculo").Descendants("km").FirstOrDefault().Value,
                            motor = t.Element("vehiculo").Descendants("motor").FirstOrDefault().Value,
                            potencia = t.Element("vehiculo").Descendants("potencia").FirstOrDefault().Value,
                            combustible = t.Element("vehiculo").Descendants("combustible").Attributes("id").FirstOrDefault().Value,
                            cilindrada = t.Element("vehiculo").Descendants("cilindrada").FirstOrDefault().Value,
                            tipodireccion = t.Element("vehiculo").Descendants("tipodireccion").Attributes("id").FirstOrDefault().Value,
                            techo = t.Element("vehiculo").Descendants("techo").Attributes("id").FirstOrDefault().Value,
                            puertas = t.Element("vehiculo").Descendants("puertas").FirstOrDefault().Value,
                            comentarios = t.Element("vehiculo").Descendants("comentarios").FirstOrDefault().Value,
                            equi = t.Descendants("equipamiento").Elements().Select( q => new { q.Name, q.Value }),
                            img = from im in t.Descendants("fotos").Elements("image") select new { source = im.Attribute("source").Value, name = im.Value }

                        };


            foreach (var item in query)
            {
                Console.WriteLine("\n> Aviso número: "+ count);
                Console.WriteLine("> id veh automotora: {0}, " +
                    "patente: {1}, " +
                    "revisión diccionario: {2}" +
                    "\n---------------------------------------------------------------------------------------\n" +
                    "sucursal: {3} \n" +
                    "tipo: {4} \n" +
                    "marca: {5} \n" +
                    "cod Marca: {6} \n" +
                    "modelo: {7} \n" +
                    "version: {8} \n" +
                    "carroceria: {9} \n" +
                    "ano: {10} \n" +
                    "precio: {11} \n" +
                    "color: {12} \n" +
                    "km: {13} \n" +
                    "motor: {14} \n" +
                    "potencia: {15} \n" +
                    "combustible: {16} \n" +
                    "cilindrada: {17} \n" +
                    "tipodireccion: {18} \n" +
                    "techo: {19} \n" +
                    "puertas: {20} \n" +
                    "comentarios: {21}",
                    item.idfuente, item.patente, item.revision, item.sucursal, item.tipo, item.txmarca, item.marca, item.modelo, item.version, item.carroceria, item.ano, item.precio, item.color, item.km, item.motor, item.potencia, item.combustible, item.cilindrada, item.tipodireccion, item.techo, item.puertas, item.comentarios);
                Console.WriteLine("\n>Equipamiento:<");
                foreach (var list in item.equi) { Console.WriteLine("nombre: {0}, valor: {1}", list.Name, list.Value); }
                Console.WriteLine("\n>Imágenes:<");
                foreach (var list in item.img) { Console.WriteLine("img: {0}, name: {1}", list.source, list.name); }

                count = count + 1;
            }

            Console.ReadLine();

        }
    }
}
