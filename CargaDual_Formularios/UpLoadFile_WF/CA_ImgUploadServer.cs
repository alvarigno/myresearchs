using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UpLoadFile_WF
{
    public class CA_ImgUploadServer
    {

        

        public string _urlPhotoServer = "https://upLoad-chileautos.li.csnstatic.com/chileautos/";
        public string _urlPhotoServerFinal = "https://chileautos.li.csnstatic.com/chileautos/";

        //public string _urlPhotoServer = "https://staging-chileautos.li.csnstatic.com/chileautos/";
        //public string _urlPhotoServerFinal = "https://staging-chileautos.li.csnstatic.com/chileautos/";

        public byte[] imgToByteArray(string inImg)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(inImg);
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        public List<string> Uploadimage(String[] files)
        {
            int count = 1;
            string categoria = "auto";
            string vendedor = "automotora";
            
            List<string> fotos = new List<string>();

            try
            {


                string urlUploadPhotoServer = _urlPhotoServer;
                urlUploadPhotoServer = String.Concat(urlUploadPhotoServer, categoria, "/", vendedor, "/");
                _urlPhotoServerFinal = String.Concat(_urlPhotoServerFinal, categoria, "/", vendedor, "/");

                foreach (var img in files)
                {

                    byte[] image = imgToByteArray(img);

                    if (count <= 20) { 
                        using (var client = new HttpClient())
                        {
                            //client.DefaultRequestHeaders.TryAddWithoutValidation("Username", "liveimages-stg@chileautos.cl");
                            //client.DefaultRequestHeaders.TryAddWithoutValidation("Password", "UdDX6eTJejSYBvTm7CMl");

                            client.DefaultRequestHeaders.TryAddWithoutValidation("Username", "liveimages@chileautos.cl");
                            client.DefaultRequestHeaders.TryAddWithoutValidation("Password", "13Hq0hMLVYweN4bNJVVf");
                        

                            Uri uri = new Uri(img);
                            string filename = System.IO.Path.GetFileName(uri.LocalPath);

                            var content = new MultipartFormDataContent();
                            var imageContent = new ByteArrayContent(image);

                            content.Add(imageContent, Path.GetFileName(img), filename);
                            imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                            using (var response = client.PostAsync(urlUploadPhotoServer + DateTime.Now.ToFileTime() + ".jpg", content).Result)
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    var csnFile = response.Content.ReadAsStringAsync().Result;
                                    fotos.Add(string.Concat(_urlPhotoServerFinal, csnFile));
                                }
                            }

                        }
                    }
                    count = count + 1;


                }

            }
            catch (Exception e)
            {

                Console.WriteLine("error: " + e.Message);

            }

            return fotos;
        }

    }
}
