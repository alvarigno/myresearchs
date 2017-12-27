using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccesoDatos.Data;
using System.Data.Entity.Core.Objects;

namespace WebApiAutomotoras.Controllers
{
    public class VerificaAcceso
    {


        public string VerificaIpAddress(string token)
        {

            Object ip = "0";

            baseprod2Entities database = new baseprod2Entities();

            ObjectParameter respuestaParam = new ObjectParameter("respuesta", typeof(bool));
            ObjectParameter ipaddressParam = new ObjectParameter("ipaddress", typeof(string));
            database.SPR_Valida_ip_x_xkey(token, respuestaParam, ipaddressParam);

            if (Boolean.Parse(respuestaParam.Value.ToString()) == true)
            {
                ip = ipaddressParam.Value;
            }
            else
            {
                HttpContext.Current.Response.AddHeader("authenticationToken", token);
                HttpContext.Current.Response.AddHeader("authenticationTokenStatus", "NotAuthorized");
                ip = "0";
            }

            return ip.ToString();
        }

        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }

    }
}