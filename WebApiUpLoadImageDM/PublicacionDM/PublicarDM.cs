using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using AccesoDatos;
using System.Reflection;
using System.Configuration;

namespace PublicacionDM
{
    public class PublicarDM
    {

        static private string provider_DM = ConfigurationManager.AppSettings["provider_DM"].ToString();
        static private string key_DM = ConfigurationManager.AppSettings["key_DM"].ToString();
        static private string url_DM = ConfigurationManager.AppSettings["url_DM"].ToString();

        public static Dictionary<string, string> DicCategorias = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCombustibles = new Dictionary<string, string>();
        public static Dictionary<string, string> DicTransmision = new Dictionary<string, string>();
        public static Dictionary<string, string> DicDireccion = new Dictionary<string, string>();
        public static Dictionary<string, string> DicNumPuertas = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarrocerias = new Dictionary<string, string>();
        public static Dictionary<string, string> DicColor = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarroceriasMotos = new Dictionary<string, string>();

        public static void CargaDiccionarios()
        {
            //Diccionario DicCategorias
            DicCategorias.Add("1", "CAR");
            DicCategorias.Add("2", "MOTORCYCLE");
            DicCategorias.Add("4", "TRUCK");
            DicCategorias.Add("11", "MOTORHOME");
            DicCategorias.Add("8", "NAVIGATION");

            //Diccionario Combustibles
            DicCombustibles.Add("1", "Bencina");
            DicCombustibles.Add("2", "Diesel");
            DicCombustibles.Add("4", "Eléctrico / Híbrido");
            DicCombustibles.Add("5", "Eléctrico / Híbrido");
            DicCombustibles.Add("3", "Gas / Bencina");
            DicCombustibles.Add("10", "Otros");

            //Diccionario Transmisión
            DicTransmision.Add("S", "Automática");
            DicTransmision.Add("N", "Manual");

            //Diccionario Dirección
            DicDireccion.Add("H", "Asistida");
            DicDireccion.Add("M", "Hidráulica");
            DicDireccion.Add("A", "Mecánica");

            //Diccionario Número de puertas
            DicNumPuertas.Add("2","2");
            DicNumPuertas.Add("3", "3");
            DicNumPuertas.Add("4", "4");
            DicNumPuertas.Add("5", "5 o más");

            //Diccionario DicCarrocerias autos
            DicCarrocerias.Add("CS", "Camioneta");
            DicCarrocerias.Add("CA", "Convertible");
            DicCarrocerias.Add("CO", "Coupé");
            DicCarrocerias.Add("ES", "Familiar");
            DicCarrocerias.Add("HA", "Hatchback");
            DicCarrocerias.Add("OD", "Monovolumen");
            DicCarrocerias.Add("PU", "Pick Up");
            DicCarrocerias.Add("CP", "Pick Up");
            DicCarrocerias.Add("SA", "Sedán");
            DicCarrocerias.Add("MM", "Utilitario");

            //Diccionario de Colores
            DicColor.Add("Amarillo", "Amarillo");
            DicColor.Add("Azul","Azul");
            DicColor.Add("Beige","Beige");
            DicColor.Add("Blanco","Blanco");
            DicColor.Add("Bordó","Bordó");
            DicColor.Add("Bronce","Bronce");
            DicColor.Add("eleste","eleste");
            DicColor.Add("hampagne","hampagne");
            DicColor.Add("Gris","Gris");
            DicColor.Add("Marrón","Marrón");
            DicColor.Add("Naranja","Naranja");
            DicColor.Add("Negro","Negro");
            DicColor.Add("Ocre","Ocre");
            DicColor.Add("Plateado","Plateado ");
            DicColor.Add("Rojo","Rojo");
            DicColor.Add("Rosa","Rosa");
            DicColor.Add("Verde","Verde");
            DicColor.Add("Violeta", "Violeta");


            //Diccionario DicCarrocerias Motos
            DicCarroceriasMotos.Add("Chopper", "Custom y Choppers");
            DicCarroceriasMotos.Add("Cuadrimoto", "Cuatriciclos y Triciclos");
            DicCarroceriasMotos.Add("Custom", "Custom y Choppers");
            DicCarroceriasMotos.Add("Deportivas", "Deportivas");
            DicCarroceriasMotos.Add("Enduro - cross", "Cross y Enduro");
            DicCarroceriasMotos.Add("Moto de nieve", "Motos de Nieve");
            DicCarroceriasMotos.Add("Racing", "Touring y Trails");
            DicCarroceriasMotos.Add("Retro", "Custom y Choppers");
            DicCarroceriasMotos.Add("Scooter", "Scooters y Ciclomotores");
            DicCarroceriasMotos.Add("Sport calle - urbanas", "");
            DicCarroceriasMotos.Add("Todo terrenos", "Touring y Trails");
            DicCarroceriasMotos.Add("Trabajo - calle", "Calle y Naked");

        }

