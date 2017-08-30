using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SubidaImagenesServer
{
    public class UpLoadImageCA
    {

        public string _urlPhotoServer = "https://staging-chileautos.li.csnstatic.com/chileautos/";
        public string _urlPhotoServerFinal = "https://chileautos.li.csnstatic.com/chileautos/";

        public byte[] imgToByteArray(string inImg)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(inImg);
            using (System.IO.MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        public string Uploadimage(string img)
        {
            string foto = "";
            string categoria = "auto";
            string vendedor = "automotora";

          //  try
          //  {


                string urlUploadPhotoServer = _urlPhotoServer;
                urlUploadPhotoServer = String.Concat(urlUploadPhotoServer, categoria, "/", vendedor, "/");
                _urlPhotoServerFinal = String.Concat(_urlPhotoServerFinal, categoria, "/", vendedor, "/");

                    byte[] image = imgToByteArray(img);


                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Username", "liveimages-stg@chileautos.cl");
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Password", "UdDX6eTJejSYBvTm7CMl");

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
                                foto  = string.Concat(_urlPhotoServerFinal, csnFile);
                            }
                        }

                    }

           // }
           // catch (Exception e)
           // {
           //
           //     Console.WriteLine("error: " + e.Message);
           //     foto = e.Message;
           //
           // }

            return foto;
        }

    }
}