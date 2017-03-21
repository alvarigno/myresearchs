using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/**
 * @author     Allware Ltda. (http://www.allware.cl)
 * @copyright  2015 Transbank S.A. (http://www.tranbank.cl)
 * @date       May 2016
 * @license    GNU LGPL
 * @version    1.0
 */

namespace Transbank.NET.sample.certificates
{
    public class CertNormal
    {
        static private string publiccert = ConfigurationManager.AppSettings["public_cert"].ToString();
        static private string webpaycert = ConfigurationManager.AppSettings["webpay_cert"].ToString();
        static private string password = ConfigurationManager.AppSettings["password"].ToString();
        static private string commercecode = ConfigurationManager.AppSettings["commerce_code"].ToString();

        internal static Dictionary<string, string> certificate()
        {


            /** Crea un Dictionary para almacenar los datos de integración pruebas */
            Dictionary<string, string> certificate = new Dictionary<string, string>();

            /** Agregar datos de integración a Dictionary */

            String certFolder = System.Web.HttpContext.Current.Server.MapPath(".");

            /** Modo de Utilización */
            certificate.Add("environment", "INTEGRACION");

            /** Certificado Publico (Dirección fisica de certificado o contenido) */
            certificate.Add("public_cert", certFolder + "\\certificates\\"+ publiccert);

            /** Ejemplo de Ruta de Certificado de Salida */
            certificate.Add("webpay_cert", certFolder + "\\certificates\\"+ webpaycert);

            /** Ejemplo de Password de Certificado de Salida */
            certificate.Add("password", password);

            /** Codigo Comercio */
            certificate.Add("commerce_code", commercecode);

            return certificate;

        }

    }
}