        public static void CleanAllDiccionary() {

            DicCategorias.Clear();
            DicCombustibles.Clear();
            DicTransmision.Clear();
            DicDireccion.Clear();
            DicNumPuertas.Clear();
            DicCarrocerias.Clear();
            DicColor.Clear();
            DicCarroceriasMotos.Clear();

        }

        /// <summary>
        ///´Proceso que toma los datos y los pasa a la api DeMotores para su publicación. de acuerdo al idusuario designado.
        /// </summary>
        /// <returns></returns>
        public string InsertarPublicacion( string codauto) {

            string result ="";
            string data = "";

            try {

                ProveedorDatos PrDatos = new ProveedorDatos();

                var datosvehiculo = PrDatos.EntregaDatosVehiculo(codauto);

                if (datosvehiculo != null)
                {

                    data = ArmaDatosParaPublicar(datosvehiculo);
                    result = PublicaenDeMotoresApi(data);

                }
                else
                {

                    result = "no Existen datos para el registro: " + codauto;

                }

                return result;

            } catch (Exception ex){

                return result = "Mensaje: " + ex;

            }

        }

        public static string ArmaDatosParaPublicar(object datosvehiculo) {

            string data = "";
            string useriddm = "11053485";

            Type myType = datosvehiculo.GetType();

            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            int verificaCategoriaVehiculo = 0;

            if (!String.IsNullOrEmpty(datosvehiculo.GetType().GetProperty("idCategoria").GetValue(datosvehiculo, null).ToString()))
            {

                verificaCategoriaVehiculo = int.Parse(datosvehiculo.GetType().GetProperty("idCategoria").GetValue(datosvehiculo, null).ToString());

            }
            
            if (verificaCategoriaVehiculo == 1)
            {

                data = ProcesaDatosCar(props, datosvehiculo, useriddm);

            }

            if (verificaCategoriaVehiculo == 2)
            {

                data = ProcesaDatosMotorcycle(props, datosvehiculo, useriddm);

            }

            if (verificaCategoriaVehiculo == 4)
            {

                data = ProcesaDatosTruck(props, datosvehiculo, useriddm);

            }

            if (verificaCategoriaVehiculo == 11)
            {

                data = ProcesaDatosMotorhome(props, datosvehiculo, useriddm);

            }

            return data;
        }


        public static string ProcesaDatosCar(IList<PropertyInfo> props, object datosvehiculo, string useriddm) {

            string datosparademotores = "";
            string subtitulo = "";


            foreach (PropertyInfo prop in props)
            {
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "Tipoveh")
                {
                    if (propValue.ToString() == "CL")
                    {

                        datosparademotores = datosparademotores + "oldOrClassic=yes&";

                    }

                    if (propValue.ToString() == "TX")
                    {

                        datosparademotores = datosparademotores + "cab=yes&";

                    }

                }

                if (propName.ToString() == "marca")
                {

                    datosparademotores = datosparademotores + "brand=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + propValue.ToString();
                }

                if (propName.ToString() == "modelo")
                {

                    datosparademotores = datosparademotores + "model=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + " " + propValue.ToString();
                }

                if (propName.ToString() == "version")
                {

                    datosparademotores = datosparademotores + "version=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + " " + propValue.ToString();
                }

                if (propName.ToString() == "ANO")
                {

                    datosparademotores = datosparademotores + "year=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "combustible")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "fuel=" + ConsultaCombustible(DicCombustibles, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "tipo_cambio")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "transmission=" + DicTransmision[propValue.ToString()] + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "tipo_direccion")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "steering=" + DicDireccion[propValue.ToString()] + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "Puertas")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "doors=" + DicNumPuertas[propValue.ToString()] + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "Carroceria")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "segment=" + DicCarrocerias[propValue.ToString()] + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "color")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "color=" + ConsultaColor(DicColor, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "km")
                {

                    datosparademotores = datosparademotores + "mileage=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "PESOS")
                {

                    datosparademotores = datosparademotores + "price=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "Pesosdos")
                {

                    if (propValue == null)
                    {

                        datosparademotores = datosparademotores + "currency=CLP&";

                    }
                    else
                    {

                        datosparademotores = datosparademotores + "currency=USD&";

                    }

                }

                if (propName.ToString() == "patente")
                {
                    if (propValue == null)
                    {

                        datosparademotores = datosparademotores + "licensePlate=" + propValue.ToString() + "&";

                    }

                }

                if (propName.ToString() == "otros")
                {

                    datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "cod_autoCH")
                {

                    datosparademotores = datosparademotores + "providerVehicleId=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "unico_dueno")
                {

                    if (propValue.ToString() == "S")
                    {

                        datosparademotores = datosparademotores + "uniqueOwner=yes&";

                    }

                }

                if (propName.ToString() == "imagenes")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoImagenes(propValue.ToString());

                    }

                }

                if (propName.ToString() == "especificaciones")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoEspecificaciones(propValue.ToString());

                    }

                }

            }

