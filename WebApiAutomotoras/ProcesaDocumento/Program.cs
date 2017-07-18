using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using AccesoDatos.Data;
using ProcesaDocumento.Models;
using System.Net;

namespace ProcesaDocumento
{
    public class Program
    {

        static int count = 1;
        public List<string> imagenes = new List<string>();
        static PublicacionModel datapublicacion = new PublicacionModel();
        static datosVehiculo datavehiculo = new datosVehiculo();
        static datosEquipamiento dataequipamiento = new datosEquipamiento();
        static fotos datafotos = new fotos();

        static List<PublicacionModel> listado = new List<PublicacionModel>();

        public void ObtieneDocumentoXml(string rutaxml) {

            XDocument main = XDocument.Load(rutaxml);
            if (revisaxkey(main)) {

                ExtraeDataXml(main);

            }

        }

        public static Boolean revisaxkey(XDocument main)
        {

            Boolean revision = false;

            //identifica el x-key
            var identificacion = main.Descendants("publicacion")
                .Descendants("identificacion")
                .Select(e => new {
                    xkey = e.Descendants("x-key").FirstOrDefault().Value,
                    name = e.Attributes("name").FirstOrDefault().Value,
                    fechacreacion = e.Attributes("fecha-creacion").FirstOrDefault().Value,
                });

            //muestra el x-key
            foreach (var result in identificacion)
            {

                baseprod2Entities database = new baseprod2Entities();
                string[] verifica = AccesoDatos.Program.VerificaXkey(result.xkey);

                if (Boolean.Parse(verifica[0]))
                {

                    revision = true;

                }

            }
            
            return revision;

        }
        
        public static List<PublicacionModel> ExtraeDataXml(XDocument main)
        {
            listado.Clear();
            //recuepra toda la información de cada nodo del XML que está procesando.
            var query = from t in main.Descendants("aviso")
                        select new
                        {
                            idfuente = t.Attribute("id").Value,
                            patente = t.Attribute("patente").Value,
                            revision = t.Attribute("rdiccionario").Value,
                            sucursal = t.Element("vehiculo").Descendants("sucursal").Attributes("id").FirstOrDefault().Value,
                            categoria = t.Element("vehiculo").Descendants("categoria").Attributes("id").FirstOrDefault().Value,
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
                dVehiculo.categoria = int.Parse(item.categoria);
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
                    string pathfile = System.IO.Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded", dpublicacion.idfuente.ToString());
                   
                    fotos datalocal = new fotos();

                    datalocal.url = list.source;

                    if (!Directory.Exists(pathfile))
                    {
                        pathfile = CrearDirectorioImagenes(dpublicacion.idfuente.ToString());
                    }

                    datalocal.url = DescargaImagen(list.source, pathfile);
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

        private static string CrearDirectorioImagenes(string namefolder)
        {

            string pathfile = "";
            string directorio = "";
            if (!String.IsNullOrEmpty(namefolder))
            {
                directorio = namefolder;
            }
            else
            {
                directorio = "defecto";
            }

            pathfile = System.IO.Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded", directorio);
            System.IO.Directory.CreateDirectory(pathfile);

            return pathfile;
        }

        private static string DescargaImagen(string urlfilename, string namefolder)
        {
            string pathimagen = "";

            try
            {

                string pathfile = System.IO.Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded", namefolder);
                System.IO.Directory.CreateDirectory(pathfile);

                string filename = "";
                Uri uri = new Uri(urlfilename);

                filename = System.IO.Path.GetFileName(uri.Segments.Last(seg => seg.Contains(".")));

                using (WebClient webClient = new WebClient())
                {

                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate (object sender, DownloadProgressChangedEventArgs e)
                    {
                        Console.WriteLine("Downloaded:" + e.ProgressPercentage.ToString());
                    });

                    webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler
                        (delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                        {
                            if (e.Error == null && !e.Cancelled)
                            {
                                Console.WriteLine("Download completed!");

                            }
                        });
                    webClient.DownloadFile(new System.Uri(urlfilename), pathfile + "\\" + filename);
                    pathimagen = pathfile + "\\" + filename;
                }

            }
            catch (Exception e)
            {

                pathimagen = e.Message;

            }

            return pathimagen;
        }

    }
}