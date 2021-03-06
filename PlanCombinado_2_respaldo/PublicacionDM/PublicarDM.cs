﻿using System;
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
        static private string url_DM_Elimina = ConfigurationManager.AppSettings["url_DM_Elimina"].ToString();

        public static Dictionary<string, string> DicCategorias = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCombustibles = new Dictionary<string, string>();
        public static Dictionary<string, string> DicTransmision = new Dictionary<string, string>();
        public static Dictionary<string, string> DicDireccion = new Dictionary<string, string>();
        public static Dictionary<string, string> DicNumPuertas = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarrocerias = new Dictionary<string, string>();
        public static Dictionary<string, string> DicColor = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarroceriasMotos = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCarroceriasNauticos = new Dictionary<string, string>();

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
            DicTransmision.Add("", "Manual");

            //Diccionario Dirección
            DicDireccion.Add("H", "Asistida");
            DicDireccion.Add("M", "Hidráulica");
            DicDireccion.Add("A", "Mecánica");

            //Diccionario Número de puertas
            DicNumPuertas.Add("0", "2");
            DicNumPuertas.Add("1", "2");
            DicNumPuertas.Add("2","2");
            DicNumPuertas.Add("3", "3");
            DicNumPuertas.Add("4", "4");
            DicNumPuertas.Add("5", "5 o más");


            //Diccionario DicCarrocerias autos
            DicCarrocerias.Add("CS", "Camioneta");
            DicCarrocerias.Add("BH", "Camioneta");
            DicCarrocerias.Add("CA", "Convertible");
            DicCarrocerias.Add("RO", "Convertible");
            DicCarrocerias.Add("CO", "Coupé");
            DicCarrocerias.Add("TA", "Coupé");
            DicCarrocerias.Add("ES", "Familiar");
            DicCarrocerias.Add("VA", "Familiar");
            DicCarrocerias.Add("HA", "Hatchback");
            DicCarrocerias.Add("MC", "Hatchback");
            DicCarrocerias.Add("MM", "Hatchback");
            DicCarrocerias.Add("OD", "Monovolumen");
            DicCarrocerias.Add("PU", "Pick Up");
            DicCarrocerias.Add("DC", "Pick Up");
            DicCarrocerias.Add("CP", "Pick Up");
            DicCarrocerias.Add("OC", "Pick Up");
            DicCarrocerias.Add("DP", "Pick Up");
            DicCarrocerias.Add("SA", "Sedán");
            DicCarrocerias.Add("SH", "Sedán");
            DicCarrocerias.Add("PV", "Utilitario");
            DicCarrocerias.Add("RV", "Utilitario");
            DicCarrocerias.Add("FW", "Utilitario");
            DicCarrocerias.Add("WV", "Utilitario");
            DicCarrocerias.Add("BU", "Utilitario");
            DicCarrocerias.Add("CV", "Utilitario");
            DicCarrocerias.Add("CC", "Utilitario");

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
            DicCarroceriasMotos.Add("CH", "Custom y Choppers");
            DicCarroceriasMotos.Add("CD", "Cuatriciclos y Triciclos");
            DicCarroceriasMotos.Add("MU", "Custom y Choppers");
            DicCarroceriasMotos.Add("MD", "Deportivas");
            DicCarroceriasMotos.Add("ME", "Cross y Enduro");
            DicCarroceriasMotos.Add("MN", "Motos de Nieve");
            DicCarroceriasMotos.Add("RA", "Touring y Trails");
            DicCarroceriasMotos.Add("MR", "Custom y Choppers");
            DicCarroceriasMotos.Add("SC", "Scooters y Ciclomotores");
            DicCarroceriasMotos.Add("MS", "Touring y Trails");
            DicCarroceriasMotos.Add("MO", "Touring y Trails");
            DicCarroceriasMotos.Add("MT", "Calle y Naked");

            //Diccionario DicCarrocerias Náuticos
            DicCarroceriasNauticos.Add("AF", "Otros");
            DicCarroceriasNauticos.Add("BO", "Botes");
            DicCarroceriasNauticos.Add("JS", "Motos de agua");
            DicCarroceriasNauticos.Add("KY", "Kayak");
            DicCarroceriasNauticos.Add("LA", "Lanchas");
            DicCarroceriasNauticos.Add("MG", "Motos de agua");
            DicCarroceriasNauticos.Add("MM", "Otros");
            DicCarroceriasNauticos.Add("MS", "Otros");
            DicCarroceriasNauticos.Add("N","Otros");
            DicCarroceriasNauticos.Add("TB", "Otros");
            DicCarroceriasNauticos.Add("TS", "Otros");
            DicCarroceriasNauticos.Add("VE", "Embarcaciones a vela");
            DicCarroceriasNauticos.Add("WS", "Otros");
            DicCarroceriasNauticos.Add("YA", "Otros");
            DicCarroceriasNauticos.Add("ZO", "Gomones");
            DicCarroceriasNauticos.Add("", "Otros");

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
            DicCarroceriasNauticos.Clear();

        }

        /// <summary>
        ///´Proceso que toma los datos y los pasa a la api DeMotores para su publicación. de acuerdo al idusuario designado.
        /// </summary>
        /// <returns></returns>
        public string InsertarPublicacion(string codauto) {

            string result ="";
            string data = "";
            string estado = "";
            string[] coddemotores = new string[2];

            try {

                ProveedorDatos PrDatos = new ProveedorDatos();

                var datosvehiculo = PrDatos.EntregaDatosVehiculo(codauto);

                if (datosvehiculo != null)
                {

                   string idCategoria =  (string)datosvehiculo.GetType().GetProperty("Categoria").GetValue(datosvehiculo, null);

                    if (!String.IsNullOrEmpty(idCategoria))
                    {

                        coddemotores = ConsultaCodigoDeMotores(datosvehiculo);
                        if (String.IsNullOrEmpty(coddemotores[0]) && String.IsNullOrEmpty(coddemotores[1]))
                        {

                            data = ArmaDatosParaPublicar(datosvehiculo);
                            string[] codigodemotores = PublicaenDeMotoresApi(data, codauto, coddemotores[0], coddemotores[1]);
                            result = "Aviso número: " + codauto + ", en DeMotores.cl con código: " + codigodemotores[0] + ", su estado es: " + codigodemotores[1]+".";

                        }
                        else if (!String.IsNullOrEmpty(coddemotores[0]) && !String.IsNullOrEmpty(coddemotores[1]))
                        {

                            data = ArmaDatosParaPublicar(datosvehiculo);
                            if (coddemotores[1] == "0" || coddemotores[1] == "3")
                            {

                                string[] codigodemotores = PublicaenDeMotoresApi(data, codauto, coddemotores[0], coddemotores[1]);
                                result = codigodemotores[0] + " " + codigodemotores[1];

                            }
                            else
                            {

                                string[] codigodemotores = PublicaenDeMotoresApi(data, codauto, coddemotores[0], coddemotores[1]);
                                result = "Aviso número: " + codauto + ", en DeMotores.cl con código: " + codigodemotores[0] + ", su estado es: " + codigodemotores[1]+".";

                            }

                        }
                        else if (String.IsNullOrEmpty(coddemotores[0]) && !String.IsNullOrEmpty(coddemotores[1]))
                        {

                            if (coddemotores[1] == "0")
                            {

                                result = "Aviso número: " + codauto + ", posee en DeMotores un error: " + coddemotores[1];

                            }
                            else if (String.IsNullOrEmpty(coddemotores[0]))
                            {

                                result = "Aviso número: " + codauto + ", no posee un código en DeMotores.cl.";

                            }

                        }
                    }
                    else {

                        result = "No Existen datos para el registro: " + codauto+", revise el código de publicación, puede que esté eliminado.";

                    }

                }
                else
                {

                    result = "No Existen datos para el registro: " + codauto+" para ser procesado en DeMotores.";

                }

                return result;

            } catch (Exception ex){

                return result = "Mensaje: " + ex;

            }

        }

        public string EliminarPublicacion(string codauto) {

            string result = "";
            string data = "";
            string estado = "";
            string[] coddemotores = new string[2];

            try {

                ProveedorDatos PrDatos = new ProveedorDatos();

                var datosvehiculo = PrDatos.EntregaDatosVehiculo(codauto);

                if (datosvehiculo != null)
                {

                    coddemotores = ConsultaCodigoDeMotores(datosvehiculo);
                    if (String.IsNullOrEmpty(coddemotores[0]))
                    {

                        result = "Aviso número: " + codauto + " no está publicado en DeMotores.cl";

                    }
                    else {

                        
                        string resultadook = EliminaenDeMotoresApi(codauto);

                        if (resultadook == "OK")
                        {
                            estado = ActualizaEstadosPublicacion(codauto, coddemotores[0], "3", "finalizado");
                        }
                        else {
                            estado = ActualizaEstadosPublicacion(codauto, coddemotores[0], "0", "finalizado");
                        }

                        result = resultadook+", Aviso número: " + codauto + ", en DeMotores.cl su estado es: "+estado;
                        
                    }

                }
                else
                {

                    result = "No Existen datos para el registro número: " + codauto+" para ser procesado en DeMotores.";

                }

                return result;

            } catch (Exception ex) {

                result = "mensaje: " + ex;
                
            }


            return result;
        }

        public static string[] ConsultaCodigoDeMotores(object datosvehiculo) {

            string[] data = new string[2];

            Type myType = datosvehiculo.GetType();

            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            if (datosvehiculo.GetType().GetProperty("codigoDM").GetValue(datosvehiculo, null) != null) {

                data[0] = datosvehiculo.GetType().GetProperty("codigoDM").GetValue(datosvehiculo, null).ToString();

            }

            if (datosvehiculo.GetType().GetProperty("accion").GetValue(datosvehiculo, null) != null)
            {

                data[1] = datosvehiculo.GetType().GetProperty("accion").GetValue(datosvehiculo, null).ToString();

            }


            return data;
        }

        public static string ArmaDatosParaPublicar(object datosvehiculo) {

            string data = "";

            string useriddm = "";

            Type myType = datosvehiculo.GetType();

            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            int verificaCategoriaVehiculo = 0;
            string codcliente = "";

            if (!String.IsNullOrEmpty(datosvehiculo.GetType().GetProperty("idCategoria").GetValue(datosvehiculo, null).ToString()))
            {

                verificaCategoriaVehiculo = int.Parse(datosvehiculo.GetType().GetProperty("idCategoria").GetValue(datosvehiculo, null).ToString());

            }

            if (!String.IsNullOrEmpty(datosvehiculo.GetType().GetProperty("COD_CLIENTE").GetValue(datosvehiculo, null).ToString()))
            {

                codcliente = datosvehiculo.GetType().GetProperty("COD_CLIENTE").GetValue(datosvehiculo, null).ToString();

            }

            useriddm = ObtieneUidDM(codcliente);
            

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

            if (verificaCategoriaVehiculo == 8)
            {

                data = ProcesaDatosNavigation(props, datosvehiculo, useriddm);

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
                CleanAllDiccionary();
                CargaDiccionarios();
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
                   
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
                   
                    datosparademotores = datosparademotores + "fuel=" + ConsultaCombustible(DicCombustibles, propValue.ToString()) + "&";
                    
                }

                if (propName.ToString() == "tipo_cambio")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "transmission=" + DicTransmision[propValue.ToString()] + "&";
                    }
                    else { 
                        datosparademotores = datosparademotores + "transmission=Manual&";
                    }
                }

                if (propName.ToString() == "tipo_direccion")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "steering=" + DicDireccion[propValue.ToString()] + "&";
                    }
                }

                if (propName.ToString() == "Puertas")
                {
                   
                    datosparademotores = datosparademotores + "doors=" + DicNumPuertas[propValue.ToString()] + "&";
                    
                }

                if (propName.ToString() == "Carroceria")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "segment=" + DicCarrocerias[propValue.ToString()] + "&";
                    }
                }

                if (propName.ToString() == "color")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "color=" + ConsultaColor(DicColor, propValue.ToString()) + "&";
                    }
                }

                if (propName.ToString() == "km")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "mileage=" + propValue.ToString() + "&";
                    }
                }

                if (propName.ToString() == "PESOS")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "price=" + propValue.ToString() + "&";
                    }
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
                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + "licensePlate=" + propValue.ToString() + "&";

                    }

                }

                if (propName.ToString() == "otros")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";
                    }
                }

                if (propName.ToString() == "cod_autoCH")
                {

                    datosparademotores = datosparademotores + "providerVehicleId=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "unico_dueno")
                {

                    if (propValue != null)
                    {
                        if (propValue.ToString() == "S")
                        {

                            datosparademotores = datosparademotores + "uniqueOwner=yes&";

                        }
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
                CleanAllDiccionary();
                CargaDiccionarios();
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
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
                    datosparademotores = datosparademotores + "segment=" + DicCarroceriasMotos[propValue.ToString()] + "&";
                }

                if (propName.ToString() == "color")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "color=" + ConsultaColor(DicColor, propValue.ToString()) + "&";
                    }
                }

                if (propName.ToString() == "km")
                {
                    datosparademotores = datosparademotores + "mileage=" + propValue.ToString() + "&";
                }

                if (propName.ToString() == "PESOS")
                {

                    datosparademotores = datosparademotores + "price=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "cilindrada")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + "engineSize=" + propValue.ToString() + "&";

                    }

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
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";
                    }
                }

                if (propName.ToString() == "cod_autoCH")
                {

                    datosparademotores = datosparademotores + "providerVehicleId=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "unico_dueno")
                {
                    if (propValue != null)
                    {
                        if (propValue.ToString() == "S")
                        {

                            datosparademotores = datosparademotores + "uniqueOwner=yes&";

                        }
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
                CleanAllDiccionary();
                CargaDiccionarios();
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
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
                    datosparademotores = datosparademotores + "fuel=" + ConsultaCombustible(DicCombustibles, propValue.ToString()) + "&";
                }

                if (propName.ToString() == "tipo_cambio")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "transmission=" + DicTransmision[propValue.ToString()] + "&";
                    }
                    else
                    {
                        datosparademotores = datosparademotores + "transmission=Manual&";
                    }
                    
                }

                if (propName.ToString() == "tipo_direccion")
                {
                    datosparademotores = datosparademotores + "steering=" + DicDireccion[propValue.ToString()] + "&";
                }

                if (propName.ToString() == "color")
                {
                    datosparademotores = datosparademotores + "color=" + ConsultaColor(DicColor, propValue.ToString()) + "&";
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
                    if (propValue != null)
                    {
                        if (propValue.ToString() == "S")
                        {

                            datosparademotores = datosparademotores + "uniqueOwner=yes&";

                        }
                    }

                }

                if (propName.ToString() == "motor")
                {

                    if (propValue != null)
                    {

                        datosparademotores = datosparademotores + "enginePower="+ propValue.ToString() + "&";

                    }

                }

                if (propName.ToString() == "otros")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";
                    }
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

            datosparademotores = datosparademotores + "segment=camión" + "&provider=" + provider_DM + "&key=" + key_DM + "&userId=" + useriddm + "&subtitle=" + subtitulo;

            return datosparademotores;

        }

        public static string ProcesaDatosMotorhome(IList<PropertyInfo> props, object datosvehiculo, string useriddm)
        {

            string datosparademotores = "";
            string subtitulo = "";


            foreach (PropertyInfo prop in props)
            {
                CleanAllDiccionary();
                CargaDiccionarios();
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
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
                    datosparademotores = datosparademotores + "steering=" + DicDireccion[propValue.ToString()] + "&";
                }

                if (propName.ToString() == "combustible")
                {
                    datosparademotores = datosparademotores + "fuel=" + ConsultaCombustible(DicCombustibles, propValue.ToString()) + "&";
                }

                if (propName.ToString() == "color")
                {
                    datosparademotores = datosparademotores + "color=" + ConsultaColor(DicColor, propValue.ToString()) + "&";
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
                    if (propValue != null)
                    {
                        if (propValue.ToString() == "S")
                        {

                            datosparademotores = datosparademotores + "uniqueOwner=yes&";

                        }
                    }

                }

                if (propName.ToString() == "otros")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";
                    }
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
            
            datosparademotores = datosparademotores + "segment=MOTOR HOME&provider=" + provider_DM + "&key=" + key_DM + "&userId=" + useriddm + "&subtitle=" + subtitulo;

            return datosparademotores;

        }

        public static string ProcesaDatosNavigation(IList<PropertyInfo> props, object datosvehiculo, string useriddm)
        {

            string datosparademotores = "";
            string subtitulo = "";
            
            foreach (PropertyInfo prop in props)
            {
                CleanAllDiccionary();
                CargaDiccionarios();
                object propName = prop.Name;
                object propValue = prop.GetValue(datosvehiculo, null);

                if (propName.ToString() == "idCategoria")
                {
                    datosparademotores = datosparademotores + "vehicleType=" + ConsultaCategoria(DicCategorias, propValue.ToString()) + "&";
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
                    if (propValue != null)
                    {
                        if (propValue.ToString() == "N")
                        {

                            datosparademotores = datosparademotores + "used=yes&";

                        }
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
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "description=" + propValue.ToString() + "&";
                    }
                }

                if (propName.ToString() == "cod_autoCH")
                {

                    datosparademotores = datosparademotores + "providerVehicleId=" + propValue.ToString() + "&";

                }

                if (propName.ToString() == "unico_dueno")
                {
                    if (propValue != null)
                    {
                        if (propValue.ToString() == "S")
                        {

                            datosparademotores = datosparademotores + "uniqueOwner=yes&";

                        }
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
                
                if (propName.ToString() == "Tipoveh")
                {
                    if (propValue != null)
                    {
                        datosparademotores = datosparademotores + "segment=" + DicCarroceriasNauticos[propValue.ToString()] + "&";
                    }
                }

            }

            datosparademotores = datosparademotores + "provider=" + provider_DM + "&key=" + key_DM + "&userId=" + useriddm + "&subtitle=" + subtitulo;

            return datosparademotores;

        }

        public static string[] PublicaenDeMotoresApi(string data, string codauto, string coddemotores, string accion) {

            string[] respuesta = new string[2];

            string url = url_DM;
            string responseString;

            if (accion == "3")
            {

                respuesta[0] = "Aviso con código: " + codauto + ", posee en DeMotores un estado: ";
                respuesta[1] = "finalizado.";

            } else if(accion == "0")
            {

                respuesta[0] = "Aviso con código: " + codauto + ", posee en DeMotores un: ";
                respuesta[1] = "error.";

            }
            else
            {

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

                        respuesta[0] = responseString;

                        if (!String.IsNullOrEmpty(coddemotores) && respuesta[0] == coddemotores)
                        {

                            respuesta[1] = ActualizaEstadosPublicacion(codauto, coddemotores, "2", "modificado");

                        }
                        else
                        {

                            respuesta[1] = ActualizaEstadosPublicacion(codauto, respuesta[0], "1", "publicado");

                        }

                    }
                    else
                    {
                        if (String.IsNullOrEmpty(coddemotores))
                        {

                            respuesta[0] = responseString;
                            respuesta[1] = ActualizaEstadosPublicacion(codauto, "", "0", "publicado");

                        }
                        else
                        {

                            respuesta[0] = responseString;
                            respuesta[1] = ActualizaEstadosPublicacion(codauto, coddemotores, "0", "modificado");

                        }


                    }

                }

            }

            return respuesta;
        }

        public static string EliminaenDeMotoresApi(string data)
        {
             
            string respuesta = "";

            string url = url_DM_Elimina;
            string responseString;
            string eliminardm = "provider="+provider_DM+"&key="+key_DM+"&providerVehicleId="+data;
            respuesta = eliminardm;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
                var action = Uri.EscapeUriString(url);
            
                var content = new StringContent(eliminardm, Encoding.UTF8, "application/x-www-form-urlencoded");
            
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

                imagenes = imagenes + "images=" + nums[i]+"&";

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

        public static string ActualizaEstadosPublicacion(string codauto, string coddm, string accion, string estado) {

            string data = "";
            string[] resultado = new string[3];

            ProveedorDatos PrDatos = new ProveedorDatos();
            resultado = PrDatos.ActualizaEstados(codauto, coddm, accion, estado);

            data = resultado[0];

            return data;
        }

        public static string ObtieneUidDM(string codclienteca)
        {

            string data = "";
            string[] resultado = new string[2];

            ProveedorDatos PrDatos = new ProveedorDatos();
            resultado = PrDatos.OtieneCodUsuario(codclienteca);

            data = resultado[0];

            return data;
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