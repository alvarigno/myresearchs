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
        static int count2array = 0;
        public List<string> imagenes = new List<string>();
        static PublicacionModel datapublicacion = new PublicacionModel();
        static datosVehiculo datavehiculo = new datosVehiculo();
        static datosEquipamiento dataequipamiento = new datosEquipamiento();
        static fotos datafotos = new fotos();
        
        static List<PublicacionModel> listado = new List<PublicacionModel>();

        static void Main(string[] args)
        {

            XDocument main = XDocument.Load(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)+"\\xml\\base.xml");

            Console.WriteLine("Revisión de Avisos");

            //identifica el x-key
            var identificacion = main.Descendants("publicacion")
                .Descendants("identificacion")
                .Select(e => new {
                    xkey = e.Descendants("x-key").FirstOrDefault().Value
                });

            //muestra el x-key
            foreach (var result in identificacion)
            {
                Console.WriteLine(" x-Key de validación: {0}", result.xkey);
            }


            //recuepra toda la información de cada nodo del XML que está procesando.
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
                            modelo = t.Element("vehiculo").Descendants("modelo").FirstOrDefault().Value,
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
                            img = from im in t.Descendants("fotos").Elements("image")
                                  select new {
                                      source = im.Attribute("source").Value, name = im.Value
                                  }

                        };

            //Muestra cada nodo procesado en la consulta LINQ
            foreach (var item in query)
            {
                List<fotos> listimagenes = new List<fotos>();
                Console.WriteLine("\n> Aviso número: "+ count);
                Console.WriteLine("> id veh automotora: {0}, " +
                    "patente: {1}, " +
                    "revisión diccionario: {2}" +
                    "\n---------------------------------------------------------------------------------------\n" +
                    " sucursal: {3} \n" +
                    " tipo: {4} \n" +
                    " marca: {5} \n" +
                    " cod Marca: {6} \n" +
                    " modelo: {7} \n" +
                    " version: {8} \n" +
                    " carroceria: {9} \n" +
                    " ano: {10} \n" +
                    " precio: {11} \n" +
                    " color: {12} \n" +
                    " km: {13} \n" +
                    " motor: {14} \n" +
                    " potencia: {15} \n" +
                    " combustible: {16} \n" +
                    " cilindrada: {17} \n" +
                    " tipodireccion: {18} \n" +
                    " techo: {19} \n" +
                    " puertas: {20} \n" +
                    " comentarios: {21}",
                    datapublicacion.idfuente = int.Parse(item.idfuente), 
                    datavehiculo.patente = item.patente,
                    datapublicacion.revision = item.revision, 
                    datapublicacion.codCliente = int.Parse(item.sucursal), 
                    datavehiculo.tipo = item.tipo, 
                    datavehiculo.txtmarca = item.txmarca, 
                    datavehiculo.marca = int.Parse(item.marca), 
                    datavehiculo.modelo = item.modelo, 
                    datavehiculo.version = item.version, 
                    datavehiculo.carroceria = item.carroceria, 
                    datavehiculo.ano = int.Parse(item.ano), 
                    datavehiculo.precio = int.Parse(item.precio), 
                    datavehiculo.color = item.color,
                    datavehiculo.kilom = int.Parse(item.km), 
                    datavehiculo.motor = item.motor, 
                    datavehiculo.potencia = item.potencia, 
                    datavehiculo.combustible = int.Parse(item.combustible), 
                    datavehiculo.cilindrada = int.Parse(item.cilindrada), 
                    datavehiculo.tipoDireccion = item.tipodireccion, 
                    datavehiculo.techo = item.techo, 
                    datavehiculo.puertas = int.Parse(item.puertas), 
                    datavehiculo.comentario = item.comentarios
                    );

                Console.WriteLine("\n>Equipamiento:<");
                foreach (var list in item.equi) {

                    if (list.Name == "airbag")
                    {

                        dataequipamiento.airbag = list.Value;
                        Console.WriteLine(
                            " airbag: " + dataequipamiento.airbag);

                    }

                    if (list.Name == "aireacondicionado")
                    {

                        dataequipamiento.aireAcon = list.Value;
                        Console.WriteLine(
                            " Aire Acondicionado: " + dataequipamiento.aireAcon);

                    }

                    if (list.Name == "alarma")
                    {

                        dataequipamiento.alarma = list.Value;
                        Console.WriteLine(
                            " Alarma: " + dataequipamiento.alarma );

                    }

                    if (list.Name == "alzavidrios")
                    {

                        dataequipamiento.alzaVidrios = list.Value;
                        Console.WriteLine(
                             " Alza Vidrios Eléctrico: " + dataequipamiento.alzaVidrios
                            );
                    }

                    if (list.Name == "catalitico")
                    {

                        dataequipamiento.catalitico = list.Value;
                        Console.WriteLine(
                             " Catalítico: " + dataequipamiento.catalitico
                             );

                    }

                    if (list.Name == "cierrecentralizado")
                    {

                        dataequipamiento.cierreCentral = list.Value;
                        Console.WriteLine(
                             " Cierre Centralizado: " + dataequipamiento.cierreCentral
                            );

                    }

                    if (list.Name == "espejoselectricos")
                    {

                        dataequipamiento.espejos = list.Value;
                        Console.WriteLine(
                            " Espejos Eléctrico: " + dataequipamiento.espejos
                            );

                    }

                    if (list.Name == "frenosabs")
                    {

                        dataequipamiento.frenosAbs = list.Value;
                        Console.WriteLine(
                            " Frenos ABS: " + dataequipamiento.frenosAbs);

                    }

                    if (list.Name == "fwd")
                    {

                        dataequipamiento.fwd = list.Value;
                        Console.WriteLine(
                            " Fwd: " + dataequipamiento.fwd );

                    }

                    if (list.Name == "llantas")
                    {

                        dataequipamiento.llantas = list.Value;
                        Console.WriteLine(
                             " Llantas: " + dataequipamiento.llantas
                            );

                    }

                    if (list.Name == "nuevo")
                    {

                        dataequipamiento.nuevo = list.Value;
                        Console.WriteLine(
                            " Nuevo: " + dataequipamiento.nuevo
                            );

                    }

                    if (list.Name == "radio")
                    {

                        dataequipamiento.radio = list.Value;
                        Console.WriteLine(
                            " Radio: " + dataequipamiento.radio
                            );

                    }

                    if (list.Name == "transmision")
                    {

                        dataequipamiento.transmision = list.Value;
                        Console.WriteLine(
                            " Transmisión: " + dataequipamiento.transmision
                            );

                    }

                    if (list.Name == "unicodueno")
                    {

                        dataequipamiento.unicoDueno = list.Value;
                        Console.WriteLine(
                            " Único Dueño: " + dataequipamiento.unicoDueno
                            );

                    }


                }
                Console.WriteLine("\n>Imágenes:<");
                count2array = 0;
                foreach (var list in item.img) {
                    
                    fotos datalocal = new fotos();

                    datalocal.url = list.source;
                    datalocal.name = list.name;

                    listimagenes.Add(datalocal);

                    //datavehiculo.listadofotos = listimagenes.ToArray();

                    //Console.WriteLine("img: {0}, name: {1}", list.source, list.name);

                    Console.WriteLine(" Nº"+(count2array+1)+" URL: "+ listimagenes[count2array].url);

                    count2array = count2array + 1;

                }

                count = count + 1;

                datavehiculo.plataforma = "SIR"; //Servicio de integración remota.
                datapublicacion.dVehiculo = datavehiculo;
                datapublicacion.dEquipamiento = dataequipamiento;
                datapublicacion.dVehiculo.listadofotos = listimagenes.ToArray();

                listado.Add(datapublicacion);

            }


            Console.WriteLine("Muestra Objeto");
            int count3 = 1;
            foreach (var data in listado) {


                Console.WriteLine("\n registro Nº: "+count3);
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("  Cod. Cliente (Sucursal): "+data.codCliente+", Patente: "+data.dVehiculo.patente+", ID registro automotora: "+data.idfuente+", ip: "+data.ip+", Revisión Diccionario: "+ data.revision);
                Console.WriteLine("\n >Info Vehículo< ");
                Console.WriteLine("  Plataforma (procedencia): "+data.dVehiculo.plataforma+" (Servicio de Integración Remota)");
                Console.WriteLine("  Año: "+data.dVehiculo.ano);
                Console.WriteLine("  Carrocería: "+data.dVehiculo.carroceria);
                Console.WriteLine("  Cilindrada: "+data.dVehiculo.cilindrada);
                Console.WriteLine("  Color: "+data.dVehiculo.color);
                Console.WriteLine("  Combustible: "+data.dVehiculo.combustible);
                Console.WriteLine("  Comentario: "+data.dVehiculo.comentario);
                Console.WriteLine("  Kilometraje: "+data.dVehiculo.kilom);
                Console.WriteLine("  Cod. Marca: "+data.dVehiculo.marca);
                Console.WriteLine("  Marca: "+data.dVehiculo.txtmarca);
                Console.WriteLine("  Modelo: "+data.dVehiculo.modelo);
                Console.WriteLine("  Version: "+data.dVehiculo.version);
                Console.WriteLine("  Motor: "+data.dVehiculo.motor);
                Console.WriteLine("  Patente: "+data.dVehiculo.patente);
                Console.WriteLine("  Potencia: "+data.dVehiculo.potencia);
                Console.WriteLine("  Precio: "+data.dVehiculo.precio);
                Console.WriteLine("  Puertas: "+data.dVehiculo.puertas);
                Console.WriteLine("  Techo: "+data.dVehiculo.techo);
                Console.WriteLine("  Tipo: "+data.dVehiculo.tipo);
                Console.WriteLine("  Tipo Dirección: "+data.dVehiculo.tipoDireccion);
                Console.WriteLine("  uid Jato: "+data.dVehiculo.uidJato);
                
                Console.WriteLine("\n >listado Equipamiento< ");
                Console.WriteLine("  AirBag: "+data.dEquipamiento.airbag);
                Console.WriteLine("  Aire Acondicionado: "+data.dEquipamiento.aireAcon);
                Console.WriteLine("  Alarma: "+data.dEquipamiento.alarma);
                Console.WriteLine("  Alza Vidrios: "+data.dEquipamiento.alzaVidrios);
                Console.WriteLine("  Catalítico: "+data.dEquipamiento.catalitico);
                Console.WriteLine("  Cierre Centralizado: "+data.dEquipamiento.cierreCentral);
                Console.WriteLine("  Espejos Eléctricos: "+data.dEquipamiento.espejos);
                Console.WriteLine("  Frenos ABS: "+data.dEquipamiento.frenosAbs);
                Console.WriteLine("  Fwd: "+data.dEquipamiento.fwd);
                Console.WriteLine("  Llantas: "+data.dEquipamiento.llantas);
                Console.WriteLine("  Nuevo: "+data.dEquipamiento.nuevo);
                Console.WriteLine("  Radio: "+data.dEquipamiento.radio);
                Console.WriteLine("  Transmisión: "+data.dEquipamiento.transmision);
                Console.WriteLine("  Unico Dueño: "+data.dEquipamiento.unicoDueno);

                Console.WriteLine("\n >listado imagenes<");

                foreach (var image in datavehiculo.listadofotos) {

                    Console.WriteLine("  surce: "+image.url+", nombre: "+image.name);

                }
                Console.WriteLine("---------------------------------------------------------------------------------------\n");
                count3 = count3 + 1;
            }



            Console.ReadLine();




        }
    }
}
