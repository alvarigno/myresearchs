using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Data;
using System.Data.Entity.Core.Objects;

namespace DataAccess
{
    public class Program
    {
        static void Main(string[] args)
        {





        }


        public static string[] VerificaXkey(string xkey)
        {

            baseprodEntities baseprod = new baseprodEntities();
            ObjectParameter respuesta = new ObjectParameter("respuesta", typeof(int));
            ObjectParameter nombre = new ObjectParameter("nombre", typeof(string));
            string[] logrado = new string[2];

            var datosresultados = baseprod.SP_Valida_xKey_Acceso_Usuario(xkey, respuesta, nombre);
            //objeto de prueba
            //SP_PublicarAviso_Automotoras_Result prueba = new SP_PublicarAviso_Automotoras_Result();
            //prueba.codauto = 12345600;
            //prueba.error = 1;
            logrado[0] = respuesta.Value.ToString();
            logrado[1] = nombre.Value.ToString();


            return logrado;
        }


    }
}
