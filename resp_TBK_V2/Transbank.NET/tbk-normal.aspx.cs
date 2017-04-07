using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Webpay.Transbank.Library;
using Webpay.Transbank.Library.Wsdl.Normal;
using Webpay.Transbank.Library.Wsdl.Nullify;

/**
  * @brief      Ecommerce Plugin for chilean Webpay
  * @category   Plugins/SDK
  * @author     Allware Ltda. (http://www.allware.cl)
  * @copyright  2015 Transbank S.A. (http://www.tranbank.cl)
  * @date       Jan 2015
  * @license    GNU LGPL
  * @version    1.0
  * @link       http://transbankdevelopers.cl/
  *
  * This software was created for easy integration of ecommerce
  * portals with Transbank Webpay solution.
  *
  * Required:
  *  - .NET Framework 4.5
  *
  * Changelog:
  *  - v1.0 Original release
  *
  * See documentation and how to install at link site
  *
  */

namespace Transbank.NET
{

    public partial class tbk_normal : System.Web.UI.Page
    {

        /** Mensaje de Ejecución */
        private string message;

        private string[] keys;

        /** Crea Dictionary con datos Integración Pruebas */
        private Dictionary<string, string> certificate = Transbank.NET.sample.certificates.CertNormal.certificate();

        /** Crea Dictionary con datos de entrada */
        private Dictionary<string, string> request = new Dictionary<string, string>();

        Dictionary<string, string> responseTBK = new Dictionary<string, string>();
        Dictionary<string, string> requestTBK = new Dictionary<string, string>();
        wsInitTransactionOutput resultresponseTBK;
        string tipotbk = "";

        string urldominio = "";

        static private string urliniciotbk = ConfigurationManager.AppSettings["urlinittbk"].ToString();

        protected void Page_Load()
        {

            Webpay.Transbank.Library.Configuration configuration = new Webpay.Transbank.Library.Configuration();
            configuration.Environment = certificate["environment"];
            configuration.CommerceCode = certificate["commerce_code"];
            configuration.PublicCert = certificate["public_cert"];
            configuration.WebpayCert = certificate["webpay_cert"];
            configuration.Password = certificate["password"];

            /** Creacion Objeto Webpay */
            Webpay.Transbank.Library.Webpay webpay = new Webpay.Transbank.Library.Webpay(configuration);

            /** Información de Host para crear URL */
            String httpHost = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            String selfURL = System.Web.HttpContext.Current.Request.ServerVariables["URL"].ToString();

            string action = !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["action"]) ? HttpContext.Current.Request.QueryString["action"] : "init";

            /** Crea URL de Aplicación */
            string baseurl = "http://" + httpHost + selfURL;

            urldominio = "http://" + httpHost;
            //urldominio = "http://desarrollo.chileautos.cl";//desarrollo
            //urldominio = "https://operaciones.chileautos.cl";

            /** Crea Dictionary con descripción */
            Dictionary<string, string> description = new Dictionary<string, string>();

            description.Add("VD", "Venta Deb&iacute;to");
            description.Add("VN", "Venta Normal");
            description.Add("VC", "Venta en cuotas");
            description.Add("SI", "cuotas sin inter&eacute;s");
            description.Add("S2", "2 cuotas sin inter&eacute;s");
            description.Add("NC", "N cuotas sin inter&eacute;s");

            /** Crea Dictionary con descripción tipo de cuotas*/
            Dictionary<string, string> decripciontipocoutas = new Dictionary<string, string>();

            decripciontipocoutas.Add("VD", "Venta Debito");
            decripciontipocoutas.Add("VN", "Sin Cuotas");
            decripciontipocoutas.Add("VC", "Cuotas Normales");
            decripciontipocoutas.Add("SI", "Sin inter&eacute;s");
            decripciontipocoutas.Add("S2", "Sin inter&eacute;s");
            decripciontipocoutas.Add("NC", "Sin inter&eacute;s");

            /** Crea Dictionary con descripción tipo de pago*/
            Dictionary<string, string> decripciontipopago = new Dictionary<string, string>();

            decripciontipopago.Add("VD", "D&eacute;bito");
            decripciontipopago.Add("VN", "Cr&eacute;dito");
            decripciontipopago.Add("VC", "Cr&eacute;dito");
            decripciontipopago.Add("SI", "Cr&eacute;dito");
            decripciontipopago.Add("S2", "Cr&eacute;dito");
            decripciontipopago.Add("NC", "Cr&eacute;dito");


            /** Crea Dictionary con codigos de resultado */
            Dictionary<string, string> codes = new Dictionary<string, string>();

            codes.Add("0", "Transacci&oacute;n aprobada");
            codes.Add("-1", "Rechazo de transacci&oacute;n");
            codes.Add("-2", "Transacci&oacute;n debe reintentarse");
            codes.Add("-3", "Error en transacci&oacute;n");
            codes.Add("-4", "Rechazo de transacci&oacute;n");
            codes.Add("-5", "Rechazo por error de tasa");
            codes.Add("-6", "Excede cupo m&aacute;ximo mensual");
            codes.Add("-7", "Excede l&iacute;mite diario por transacci&oacute;n");
            codes.Add("-8", "Rubro no autorizado");



            string buyOrder;

            string tx_step = "";

            //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 200%;'>Ejemplos Webpay - Transacci&oacute;n Normal</p>");

            string currentDirName = System.IO.Directory.GetCurrentDirectory();
            //Response.Write("currentDirName: " + currentDirName);
            //Response.Write("Cetificado publico: " + configuration.PublicCert + "<br />");
            //Response.Write("Cetificado WebPay: " + configuration.WebpayCert + "<br />");

            keys = Request.Form.AllKeys;
            if (action == "init")
            {
                //Session["OC"] = Request.Form[keys[6]]; ;

                HttpCookie ChCookie = new HttpCookie("ChileautosurlFormOrigenTBK");
                ChCookie.Values.Add("urlformorigen", Request.Form[keys[14]]);
                Response.Cookies.Add(ChCookie);

            }


            string htmlinicio = "<link rel='stylesheet' type='text/css' href='https://www.chileautos.cl/Content/assets/css/chileautos.css'>" +
             "<div id='voucherarea' class='container' style='width:500px;height:700px;'>" +
             "<div class='col-xs-12 col-md-12 u-bg-gray-light'>" +
             "<div style='text-align:center;margin:0 auto;display:block;margin-bottom:15px;'>" +
             "<img src='https://www.chileautos.cl/Content/assets/img/logos/logo-caption.svg' style='width:140px;' border='0'>" +
             "<h3> PAGO CON TRANSBANK</h3>" +
             "<img src='imagenes/logos/tarjetas_webpay.gif' border='0' class='' style='width:120px;'>" +
             "<img src='imagenes/logos/WPP2.jpg' border='0' class='' style='width:70px;'>" +
             "</div>" +
             "<div class='col-sm-12'>" +
            "<div class='tab-pane active fade in'>";

            //HttpContext.Current.Response.Write("</br><a href='https://operaciones.chileautos.cl/pago_v2.asp?i=0'>&laquo; volver a index</a>");


            string htmlenlaceformulario = "<script type='text/javascript'>" +
            "function cargapagereturnlink(){ window.open('" + Request.Cookies["ChileautosurlFormOrigenTBK"]["urlformorigen"] + "', '_self');}" +
            "</script>" +
            "</br><a onclick='cargapagereturnlink()' href='javascript:void(0);'>&laquo; volver al formulario de transacciones.</a>";

            string htmlfin = "</div>" +
            "</div>" +
            "</div>" +
            "</div>";

            string stylebody = "<link rel='stylesheet' type='text/css' href='https://www.chileautos.cl/Content/assets/css/chileautos.css'>";



