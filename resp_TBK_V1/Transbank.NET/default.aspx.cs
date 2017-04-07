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

        private string hosturl = HttpContext.Current.Request.Url.Host;


        protected void Page_Load() {

            String strPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
            String strUrl = HttpContext.Current.Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            String absurl = HttpContext.Current.Request.Url.AbsoluteUri;

            if (hosturl == "localhost")
            {

            }
            else if(hosturl == "desarrollo.chileautos.cl")
            {
                strUrl = "http://desarrollo.chileautos.cl/pagoTBK";
            }
            else if (hosturl == "operaciones.chileautos.cl")
            {
                strUrl = "https://operaciones.chileautos.cl/pagoTBK";
            }

            TBK_URL_EXITO.Value = strUrl+"/tbk-normal.aspx";
            TBK_URL_FRACASO.Value = strUrl + "/tbk-normal.aspx";
            TBK_URL_FORM.Value = absurl;

        }


    }

}