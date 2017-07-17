using WebApiAutomotoras.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccesoDatos.Data;

namespace WebApiAutomotoras.App_Code
{
    public class Util
    {
        // obtiene valores del header
        public static string getValueFromHeader(string name)
        {
            var headerKey = HttpContext.Current.Request.Headers[name] ?? "";
            string strVal = "";

            if (!string.IsNullOrEmpty(headerKey.ToString()))
                strVal = headerKey.ToString();

            return strVal;
        }

        public static int getCombustibleTabautos(string combustible)
        {
            int n;
            bool esNum = int.TryParse(combustible, out n);

            if (esNum)
                return n;

            if (!string.IsNullOrEmpty(combustible))
                combustible = combustible.ToUpper();

            switch (combustible)
            {
                case "U":
                    return 1;
                case "D":
                    return 2;
                case "N":
                    return 3;
                case "H":
                    return 4;
                case "E":
                    return 5;
                case "L":
                    return 6;
                case "P":
                    return 7;
                case "A":
                    return 8;
                case "B":
                    return 9;
                case "G":
                    return 11;
                case "F":
                    return 12;
                case "M":
                    return 13;
                default:
                    return 10;
            }
        }

        // condiciones correos credito
        //public string getCorreosCredito(CreditoVehiculo solicitud)
        //{
        //    int ano, monto;
        //    string destinatarios = "";

        //    int.TryParse(solicitud.agno, out ano);
        //    int.TryParse(solicitud.agno, out monto);

        //    // Tanner
        //    if (ano > 2008 && monto > 1499999)
        //    {
        //        destinatarios += "felipe.lorca@tanner.cl";
        //    }

        //    // Gellona
        //    else
        //    {

        //    }
        //}
    }
}