            switch (action)
            {

                default:

                    tx_step = "Init";

                    try
                    {
                        HttpContext.Current.Response.Write(htmlinicio);
                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");

                        /*Valida dominio de url*/
                        string URLOrigen = Request.Form[keys[13]];
                        string URLDestino = Request.Form[keys[14]];

                        Uri myUri = new Uri(URLDestino);
                        string host = myUri.Host;

                        if (validaurlorigen(URLOrigen) == false)
                        {
                            Response.Redirect("https://www.chileautos.cl/error", true);
                        }

                        if (validaurlorigen(host) == false)
                        {
                            Response.Redirect("https://www.chileautos.cl/error", true);
                        }

                        crearcookiedatospost(URLDestino);
                        /*Fin Valida dominio de url*/

                        Random random = new Random();


                        //for(int i=0; i<keys.Length; i++) {

                        //   HttpContext.Current.Response.Write(keys[i]+": "+Request.Form[keys[i]]+"<br />");

                        //}



                        /** Monto de la transacción */
                        //decimal amount = System.Convert.ToDecimal("9990");
                        decimal amount = System.Convert.ToDecimal(Request.Form[keys[5]]);

                        DateTime now = DateTime.Now;
                        /** Orden de compra de la tienda */
                        //buyOrder = "OC_"+ now.Year+ now.Month+ now.Day+ now.Hour+ now.Minute+ now.Second;
                        buyOrder = Request.Form[keys[6]];

                        /** (Opcional) Identificador de sesión, uso interno de comercio */
                        //string sessionId = random.Next(0, 1000).ToString();
                        string sessionId = Request.Form[keys[6]];

                        /** URL Final */
                        baseurl = Request.Form[keys[11]];
                        string urlReturn = baseurl + "?action=result";

                        /** URL Final */
                        string urlFinal = baseurl + "?action=end";

                        /*manejo de datos*/
                        decimal d = 0;
                        decimal.TryParse(Request.Form[keys[5]], out d);


                        /** Crera y valida la OC en Base de datos **/
                        DateTime nowtest = DateTime.Now;
                        /** Orden de compra de la tienda */
                        string numocprueba = "OC_" + nowtest.Year + nowtest.Month + nowtest.Day + nowtest.Hour + nowtest.Minute + nowtest.Second + nowtest.Millisecond;
                        //string numocprueba = "OC_2017322144439225"; //Prueba que Comprueba si la oc existe previamente

                        //Session["OC"] = numocprueba;

                        /*Fin manejo de datos*/

                        string curFile = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\logsrequest\\" + numocprueba + "_log.txt";

                        if (File.Exists(curFile))
                        {

                            Response.Redirect("https://www.chileautos.cl/error", true);

                        }

                        if (!checkOcexiste(numocprueba))
                        {

                            if (insertaOC(numocprueba, Request.Form[keys[0]], Request.Form[keys[1]], Request.Form[keys[2]], Request.Form[keys[3]], Request.Form[keys[4]], Request.Form[keys[5]] + "0", Request.Form[keys[5]]))
                            {
                                //HttpContext.Current.Response.Write("Registrada con éxito.");
                                buyOrder = numocprueba;
                                sessionId = numocprueba;
                                Session["OC"] = buyOrder;

                                /*inicia sesion en TBK y obtiene token*/
                                wsInitTransactionInput uno = new wsInitTransactionInput();

                                tipotbk = uno.wSTransactionType.ToString();

                                request.Add("wsTransactionType", tipotbk);
                                request.Add("comerceId", configuration.CommerceCode);
                                request.Add("amount", amount.ToString());
                                request.Add("buyOrder", buyOrder.ToString());
                                request.Add("sessionId", sessionId.ToString());
                                request.Add("urlReturn", urlReturn.ToString());
                                request.Add("urlFinal", urlFinal.ToString());


                                /** Ejecutamos metodo initTransaction desde Libreria */
                                wsInitTransactionOutput result = webpay.getNormalTransaction().initTransaction(amount, buyOrder, sessionId, urlReturn, urlFinal);

                                string urlrespuesta = result.url;

                                string PublicCertuno = configuration.PublicCert;
                                string WebpayCertdos = configuration.WebpayCert;
                                /** Fin Ejecutamos metodo initTransaction desde Libreria */



                                /** Verificamos respuesta de inicio en webpay */
                                if (result.token != null && result.token != "" && urlrespuesta == urliniciotbk)
                                {

                                    //message = "Sesion iniciada con exito en Webpay";
                                    //HttpContext.Current.Response.Write("" + message + "</br></br>");

                                    HttpContext.Current.Response.Write("<script type='text/javascript'> window.onload = function(){document.forms['procesartbkpago'].submit()}</script>");
                                    HttpContext.Current.Response.Write("<form name='procesartbkpago' action=" + result.url + " method='post'><input type='hidden' name='token_ws' value=" + result.token + ">");
                                    HttpContext.Current.Response.Write("<div style='text-align:center;margin:0 auto;display:block;margin-bottom:15px;'>");
                                    HttpContext.Current.Response.Write("<img src='imagenes/icon/procesando_solicitud.gif' style='width:50%; text-align:center;' /><br />");
                                    HttpContext.Current.Response.Write("<span>Procesando Solicitud</span>");
                                    HttpContext.Current.Response.Write("</div>");
                                    //HttpContext.Current.Response.Write("<input type='submit' class='btn btn-success btn-large btn-block IdClassBtnEnviar' value='Continuar &raquo;' />");
                                    HttpContext.Current.Response.Write("</form>");


                                    /** creamos el log del mensaje de envío y su respuesta */
                                    createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result), tx_step, buyOrder);

                                }
                                else
                                {
                                    message = "webpay no disponible";
                                    HttpContext.Current.Response.Write("" + message + "</br></br>");
                                }

                                /*Fin inicia sesion en TBK y obtiene token*/

                            }
                            else
                            {

                                HttpContext.Current.Response.Write("Oc con problemas en insertar.");
                            }

                        }
                        else
                        {
                            HttpContext.Current.Response.Write("Núm. Orden de Compra: " + numocprueba + ", ya existe.");
                            HttpContext.Current.Response.Write("Oc ya existe.");
                            HttpContext.Current.Response.Write("<button class='btn btn-warning btn-large btn-block IdClassBtnEnviar' onclick='javascript:location.reload();'>Recargue el formulario</button>");

                        }
                        /*Fin*/

                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");

