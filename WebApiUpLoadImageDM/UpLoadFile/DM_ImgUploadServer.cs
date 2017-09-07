using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace UpLoadFile
{
    public class DM_ImgUploadServer
    {
        public const int OrientationId = 0x0112;

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



        public string Uploadimage(String files) {

            string urlUploadPhotoServer = "http://www.demotores.cl/frontend/upload";
            string fotos = "";
            WebResponse response = null;

                    byte[] image = imgToByteArray(files);

                    string filedir = files.ToString();
                    NameValueCollection nvc = new NameValueCollection();

                    nvc.Add("parm1", "value1");
                    nvc.Add("parm2", "value2");
                    nvc.Add("parm3", "value3");
                    fotos = HttpUploadFile(urlUploadPhotoServer, @filedir, "file", "text/html", nvc);
               

            return fotos;

        }

        public static string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            string result;
            string retorno ="";
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            System.Net.ServicePointManager.Expect100Continue = false;
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream rs = wr.GetRequestStream();
            //string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            string formdataTemplate = "";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                //string formitem = string.Format(formdataTemplate, key, nvc[key]);
                string formitem = string.Format("");
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();
            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                result = reader2.ReadToEnd();
                retorno = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while converting file", "Error!", ex.Message);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }

            return retorno;

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
