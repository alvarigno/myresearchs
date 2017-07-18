using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccesoDatos.Data;
using System.Data.Entity.Core.Objects;

namespace AccesoDatos
{
    public class Program
    {

        static void Main(string[] args)
        {

        }

        public static string[] VerificaXkey(string xkey)
        {

            baseprod2Entities baseprod = new baseprod2Entities();
            ObjectParameter respuesta = new ObjectParameter("respuesta", typeof(int));
            ObjectParameter nombre = new ObjectParameter("nombre", typeof(string));
            string[] logrado = new string[2];

            var datosresultados = baseprod.SPR_Valida_xKey_Acceso_Usuario(xkey, respuesta, nombre);
            logrado[0] = respuesta.Value.ToString();
            logrado[1] = nombre.Value.ToString();


            return logrado;
        }

    }
}