            datosparademotores = datosparademotores + "provider=" + provider_DM + "&key=" + key_DM + "&userId=" + useriddm + "&subtitle=" + subtitulo;

            return datosparademotores;

        }

        public static string ProcesaDatosMotorcycle(IList<PropertyInfo> props, object datosvehiculo, string useriddm)
        {

            string datosparademotores = "";
            string subtitulo = "";

            foreach (PropertyInfo prop in props)
            {
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "marca")
                {

                    datosparademotores = datosparademotores + "brand=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + propValue.ToString();
                }

                if (propName.ToString() == "modelo")
                {

                    datosparademotores = datosparademotores + "model=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + " " + propValue.ToString();
                }

                if (propName.ToString() == "ANO")
                {

                    datosparademotores = datosparademotores + "year=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "Carroceria")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "segment=" + DicCarroceriasMotos[propValue.ToString()] + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "color")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "color=" + ConsultaColor(DicColor, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "km")
                {

                    datosparademotores = datosparademotores + "mileage=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "PESOS")
                {

                    datosparademotores = datosparademotores + "price=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "Pesosdos")
                {

                    if (propValue == null)
                    {

                        datosparademotores = datosparademotores + "currency=CLP&";

                    }
                    else
                    {

                        datosparademotores = datosparademotores + "currency=USD&";

                    }

                }

                if (propName.ToString() == "otros")
                {

                    datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "cod_autoCH")
                {

                    datosparademotores = datosparademotores + "providerVehicleId=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "unico_dueno")
                {

                    if (propValue.ToString() == "S")
                    {

                        datosparademotores = datosparademotores + "uniqueOwner=yes&";

                    }

                }

                if (propName.ToString() == "imagenes")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoImagenes(propValue.ToString());

                    }

                }

                if (propName.ToString() == "especificaciones")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoEspecificaciones(propValue.ToString());

                    }

                }

            }
            
            datosparademotores = datosparademotores + "provider=" + provider_DM + "&key=" + key_DM + "&userId=" + useriddm + "&subtitle=" + subtitulo;

