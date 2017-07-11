using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFromApp.Controllers
{
    public class UploadFileController : Controller
    {

        public ActionResult Upload() {

            ViewBag.ip =  GetIPAddress();
            return View();

        }

        /// <summary>
        ///  Get ip address from url client.
        /// </summary>
        /// <returns></returns>
        protected string GetIPAddress()
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
