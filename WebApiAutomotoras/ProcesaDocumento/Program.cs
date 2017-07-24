using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using AccesoDatos.Data;
using ProcesaDocumento.Models;
using System.Net;
using SubidaImagenesServer;
using System.Data.Entity.Core.Objects;

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

        public void ObtieneDocumentoXml(string rutaxml, string iporigen) {

            string nombrearchivo = Path.GetFileName(rutaxml);
            XDocument main = XDocument.Load(rutaxml);
            if (revisaxkey(main)) {

                ExtraeDataXml(main, nombrearchivo, iporigen);
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
        
        public static List<PublicacionModel> ExtraeDataXml(XDocument main, string nombrearchivo, string iporigen)
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
                List<string> listimagenes = new List<string>();

                PublicacionModel dpublicacion = new PublicacionModel();
                datosVehiculo dVehiculo = new datosVehiculo();
                datosEquipamiento dEquipamiento = new datosEquipamiento();

                dpublicacion.codCliente = int.Parse(item.sucursal);
                dpublicacion.idfuente = item.idfuente;
                dpublicacion.ip = iporigen;
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

                    if (list.Name == "consignacion")
                    {
                        dEquipamiento.consignacion = list.Value;
                    }

                }


                if (ValidaDatos(dVehiculo.carroceria, dVehiculo.puertas, dEquipamiento.transmision, dVehiculo.combustible, dVehiculo.categoria ) == 5)
                { 

                    string pathfile = System.IO.Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded", dpublicacion.idfuente.ToString()+ "_"+ dpublicacion.codCliente);
                    
                    if (Directory.Exists(pathfile)) {

                        if (!ProcesaDirectorio(pathfile)) {

                            Directory.Delete(pathfile, true);

                        }
                        
                    }

                    foreach (var list in item.img)
                    {

                        fotos datalocal = new fotos();

                        datalocal.url = list.source;

                        //Creadirectorio de imágenes
                        pathfile = CrearDirectorioImagenes(dpublicacion.idfuente.ToString() + "_" + dpublicacion.codCliente);
                        
                        datalocal.url = DescargaImagen(list.source, pathfile);

                        //Carga imagen por imagen en servidor de Chileutos.
                        UpLoadImageCA UpLoadImg = new UpLoadImageCA();
                        datalocal.url = UpLoadImg.Uploadimage(datalocal.url);

                        listimagenes.Add(datalocal.url);
                    

                    }

                    count = count + 1;

                    dpublicacion.dVehiculo = dVehiculo;
                    dpublicacion.dEquipamiento = dEquipamiento;
                    dpublicacion.dVehiculo.listadofotos = listimagenes.ToArray();

                    string[] resultadosinsert = InsertaDatosParaPublicacion(dpublicacion, nombrearchivo);


                    if (!Boolean.Parse(resultadosinsert[0]))
                    {

                        resultadosinsert = ActualizaDatosParaPublicacion(dpublicacion, nombrearchivo, dpublicacion.idfuente);

                        if (Boolean.Parse(resultadosinsert[0]))
                        {
                            //Actualiza Registro con ID de Chileautos
                            PublicarChileautosModel datosparapublicar = Otienedata(int.Parse(resultadosinsert[1]));
                            datosparapublicar.ip = iporigen;
                            ///aquí quede//
                            string edicion = GetEdition(ObtieneMarcaxCategoria(datosparapublicar.categoria, datosparapublicar.marca), datosparapublicar.modelo, datosparapublicar.version, datosparapublicar.carroceria, datosparapublicar.puertas, datosparapublicar.ano, datosparapublicar.transmision);
                            datosparapublicar.uidJato = getCodigoJajoNoJato(ObtieneMarcaxCategoria(datosparapublicar.categoria, datosparapublicar.marca), datosparapublicar.modelo, datosparapublicar.version, datosparapublicar.carroceria, datosparapublicar.puertas, datosparapublicar.ano, datosparapublicar.transmision, datosparapublicar.combustible, edicion, datosparapublicar.categoria);
                            int idtabautos = ActualizaAviso(datosparapublicar);
                            ActualizaRegistroAutomotora(int.Parse(resultadosinsert[1]), idtabautos);

                        }

                    }
                    else {

                        
                        //Publica datos de vahículo, Retorna ID de Chileautos y Actualiza Registro con idCa y Fecha de Pubglicación.
                        PublicarChileautosModel datosparapublicar = Otienedata(int.Parse(resultadosinsert[1]));
                        datosparapublicar.ip = iporigen;
                        ///aquí quede//
                        string edicion = GetEdition(ObtieneMarcaxCategoria(datosparapublicar.categoria, datosparapublicar.marca), datosparapublicar.modelo, datosparapublicar.version, datosparapublicar.carroceria, datosparapublicar.puertas, datosparapublicar.ano, datosparapublicar.transmision);
                        datosparapublicar.uidJato = getCodigoJajoNoJato(ObtieneMarcaxCategoria(datosparapublicar.categoria, datosparapublicar.marca), datosparapublicar.modelo, datosparapublicar.version, datosparapublicar.carroceria, datosparapublicar.puertas, datosparapublicar.ano, datosparapublicar.transmision, datosparapublicar.combustible, edicion, datosparapublicar.categoria);
                        int idtabautos = PublicaAviso(datosparapublicar);
                        ActualizaRegistroAutomotora(int.Parse(resultadosinsert[1]),idtabautos);

                    }

                    listado.Add(dpublicacion);
                }

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
                        //Console.WriteLine("Downloaded:" + e.ProgressPercentage.ToString());
                    });

                    webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler
                        (delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                        {
                            if (e.Error == null && !e.Cancelled)
                            {
                                //Console.WriteLine("Download completed!");

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

        private static long getCodigoJajoNoJato(string marca, string modelo, string version, string carroceria, int puertas, int ano, string transmision, int combustible, string edicion, int categoria) {

            long uidJatorespuesta = 0;

            if (edicion == "")
            {

                edicion = "-";
            }

            if (transmision == "S")
            {

                transmision = "A";

            }
            else
            {

                transmision = "M";

            }

                bdToolsEntities bdTools = new bdToolsEntities();

                var uidJato = bdTools.bdj_idJato_SEL_marca_modelo_version_carroceria_ptas_ano_trans_ltl(marca, modelo, version, carroceria, puertas, ano, transmision, edicion).FirstOrDefault();

                if (uidJato == null)
                {
                    uidJato = bdTools.SP_bdj_getNonJatoID(categoria, marca, modelo, ano, carroceria, transmision, combustible.ToString()).FirstOrDefault();

                }

                uidJatorespuesta = (long)uidJato;


            return uidJatorespuesta;

        }

        private static string GetEdition(string marca, string modelo, string version, string carroceria, int puertas, int agno, string trans) {

            string edition = "";

            bdToolsEntities bdTools = new bdToolsEntities();

            var edicion = bdTools.bdj_Ltl_SEL_marca_modelo_version_carroceria_ptas_ano_trans(marca, modelo, version, carroceria, puertas, agno, trans).FirstOrDefault();

            if (String.IsNullOrEmpty(edicion)) {

                edicion = "";
            }

            return edition = edicion;

        }

        private static int ValidaDatos(string carroceria, int puertas, string transmision, int combustible, int categoria) {

            int cantidadvalida = 0;

            if (ValidaCategoria(categoria)) {

                cantidadvalida = cantidadvalida + 1;

            }

            if (ValidaCarroceria(categoria, carroceria))
            {

                cantidadvalida = cantidadvalida + 1;

            }

            if (ValidaCombustible(combustible))
            {

                cantidadvalida = cantidadvalida + 1;

            }

            if (ValidaPuertas(puertas))
            {

                cantidadvalida = cantidadvalida + 1;

            }

            if (ValidaTransmision(transmision))
            {

                cantidadvalida = cantidadvalida + 1;

            }

            return cantidadvalida;
        }

        private static Boolean ValidaCategoria(int categoria) {

            Boolean confirma = false;

            baseprod2Entities baseprod = new baseprod2Entities();

            var dato = baseprod.SP_apiCLA_Categorias(categoria).FirstOrDefault();

            if (dato.idCategoria == categoria)
            {

                confirma = true;

            }

            return confirma;

        }

        private static Boolean ValidaCarroceria(int categoria, string carroceria)
        {

            Boolean confirma = false;

            baseprod2Entities baseprod = new baseprod2Entities();

            var dato = baseprod.SP_apiCLA_CarroceriasCategoria(categoria).ToArray();

            foreach (var data in dato)
            {

                if (data.cod_carroceria == carroceria)
                {

                    confirma = true;

                }

            }


          
            return confirma;

        }

        private static Boolean ValidaCombustible(int codigo)
        {

            List<CombustibleModel> combustibles = new List<CombustibleModel>();

            combustibles.Add(new CombustibleModel { codCombustible = 1, combustible = "Bencina" });
            combustibles.Add(new CombustibleModel { codCombustible = 2, combustible = "Diesel (Petroleo)" });
            combustibles.Add(new CombustibleModel { codCombustible = 3, combustible = "Gas" });
            combustibles.Add(new CombustibleModel { codCombustible = 4, combustible = "Híbrido" });
            combustibles.Add(new CombustibleModel { codCombustible = 5, combustible = "Eléctrico" });
            combustibles.Add(new CombustibleModel { codCombustible = 10, combustible = "Otro" });

            Boolean confirma = false;

            var value = combustibles.Find(item => item.codCombustible == codigo).codCombustible;

            if (value == codigo)
            {

                confirma = true;

            }

            return confirma;

        }

        private static Boolean ValidaPuertas(int codigo) {

            List<PuertaModel> puertas = new List<PuertaModel>();

            puertas.Add(new PuertaModel { codPuerta = 0, puerta = 0 });
            puertas.Add(new PuertaModel { codPuerta = 2, puerta = 2 });
            puertas.Add(new PuertaModel { codPuerta = 3, puerta = 3 });
            puertas.Add(new PuertaModel { codPuerta = 4, puerta = 4 });
            puertas.Add(new PuertaModel { codPuerta = 5, puerta = 5 });

            Boolean confirma = false;

            var value = puertas.Find(item => item.codPuerta == codigo).codPuerta;

            if (value == codigo)
            {

                confirma = true;

            }

            return confirma;

        }

        private static Boolean ValidaTransmision(string codigo) {

            Boolean confirma = false;

            if (codigo.ToUpper() == "S") {

                confirma = true;

            }

            if (codigo.ToUpper() == "N")
            {

                confirma = true;

            }

            return confirma;
        }

        private static string[] InsertaDatosParaPublicacion(PublicacionModel dato, string nombre_archivo) {

            string[] resultados = new string[2];

            DateTime fecha_i_date = DateTime.Now;
            string fotos = "";
            fotos = string.Join(",", dato.dVehiculo.listadofotos);

            baseprod2Entities bdprod = new baseprod2Entities();
            ObjectParameter respuestainsertsp = new ObjectParameter("respuesta", typeof(bool));
            ObjectParameter idpinsertsp = new ObjectParameter("idinsert", typeof(int));
            Object data = bdprod.SPR_Inserta_datos_vehiculos_publicar(dato.idfuente, dato.dEquipamiento.nuevo, dato.dVehiculo.categoria, dato.dVehiculo.tipo, dato.dVehiculo.carroceria, dato.dVehiculo.marca, dato.dVehiculo.modelo, dato.dVehiculo.version, dato.dVehiculo.ano, dato.dVehiculo.precio, dato.dVehiculo.color, dato.dVehiculo.kilom, dato.dVehiculo.motor, dato.dVehiculo.combustible, dato.dVehiculo.cilindrada, dato.dVehiculo.potencia, dato.dEquipamiento.transmision, dato.dEquipamiento.aireAcon, dato.dVehiculo.tipoDireccion, dato.dEquipamiento.radio, dato.dEquipamiento.alzaVidrios, dato.dEquipamiento.espejos, dato.dEquipamiento.frenosAbs, dato.dEquipamiento.airbag, dato.dEquipamiento.unicoDueno, dato.dEquipamiento.cierreCentral, dato.dEquipamiento.catalitico, dato.dEquipamiento.fwd, dato.dEquipamiento.llantas,  dato.dVehiculo.puertas.ToString(), dato.dEquipamiento.alarma, dato.dEquipamiento.consignacion, dato.dVehiculo.techo, dato.dVehiculo.comentario, dato.dVehiculo.patente, fotos, fecha_i_date, dato.codCliente, nombre_archivo, dato.ip, respuestainsertsp, idpinsertsp);

            resultados[0] = respuestainsertsp.Value.ToString();
            resultados[1] = idpinsertsp.Value.ToString();

            return resultados;
        }

        private static string[] ActualizaDatosParaPublicacion(PublicacionModel dato, string nombre_archivo, string idfuente)
        {

            string[] resultados = new string[2];
            string fotos = "";
            fotos = string.Join(",", dato.dVehiculo.listadofotos);
            
            baseprod2Entities bdprod = new baseprod2Entities();
            ObjectParameter respuestaactualizacionsp = new ObjectParameter("respuestaupdate", typeof(bool));
            ObjectParameter idrespuestaactualizacionsp = new ObjectParameter("idregistro", typeof(int));
            Object data = bdprod.SPR_Actualiza_datos_vehiculos_publicar(dato.idfuente, dato.dEquipamiento.nuevo, dato.dVehiculo.categoria, dato.dVehiculo.tipo, dato.dVehiculo.carroceria, dato.dVehiculo.marca, dato.dVehiculo.modelo, dato.dVehiculo.version, dato.dVehiculo.ano, dato.dVehiculo.precio, dato.dVehiculo.color, dato.dVehiculo.kilom, dato.dVehiculo.motor, dato.dVehiculo.combustible, dato.dVehiculo.cilindrada, dato.dVehiculo.potencia, dato.dEquipamiento.transmision, dato.dEquipamiento.aireAcon, dato.dVehiculo.tipoDireccion, dato.dEquipamiento.radio, dato.dEquipamiento.alzaVidrios, dato.dEquipamiento.espejos, dato.dEquipamiento.frenosAbs, dato.dEquipamiento.airbag, dato.dEquipamiento.unicoDueno, dato.dEquipamiento.cierreCentral, dato.dEquipamiento.catalitico, dato.dEquipamiento.fwd, dato.dEquipamiento.llantas, dato.dVehiculo.puertas.ToString(), dato.dEquipamiento.alarma, dato.dEquipamiento.consignacion, dato.dVehiculo.techo, dato.dVehiculo.comentario, dato.dVehiculo.patente, fotos, dato.codCliente, nombre_archivo, dato.ip, respuestaactualizacionsp, idrespuestaactualizacionsp);
            
            resultados[0] = respuestaactualizacionsp.Value.ToString();
            resultados[1] = idrespuestaactualizacionsp.Value.ToString();

            return resultados;
        }

        private static int PublicaAviso(PublicarChileautosModel datosparapublicar) {

            int cod_auto=0;

            baseprod2Entities bdprod = new baseprod2Entities();

            var data = bdprod.SP_PublicarAviso_Automotoras(datosparapublicar.codCliente ,datosparapublicar.ip ,datosparapublicar.patente ,datosparapublicar.tipo ,datosparapublicar.marca ,datosparapublicar.modelo ,datosparapublicar.ano ,datosparapublicar.version ,datosparapublicar.carroceria ,datosparapublicar.puertas ,datosparapublicar.tipoDireccion ,datosparapublicar.precio ,datosparapublicar.cilindrada ,datosparapublicar.potencia ,datosparapublicar.color ,datosparapublicar.kilom ,datosparapublicar.motor ,datosparapublicar.techo ,datosparapublicar.combustible ,datosparapublicar.comentario ,datosparapublicar.uidJato ,datosparapublicar.airbag ,datosparapublicar.aireAcon ,datosparapublicar.alarma ,datosparapublicar.alzaVidrios ,datosparapublicar.nuevo ,datosparapublicar.transmision ,datosparapublicar.radio ,datosparapublicar.espejos ,datosparapublicar.frenosAbs ,datosparapublicar.unicoDueno ,datosparapublicar.cierreCentral ,datosparapublicar.catalitico ,datosparapublicar.fwd ,datosparapublicar.llantas ,datosparapublicar.listaFotos ,datosparapublicar.plataforma ,datosparapublicar.consignacion).FirstOrDefault();

            cod_auto = int.Parse(data.codauto.ToString());

            return cod_auto;
        }

        private static int ActualizaAviso(PublicarChileautosModel datosparapublicar)
        {

            int cod_auto = 0;

            baseprod2Entities bdprod = new baseprod2Entities();

            var data = bdprod.SP_ActualizarAviso_Automotoras(datosparapublicar.codauto, datosparapublicar.codCliente, datosparapublicar.ip, datosparapublicar.patente, datosparapublicar.tipo, datosparapublicar.marca, datosparapublicar.modelo, datosparapublicar.ano, datosparapublicar.version, datosparapublicar.carroceria, datosparapublicar.puertas, datosparapublicar.tipoDireccion, datosparapublicar.precio, datosparapublicar.cilindrada, datosparapublicar.potencia, datosparapublicar.color, datosparapublicar.kilom, datosparapublicar.motor, datosparapublicar.techo, datosparapublicar.combustible, datosparapublicar.comentario, datosparapublicar.uidJato, datosparapublicar.airbag, datosparapublicar.aireAcon, datosparapublicar.alarma, datosparapublicar.alzaVidrios, datosparapublicar.nuevo, datosparapublicar.transmision, datosparapublicar.radio, datosparapublicar.espejos, datosparapublicar.frenosAbs, datosparapublicar.unicoDueno, datosparapublicar.cierreCentral, datosparapublicar.catalitico, datosparapublicar.fwd, datosparapublicar.llantas, datosparapublicar.listaFotos, datosparapublicar.plataforma, datosparapublicar.consignacion).FirstOrDefault();

            cod_auto = int.Parse(data.codauto.ToString());

            return cod_auto;
        }

        private static string ObtieneMarcaxCategoria(int categoria, int idmarca)
        {

            string nombremarca = "";

            baseprod2Entities baseprod = new baseprod2Entities();

            var dato = baseprod.SP_apiCLA_MarcasCategoria(categoria,null,null).ToArray();

            foreach (var data in dato)
            {

                if (data.codMarca == idmarca)
                {

                    nombremarca = data.marca;

                }

            }

            return nombremarca;

        }

        private static PublicarChileautosModel Otienedata(int id) {

            baseprod2Entities bdprod = new baseprod2Entities();
            PublicarChileautosModel datospublicar = new PublicarChileautosModel();

            ObjectParameter respuestadatossp = new ObjectParameter("respuesta", typeof(bool));
            var datos = bdprod.SPR_Obtiene_dato_vehiculo_Automotora_publicar_en_CA(id, respuestadatossp);

            foreach (var d in datos) {

                datospublicar.codauto = d.cod_auto_CA; 
                datospublicar.codCliente = d.sucursal;
                datospublicar.patente = d.patente;
                datospublicar.categoria = d.categoria;
                datospublicar.tipo = d.Tipo_vehiculo;
                datospublicar.marca = d.Marca;
                datospublicar.modelo = d.Modelo;
                datospublicar.ano = d.Ano;
                datospublicar.version = d.Version;
                datospublicar.carroceria = d.Carroceria;
                datospublicar.puertas = int.Parse(d.Puertas);
                datospublicar.tipoDireccion = d.tipo_direccion;
                datospublicar.precio = d.Precio;
                datospublicar.cilindrada = d.Cilindrada;
                datospublicar.potencia = d.potencia;
                datospublicar.color = d.Color;
                datospublicar.kilom = d.KM;
                datospublicar.motor = d.motor;
                datospublicar.techo = d.Techo;
                datospublicar.combustible = d.combustible;
                datospublicar.comentario = d.comentarios;

                //equipamiento

                datospublicar.airbag = d.airbag;
                datospublicar.aireAcon = d.aire_acondicionado;
                datospublicar.alarma = d.Alarma;
                datospublicar.alzaVidrios = d.alzavidrios_electricos;
                datospublicar.nuevo = d.Nuevo_o_usado;
                datospublicar.transmision = d.tipo_cambio;
                datospublicar.radio = d.radio;
                datospublicar.espejos = d.espejos_electricos;
                datospublicar.frenosAbs = d.frenos_ABS;
                datospublicar.unicoDueno = d.unico_dueno;
                datospublicar.cierreCentral = d.cierre_centralizado;
                datospublicar.catalitico = d.catalitico;
                datospublicar.fwd = d.fwd;
                datospublicar.llantas = d.Llantas;
                datospublicar.listaFotos = d.fotos;
                datospublicar.consignacion = d.consignacion;

            }


            Boolean resultado = Boolean.Parse(respuestadatossp.Value.ToString());
            return datospublicar;
        }

        private static bool SiArchivoEstaBloqueado(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //archivo en uso
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //archivo liberado
            return false;
        }

        private static Boolean ProcesaDirectorio(string targetDirectory)
        {
            Boolean eliminado = true;
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                FileInfo file = new FileInfo(fileName);
                
                while (eliminado == true) {

                    eliminado = SiArchivoEstaBloqueado(file);

                }

            }

            return eliminado;
        }

        private static Boolean ActualizaRegistroAutomotora(int id, int codauto) {

            baseprod2Entities bdprod = new baseprod2Entities();

            Boolean respuesta = false;

            ObjectParameter respuestaspr = new ObjectParameter("respuesta", typeof(bool));
            var dato = bdprod.SPR_Inserta_CodAutoCA_de_CA_en_Automotora(id,codauto, respuestaspr);

            respuesta = Boolean.Parse(respuestaspr.Value.ToString());

            return respuesta;

        }


    }
}