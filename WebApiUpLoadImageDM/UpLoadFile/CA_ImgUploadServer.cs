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
            System.Drawing.Image img = System.Drawing.Image.FromFile(inImg);
            OrientImage(img);
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
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

        public enum ExifOrientations
        {
            Unknown = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomRight = 3,
            BottomLeft = 4,
            LeftTop = 5,
            RightTop = 6,
            RightBottom = 7,
            LeftBottom = 8,
        }


        public static ExifOrientations ImageOrientation(Image img)
        {

            int orientation_index = Array.IndexOf(img.PropertyIdList, OrientationId);


            if (orientation_index < 0) return ExifOrientations.Unknown;

            return (ExifOrientations)img.GetPropertyItem(OrientationId).Value[0];
        }


        public static Image OrientImage(Image img)
        {

            ExifOrientations orientation = ImageOrientation(img);

            switch (orientation)
            {
                case ExifOrientations.Unknown:
                case ExifOrientations.TopLeft:
                    break;
                case ExifOrientations.TopRight:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case ExifOrientations.BottomRight:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case ExifOrientations.BottomLeft:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case ExifOrientations.LeftTop:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case ExifOrientations.RightTop:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case ExifOrientations.RightBottom:
                    img.RotateFlip(RotateFlipType.Rotate90FlipY);
                    break;
                case ExifOrientations.LeftBottom:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }


            SetImageOrientation(img, ExifOrientations.TopLeft);
            return img;
        }


        public static void SetImageOrientation(Image img, ExifOrientations orientation)
        {
            const int OrientationId = 0x0112;

            int orientation_index = Array.IndexOf(img.PropertyIdList, OrientationId);

            if (orientation_index < 0) return;

            PropertyItem item = img.GetPropertyItem(OrientationId);
            item.Value[0] = (byte)orientation;
            img.SetPropertyItem(item);
        }

    }
}
