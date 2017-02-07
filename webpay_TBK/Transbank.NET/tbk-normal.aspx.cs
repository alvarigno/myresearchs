using System;
using System.Collections.Generic;
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

        /** Crea Dictionary con datos Integración Pruebas */
        private Dictionary<string, string> certificate = Transbank.NET.sample.certificates.CertNormal.certificate();

        /** Crea Dictionary con datos de entrada */
        private Dictionary<string, string> request = new Dictionary<string, string>();

        Dictionary<string, string> responseTBK = new Dictionary<string, string>();
        Dictionary<string, string> requestTBK = new Dictionary<string, string>();
        wsInitTransactionOutput resultresponseTBK;

        protected void Page_Load()
        {

            Configuration configuration = new Configuration();
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

            /** Crea Dictionary con descripción */
            Dictionary<string, string> description = new Dictionary<string, string>();

            description.Add("VD", "Venta Deb&iacute;to");
            description.Add("VN", "Venta Normal");
            description.Add("VC", "Venta en cuotas");
            description.Add("SI", "cuotas sin inter&eacute;s");
            description.Add("S2", "2 cuotas sin inter&eacute;s");
            description.Add("NC", "N cuotas sin inter&eacute;s");

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


            HttpContext.Current.Response.Write("<link rel='stylesheet' type='text/css' href='http://webpreview.cl.csnglobal.net/Content/assets/css/chileautos.css'>");
            HttpContext.Current.Response.Write("<link rel='stylesheet' type='text/css' href='https://chileautos.cl/Content/assets/css/chileautos.css'>");
            HttpContext.Current.Response.Write("<link rel='stylesheet' type='text/css' href='https://www.chileautos.cl/Content/assets/css/chileautos.css'>");
            HttpContext.Current.Response.Write("<div class='container' style='width:500px;height:700px;'>");
            HttpContext.Current.Response.Write("<div class='col-xs-12 col-md-12 u-bg-gray-light'>");
            HttpContext.Current.Response.Write("<div style='text-align:center;margin:0 auto;display:block;margin-bottom:15px;'>");
            HttpContext.Current.Response.Write("<h3> PAGO CON TRANSBANK</h3>");
            HttpContext.Current.Response.Write("<img src='/imagenes/logos/tarjetas_webpay.gif' border='0'>");
            HttpContext.Current.Response.Write("<img src= '/imagenes/logos/WPP2.jpg' border='0'>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("<div class='search-wrapper'>");
            HttpContext.Current.Response.Write("<div class='tab-content tab-content--search'>");
            HttpContext.Current.Response.Write("<div id='tab-marca-y-modelo' class='tab-pane active fade in' role='tabpanel'>");

            switch (action)
            {

                default:

                    tx_step = "Init";

                    try
                    {

                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Confirmación de Datos para inicio de Proceso</p>");

                        Random random = new Random();
                        string[] keys = Request.Form.AllKeys;

                        HttpContext.Current.Response.Write("<label>Nombre:</label> <strong>" + Request.Form[keys[0]] + "</strong><br />");
                        HttpContext.Current.Response.Write("<label>RUT:</label> <strong>" + Request.Form[keys[1]] + "-" + Request.Form[keys[2]] + "</strong><br />");
                        HttpContext.Current.Response.Write("<label>Motivo:</label> <strong>" + Request.Form[keys[3]] + "</strong><br />");
                        HttpContext.Current.Response.Write("<label>Monto:</label> <strong>" + Request.Form[keys[5]] + "</strong><br />");
                        HttpContext.Current.Response.Write("<label>Comentario:</label> <strong>" + Request.Form[keys[4]] + "</strong><br />");

                        /** Monto de la transacción */
                        //decimal amount = System.Convert.ToDecimal("9990");
                        decimal amount = System.Convert.ToDecimal(Request.Form[keys[5]]);

                        DateTime now = DateTime.Now;
                        /** Orden de compra de la tienda */
                        //buyOrder = "OC_"+ now.Year+ now.Month+ now.Day+ now.Hour+ now.Minute+ now.Second;
                        buyOrder = Request.Form[keys[6]];

                        /** (Opcional) Identificador de sesión, uso interno de comercio */
                        string sessionId = random.Next(0, 1000).ToString();

                        /** URL Final */
                        baseurl = Request.Form[keys[11]];
                        string urlReturn = baseurl + "?action=result";

                        /** URL Final */
                        string urlFinal = baseurl + "?action=end";


                        request.Add("comerceId", configuration.CommerceCode);
                        request.Add("amount", amount.ToString());
                        request.Add("buyOrder", buyOrder.ToString());
                        request.Add("sessionId", sessionId.ToString());
                        request.Add("urlReturn", urlReturn.ToString());
                        request.Add("urlFinal", urlFinal.ToString());

                        /** Ejecutamos metodo initTransaction desde Libreria */
                        wsInitTransactionOutput result = webpay.getNormalTransaction().initTransaction(amount, buyOrder, sessionId, urlReturn, urlFinal);
                        


                        /** Verificamos respuesta de inicio en webpay */
                        if (result.token != null && result.token != "")
                        {
                            message = "Sesion iniciada con exito en Webpay";
                        }
                        else
                        {
                            message = "webpay no disponible";
                        }

                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");

                        HttpContext.Current.Response.Write("" + message + "</br></br>");
                        HttpContext.Current.Response.Write("<form action=" + result.url + " method='post'><input type='hidden' name='token_ws' value=" + result.token + "><input type='submit' value='Continuar &raquo;'></form>");

                        requestTBK = request;
                        resultresponseTBK = result;

                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result), tx_step);

                    }
                    catch (Exception ex)
                    {
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>Respuesta</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                        HttpContext.Current.Response.Write("Favor de completar el formulario con datos correctos.");
                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), ex.Message, tx_step);
                    }

                    break;

                case "result":

                    tx_step = "Get Result";

                    try
                    {

                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Resultado de Transacción</p>");

                        /** Obtiene Información POST */
                        string[] keysPost = Request.Form.AllKeys;

                        /** Token de la transacción */
                        string token = Request.Form["token_ws"];

                        request.Add("token", token.ToString());

                        transactionResultOutput result = webpay.getNormalTransaction().getTransactionResult(token);

                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br> " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result) + "</p>");

                        if (result.detailOutput[0].responseCode == 0)
                        {
                            message = "Pago ACEPTADO por webpay (se deben guardar datos para mostrar voucher)";

                            HttpContext.Current.Response.Write("<script>localStorage.setItem('authorizationCode', " + result.detailOutput[0].authorizationCode + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('commercecode', " + result.detailOutput[0].commerceCode + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('amount', " + result.detailOutput[0].amount + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('buyOrder', " + result.detailOutput[0].buyOrder + ")</script>");


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
                            ChCookie.Values.Add("VCI", result.VCI);
                            ChCookie.Values.Add("detalletransaccion", result.detailOutput[0].amount + "-" + result.detailOutput[0].authorizationCode + "-" + result.detailOutput[0].buyOrder + "-" + result.detailOutput[0].commerceCode + "-" + result.detailOutput[0].paymentTypeCode + "-" + result.detailOutput[0].responseCode + "-" + result.detailOutput[0].sharesAmount + "-" + result.detailOutput[0].sharesAmountSpecified + "-" + result.detailOutput[0].sharesNumber + "-" + result.detailOutput[0].sharesNumberSpecified);
                            ChCookie.Values.Add("resoluciontransaccion", HttpUtility.HtmlEncode(result.cardDetail));
                            Response.Cookies.Add(ChCookie);

                            createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result), tx_step);


                        }
                        else
                        {
                            message = "Pago RECHAZADO por webpay <br />Código : " + result.detailOutput[0].responseCode + "<br /> Descripción: " + codes[result.detailOutput[0].responseCode.ToString()];

                            HttpCookie ChCookie = new HttpCookie("ChileautosSettingTBK");
                            ChCookie.Values.Add("authorizationCode", result.detailOutput[0].authorizationCode);
                            ChCookie.Values.Add("commercecode", result.detailOutput[0].commerceCode);
                            ChCookie.Values.Add("amount", result.detailOutput[0].amount.ToString());
                            ChCookie.Values.Add("buyOrder", result.detailOutput[0].buyOrder);
                            ChCookie.Values.Add("responseCode", result.detailOutput[0].responseCode.ToString());
                            Response.Cookies.Add(ChCookie);

                            createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result), tx_step);
                        }

                        HttpContext.Current.Response.Write(message + "</br></br>");
                        HttpContext.Current.Response.Write("<form action=" + result.urlRedirection + " method='post'><input type='hidden' name='token_ws' value=" + token + "><input type='submit' value='Continuar &raquo;'></form>");

                    }
                    catch (Exception ex)
                    {
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), ex.Message, tx_step);
                    }

                    break;

                case "end":

                    tx_step = "End";

                    try
                    {

                        //HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Step: " + tx_step + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-weight: bold; font-size: 150%;'>Fin de Proceso</p>");

                        ///

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
                                responseTBK.Add("VCI", Request.Cookies["ChileautosSettingTBK"]["VCI"]);
                                responseTBK.Add("detalletransaccion", Request.Cookies["ChileautosSettingTBK"]["detalletransaccion"]);
                                responseTBK.Add("resoluciontransaccion", HttpUtility.HtmlDecode(Request.Cookies["ChileautosSettingTBK"]["resoluciontransaccion"]));
                            }
                        }
                        ///
                        request.Add("", "");

                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(responseTBK) + "</p>");
                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Request.Form["token_ws"]) + "</p>");

                        //HttpContext.Current.Response.Write("<label>Código de Autorización:</label> <strong>" + responseTBK["authorizationCode"]+ "</strong><br />");
                        //HttpContext.Current.Response.Write("<label>Núm. Órden:</label> <strong>" + responseTBK["buyOrder"] + "</strong><br />");
                        //HttpContext.Current.Response.Write("resoluciontransaccion <strong>" + responseTBK["resoluciontransaccion"] + "</strong><br />");

                        string codpago ="";
                        if (description.ContainsKey(responseTBK["paymentTypeCode"]))
                            codpago = description[responseTBK["paymentTypeCode"]];

                        string codrespuesta = "";
                        if (codes.ContainsKey(responseTBK["responseCode"]))
                            codrespuesta = codes[responseTBK["responseCode"]];

                        HttpContext.Current.Response.Write("<span>Núm. Orden: </span> <strong>" + responseTBK["buyOrder"] + "</strong><br />");
                        HttpContext.Current.Response.Write("<span>Monto: </span> <strong>" + responseTBK["amount"] + "</strong><br />");
                        HttpContext.Current.Response.Write("<span>Fecha Transacción: </span> <strong>" + responseTBK["transactionDate"] + "</strong><br />");
                        HttpContext.Current.Response.Write("<span>Núm. Tarjeta: </span> <strong>" + responseTBK["Cardnumber"] + "</strong><br />");
                        HttpContext.Current.Response.Write("<span>Núm.Cuotas: </span> <strong>" + responseTBK["sharesNumber"] + "</strong><br />");
                        HttpContext.Current.Response.Write("<span>Cód. pago: </span> <strong>" + codpago + "</strong><br />");
                        HttpContext.Current.Response.Write("<span>Cód. Autorización: </span> <strong>" + responseTBK["authorizationCode"] + "</strong><br />");
                        HttpContext.Current.Response.Write("<span>Código de Respuesta: </span> <strong>" + codrespuesta + "</strong><br />");

                        //HttpContext.Current.Response.Write("<span>Cód. Comercio: </span> <strong>" + responseTBK["commerceCode"] + "</strong><br />");
                        //HttpContext.Current.Response.Write("<span>Fecha Expiración: </span> <strong>" + responseTBK["CardExpirationDate"] + "</strong><br />");
                        //HttpContext.Current.Response.Write("<span>Forma: </span> <strong>" + responseTBK["VCI"] + "</strong><br />");

                        responseTBK.Add("token_ws", new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Request.Form["token_ws"]));

                        message = "Transacci&oacute;n Finalizada";
                        HttpContext.Current.Response.Write("<br /><br /><strong>" + message + "</strong><br /><br />");

                        string next_page = baseurl + "?action=nullify";

                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'><input type='hidden' name='commercecode' id='commercecode' value=''><input type='hidden' name='authorizationCode' id='authorizationCode' value=''><input type='hidden' name='amount' id='amount' value=''><input type='hidden' name='buyOrder' id='buyOrder' value=''><input type='submit' value='Anular Transacci&oacute;n &raquo;'></form>");
                        HttpContext.Current.Response.Write("<script>var commercecode = localStorage.getItem('commercecode');document.getElementById('commercecode').value = commercecode;</script>");
                        HttpContext.Current.Response.Write("<script>var authorizationCode = localStorage.getItem('authorizationCode');document.getElementById('authorizationCode').value = authorizationCode;</script>");
                        HttpContext.Current.Response.Write("<script>var amount = localStorage.getItem('amount');document.getElementById('amount').value = amount;</script>");
                        HttpContext.Current.Response.Write("<script>var buyOrder = localStorage.getItem('buyOrder');document.getElementById('buyOrder').value = buyOrder;</script>");

                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(responseTBK), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(Request.Form["token_ws"]), tx_step);

                        HttpCookie ChCookie = new HttpCookie("ChileautosSettingTBK");
                        ChCookie.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(ChCookie);

                    }
                    catch (Exception ex)
                    {

                        HttpCookie ChCookie = new HttpCookie("ChileautosSettingTBK");
                        ChCookie.Expires = DateTime.Now.AddDays(-1d);
                        Response.Cookies.Add(ChCookie);

                        //HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), ex.Message, tx_step);
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
                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(resultNullify), tx_step);

                    }
                    catch (Exception ex)
                    {
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightyellow;'><strong>request</strong></br></br>" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "</p>");
                        HttpContext.Current.Response.Write("<p style='font-size: 100%; background-color:lightgrey;'><strong>result</strong></br></br> Ocurri&oacute; un error en la transacci&oacute;n (Validar correcta configuraci&oacute;n de parametros). " + ex.Message + "</p>");
                        createlog(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request), ex.Message, tx_step);
                    }

                    break;

            }

            //HttpContext.Current.Response.Write("</br><a href='https://operaciones.chileautos.cl/pago_v2.asp?i=0'>&laquo; volver a index</a>");
            HttpContext.Current.Response.Write("</br><a href='http://"+ httpHost + "/default.aspx'>&laquo; volver a index</a>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("</div>");


        }

        protected void createlog(string recibetTBK, string resultresponseTBK, string tx_step) {

            string m_exePath = string.Empty;
            //m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //m_exePath = @"\logs";
            //m_exePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)+"\\logs";

            if (tx_step == "Init") {
                m_exePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\logsrequest";
            } else {
                m_exePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\logsresponse";
            }


            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log.txt"))
                {
                    Log(recibetTBK, resultresponseTBK, w);
                }
            }
            catch (Exception ex)
            {
            }

        }

        protected void Log(string logMessage, string logMessage2,  TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :Request");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
                txtWriter.WriteLine("  :Response");
                txtWriter.WriteLine("  :{0}", logMessage2);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }



    }
}