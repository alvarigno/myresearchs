using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UpLoadFile
{
    public class CA_ImgUploadServer
    {

        public const int OrientationId = 0x0112;

        public string _urlPhotoServer = "https://chileautos.storage.csnstatic.com/chileautos/";
        public string _urlPhotoServerFinal = "https://chileautos.pxcrush.net/chileautos/";

        public byte[] imgToByteArray(string inImg)
        {
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromFile(inImg);

            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
            
        }

        public string Uploadimage(String files)
        {
            int count = 1;
            string categoria = "auto";
            string vendedor = "automotora";
            
            string fotos = "";

            try
            {


                string urlUploadPhotoServer = _urlPhotoServer;
                urlUploadPhotoServer = String.Concat(urlUploadPhotoServer, categoria, "/", vendedor, "/");
                _urlPhotoServerFinal = String.Concat(_urlPhotoServerFinal, categoria, "/", vendedor, "/");


                    byte[] image = imgToByteArray(files);

                    if (count <= 20) { 
                        using (var client = new HttpClient())
                        {
                            //client.DefaultRequestHeaders.TryAddWithoutValidation("Username", "liveimages-stg@chileautos.cl");
                            //client.DefaultRequestHeaders.TryAddWithoutValidation("Password", "UdDX6eTJejSYBvTm7CMl");

                            client.DefaultRequestHeaders.TryAddWithoutValidation("Username", "liveimages@chileautos.cl");
                            client.DefaultRequestHeaders.TryAddWithoutValidation("Password", "13Hq0hMLVYweN4bNJVVf");
                        

                            Uri uri = new Uri(files);
                            string filename = System.IO.Path.GetFileName(uri.LocalPath);

                            var content = new MultipartFormDataContent();
                            var imageContent = new ByteArrayContent(image);

                            content.Add(imageContent, Path.GetFileName(files), filename);
                            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                            using (var response = client.PostAsync(urlUploadPhotoServer + DateTime.Now.ToFileTime() + ".jpg", content).Result)
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    var csnFile = response.Content.ReadAsStringAsync().Result;
                                    fotos = string.Concat(_urlPhotoServerFinal, csnFile);
                                }
                            }

                        }
                    }
                    count = count + 1;


                

            }
            catch (Exception e)
            {

                Console.WriteLine("error: " + e.Message);

            }

            return fotos;
        }

    }
}
