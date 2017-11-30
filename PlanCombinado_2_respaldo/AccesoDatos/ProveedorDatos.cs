using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccesoDatos.Data;
using System.Data.Entity.Core.Objects;

namespace AccesoDatos
{
    public class ProveedorDatos
    {

        public Object EntregaDatosVehiculo(string codauto) {

            //string[] datos = new string[17];

            object datosresultados = null;

            baseprod2Entities baseprod = new baseprod2Entities();
            ObjectParameter respuesta = new ObjectParameter("respuesta", typeof(string));
            ObjectParameter error = new ObjectParameter("error", typeof(int));
            string[] logrado = new string[2];

            datosresultados = baseprod.SPR_Obtiene_datos_vehiculo_publicar_DM(int.Parse(codauto), respuesta, error).FirstOrDefault();
            logrado[0] = respuesta.Value.ToString();
            logrado[1] = error.Value.ToString();

            return datosresultados;
        }

        public String[] ActualizaEstados(string codauto, string coddm, string accion, string estado) {

            string[] logrado = new string[3];
            int codigodm = 0;
            baseprod2Entities baseprod = new baseprod2Entities();
            ObjectParameter respuesta = new ObjectParameter("respuesta", typeof(string));
            ObjectParameter error = new ObjectParameter("error", typeof(int));
            ObjectParameter codigodemotores = new ObjectParameter("codigodemotores", typeof(int));

            if (String.IsNullOrEmpty(coddm))
            {
                codigodm = 0;
            }else {
                codigodm = int.Parse(coddm);
            }

            var datosresultados = baseprod.SPR_Actualiza_Estados_publicacion_DM(int.Parse(codauto), codigodm , int.Parse(accion), estado, respuesta, error,codigodemotores);
            logrado[0] = respuesta.Value.ToString();
            logrado[1] = error.Value.ToString();
            logrado[2] = codigodemotores.Value.ToString();

            return logrado;
        }

        public String[] OtieneCodUsuario(string codcliente)
        {

            string[] logrado = new string[2];
            baseprod2Entities baseprod = new baseprod2Entities();
            ObjectParameter respuestauiddm = new ObjectParameter("CodIduDM", typeof(int));
            ObjectParameter error = new ObjectParameter("error", typeof(int));

            var datosresultados = baseprod.SPR_Obtiene_CodUsuarioDM_publicar_DM(int.Parse(codcliente), respuestauiddm, error);
            logrado[0] = respuestauiddm.Value.ToString();
            logrado[1] = error.Value.ToString();

            return logrado;
        }


    }
}