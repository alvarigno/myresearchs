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

            if (revisaxkey(main))
            {

                List<PublicacionModel> query = ExtraeDataXml(main);
                MuestraRegistros(query);

            }
            else {

                Console.WriteLine("X-Key no valido");

            }

            Console.ReadLine();

        }

        public static Boolean revisaxkey(XDocument main) {

            Boolean revision = true;

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

            return revision;

        }


        public static List<PublicacionModel> ExtraeDataXml(XDocument main) {

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
                            equi = t.Descendants("equipamiento").Elements().Select(q => new { q.Name, q.Value }),
                            img = from im in t.Descendants("fotos").Elements("image")
                                  select new
                                  {
                                      source = im.Attribute("source").Value,
                                      name = im.Value
                                  }

                        };

            foreach (var item in query)
            {
                List<fotos> listimagenes = new List<fotos>();

                PublicacionModel dpublicacion = new PublicacionModel();
                datosVehiculo dVehiculo = new datosVehiculo();
                datosEquipamiento dEquipamiento = new datosEquipamiento();

                dpublicacion.idfuente = int.Parse(item.idfuente);
                dVehiculo.patente = item.patente;
                dpublicacion.revision = item.revision;
                dpublicacion.codCliente = int.Parse(item.sucursal);
                dVehiculo.tipo = item.tipo;
                dVehiculo.txtmarca = item.txmarca;
                dVehiculo.marca = int.Parse(item.marca);
                dVehiculo.modelo = item.modelo;
                dVehiculo.version = item.version;
                dVehiculo.carroceria = item.carroceria;
                dVehiculo.ano = int.Parse(item.ano);
                dVehiculo.precio = int.Parse(item.precio);
                dVehiculo.color = item.color;
                dVehiculo.kilom = int.Parse(item.km);
                dVehiculo.motor = item.motor;
                dVehiculo.potencia = item.potencia;
                dVehiculo.combustible = int.Parse(item.combustible);
                dVehiculo.cilindrada = int.Parse(item.cilindrada);
                dVehiculo.tipoDireccion = item.tipodireccion;
                dVehiculo.techo = item.techo;
                dVehiculo.puertas = int.Parse(item.puertas);
                dVehiculo.comentario = item.comentarios;

                foreach (var list in item.equi)
                {

                    if (list.Name == "airbag")
                    {
                        dEquipamiento.airbag = list.Value;
                    }

                    if (list.Name == "aireacondicionado")
                    {
                        dEquipamiento.aireAcon = list.Value;
                    }

                    if (list.Name == "alarma")
                    {
                        dEquipamiento.alarma = list.Value;
                    }

                    if (list.Name == "alzavidrios")
                    {
                        dEquipamiento.alzaVidrios = list.Value;
                    }

                    if (list.Name == "catalitico")
                    {
                        dEquipamiento.catalitico = list.Value;
                    }

                    if (list.Name == "cierrecentralizado")
                    {
                        dEquipamiento.cierreCentral = list.Value;
                    }

                    if (list.Name == "espejoselectricos")
                    {
                        dEquipamiento.espejos = list.Value;
                    }

                    if (list.Name == "frenosabs")
                    {
                        dEquipamiento.frenosAbs = list.Value;
                    }

                    if (list.Name == "fwd")
                    {
                        dEquipamiento.fwd = list.Value;

                    }

                    if (list.Name == "llantas")
                    {
                        dEquipamiento.llantas = list.Value;
                    }

                    if (list.Name == "nuevo")
                    {
                        dEquipamiento.nuevo = list.Value;
                    }

                    if (list.Name == "radio")
                    {
                        dEquipamiento.radio = list.Value;
                    }

                    if (list.Name == "transmision")
                    {
                        dEquipamiento.transmision = list.Value;
                    }

                    if (list.Name == "unicodueno")
                    {
                        dEquipamiento.unicoDueno = list.Value;
                    }


                }

                foreach (var list in item.img)
                {

                    fotos datalocal = new fotos();

                    datalocal.url = list.source;
                    datalocal.name = list.name;

                    listimagenes.Add(datalocal);

                }

                count = count + 1;

                datavehiculo.plataforma = "SIR"; //Servicio de integración remota.
                dpublicacion.dVehiculo = dVehiculo;
                dpublicacion.dEquipamiento = dEquipamiento;
                dpublicacion.dVehiculo.listadofotos = listimagenes.ToArray();

                listado.Add(dpublicacion);

            }

            return listado;

        }

        public static void MuestraRegistros(List<PublicacionModel> listado) {

            Console.WriteLine("Muestra Objeto");
            int count3 = 1;
            foreach (var data in listado)
            {


                Console.WriteLine("\n registro Nº: " + count3);
                Console.WriteLine("----------------------------------------------------------------------------------------------------");
                Console.WriteLine("  Cod. Cliente (Sucursal): " + data.codCliente + ", Patente: " + data.dVehiculo.patente + ", ID registro automotora: " + data.idfuente + ", ip: " + data.ip + ", Revisión Diccionario: " + data.revision);
                Console.WriteLine("\n >Info Vehículo< ");
                Console.WriteLine("  Plataforma (procedencia): " + data.dVehiculo.plataforma + " (Servicio de Integración Remota)");
                Console.WriteLine("  Año: " + data.dVehiculo.ano);
                Console.WriteLine("  Carrocería: " + data.dVehiculo.carroceria);
                Console.WriteLine("  Cilindrada: " + data.dVehiculo.cilindrada);
                Console.WriteLine("  Color: " + data.dVehiculo.color);
                Console.WriteLine("  Combustible: " + data.dVehiculo.combustible);
                Console.WriteLine("  Comentario: " + data.dVehiculo.comentario);
                Console.WriteLine("  Kilometraje: " + data.dVehiculo.kilom);
                Console.WriteLine("  Cod. Marca: " + data.dVehiculo.marca);
                Console.WriteLine("  Marca: " + data.dVehiculo.txtmarca);
                Console.WriteLine("  Modelo: " + data.dVehiculo.modelo);
                Console.WriteLine("  Version: " + data.dVehiculo.version);
                Console.WriteLine("  Motor: " + data.dVehiculo.motor);
                Console.WriteLine("  Patente: " + data.dVehiculo.patente);
                Console.WriteLine("  Potencia: " + data.dVehiculo.potencia);
                Console.WriteLine("  Precio: " + data.dVehiculo.precio);
                Console.WriteLine("  Puertas: " + data.dVehiculo.puertas);
                Console.WriteLine("  Techo: " + data.dVehiculo.techo);
                Console.WriteLine("  Tipo: " + data.dVehiculo.tipo);
                Console.WriteLine("  Tipo Dirección: " + data.dVehiculo.tipoDireccion);
                Console.WriteLine("  uid Jato: " + data.dVehiculo.uidJato);

                Console.WriteLine("\n >listado Equipamiento< ");
                Console.WriteLine("  AirBag: " + data.dEquipamiento.airbag);
                Console.WriteLine("  Aire Acondicionado: " + data.dEquipamiento.aireAcon);
                Console.WriteLine("  Alarma: " + data.dEquipamiento.alarma);
                Console.WriteLine("  Alza Vidrios: " + data.dEquipamiento.alzaVidrios);
                Console.WriteLine("  Catalítico: " + data.dEquipamiento.catalitico);
                Console.WriteLine("  Cierre Centralizado: " + data.dEquipamiento.cierreCentral);
                Console.WriteLine("  Espejos Eléctricos: " + data.dEquipamiento.espejos);
                Console.WriteLine("  Frenos ABS: " + data.dEquipamiento.frenosAbs);
                Console.WriteLine("  Fwd: " + data.dEquipamiento.fwd);
                Console.WriteLine("  Llantas: " + data.dEquipamiento.llantas);
                Console.WriteLine("  Nuevo: " + data.dEquipamiento.nuevo);
                Console.WriteLine("  Radio: " + data.dEquipamiento.radio);
                Console.WriteLine("  Transmisión: " + data.dEquipamiento.transmision);
                Console.WriteLine("  Unico Dueño: " + data.dEquipamiento.unicoDueno);

                Console.WriteLine("\n >listado imagenes<");

              foreach (var image in data.dVehiculo.listadofotos)
              {

                  Console.WriteLine("  surce: " + image.url + ", nombre: " + image.name);

              }
                Console.WriteLine("---------------------------------------------------------------------------------------\n");
                count3 = count3 + 1;
            }

        }

    }
}