                        HttpContext.Current.Response.Write(htmlenlaceformulario);
                        HttpContext.Current.Response.Write(htmlfin);

                    }
                    catch (Exception ex)
                    {
                        //HttpContext.Current.Response.Write(htmlinicio);

                        HttpContext.Current.Response.Write("<div>");
                        HttpContext.Current.Response.Write("<h4 style='color:#ff0000;'>Transacción Rechazada<br /><span>Núm.Orden: </span><strong>" + Session["OC"] + "</strong></h4>");
                        HttpContext.Current.Response.Write("<script type='text/javascript'>function cargapagereturnlinkBtn(){ window.open('" + Request.Cookies["ChileautosurlFormOrigenTBK"]["urlformorigen"] + "', '_self');}</script>");
                        HttpContext.Current.Response.Write("<table class='table table-striped table-hover'>");
                        //HttpContext.Current.Response.Write("<tr><td style='color:#009933;'><h4>Error</h4></td></tr>");
                        //HttpContext.Current.Response.Write("<tr><td></td></tr>");
                        HttpContext.Current.Response.Write("<tr><td>");
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; '><strong>Descrpción</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                        //HttpContext.Current.Response.Write("Favor de completar el formulario con datos correctos.");
                        HttpContext.Current.Response.Write("</td></tr>");
                        HttpContext.Current.Response.Write("<tr><td><button class='btn btn-warning btn-large btn-block' onclick='cargapagereturnlinkBtn()' >Regresar al formulario de pago.</button></td></tr>");
                        HttpContext.Current.Response.Write("</table>");
                        HttpContext.Current.Response.Write("</div>");
                        HttpContext.Current.Response.Write(htmlenlaceformulario);
                        HttpContext.Current.Response.Write(htmlfin);
                        /** creamos el log del mensaje de envío y su respuesta del error */
                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), ex.Message, tx_step, Session["OC"].ToString());
                    }

                    break;

                case "result":

                    tx_step = "Get Result";

                    try
                    {



                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Resultado de Transacción</p>");

                        /** Obtiene Información POST */
                        string[] keysPost = Request.Form.AllKeys;

                        /** Token de la transacción */
                        string token = Request.Form["token_ws"];

                        request.Add("token", token.ToString());

                        transactionResultOutput result = webpay.getNormalTransaction().getTransactionResult(token);

                        string idsession = result.sessionId;

                        string urlrespuesta = result.urlRedirection;

                        wsInitTransactionInput uno = new wsInitTransactionInput();

                        tipotbk = uno.wSTransactionType.ToString();
                        string urlfinalEndPoint = uno.finalURL;

                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br> " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");

                        if (result.detailOutput[0].responseCode == 0)
                        {
                            message = "Pago ACEPTADO por webpay (se deben guardar datos para mostrar voucher)";

                            //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Resultado de Transacción</p>");

                            //HttpContext.Current.Response.Write("<script>localStorage.setItem('authorizationCode', " + result.detailOutput[0].authorizationCode + ")</script>");
                            //HttpContext.Current.Response.Write("<script>localStorage.setItem('commercecode', " + result.detailOutput[0].commerceCode + ")</script>");
                            //HttpContext.Current.Response.Write("<script>localStorage.setItem('amount', " + result.detailOutput[0].amount + ")</script>");
                            //HttpContext.Current.Response.Write("<script>localStorage.setItem('buyOrder', " + result.detailOutput[0].buyOrder + ")</script>");

                            //requestTBK.Add("authorizationCode", result.detailOutput[0].authorizationCode);
                            //requestTBK.Add("commercecode", result.detailOutput[0].commerceCode);
                            //requestTBK.Add("amount", result.detailOutput[0].amount.ToString());
                            //requestTBK.Add("buyOrder", result.detailOutput[0].buyOrder);

                            //string datosresoult = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result.detailOutput);

                            HttpCookie ChCookie = new HttpCookie("ChileautosSettingTBK");
                            ChCookie.Values.Add("accountingDate", result.accountingDate);
                            ChCookie.Values.Add("buyOrder", result.detailOutput[0].buyOrder);
                            ChCookie.Values.Add("Cardnumber", result.cardDetail.cardNumber);
                            ChCookie.Values.Add("CardExpirationDate", result.cardDetail.cardExpirationDate);
                            ChCookie.Values.Add("authorizationCode", result.detailOutput[0].authorizationCode);
                            ChCookie.Values.Add("paymentTypeCode", result.detailOutput[0].paymentTypeCode);
                            ChCookie.Values.Add("responseCode", result.detailOutput[0].responseCode.ToString());
                            ChCookie.Values.Add("sharesNumber", result.detailOutput[0].sharesNumber.ToString());
                            ChCookie.Values.Add("amount", result.detailOutput[0].amount.ToString());
                            ChCookie.Values.Add("commerceCode", result.detailOutput[0].commerceCode);
                            ChCookie.Values.Add("sessionId", result.sessionId);
                            ChCookie.Values.Add("transactionDate", result.transactionDate.ToString());
                            ChCookie.Values.Add("transactionDateSpecified", result.transactionDateSpecified.ToString());
                            ChCookie.Values.Add("VCI", result.VCI);
                            ChCookie.Values.Add("detalletransaccion", result.detailOutput[0].amount + "-" + result.detailOutput[0].authorizationCode + "-" + result.detailOutput[0].buyOrder + "-" + result.detailOutput[0].commerceCode + "-" + result.detailOutput[0].paymentTypeCode + "-" + result.detailOutput[0].responseCode + "-" + result.detailOutput[0].sharesAmount + "-" + result.detailOutput[0].sharesAmountSpecified + "-" + result.detailOutput[0].sharesNumber + "-" + result.detailOutput[0].sharesNumberSpecified);
                            ChCookie.Values.Add("resoluciontransaccion", HttpUtility.HtmlEncode(result.cardDetail));
                            Response.Cookies.Add(ChCookie);

                            //HttpContext.Current.Response.Write("Día: "+result.transactionDate.Day+"<br />");
                            //HttpContext.Current.Response.Write("Mes: "+result.transactionDate.Month + "<br />");
                            //HttpContext.Current.Response.Write("Año: "+result.transactionDate.Year + "<br />");
                            //HttpContext.Current.Response.Write("Hora: "+result.transactionDate.Hour + "<br />");
                            //HttpContext.Current.Response.Write("Minutos: "+result.transactionDate.Minute + "<br />");
                            //HttpContext.Current.Response.Write("Segundos: "+result.transactionDate.Second + "<br />");
                            //HttpContext.Current.Response.Write("Milesimas: "+result.transactionDate.Millisecond + "<br />");
                            //HttpContext.Current.Response.Write("Detalle salida: "+ new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result.detailOutput) + "<br />");
                            //HttpContext.Current.Response.Write("idTransaccion: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request["token"]) + "<br />");

                            Dictionary<string, string> detalletransaccion = new Dictionary<string, string>();
                            detalletransaccion.Add("idOC", result.detailOutput[0].buyOrder);
                            detalletransaccion.Add("tipo_transaccion", tipotbk);
                            detalletransaccion.Add("TBK_Respuesta", result.detailOutput[0].responseCode.ToString());
                            detalletransaccion.Add("TBK_Monto", result.detailOutput[0].amount.ToString());
                            detalletransaccion.Add("TBK_cod_autorizacion", result.detailOutput[0].authorizationCode);
                            detalletransaccion.Add("TBK_final_tarjeta", result.cardDetail.cardNumber.ToString());
                            detalletransaccion.Add("TBK_fecha_contable", result.accountingDate);
                            detalletransaccion.Add("TBK_fecha_transaccion", result.accountingDate);
                            detalletransaccion.Add("TBK_hora_transaccion", result.transactionDate.Hour.ToString() + result.transactionDate.Minute.ToString() + result.transactionDate.Second.ToString());
                            detalletransaccion.Add("TBK_id_session", result.sessionId);
                            detalletransaccion.Add("TBK_id_transaccion", result.sessionId);
                            detalletransaccion.Add("TBK_tipo_pago", result.detailOutput[0].paymentTypeCode);
                            detalletransaccion.Add("TBK_numero_cuotas", result.detailOutput[0].sharesNumber.ToString());
                            detalletransaccion.Add("TBK_tasa_interes_max", "");
                            detalletransaccion.Add("fecha_f_trans", result.transactionDate.ToString());

                            /** creamos el log del mensaje de envío y su respuesta */
                            createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result), tx_step, result.detailOutput[0].buyOrder);

                            if (VerificaOCExiste(result.detailOutput[0].buyOrder))
                            {
                                HttpContext.Current.Response.Write(htmlinicio);

                                HttpContext.Current.Response.Write("<div>");
                                HttpContext.Current.Response.Write("<h4 style='color:#ff0000;'>Transacción Rechazada<br /><span>Núm.Orden: </span><strong>" + result.detailOutput[0].buyOrder + "</strong></h4>");
                                HttpContext.Current.Response.Write("<script type='text/javascript'>function cargapagereturnlinkBtn(){ window.open('" + Request.Cookies["ChileautosurlFormOrigenTBK"]["urlformorigen"] + "', '_self');}</script>");
                                HttpContext.Current.Response.Write("<table class='table table-striped table-hover'>");
                                HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td colspan='2'><p>Las posibles causas de este rechazo son:</p>");
                                HttpContext.Current.Response.Write("<ul><li>Error en el ingreso de los datos de su tarjeta de Crédito o Débito (fecha y/o Código de seguridad).</li>");
                                HttpContext.Current.Response.Write("<li>Su tarjeta de crédito o Débito no cuenta con el cupo necesario parta cancelar la compra.</li>");
                                HttpContext.Current.Response.Write("<li>Tarjeta aún no habilitada en el sistema financiero.</li>");
                                HttpContext.Current.Response.Write("</ul></td></tr>");
                                HttpContext.Current.Response.Write("<tr><td></td></tr>");
                                HttpContext.Current.Response.Write("<tr><td><button class='btn btn-warning btn-large btn-block'  onclick='cargapagereturnlinkBtn()' >Regresar al formulario de pago.</button></td></tr>");
                                HttpContext.Current.Response.Write("</table>");
                                //HttpContext.Current.Response.Write("OC número: " + result.detailOutput[0].buyOrder + "<br />");
                                //HttpContext.Current.Response.Write("Ya está autorizada. <br /> Favor de intentarlo de nuevo.<br /><br />");
                                HttpContext.Current.Response.Write("</div>");
                                HttpContext.Current.Response.Write("<br />");
                                HttpContext.Current.Response.Write(htmlenlaceformulario);
                                HttpContext.Current.Response.Write(htmlfin);

                                tx_step = tx_step + " Sin llamda a acknowledgeTransaction " + token;
                                /** creamos el log del acknowledgeTransaction de envío y su respuesta */
                                createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result), tx_step, result.detailOutput[0].buyOrder);


                            }
                            else
                            {

                                //HttpContext.Current.Response.Write(message + "</br></br>");
                                HttpContext.Current.Response.Write(stylebody);
                                HttpContext.Current.Response.Write("<div style='position:absolute; top:0; left:0; width:100%; height:100%; background:url(\'https://webpay3g.transbank.cl/webpayserver/imagenes/background.gif\');'>");
                                HttpContext.Current.Response.Write("<script type='text/javascript'> window.onload = function(){document.forms['acknowledgeTransaction'].submit()}</script>");
                                HttpContext.Current.Response.Write("<form action=" + result.urlRedirection + " name='acknowledgeTransaction' method='post'><input type='hidden' name='token_ws' value=" + token + ">");
                                //HttpContext.Current.Response.Write("<input type='submit' class='btn btn-success btn-large btn-block IdClassBtnEnviar' value='Continuar &raquo;'>");
                                HttpContext.Current.Response.Write("</form>");
                                HttpContext.Current.Response.Write("</div>");

                                tx_step = tx_step + " Con llamda a acknowledgeTransaction, toke: " + token;
                                /** creamos el log del mensaje de envío y su respuesta */
                                createlog(token, "", tx_step, result.detailOutput[0].buyOrder);

                                /** creamos el update de la transacción en BD */
                                UpDateEstadoDocumento(detalletransaccion);

                            }

                        }
                        else
                        {

                            message = "Pago RECHAZADO por webpay <br />Código : " + result.detailOutput[0].responseCode + "<br /> Descripción: " + codes[result.detailOutput[0].responseCode.ToString()];

                            //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Resultado de Transacción</p>");

                            HttpCookie ChCookie = new HttpCookie("ChileautosSettingTBK");
                            ChCookie.Values.Add("accountingDate", result.accountingDate);
                            ChCookie.Values.Add("buyOrder", result.detailOutput[0].buyOrder);
                            ChCookie.Values.Add("Cardnumber", result.cardDetail.cardNumber);
                            ChCookie.Values.Add("CardExpirationDate", result.cardDetail.cardExpirationDate);
                            ChCookie.Values.Add("authorizationCode", result.detailOutput[0].authorizationCode);
                            ChCookie.Values.Add("paymentTypeCode", result.detailOutput[0].paymentTypeCode);
                            ChCookie.Values.Add("responseCode", result.detailOutput[0].responseCode.ToString());
                            ChCookie.Values.Add("sharesNumber", result.detailOutput[0].sharesNumber.ToString());
                            ChCookie.Values.Add("amount", result.detailOutput[0].amount.ToString());
                            ChCookie.Values.Add("commerceCode", result.detailOutput[0].commerceCode);
                            ChCookie.Values.Add("sessionId", result.sessionId);
                            ChCookie.Values.Add("transactionDate", result.transactionDate.ToString());
                            ChCookie.Values.Add("transactionDateSpecified", result.transactionDateSpecified.ToString());
                            ChCookie.Values.Add("VCI", result.VCI);
                            ChCookie.Values.Add("detalletransaccion", result.detailOutput[0].amount + "-" + result.detailOutput[0].authorizationCode + "-" + result.detailOutput[0].buyOrder + "-" + result.detailOutput[0].commerceCode + "-" + result.detailOutput[0].paymentTypeCode + "-" + result.detailOutput[0].responseCode + "-" + result.detailOutput[0].sharesAmount + "-" + result.detailOutput[0].sharesAmountSpecified + "-" + result.detailOutput[0].sharesNumber + "-" + result.detailOutput[0].sharesNumberSpecified);
                            ChCookie.Values.Add("resoluciontransaccion", HttpUtility.HtmlEncode(result.cardDetail));
                            Response.Cookies.Add(ChCookie);

                            Dictionary<string, string> detalletransaccion = new Dictionary<string, string>();
                            detalletransaccion.Add("idOC", result.detailOutput[0].buyOrder);
                            detalletransaccion.Add("tipo_transaccion", tipotbk);
                            detalletransaccion.Add("TBK_Respuesta", result.detailOutput[0].responseCode.ToString());
                            detalletransaccion.Add("TBK_Monto", result.detailOutput[0].amount.ToString());
                            detalletransaccion.Add("TBK_cod_autorizacion", result.detailOutput[0].authorizationCode);
                            detalletransaccion.Add("TBK_final_tarjeta", result.cardDetail.cardNumber.ToString());
                            detalletransaccion.Add("TBK_fecha_contable", result.accountingDate);
                            detalletransaccion.Add("TBK_fecha_transaccion", result.accountingDate);
                            detalletransaccion.Add("TBK_hora_transaccion", result.transactionDate.Hour.ToString() + result.transactionDate.Minute.ToString() + result.transactionDate.Second.ToString());
                            detalletransaccion.Add("TBK_id_session", result.sessionId);
                            detalletransaccion.Add("TBK_id_transaccion", result.sessionId);
                            detalletransaccion.Add("TBK_tipo_pago", result.detailOutput[0].paymentTypeCode);
                            detalletransaccion.Add("TBK_numero_cuotas", result.detailOutput[0].sharesNumber.ToString());
                            detalletransaccion.Add("TBK_tasa_interes_max", "");
                            detalletransaccion.Add("fecha_f_trans", result.transactionDate.ToString());

                            /** creamos el log del mensaje de envío y su respuesta */
                            createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result), tx_step, result.detailOutput[0].buyOrder);

                            if (VerificaOCExiste(result.detailOutput[0].buyOrder))
                            {
                                HttpContext.Current.Response.Write(htmlinicio);

                                HttpContext.Current.Response.Write("<div>");
                                HttpContext.Current.Response.Write("<h4 style='color:#ff0000;'>Transacción Rechazada<br /><span>Núm.Orden: </span><strong>" + result.detailOutput[0].buyOrder + "</strong></h4>");
                                HttpContext.Current.Response.Write("<script type='text/javascript'>function cargapagereturnlinkBtn(){ window.open('" + Request.Cookies["ChileautosurlFormOrigenTBK"]["urlformorigen"] + "', '_self');}</script>");
                                HttpContext.Current.Response.Write("<table class='table table-striped table-hover'>");
                                HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td colspan='2'><p>Las posibles causas de este rechazo son:</p>");
                                HttpContext.Current.Response.Write("<ul><li>Error en el ingreso de los datos de su tarjeta de Crédito o Débito (fecha y/o Código de seguridad).</li>");
                                HttpContext.Current.Response.Write("<li>Su tarjeta de crédito o Débito no cuenta con el cupo necesario parta cancelar la compra.</li>");
                                HttpContext.Current.Response.Write("<li>Tarjeta aún no habilitada en el sistema financiero.</li>");
                                HttpContext.Current.Response.Write("</ul></td></tr>");
                                HttpContext.Current.Response.Write("<tr><td></td></tr>");
                                HttpContext.Current.Response.Write("<tr><td><button class='btn btn-warning btn-large btn-block'  onclick='cargapagereturnlinkBtn()' >Regresar al formulario de pago.</button></td></tr>");
                                HttpContext.Current.Response.Write("</table>");
                                //HttpContext.Current.Response.Write("OC número: " + result.detailOutput[0].buyOrder + "<br />");
                                //HttpContext.Current.Response.Write("Ya está autorizada. <br /> Favor de intentarlo de nuevo.<br /><br />");
                                HttpContext.Current.Response.Write("</div>");
                                HttpContext.Current.Response.Write("<br />");
                                HttpContext.Current.Response.Write(htmlenlaceformulario);
                                HttpContext.Current.Response.Write(htmlfin);

                                tx_step = tx_step + " Sin llamda a acknowledgeTransaction ";
                                /** creamos el log del acknowledgeTransaction de envío y su respuesta */
                                createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result), tx_step, result.detailOutput[0].buyOrder);

                            }
                            else
                            {

                                //HttpContext.Current.Response.Write(message + "</br></br>");
                                HttpContext.Current.Response.Write(stylebody);
                                HttpContext.Current.Response.Write("<div style='position:absolute; top:0; left:0; width:100%; height:100%; background:url(\'https://webpay3g.transbank.cl/webpayserver/imagenes/background.gif\');'>");
                                HttpContext.Current.Response.Write("<script type='text/javascript'> window.onload = function(){document.forms['acknowledgeTransaction'].submit()}</script>");
                                HttpContext.Current.Response.Write("<form action=" + result.urlRedirection + " name='acknowledgeTransaction' method='post'><input type='hidden' name='token_ws' value=" + token + ">");
                                //HttpContext.Current.Response.Write("<input type='submit' class='btn btn-success btn-large btn-block IdClassBtnEnviar' value='Continuar &raquo;'>");
                                HttpContext.Current.Response.Write("</form>");
                                HttpContext.Current.Response.Write("</div>");

                                tx_step = tx_step + " Con llamda a acknowledgeTransaction, toke: " + token;
                                /** creamos el log del mensaje de envío y su respuesta */
                                createlog(token, "", tx_step, result.detailOutput[0].buyOrder);

                                /** creamos el update de la transacción en BD */
                                UpDateEstadoDocumento(detalletransaccion);

                            }

                        }



                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write(htmlinicio);
                        HttpContext.Current.Response.Write("<div>");
                        HttpContext.Current.Response.Write("<h4 style='color:#ff0000;'>Transacción Rechazada<br /><span>Núm.Orden: </span><strong>" + Session["OC"] + "</strong></h4>");
                        HttpContext.Current.Response.Write("<script type='text/javascript'>function cargapagereturnlinkBtn(){ window.open('" + Request.Cookies["ChileautosurlFormOrigenTBK"]["urlformorigen"] + "', '_self');}</script>");
                        HttpContext.Current.Response.Write("<table class='table table-striped table-hover'>");
                        HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td colspan='2'><p>Las posibles causas de este rechazo son:</p>");
                        HttpContext.Current.Response.Write("<ul><li>Error en el ingreso de los datos de su tarjeta de Crédito o Débito (fecha y/o Código de seguridad).</li>");
                        HttpContext.Current.Response.Write("<li>Su tarjeta de crédito o Débito no cuenta con el cupo necesario parta cancelar la compra.</li>");
                        HttpContext.Current.Response.Write("<li>Tarjeta aún no habilitada en el sistema financiero.</li>");
                        HttpContext.Current.Response.Write("</ul></td></tr>");
                        HttpContext.Current.Response.Write("<tr><td></td></tr>");
                        HttpContext.Current.Response.Write("<tr><td><button class='btn btn-warning btn-large btn-block' onclick='cargapagereturnlinkBtn()' >Regresar al formulario de pago.</button></td></tr>");
                        HttpContext.Current.Response.Write("</table>");
                        HttpContext.Current.Response.Write("</div>");
                        HttpContext.Current.Response.Write("<br />");
                        HttpContext.Current.Response.Write(htmlenlaceformulario);
                        HttpContext.Current.Response.Write(htmlfin);

                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>Descripción</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                        /** creamos el log del mensaje de envío y su respuesta */
                        /** fecha del error */
                        DateTime now = DateTime.Now;
                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), ex.Message, tx_step, "error_" + now.Year + now.Month + now.Day + now.Hour + now.Minute + now.Second);
                    }

                    break;

                case "end":

                    tx_step = "End";

                    try
                    {

                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Fin de Proceso</p>");

                        string urldestpost = "";

                        /*Recupera la URL Retorno*/
                        if (Request.Cookies["ChileautosUrlDestino"] != null)
                        {
                            if (Request.Cookies["ChileautosUrlDestino"]["URLRetorno"] != null)
                            {
                                urldestpost = Request.Cookies["ChileautosUrlDestino"]["URLRetorno"];
                            }
                        }


                        if (Request.Cookies["ChileautosSettingTBK"] != null)
                        {
                            if (Request.Cookies["ChileautosSettingTBK"]["authorizationCode"] != null)
                            {

                                responseTBK.Add("accountingDate", Request.Cookies["ChileautosSettingTBK"]["accountingDate"]);
                                responseTBK.Add("buyOrder", Request.Cookies["ChileautosSettingTBK"]["buyOrder"]);
                                responseTBK.Add("Cardnumber", Request.Cookies["ChileautosSettingTBK"]["Cardnumber"]);
                                responseTBK.Add("CardExpirationDate", Request.Cookies["ChileautosSettingTBK"]["CardExpirationDate"]);
                                responseTBK.Add("authorizationCode", Request.Cookies["ChileautosSettingTBK"]["authorizationCode"]);
                                responseTBK.Add("paymentTypeCode", Request.Cookies["ChileautosSettingTBK"]["paymentTypeCode"]);
                                responseTBK.Add("responseCode", Request.Cookies["ChileautosSettingTBK"]["responseCode"]);
                                responseTBK.Add("sharesNumber", Request.Cookies["ChileautosSettingTBK"]["sharesNumber"]);
                                responseTBK.Add("amount", Request.Cookies["ChileautosSettingTBK"]["amount"]);
                                responseTBK.Add("commerceCode", Request.Cookies["ChileautosSettingTBK"]["commerceCode"]);
                                responseTBK.Add("sessionId", Request.Cookies["ChileautosSettingTBK"]["sessionId"]);
                                responseTBK.Add("transactionDate", Request.Cookies["ChileautosSettingTBK"]["transactionDate"]);
                                responseTBK.Add("transactionDateSpecified", Request.Cookies["ChileautosSettingTBK"]["transactionDateSpecified"]);
                                responseTBK.Add("VCI", Request.Cookies["ChileautosSettingTBK"]["VCI"]);
                                responseTBK.Add("detalletransaccion", Request.Cookies["ChileautosSettingTBK"]["detalletransaccion"]);
                                responseTBK.Add("resoluciontransaccion", HttpUtility.HtmlDecode(Request.Cookies["ChileautosSettingTBK"]["resoluciontransaccion"]));

                                string codpago = "";
                                if (decripciontipopago.ContainsKey(responseTBK["paymentTypeCode"]))
                                {
                                    codpago = decripciontipopago[responseTBK["paymentTypeCode"]];
                                }
                                string codrespuesta = "";
                                if (codes.ContainsKey(responseTBK["responseCode"]))
                                {
                                    codrespuesta = codes[responseTBK["responseCode"]];
                                }

                                string tipocoutas = "";
                                if (decripciontipocoutas.ContainsKey(responseTBK["paymentTypeCode"]))
                                {

                                    tipocoutas = decripciontipocoutas[responseTBK["paymentTypeCode"]];
                                }

                                string motivotbkplan = MotivoTransaccion(responseTBK["buyOrder"]);

                                if (int.Parse(responseTBK["responseCode"]) == 0)
                                {
                                    HttpContext.Current.Response.Write(htmlinicio);
                                    HttpContext.Current.Response.Write("<div>");
                                    HttpContext.Current.Response.Write("<h4 style='color:#009933;'>Transacción Aprobada <br /><span>Núm.Orden: </span><strong>" + responseTBK["buyOrder"] + "</strong></h4>");
                                    HttpContext.Current.Response.Write("<table class='table table-striped table-hover'>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td class='col-sm-4'><span>Núm. Orden: </span></td><td><strong>" + responseTBK["buyOrder"] + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Motivo: </span></td><td><strong>" + motivotbkplan + "</strong></td></tr>");

                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Monto: </span></td><td><strong>" + String.Format("{0:C0}", int.Parse(responseTBK["amount"])) + "</strong></td></tr>");

                                    decimal d = 0;
                                    decimal.TryParse(responseTBK["amount"], out d);

                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Monto: </span></td><td><strong>&#36;" + d.ToString("N0") + "</strong></td></tr>");


                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Fecha Transacción: </span></td><td><strong>" + responseTBK["transactionDate"] + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Núm. Tarjeta: </span></td><td><strong>" + responseTBK["Cardnumber"] + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Tipo Pago: </span></td><td><strong>" + codpago + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Tipo Cuota(s): </span></td><td><strong>" + tipocoutas + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Núm.Cuotas: </span></td><td><strong>" + responseTBK["sharesNumber"] + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Cód. Autorización: </span></td><td><strong>" + responseTBK["authorizationCode"] + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Código de Respuesta: </span></td><td><strong>" + codrespuesta + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("</table>");
                                    HttpContext.Current.Response.Write("</div>");
                                    HttpContext.Current.Response.Write("<br />");
                                    HttpContext.Current.Response.Write("<button class='btn btn-warning btn-large btn-block' onclick=\"printDiv('voucherarea')\">imprimir comprobante</button>");
                                    HttpContext.Current.Response.Write("<script type='text/javascript'>function printDiv(divName) {");
                                    HttpContext.Current.Response.Write("var printContents = document.getElementById(divName).innerHTML;var originalContents = document.body.innerHTML;document.body.innerHTML = printContents;window.print();");
                                    HttpContext.Current.Response.Write("document.body.innerHTML = originalContents;}</script>");

                                    /*Form que regresa a origen*/

                                    HttpContext.Current.Response.Write("<br />");
                                    HttpContext.Current.Response.Write("<script type='text/javascript'>");
                                    HttpContext.Current.Response.Write("function cargapage(){ window.open('" + urldestpost + "?OC_TBK=" + responseTBK["buyOrder"] + "&PLAN_TBK=" + motivotbkplan + "&COD_respuesta=" + responseTBK["responseCode"] + "', '_self');}");
                                    HttpContext.Current.Response.Write("</script>");
                                    HttpContext.Current.Response.Write("<div style='width:100%;'>");
                                    HttpContext.Current.Response.Write("<form action=" + urldestpost + " name='retornoorigenurl' method='post'>");
                                    HttpContext.Current.Response.Write("<input type='hidden' name='OC_TBK' value=" + responseTBK["buyOrder"] + ">");
                                    HttpContext.Current.Response.Write("<input type='hidden' name='PLAN_TBK' value=" + motivotbkplan + ">");
                                    HttpContext.Current.Response.Write("<input type='hidden' name='COD_respuesta' value=" + responseTBK["responseCode"] + ">");
                                    //HttpContext.Current.Response.Write("<input id='IdClassBtnEnviar' type='submit' class='btn btn-success btn-large btn-block IdClassBtnEnviar' value='Continuar Proceso &raquo;'>");
                                    HttpContext.Current.Response.Write("<a id='IdClassBtnEnviar' onclick='cargapage()' href='javascript:void(0);' class='btn btn-success btn-large btn-block IdClassBtnEnviar'>Continuar Proceso &raquo;</a>");
                                    HttpContext.Current.Response.Write("</form>");
                                    HttpContext.Current.Response.Write("</div>");
                                    //HttpContext.Current.Response.Write("URL Retorno"+ urldestpost);
                                    /*Fin Form que regresa a origen*/

                                    //enviaremail(responseTBK["buyOrder"]);
                                    HttpContext.Current.Response.Write(htmlfin);

                                }
                                else
                                {
                                    HttpContext.Current.Response.Write(htmlinicio);
                                    HttpContext.Current.Response.Write("<div>");
                                    HttpContext.Current.Response.Write("<h4 style='color:#ff0000;'>Transacción Rechazada <br /><span>Núm.Orden: </span><strong>" + responseTBK["buyOrder"] + "</strong></h4>");
                                    HttpContext.Current.Response.Write("<table class='table table-striped table-hover'>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td class='col-sm-4'><span>Núm. Orden: </span></td><td><strong>" + responseTBK["buyOrder"] + "</strong></td></tr>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Motivo: </span></td><td><strong>" + motivotbkplan + "</strong></td></tr>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Monto: </span></td><td><strong>" + responseTBK["amount"] + "</strong></td></tr>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Fecha Transacción: </span></td><td><strong>" + responseTBK["transactionDate"] + "</strong></td></tr>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Núm. Tarjeta: </span></td><td><strong>" + responseTBK["Cardnumber"] + "</strong></td></tr>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Núm.Cuotas: </span></td><td><strong>" + responseTBK["sharesNumber"] + "</strong></td></tr>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Cód. pago: </span></td><td><strong>" + codpago + "</strong></td></tr>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Cód. Autorización: </span></td><td><strong>" + responseTBK["authorizationCode"] + "</strong></td></tr>");
                                    //HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td><span>Código de Respuesta: </span></td><td><strong>" + codrespuesta + "</strong></td></tr>");
                                    HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td colspan='2'><p>Las posibles causas de este rechazo son:</p>");
                                    HttpContext.Current.Response.Write("<ul><li>Error en el ingreso de los datos de su tarjeta de Crédito o Débito (fecha y/o Código de seguridad).</li>");
                                    HttpContext.Current.Response.Write("<li>Su tarjeta de crédito o Débito no cuenta con el cupo necesario parta cancelar la compra.</li>");
                                    HttpContext.Current.Response.Write("<li>Tarjeta aún no habilitada en el sistema financiero.</li>");
                                    HttpContext.Current.Response.Write("</ul></td></tr>");
                                    HttpContext.Current.Response.Write("</table>");
                                    HttpContext.Current.Response.Write("</div>");
                                    HttpContext.Current.Response.Write("<br />");

                                    /*Form que regresa a origen*/
                                    HttpContext.Current.Response.Write("<br />");
                                    HttpContext.Current.Response.Write("<script type='text/javascript'>");
                                    HttpContext.Current.Response.Write("function cargapage(){ window.open('" + urldestpost + "?OC_TBK=" + responseTBK["buyOrder"] + "&PLAN_TBK=" + motivotbkplan + "&COD_respuesta=" + responseTBK["responseCode"] + "', '_self');}");
                                    HttpContext.Current.Response.Write("</script>");
                                    HttpContext.Current.Response.Write("<div style='width:100%;'>");
                                    HttpContext.Current.Response.Write("<form action=" + urldestpost + " name='retornoorigenurl' method='post'>");
                                    HttpContext.Current.Response.Write("<input type='hidden' name='OC_TBK' value=" + responseTBK["buyOrder"] + ">");
                                    HttpContext.Current.Response.Write("<input type='hidden' name='PLAN_TBK' value=" + motivotbkplan + ">");
                                    HttpContext.Current.Response.Write("<input type='hidden' name='COD_respuesta' value=" + responseTBK["responseCode"] + ">");
                                    //HttpContext.Current.Response.Write("<input type='submit' class='btn btn-success btn-large btn-block IdClassBtnEnviar' value='Continuar Proceso &raquo;'>");
                                    HttpContext.Current.Response.Write("<a id='IdClassBtnEnviar' onclick='cargapage()' href='javascript:void(0);' class='btn btn-success btn-large btn-block IdClassBtnEnviar'>Continuar Proceso &raquo;</a>");
                                    HttpContext.Current.Response.Write("</form>");
                                    HttpContext.Current.Response.Write("</div>");

                                    //HttpContext.Current.Response.Write("URL Retorno" + urldestpost);
                                    /*Fin Form que regresa a origen*/

                                    HttpContext.Current.Response.Write(htmlfin);
                                }



                                //HttpContext.Current.Response.Write("<span>Cód. Comercio: </span> <strong>" + responseTBK["commerceCode"] + "</strong><br />");
                                //HttpContext.Current.Response.Write("<span>Fecha Expiración: </span> <strong>" + responseTBK["CardExpirationDate"] + "</strong><br />");
                                //HttpContext.Current.Response.Write("<span>Forma: </span> <strong>" + responseTBK["VCI"] + "</strong><br />");

                                responseTBK.Add("token_ws", new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Request.Form["token_ws"]));

                                message = "Transacci&oacute;n Finalizada";
                                //HttpContext.Current.Response.Write("<strong>" + message + "</strong><br /><br />");


                                string next_page = baseurl + "?action=nullify";

                                //HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'><input type='hidden' name='commercecode' id='commercecode' value=''><input type='hidden' name='authorizationCode' id='authorizationCode' value=''><input type='hidden' name='amount' id='amount' value=''><input type='hidden' name='buyOrder' id='buyOrder' value=''><input type='submit' value='Anular Transacci&oacute;n &raquo;'></form>");
                                //HttpContext.Current.Response.Write("<script>var commercecode = localStorage.getItem('commercecode');document.getElementById('commercecode').value = commercecode;</script>");
                                //HttpContext.Current.Response.Write("<script>var authorizationCode = localStorage.getItem('authorizationCode');document.getElementById('authorizationCode').value = authorizationCode;</script>");
                                //HttpContext.Current.Response.Write("<script>var amount = localStorage.getItem('amount');document.getElementById('amount').value = amount;</script>");
                                //HttpContext.Current.Response.Write("<script>var buyOrder = localStorage.getItem('buyOrder');document.getElementById('buyOrder').value = buyOrder;</script>");

                                //createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(responseTBK), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Request.Form["token_ws"]), tx_step, Request.Form[keys[6]]);
                                //leearchivo(responseTBK["buyOrder"]);

                                HttpCookie ChCookie = new HttpCookie("ChileautosSettingTBK");
                                ChCookie.Expires = DateTime.Now.AddDays(-1d);
                                Response.Cookies.Add(ChCookie);

                                HttpCookie ChCookieurl = new HttpCookie("ChileautosUrlDestino");
                                ChCookieurl.Expires = DateTime.Now.AddDays(-1d);
                                Response.Cookies.Add(ChCookieurl);

                                HttpCookie ChCookieurlorigen = new HttpCookie("ChileautosurlFormOrigenTBK");
                                ChCookieurlorigen.Expires = DateTime.Now.AddDays(-1d);
                                Response.Cookies.Add(ChCookieurlorigen);

                            }

                        }
                        else
                        {
                            HttpContext.Current.Response.Write(htmlinicio);
                            HttpContext.Current.Response.Write("<div>");
                            HttpContext.Current.Response.Write("<h4 style='color:#ff0000;'>Transacción Rechazada<br /><span>Núm.Orden: </span><strong>" + Session["OC"] + "</strong></h4>");
                            HttpContext.Current.Response.Write("<script type='text/javascript'>function cargapagereturnlinkBtn(){ window.open('" + Request.Cookies["ChileautosurlFormOrigenTBK"]["urlformorigen"] + "', '_self');}</script>");
                            HttpContext.Current.Response.Write("<table class='table table-striped table-hover'>");
                            HttpContext.Current.Response.Write("<tr style='font-size:13px;'><td colspan='2'><p>Las posibles causas de este rechazo son:</p>");
                            HttpContext.Current.Response.Write("<ul><li>Error en el ingreso de los datos de su tarjeta de Crédito o Débito (fecha y/o Código de seguridad).</li>");
                            HttpContext.Current.Response.Write("<li>Su tarjeta de crédito o Débito no cuenta con el cupo necesario parta cancelar la compra.</li>");
                            HttpContext.Current.Response.Write("<li>Tarjeta aún no habilitada en el sistema financiero.</li>");
                            HttpContext.Current.Response.Write("</ul></td></tr>");
                            //HttpContext.Current.Response.Write("<tr><td><strong>Proceso anulado</strong></td></tr>");
                            HttpContext.Current.Response.Write("<tr><td></td></tr>");
                            HttpContext.Current.Response.Write("<tr><td><button class='btn btn-warning btn-large btn-block'  onclick='cargapagereturnlinkBtn()' >Regresar al formulario de pago.</button></td></tr>");
                            HttpContext.Current.Response.Write("</table>");
                            HttpContext.Current.Response.Write("</div>");
                            HttpContext.Current.Response.Write("<br />");
                            HttpContext.Current.Response.Write(htmlenlaceformulario);
                            HttpContext.Current.Response.Write(htmlfin);

                        }
                        ///
                        request.Add("", "");
                        Session["OC"] = "";
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(responseTBK) + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Request.Form["token_ws"]) + "</p>");

                        //HttpContext.Current.Response.Write("<label>Código de Autorización:</label> <strong>" + responseTBK["authorizationCode"]+ "</strong><br />");
                        //HttpContext.Current.Response.Write("<label>Núm. Órden:</label> <strong>" + responseTBK["buyOrder"] + "</strong><br />");
                        //HttpContext.Current.Response.Write("resoluciontransaccion <strong>" + responseTBK["resoluciontransaccion"] + "</strong><br />");

                    }
                    catch (Exception ex)
                    {
                        //HttpContext.Current.Response.Write(htmlinicio);
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>Descripción</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");

                        if (Request.Cookies["ChileautosSettingTBK"] != null)
                        {
                            if (Request.Cookies["ChileautosSettingTBK"]["authorizationCode"] != null)
                            {

                                /** creamos el log del mensaje de envío y su respuesta */
                                createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), ex.Message, tx_step, Request.Cookies["ChileautosSettingTBK"]["buyOrder"]);

                            }
                        }
                        HttpContext.Current.Response.Write(htmlenlaceformulario);
                        HttpContext.Current.Response.Write(htmlfin);
                        HttpCookie ChCookie = new HttpCookie("ChileautosSettingTBK");
                        ChCookie.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(ChCookie);

                        HttpCookie ChCookieurl = new HttpCookie("ChileautosUrlDestino");
                        ChCookieurl.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(ChCookieurl);

                        HttpCookie ChCookieurlorigen = new HttpCookie("ChileautosurlFormOrigenTBK");
                        ChCookieurlorigen.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(ChCookieurlorigen);

                    }

                    break;

                case "nullify":

                    tx_step = "nullify";

                    try
                    {

                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Confirmación de Anulación</p>");

                        /** Obtiene Información POST */
                        string[] keysNullify = Request.Form.AllKeys;

                        /** Codigo de Comercio */
                        string commercecode = Request.Form["commercecode"];

                        /** Código de autorización de la transacción que se requiere anular */
                        string authorizationCode = Request.Form["authorizationCode"];

                        /** Monto autorizado de la transacción que se requiere anular */
                        decimal authorizedAmount = Int64.Parse(Request.Form["amount"]);

                        /** Orden de compra de la transacción que se requiere anular */
                        buyOrder = Request.Form["buyOrder"];

                        /** Monto que se desea anular de la transacción */
                        decimal nullifyAmount = 3;

                        request.Add("authorizationCode", authorizationCode.ToString());
                        request.Add("authorizedAmount", authorizedAmount.ToString());
                        request.Add("buyOrder", buyOrder.ToString());
                        request.Add("nullifyAmount", nullifyAmount.ToString());
                        request.Add("commercecode", commercecode.ToString());

                        nullificationOutput resultNullify = webpay.getNullifyTransaction().nullify(authorizationCode, authorizedAmount, buyOrder, nullifyAmount, commercecode);

                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(resultNullify) + "</p>");

                        message = "Transacci&oacute;n Finalizada";
                        HttpContext.Current.Response.Write(message + "</br></br>");
                        /** creamos el log del mensaje de envío y su respuesta */
                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(resultNullify), tx_step, buyOrder.ToString());

                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                        //createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), ex.Message, tx_step, Request.Form[keys[6]]);
                    }

                    break;

            }





        }

        /*Obtiene los datos del proceso de TBK para procesar la creación de log*/
        protected void createlog(string recibetTBK, string resultresponseTBK, string tx_step, string filename)
        {

            string m_exePath = string.Empty;
            //m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //m_exePath = @"\logs";
            //m_exePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)+"\\logs";

            if (tx_step == "Init")
            {
                m_exePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\logsrequest";
            }
            else
            {
                m_exePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\logsresponse";
            }


            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + filename + "_log.txt"))
                {
                    Log(recibetTBK, resultresponseTBK, w, tx_step);
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error: " + ex.Message);
            }

        }

        /*Crear los log según corresponda, de acuerdo al stepname*/
        protected void Log(string logMessage, string logMessage2, TextWriter txtWriter, string stepname)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :{0}", stepname);
                txtWriter.WriteLine("  :Request");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
                txtWriter.WriteLine("  :Response");
                txtWriter.WriteLine("  :{0}", logMessage2);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error: " + ex.Message);
            }
        }

        /*Actualiza la OC procesada en BBDD cambiando estado a 1, si el pago fue exitoso*/
        public void UpDateEstadoDocumento(Dictionary<string, string> datosupdate)
        {

            int datoestado = 0;

            if (int.Parse(datosupdate["TBK_Respuesta"]) == 0)
            {

                datoestado = 1;

            }

            myConnection myConn = new myConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myConnection.GetConnection();
                    //connection.CommandText = "UPDATE [dbo].[transbank_pagos] SET[TBK_TIPO_TRANSACCION] = '" + datosupdate["tipo_transaccion"] + "',[TBK_RESPUESTA] = '" + datosupdate["TBK_Respuesta"] + "',[TBK_CODIGO_AUTORIZACION] = '" + datosupdate["TBK_cod_autorizacion"] + "',[TBK_FINAL_NUMERO_TARJETA] = '" + datosupdate["TBK_final_tarjeta"] + "',[TBK_FECHA_CONTABLE] = '" + datosupdate["TBK_fecha_contable"] + "',[TBK_FECHA_TRANSACCION] = '" + datosupdate["TBK_fecha_transaccion"] + "',[TBK_HORA_TRANSACCION] = '" + datosupdate["TBK_hora_transaccion"] + "',[TBK_ID_SESION] = '" + datosupdate["TBK_id_session"] + "',[TBK_ID_TRANSACCION] = '" + datosupdate["TBK_id_transaccion"] + "',[TBK_TIPO_PAGO] = '" + datosupdate["TBK_tipo_pago"] + "',[TBK_NUMERO_CUOTAS] = '" + datosupdate["TBK_numero_cuotas"] + "',[TBK_TASA_INTERES_MAX] = '" + datosupdate["TBK_tasa_interes_max"] + "',[ESTADO]= " + datoestado + " , [fecha_f_trans] = CONVERT(DATETIME, '" + datosupdate["fecha_f_trans"] + "', 120) WHERE TBK_ORDEN_COMPRA = '" + datosupdate["idOC"] + "'";
                    connection.CommandText = "UPDATE [dbo].[transbank_pagos] SET[TBK_TIPO_TRANSACCION] = '" + datosupdate["tipo_transaccion"] + "',[TBK_RESPUESTA] = '" + datosupdate["TBK_Respuesta"] + "',[TBK_CODIGO_AUTORIZACION] = '" + datosupdate["TBK_cod_autorizacion"] + "',[TBK_FINAL_NUMERO_TARJETA] = '" + datosupdate["TBK_final_tarjeta"] + "',[TBK_FECHA_CONTABLE] = '" + datosupdate["TBK_fecha_contable"] + "',[TBK_FECHA_TRANSACCION] = '" + datosupdate["TBK_fecha_transaccion"] + "',[TBK_HORA_TRANSACCION] = '" + datosupdate["TBK_hora_transaccion"] + "',[TBK_ID_SESION] = '" + datosupdate["TBK_id_session"] + "',[TBK_ID_TRANSACCION] = '" + datosupdate["TBK_id_transaccion"] + "',[TBK_TIPO_PAGO] = '" + datosupdate["TBK_tipo_pago"] + "',[TBK_NUMERO_CUOTAS] = '" + datosupdate["TBK_numero_cuotas"] + "',[TBK_TASA_INTERES_MAX] = '" + datosupdate["TBK_tasa_interes_max"] + "',[ESTADO]= " + datoestado + " , [fecha_f_trans] = '" + datosupdate["fecha_f_trans"] + "' WHERE TBK_ORDEN_COMPRA = '" + datosupdate["idOC"] + "'";
                    connection.ExecuteNonQuery();
                    connection.Connection.Close();
                    connection.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }
            }
            catch (Exception ex)
            {

                HttpContext.Current.Response.Write("Error: " + ex.Message);

            }

        }

        /* envia datos de pago aceptado en webpay y verificados en BBDD */
        protected void enviaremail(string ordencompra)
        {

            Dictionary<string, string> datoamensajes = mensajeemail(ordencompra);

            if (datoamensajes != null)
            {
                /*datos formales*/
                string emailsfrom = "ventas2@chileautos.cl";
                string emailsdestino = "acooper@chileautos.cl,contabilidad@chileautos.cl";
                string procedenciamail = "pago TBK";

                /*datos de prueba*/
                string emailsfromtest = "alvaro.emparan@gmail.com";
                string emailsdestinotest = "alvaro.emparan@gmail.com,aemparan@chileautos.cl";
                string procedenciamailtest = "pago prueba TBK";

                //string apiPublicar = "https://ws.chileautos.cl/api-cla/EnvioCorreo/Contactenos";
                //string parametros = "Nombre="+ datoamensajes["txt_nombre"] + "&EmailFrom="+ emailsfrom + "&EmailTo=" + emailsdestino + "&Comentario= Se ha informado de un pago en Chileautos.cl. <br /><br /> el sr(a). " + datoamensajes["txt_nombre"] + ", con rut: " + datoamensajes["txt_rut"] + ", efectuó una transacción con motivo de: " + datoamensajes["cmb_motivo"] + ", cuyo monto es: " + datoamensajes["TBK_MONTO2"] + ", realizada con " + datoamensajes["TBK_TIPO_PAGO"] + ".<br /> El número de la órden de compra es:  " + datoamensajes["TBK_ORDEN_COMPRA"] + ". <br /><br /> Este fue su comentario: " + datoamensajes["txt_comentario"] + ".<br /><br />&Asunto=Pago TransBank - " + datoamensajes["TBK_ORDEN_COMPRA"] + ", realizada con " + datoamensajes["TBK_TIPO_PAGO"] + "&Pie=" + procedenciamail;
                string apiPublicar = "http://dws.chileautos.cl/api-cla/EnvioCorreo/Contactenos";
                string parametros = "Nombre=" + datoamensajes["txt_nombre"] + "&EmailFrom=" + emailsfromtest + "&EmailTo=" + emailsdestinotest + "&Comentario= Se ha informado de un pago en Chileautos.cl. <br /><br /> el sr(a). " + datoamensajes["txt_nombre"] + ", con rut: " + datoamensajes["txt_rut"] + ", efectuó una transacción con motivo de: " + datoamensajes["cmb_motivo"] + ", cuyo monto es: " + datoamensajes["TBK_MONTO2"] + ", realizada con " + datoamensajes["TBK_TIPO_PAGO"] + ".<br /> El número de la órden de compra es:  " + datoamensajes["TBK_ORDEN_COMPRA"] + ". <br /><br /> Este fue su comentario: " + datoamensajes["txt_comentario"] + ".<br /><br />&Asunto=Pago TransBank - " + datoamensajes["TBK_ORDEN_COMPRA"] + ", realizada con " + datoamensajes["TBK_TIPO_PAGO"] + "&Pie=" + procedenciamailtest;

                try
                {
                    System.Net.WebClient wc = new System.Net.WebClient();
                    wc.Encoding = System.Text.Encoding.UTF8;
                    string data = parametros;
                    wc.Headers["Content-type"] = "application/x-www-form-urlencoded";
                    string resultado = wc.UploadString(apiPublicar, "POST", data);

                }
                catch (Exception ex)
                {
                    HttpContext.Current.Response.Write("Error: " + ex.Message);
                }
            }
        }

        /* obtiene los datos registrados de la OC procesada en TBK en Base de datos con estado = 1*/
        protected Dictionary<string, string> mensajeemail(string ordencompra)
        {

            Dictionary<string, string> datoamensajes = new Dictionary<string, string>();

            myConnection myConn = new myConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myConnection.GetConnection();
                    connection.CommandText = "Select TBK_ORDEN_COMPRA, txt_nombre, txt_rut, txt_digito, cmb_motivo, txt_comentario, TBK_MONTO2, TBK_TIPO_PAGO from transbank_pagos where TBK_ORDEN_COMPRA = '" + ordencompra + "' and Estado = 1 ";

                    using (var reader = connection.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            if (reader.Read())
                            {

                                datoamensajes.Add("TBK_ORDEN_COMPRA", reader["TBK_ORDEN_COMPRA"].ToString());
                                datoamensajes.Add("txt_nombre", reader["txt_nombre"].ToString());
                                datoamensajes.Add("txt_rut", reader["txt_rut"].ToString() + "-" + reader["txt_digito"].ToString());

                                if (reader["cmb_motivo"].ToString() == "Pago 1")
                                {
                                    datoamensajes.Add("cmb_motivo", "Pago 1%");
                                }
                                else
                                {
                                    datoamensajes.Add("cmb_motivo", reader["cmb_motivo"].ToString());
                                }

                                datoamensajes.Add("txt_comentario", reader["txt_comentario"].ToString());
                                datoamensajes.Add("TBK_MONTO2", reader["TBK_MONTO2"].ToString());

                                if (reader["TBK_TIPO_PAGO"].ToString() == "VD")
                                {
                                    datoamensajes.Add("TBK_TIPO_PAGO", "Tarjeta Débito");
                                }
                                else
                                {

                                    datoamensajes.Add("TBK_TIPO_PAGO", "Tarjeta Crédito");

                                }


                            }

                        }

                    }
                    connection.Connection.Close();
                    connection.Connection.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }


            }
            catch (Exception ex)
            {

                HttpContext.Current.Response.Write("Error: " + ex.Message);

            }

            return datoamensajes;
        }

        public Boolean VerificaOCExiste(string idoc)
        {

            myConnection myConnexiste = new myConnection();
            Boolean verificaOCexiste = false;
            //string tbk_autorizacion = "";

            try
            {
                using (var connectionexisteoc = new System.Data.SqlClient.SqlCommand())
                {
                    connectionexisteoc.Connection = myConnection.GetConnection();
                    connectionexisteoc.CommandText = "select TBK_CODIGO_AUTORIZACION from dbo.transbank_pagos where TBK_ORDEN_COMPRA = '" + idoc + "' and TBK_CODIGO_AUTORIZACION != ''";

                    using (var readerexiste = connectionexisteoc.ExecuteReader())
                    {
                        if (readerexiste.HasRows)
                        {
                            //tbk_autorizacion = readerexiste["TBK_CODIGO_AUTORIZACION"].ToString();
                            verificaOCexiste = true;
                        }

                    }
                    connectionexisteoc.Connection.Close();
                    connectionexisteoc.Connection.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }
            }
            catch (Exception ex)
            {

                HttpContext.Current.Response.Write("Error: " + ex.Message);

            }

            return verificaOCexiste;
        }


        public string MotivoTransaccion(string oc)
        {

            string respuesta = "";

            myConnection myConnMotivo = new myConnection();

            try
            {
                using (var myConnexisteMotivo = new System.Data.SqlClient.SqlCommand())
                {
                    myConnexisteMotivo.Connection = myConnection.GetConnection();
                    myConnexisteMotivo.CommandText = "select cmb_motivo from dbo.transbank_pagos where TBK_ORDEN_COMPRA = '" + oc + "'";

                    using (var reader = myConnexisteMotivo.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {

                                if (reader["cmb_motivo"].ToString() == "Pago 1")
                                {
                                    respuesta = "Pago 1%";
                                }
                                else
                                {

                                    respuesta = reader["cmb_motivo"].ToString();

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpContext.Current.Response.Write("Error: " + ex.Message);
            }

            return respuesta;
        }


        public string ChangeEncodingFormat(string DataChangeEnconde)
        {
            string cambiadatos = DataChangeEnconde;
            //////////////////////////////Original Code//////////////////////////////////////////
            string propEncodeString = string.Empty;

            if (cambiadatos.Contains("Ã") && cambiadatos != null)
            {
                byte[] utf8_Bytes = new byte[DataChangeEnconde.Length];
                for (int i = 0; i < DataChangeEnconde.Length; ++i)
                {
                    utf8_Bytes[i] = (byte)DataChangeEnconde[i];
                }

                propEncodeString = Encoding.UTF8.GetString(utf8_Bytes, 0, utf8_Bytes.Length);
                //Console.WriteLine("trae caracter raro Ã ");
            }
            else
            {
                propEncodeString = DataChangeEnconde;
                //Console.WriteLine("NO trae caracter raro Ã ");
            }

            ///////////////////////End New Code v4 ////////////////////////////////////////////
            return propEncodeString;
        }

        public Boolean checkOcexiste(string numoc)
        {

            Boolean revision = false;

            myConnection myConnOC = new myConnection();

            try
            {
                using (var myConnexisteOc = new System.Data.SqlClient.SqlCommand())
                {
                    myConnexisteOc.Connection = myConnection.GetConnection();
                    myConnexisteOc.CommandText = "select TBK_ORDEN_COMPRA from dbo.transbank_pagos where TBK_ORDEN_COMPRA = '" + numoc + "'";

                    using (var reader = myConnexisteOc.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            revision = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                HttpContext.Current.Response.Write("Error: " + ex.Message);
            }

            return revision;
        }

        public Boolean insertaOC(string TBK_ORDEN_COMPRA, string txt_nombre, string txt_rut, string txt_digito, string cmb_motivo, string txt_comentario, string TBK_MONTO, string TBK_MONTO2)
        {

            Boolean revisionInsertOc = false;

            myConnection myConnOc = new myConnection();

            try
            {
                using (var connectionInsertOc = new System.Data.SqlClient.SqlCommand())
                {
                    connectionInsertOc.Connection = myConnection.GetConnection();
                    //connection.CommandText = "UPDATE [dbo].[transbank_pagos] SET[TBK_TIPO_TRANSACCION] = '" + datosupdate["tipo_transaccion"] + "',[TBK_RESPUESTA] = '" + datosupdate["TBK_Respuesta"] + "',[TBK_CODIGO_AUTORIZACION] = '" + datosupdate["TBK_cod_autorizacion"] + "',[TBK_FINAL_NUMERO_TARJETA] = '" + datosupdate["TBK_final_tarjeta"] + "',[TBK_FECHA_CONTABLE] = '" + datosupdate["TBK_fecha_contable"] + "',[TBK_FECHA_TRANSACCION] = '" + datosupdate["TBK_fecha_transaccion"] + "',[TBK_HORA_TRANSACCION] = '" + datosupdate["TBK_hora_transaccion"] + "',[TBK_ID_SESION] = '" + datosupdate["TBK_id_session"] + "',[TBK_ID_TRANSACCION] = '" + datosupdate["TBK_id_transaccion"] + "',[TBK_TIPO_PAGO] = '" + datosupdate["TBK_tipo_pago"] + "',[TBK_NUMERO_CUOTAS] = '" + datosupdate["TBK_numero_cuotas"] + "',[TBK_TASA_INTERES_MAX] = '" + datosupdate["TBK_tasa_interes_max"] + "',[ESTADO]= " + datoestado + " , [fecha_f_trans] = CONVERT(DATETIME, '" + datosupdate["fecha_f_trans"] + "', 120) WHERE TBK_ORDEN_COMPRA = '" + datosupdate["idOC"] + "'";

                    connectionInsertOc.CommandText = "INSERT INTO [transbank_pagos]([TBK_ORDEN_COMPRA], [txt_nombre], [txt_rut], [txt_digito], [cmb_motivo], [txt_comentario], [TBK_MONTO],[TBK_MONTO2],[fecha_i_trans])";
                    connectionInsertOc.CommandText = connectionInsertOc.CommandText + "VALUES('" + TBK_ORDEN_COMPRA + "','" + txt_nombre + "','" + txt_rut + "','" + txt_digito + "','" + cmb_motivo + "','" + txt_comentario + "','" + TBK_MONTO + "','" + TBK_MONTO2 + "',getdate())";

                    connectionInsertOc.ExecuteNonQuery();
                    connectionInsertOc.Connection.Close();
                    connectionInsertOc.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }

                revisionInsertOc = true;

            }
            catch (Exception ex)
            {

                HttpContext.Current.Response.Write("Error: " + ex.Message);
                revisionInsertOc = false;
            }

            return revisionInsertOc;
        }

        public Boolean validaurlorigen(string urldominio)
        {

            Boolean urlvalida = false;

            /** Crea Dictionary con dominios Chileautos */
            Dictionary<string, string> domchileautos = new Dictionary<string, string>();

            domchileautos.Add("www.chileautos.cl", "Chileautos.cl");
            domchileautos.Add("chileinformes.chileautos.cl", "Chileinformes.cl");
            domchileautos.Add("localhost:50214", "Deydiperezlocalhost");
            domchileautos.Add("localhost:54128", "AlvaroEmparanlocalhost");
            domchileautos.Add("localhost", "localhost");
            domchileautos.Add("operaciones.chileautos.cl", "Operaciones");
            domchileautos.Add("desarrollo.chileautos.cl", "Desarrollo");
            domchileautos.Add("web.dev.retail.ca.csnglobal.net", "staying");
            domchileautos.Add("fotos.chileautos.cl", "fotoschileautos.cl");
            domchileautos.Add("desarrollofotos.chileautos.cl", "desarrollofotoschileautos.cl");
            domchileautos.Add("m.chileautos.cl", "mchileautos.cl");
            domchileautos.Add("desarrollom.chileautos.cl", "desarrollomchileautos.cl");



            if (domchileautos.ContainsKey(urldominio))
            {
                urlvalida = true;
            }

            return urlvalida;
        }



        public void crearcookiedatospost(string urldestino)
        {

            /* crea cookie con datos de Post para el final*/
            HttpCookie ChCookie = new HttpCookie("ChileautosUrlDestino");
            ChCookie.Values.Add("URLRetorno", urldestino);
            Response.Cookies.Add(ChCookie);

        }


        /**nuevo objeto serializar json*/
        /*
         
                JObject googleSearch = JObject.Parse(HtmlResult);

                // get JSON result objects into a list
                IList<JToken> results = googleSearch["datosVehiculo"].Children().ToList();
                // serialize JSON results into .NET objects
                IList<datosVehiculo> InfodatosVehiculo = new List<datosVehiculo>();
                foreach (JToken result in results)
                {
                    datosVehiculo vehiculo = JsonConvert.DeserializeObject<datosVehiculo>(result.ToString());
                    InfodatosVehiculo.Add(vehiculo);

                }
         
         */




    }
}