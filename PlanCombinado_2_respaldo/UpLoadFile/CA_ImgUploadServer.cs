using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace UpLoadFile
{
    public class CA_ImgUploadServer
    {

        public string Uploadimage(String files)
        {

            
            string fotos = "";

            


               
                WebClient myWebClient = new WebClient();
                
                byte[] responseArray = myWebClient.UploadFile("https://ws.chileautos.cl/api-cla/Upload/auto/particular", files);
                
            if (responseArray != null && responseArray.Length > 0)
            {
                    
                    String prova = "";
                    
                    prova = System.Text.Encoding.ASCII.GetString(responseArray);
                    
                    var test = JObject.Parse(prova);
                    JArray items = (JArray)test["images"];
                    string file = items.ToString().Replace("[", "").Replace("]", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").Replace("\"", "");
                    int length = items.Count;
                    fotos = file;

            }




            return fotos;
        }

    }
}
