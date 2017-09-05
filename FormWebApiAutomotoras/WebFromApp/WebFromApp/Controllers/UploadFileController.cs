using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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


        public static void SubeArchivos() {

            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();

            byte[] databytearraystring = dataFileToByteArray(@"C:\Chileautos_Desarrollo\desarrollo\integrador\fileLoaded\1028_0509201716234525.xml");
            form.Add(new ByteArrayContent(databytearraystring, 0, databytearraystring.Count()), "1028_0509201716234525", "1028_0509201716234525.xml");
            httpClient.DefaultRequestHeaders.Add("x-key", "B451AA1ACC6275A9DED6C799722B50CB");
            HttpResponseMessage response = httpClient.PostAsync("http://desarrollo.chileautos.cl/integrador/API-CLAAutomotora/Upload", form).Result;

            httpClient.Dispose();
            string sd = response.Content.ReadAsStringAsync().Result;

        }
        
        private static byte[] dataFileToByteArray(string fullFilePath)
        {
            FileStream fs = System.IO.File.OpenRead(fullFilePath);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            return bytes;
        }

    }
}