            return datosparademotores;

        }

        public static string ProcesaDatosTruck(IList<PropertyInfo> props, object datosvehiculo, string useriddm)
        {

            string datosparademotores = "";
            string subtitulo = "";
            
            foreach (PropertyInfo prop in props)
            {
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "marca")
                {

                    datosparademotores = datosparademotores + "brand=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + propValue.ToString();
                }

                if (propName.ToString() == "modelo")
                {

                    datosparademotores = datosparademotores + "model=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + " " + propValue.ToString();
                }

                if (propName.ToString() == "version")
                {

                    datosparademotores = datosparademotores + "version=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + " " + propValue.ToString();
                }

                if (propName.ToString() == "ANO")
                {

                    datosparademotores = datosparademotores + "year=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "combustible")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "fuel=" + ConsultaCombustible(DicCombustibles, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "tipo_cambio")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "transmission=" + DicTransmision[propValue.ToString()] + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "tipo_direccion")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "steering=" + DicDireccion[propValue.ToString()] + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "color")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "color=" + ConsultaColor(DicColor, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "km")
                {

                    datosparademotores = datosparademotores + "mileage=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "PESOS")
                {

                    datosparademotores = datosparademotores + "price=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "Pesosdos")
                {

                    if (propValue == null)
                    {

                        datosparademotores = datosparademotores + "currency=CLP&";

                    }
                    else
                    {

                        datosparademotores = datosparademotores + "currency=USD&";

                    }

                }

                if (propName.ToString() == "unico_dueno")
                {

                    if (propValue.ToString() == "S")
                    {

                        datosparademotores = datosparademotores + "uniqueOwner=yes&";

                    }

                }

                if (propName.ToString() == "motor")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + "engine="+ propValue.ToString() + "&";

                    }

                }

                if (propName.ToString() == "otros")
                {

                    datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "cod_autoCH")
                {

                    datosparademotores = datosparademotores + "providerVehicleId=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "imagenes")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoImagenes(propValue.ToString());

                    }

                }

                if (propName.ToString() == "especificaciones")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoEspecificaciones(propValue.ToString());

                    }

                }

            }

            datosparademotores = datosparademotores + "provider=" + provider_DM + "&key=" + key_DM + "&userId=" + useriddm + "&subtitle=" + subtitulo;

            return datosparademotores;

        }

        public static string ProcesaDatosMotorhome(IList<PropertyInfo> props, object datosvehiculo, string useriddm)
        {

            string datosparademotores = "";
            string subtitulo = "";


            foreach (PropertyInfo prop in props)
            {
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "marca")
                {

                    datosparademotores = datosparademotores + "brand=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + propValue.ToString();
                }

                if (propName.ToString() == "modelo")
                {

                    datosparademotores = datosparademotores + "model=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + " " + propValue.ToString();
                }

                if (propName.ToString() == "version")
                {

                    datosparademotores = datosparademotores + "version=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + " " + propValue.ToString();
                }

                if (propName.ToString() == "ANO")
                {

                    datosparademotores = datosparademotores + "year=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "tipo_direccion")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "steering=" + DicDireccion[propValue.ToString()] + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "combustible")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "fuel=" + ConsultaCombustible(DicCombustibles, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "color")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "color=" + ConsultaColor(DicColor, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "km")
                {

                    datosparademotores = datosparademotores + "mileage=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "PESOS")
                {

                    datosparademotores = datosparademotores + "price=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "Pesosdos")
                {

                    if (propValue == null)
                    {

                        datosparademotores = datosparademotores + "currency=CLP&";

                    }
                    else
                    {

                        datosparademotores = datosparademotores + "currency=USD&";

                    }

                }

                if (propName.ToString() == "unico_dueno")
                {

                    if (propValue.ToString() == "S")
                    {

                        datosparademotores = datosparademotores + "uniqueOwner=yes&";

                    }

                }

                if (propName.ToString() == "otros")
                {

                    datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "cod_autoCH")
                {

                    datosparademotores = datosparademotores + "providerVehicleId=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "imagenes")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoImagenes(propValue.ToString());

                    }

                }

                if (propName.ToString() == "especificaciones")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoEspecificaciones(propValue.ToString());

                    }

                }

            }

            datosparademotores = datosparademotores + "provider=" + provider_DM + "&key=" + key_DM + "&userId=" + useriddm + "&subtitle=" + subtitulo;

            return datosparademotores;

        }

        public static string ProcesaDatosNavigation(IList<PropertyInfo> props, object datosvehiculo, string useriddm)
        {

            string datosparademotores = "";
            string subtitulo = "";
            
            foreach (PropertyInfo prop in props)
            {
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    CargaDiccionarios();
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
                    CleanAllDiccionary();
                }

                if (propName.ToString() == "marca")
                {

                    datosparademotores = datosparademotores + "brand=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + propValue.ToString();
                }

                if (propName.ToString() == "modelo")
                {

                    datosparademotores = datosparademotores + "model=" + propValue.ToString() + "&";
                    subtitulo = subtitulo + " " + propValue.ToString();
                }

                if (propName.ToString() == "ANO")
                {

                    datosparademotores = datosparademotores + "year=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "nuevo")
                {

                    if (propValue.ToString() == "N") {

                        datosparademotores = datosparademotores + "used=yes&";

                    }

                }

                if (propName.ToString() == "PESOS")
                {

                    datosparademotores = datosparademotores + "price=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "Pesosdos")
                {

                    if (propValue == null)
                    {

                        datosparademotores = datosparademotores + "currency=CLP&";

                    }
                    else
                    {

                        datosparademotores = datosparademotores + "currency=USD&";

                    }

                }

                if (propName.ToString() == "otros")
                {

                    datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "cod_autoCH")
                {

                    datosparademotores = datosparademotores + "providerVehicleId=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "unico_dueno")
                {

                    if (propValue.ToString() == "S")
                    {

                        datosparademotores = datosparademotores + "uniqueOwner=yes&";

                    }

                }

                if (propName.ToString() == "imagenes")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoImagenes(propValue.ToString());

                    }

                }

                if (propName.ToString() == "especificaciones")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + ListadoEspecificaciones(propValue.ToString());

                    }

                }
                
            }

            datosparademotores = datosparademotores + "provider=" + provider_DM + "&key=" + key_DM + "&userId=" + useriddm + "&subtitle=" + subtitulo;

            return datosparademotores;

        }

        public static string PublicaenDeMotoresApi(string data) {

            string respuesta = "";

            string url = url_DM;
            string responseString;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var action = Uri.EscapeUriString(url);

                var content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = client.PostAsync(action, content).Result;
                var responseContent = response.Content;
                responseString = responseContent.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    respuesta = responseString;
                }
                else
                {
                    respuesta = responseString;
                }

            }

            return respuesta;
        }

        public static string ConsultaCategoria(Dictionary<string, string> destino, string compara)
        {

            string resultado = "";

            if (String.IsNullOrEmpty(compara))
            {

                compara = resultado;

            }

            int cost = 0;

            foreach (KeyValuePair<string, string> a in destino)
            {
                cost = LevenshteinDistance.Compute(a.Key, compara);

                if (cost == 0)
                {

                    resultado = a.Value;

                }

            }

            return resultado;

        }

        public static string ConsultaCombustible(Dictionary<string, string> destino, string compara)
        {

            string resultado = "";

            resultado = destino[compara];

            return resultado;

        }
        
        public static string ConsultaColor(Dictionary<string,string> destino, string compara) {
            
            string resultado = "Otro Color";

            if (String.IsNullOrEmpty(compara)) {

                compara = resultado;

            }

            int cost = 0;

            foreach (KeyValuePair<string, string> a in destino)
            {
                cost = LevenshteinDistance.Compute(a.Key, compara.First().ToString().ToUpper() + compara.Substring(1));

                if (cost == 0)
                {

                    resultado = a.Key;

                }

            }

            return resultado;

        }

        public static string ListadoImagenes(string data) {

            string imagenes = "";

            string[] nums = data.Split(',').ToArray();

            for (int i = 0; i < nums.Length; i++) {

                imagenes = imagenes + "image[" + i + "]=" + nums[i]+"&";

            }

            return imagenes;

        }

        public static string ListadoEspecificaciones(string data)
        {

            string especificaciones = "";

            string[] nums = data.Split(';').ToArray();

            for (int i = 0; i < nums.Length; i++)
            {

                especificaciones = especificaciones + nums[i] + "&";

            }

            return especificaciones;

        }

        /// <summary>
        /// Logra la major aproximación entre dos arreglos.
        /// </summary>
        static class LevenshteinDistance
        {
            /// <summary>
            /// Comprara la distancia entre dos arreglos, es decir, cuantas veces debe un arreglo ser modificado para que sean iguales, mientras menos modificaciones, más ´similar es. Si el resultado es cero, son iguales.
            /// </summary>
            public static int Compute(string s, string t)
            {
                int n = s.Length;
                int m = t.Length;
                int[,] d = new int[n + 1, m + 1];

                // Step 1
                if (n == 0)
                {
                    return m;
                }

                if (m == 0)
                {
                    return n;
                }

                // Step 2
                for (int i = 0; i <= n; d[i, 0] = i++)
                {
                }

                for (int j = 0; j <= m; d[0, j] = j++)
                {
                }

                // Step 3
                for (int i = 1; i <= n; i++)
                {
                    //Step 4
                    for (int j = 1; j <= m; j++)
                    {
                        // Step 5
                        int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                        // Step 6
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                    }
                }
                // Step 7
                return d[n, m];
            }
        }

    }
}