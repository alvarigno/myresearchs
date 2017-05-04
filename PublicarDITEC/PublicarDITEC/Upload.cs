using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace PublicarDITEC
{
    public class Upload
    {


        public string _urlPhotoServer = "https://staging-chileautos.li.csnstatic.com/chileautos/";
        public string _urlPhotoServerFinal = "https://chileautos.li.csnstatic.com/chileautos/";

        public byte[] imgToByteArray(string inImg)
        {
            System.Drawing.Image img = System.Drawing.Image.FromFile(inImg);
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }

        public List<string> Uploadimage(int codditec, string categoria, string vendedor)
        {

            String searchFolder = @"C:\Users\Álvaro\Source\Repos\myresearchs\ReadExcelFiles\ReadExcelFiles\bin\Debug\carga\"+ codditec;
            var filters = new String[] { "jpg"};
            var files = GetFilesImages(searchFolder, filters, false);
            List<string> fotos = new List<string>();

            try {


                string urlUploadPhotoServer = _urlPhotoServer;
                urlUploadPhotoServer = String.Concat(urlUploadPhotoServer, categoria, "/", vendedor, "/");
                _urlPhotoServerFinal = String.Concat(_urlPhotoServerFinal, categoria, "/", vendedor, "/");

                foreach (var img in files)
                {

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
                                fotos.Add(string.Concat(_urlPhotoServerFinal, csnFile));
                            }
                        }

                    }


                }

            } catch (Exception e) {

                Console.WriteLine("error: " + e.Message);

            }

            return fotos;
        }



        public static String[] GetFilesImages(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound.ToArray();
        }



    }
}
