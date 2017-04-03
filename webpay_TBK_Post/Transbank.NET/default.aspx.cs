using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


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

    public partial class _default : System.Web.UI.Page
    {

        private string[] keys;

        protected void Page_Load()
        {

            Uri myuri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);
            string pathQuery = myuri.PathAndQuery;
            String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(pathQuery, "");

            if (Request.Url.Host == "localhost") {
                
            } else if(Request.Url.Host == "desarrollo.chileautos.cl") {

                strUrl = strUrl + "/pagoTBKV2"; // para publicar en desarrollo

            }
            else if (Request.Url.Host == "operaciones.chileautos.cl")
            {

                strUrl = strUrl + "/pagoTBKV2"; // para publicar en desarrollo

            }



            if (Request.QueryString["OC_TBK"] != null) {
                keys = Request.QueryString.AllKeys;
                for (int i = 0; i < keys.Length; i++)
                {

                    HttpContext.Current.Response.Write(keys[i] + ": " + Request.QueryString[keys[i]] + "<br />");


                }

            } else {

                HttpContext.Current.Response.Write("    <div id='tab-marca-y-modelo' class='tab-pane active fade in' role='tabpanel'>                                                                                                                    ");
                HttpContext.Current.Response.Write("        <form id='form1' class='pruebaform1' action='tbk-normal.aspx?i=0' method='post' target='_blank'>                                                                                                                  ");
                HttpContext.Current.Response.Write("                                                                                                                                                                                                        ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
				HttpContext.Current.Response.Write("				<div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<input name = 'txt_nombre' type='text' id='txt_nombre' placeholder='Su nombre' class='form-control'>                                                                            ");
                HttpContext.Current.Response.Write("                    </div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
                HttpContext.Current.Response.Write("            </div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
				HttpContext.Current.Response.Write("				<div class='col-sm-9 col-xs-9 form-row'>                                                                                                                                                ");
				HttpContext.Current.Response.Write("					<div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<input name = 'txt_rut' type='text' id='txt_rut' size='12' maxlength='8' placeholder='RUT' class='form-control'>                                                                ");
				HttpContext.Current.Response.Write("					</div>                                                                                                                                                                              ");
				HttpContext.Current.Response.Write("				</div>                                                                                                                                                                                  ");
	            HttpContext.Current.Response.Write("                                                                                                                                                                                                        ");
				HttpContext.Current.Response.Write("				<div class='col-sm-1 col-xs-1 form-row'>                                                                                                                                                ");
				HttpContext.Current.Response.Write("					<div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<p>-</p>                                                                                                                                                                        ");
				HttpContext.Current.Response.Write("					</div>                                                                                                                                                                              ");
				HttpContext.Current.Response.Write("				</div>                                                                                                                                                                                  ");
				HttpContext.Current.Response.Write("				<div class='col-sm-2 col-xs-2 form-row'>                                                                                                                                                ");
				HttpContext.Current.Response.Write("					<div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<input type = 'text' name='txt_digito' id='txt_digito' size='2' maxlength='1' class='form-control'>                                                                             ");
				HttpContext.Current.Response.Write("					</div>                                                                                                                                                                              ");
				HttpContext.Current.Response.Write("				</div>                                                                                                                                                                                  ");
				HttpContext.Current.Response.Write("			</div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
				HttpContext.Current.Response.Write("				<div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<select name = 'cmb_motivo' id='cmb_motivo' class='dropdown form-control'>                                                                                                      ");
				HttpContext.Current.Response.Write("						   <option value = 'Pago 1%' > Pago 1%</option>                                                                                                                                 ");
                HttpContext.Current.Response.Write("                           <option value = 'Pre-pago' selected='selected'>Pre-pago</option>                                                                                                             ");
				HttpContext.Current.Response.Write("						   <option value = 'Pago cuota mensual automotora' > Pago cuota mensual automotora</option>                                                                                     ");
				HttpContext.Current.Response.Write("						   <option value = 'Destacar aviso particular' > Destacar aviso particular</option>                                                                                             ");
				HttpContext.Current.Response.Write("						   <option value = 'Tomar fotografias a domicilio' > Tomar fotografias a domicilio</option>                                                                                     ");
    			HttpContext.Current.Response.Write("							</select>                                                                                                                                                                   ");
                HttpContext.Current.Response.Write("                    </div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
				HttpContext.Current.Response.Write("			</div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
				HttpContext.Current.Response.Write("				<div class='col-sm-12'>                                                                                                                                                                 ");
				HttpContext.Current.Response.Write("					<div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<textarea name = 'txt_comentario' type='text' id='txt_comentario' class='form-control' placeholder='Comentarios (Si es particular, coloque aqui el codigo del auto, patente o marca y modelo del vehiculo)' rows='4'></textarea>");
                HttpContext.Current.Response.Write("					</div>                                                                                                                                                                              ");
				HttpContext.Current.Response.Write("				</div>                                                                                                                                                                                  ");
				HttpContext.Current.Response.Write("			</div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
                HttpContext.Current.Response.Write("                <div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<nobr><INPUT TYPE = 'TEXT' ID='TBK_MONTO2' NAME='TBK_MONTO2' VALUE='' maxlength='7' class='form-control' placeholder='Monto a pagar a Chileautos $ (en pesos)' ></nobr>         ");
				HttpContext.Current.Response.Write("					</div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
                HttpContext.Current.Response.Write("            </div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
                HttpContext.Current.Response.Write("                <div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<INPUT TYPE = 'TEXT' class='form-control' placeholder='N� de orden:' ID='TBK_ORDEN_COMPRA' NAME='TBK_ORDEN_COMPRA' VALUE='oc_123456' >	                                        ");
				HttpContext.Current.Response.Write("						<INPUT TYPE = 'HIDDEN' class='form-control' NAME='TBK_ID_SESION' VALUE=''>                                                                                                      ");
				HttpContext.Current.Response.Write("					    <input type = 'HIDDEN' class='form-control' name='TBK_TIPO_TRANSACCION' value='TR_NORMAL'>                                                                                      ");
				HttpContext.Current.Response.Write("					    <input type = 'HIDDEN' class='form-control' id='TBK_MONTO' name='TBK_MONTO' value='10000' maxlength='7'>                                                                        ");
                HttpContext.Current.Response.Write("                    </div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
                HttpContext.Current.Response.Write("            </div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
                HttpContext.Current.Response.Write("                <div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<input name = 'textfield' type='text' placeholder='Pais pago: Chile' class='form-control' value='Chile' readonly>                                                               ");
                HttpContext.Current.Response.Write("                    </div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
                HttpContext.Current.Response.Write("            </div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
                HttpContext.Current.Response.Write("                <div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						                                                                                                                                                                                ");
                HttpContext.Current.Response.Write("                        <INPUT TYPE = 'HIDDEN' NAME='TBK_URL_EXITO' class='form-control' VALUE='"+ strUrl + "/tbk-normal.aspx'> <BR>                                                           ");
				HttpContext.Current.Response.Write("						<INPUT TYPE = 'HIDDEN' NAME='TBK_URL_FRACASO' class='form-control' VALUE='" + strUrl + "/tbk-normal.aspx'> <BR>                                                         ");
                HttpContext.Current.Response.Write("                                                                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    </div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
                HttpContext.Current.Response.Write("            </div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
                HttpContext.Current.Response.Write("                <div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<nobr><INPUT TYPE = 'TEXT' ID='TBK_URL_ORIGEN' NAME='TBK_URL_ORIGEN' VALUE='' class='form-control' placeholder='URL Origen' ></nobr>                                            ");
				HttpContext.Current.Response.Write("					</div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
                HttpContext.Current.Response.Write("            </div>                                                                                                                                                                                      ");
                HttpContext.Current.Response.Write("                                                                                                                                                                                                        ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
                HttpContext.Current.Response.Write("                <div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<nobr><INPUT TYPE = 'TEXT' ID='TBK_URL_DESTINO' NAME='TBK_URL_DESTINO' VALUE='' class='form-control' placeholder='URL Destino' ></nobr>                                         ");
				HttpContext.Current.Response.Write("					</div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
                HttpContext.Current.Response.Write("            </div>                                                                                                                                                                                      ");
                HttpContext.Current.Response.Write("                                                                                                                                                                                                        ");
				HttpContext.Current.Response.Write("			<div class='row'>                                                                                                                                                                           ");
                HttpContext.Current.Response.Write("                <div class='col-sm-12 form-row'>                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    <div class='form-group'>                                                                                                                                                            ");
				HttpContext.Current.Response.Write("						<INPUT TYPE = 'submit' VALUE='PAGAR CON REDCOMPRA o TARJETA DE CRÉDITO' SIZE='20' class='form-control' style='color:#ffffff;background-color:#003366;'>                         ");
                HttpContext.Current.Response.Write("                                                                                                                                                                                                        ");
                HttpContext.Current.Response.Write("                    </div>                                                                                                                                                                              ");
                HttpContext.Current.Response.Write("                </div>                                                                                                                                                                                  ");
                HttpContext.Current.Response.Write("            </div>                                                                                                                                                                                      ");
				HttpContext.Current.Response.Write("			                                                                                                                                                                                            ");
				HttpContext.Current.Response.Write("			<div id = 'var_paso' style='visibility:hidden; height:1px; overflow:hidden'></div>                                                                                                          ");
                HttpContext.Current.Response.Write("                                                                                                                                                                                                        ");
                HttpContext.Current.Response.Write("        </form>                                                                                                                                                                                         ");
                HttpContext.Current.Response.Write("    </div>                                                                                                                                                                                              ");

            }

        }

    }

}