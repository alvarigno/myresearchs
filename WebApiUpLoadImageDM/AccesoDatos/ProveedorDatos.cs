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


            baseprod2Entities baseprod = new baseprod2Entities();
            ObjectParameter respuesta = new ObjectParameter("respuesta", typeof(int));
            ObjectParameter error = new ObjectParameter("error", typeof(int));
            string[] logrado = new string[2];

            var datosresultados = baseprod.SPR_Obtiene_datos_vehiculo_publicar_DM(int.Parse(codauto), respuesta, error).FirstOrDefault();
            logrado[0] = respuesta.Value.ToString();
            logrado[1] = error.Value.ToString();

            return datosresultados;
        }

    }
}