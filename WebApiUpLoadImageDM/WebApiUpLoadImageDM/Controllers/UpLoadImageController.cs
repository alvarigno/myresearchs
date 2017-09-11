using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApiUpLoadImageDM.Models;
using WebApiUpLoadImageDM.Infrastructure;
using UpLoadFile;
using PublicacionDM;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
using System.Drawing.Imaging;
using System.Drawing;

namespace WebApiUpLoadImageDM.Controllers
{

    public class UpLoadImageController : ApiController
    {

        string nombrerealarchivo;
        string uploadFolderPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded\\";
        string archivolocal = "";
        Task taskDocumento;
        string img1;
        string img2;
        public const int OrientationId = 0x0112;


        [HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {

            DM_ImgUploadServer ImagesDm = new DM_ImgUploadServer();
            CA_ImgUploadServer ImagesCa = new CA_ImgUploadServer();

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = uploadFolderPath+postedFile.FileName;
                    postedFile.SaveAs(filePath);

                    //////////////////////////////////////////////////////////////
                    // Cambia la orientación de las imágenes //

                    FileStream stream = File.OpenRead(filePath);
                    byte[] fileBytes = new byte[stream.Length];

                    stream.Read(fileBytes, 0, fileBytes.Length);
                    stream.Close();
                    //Begins the process of writing the byte array back to a file


                    using (Stream file2 = File.OpenWrite(filePath))
                    {
                        file2.Write(fileBytes, 0, fileBytes.Length);
                    }

                    filePath = CambiaOrientacion(filePath);

                    ///////////////////////////////////////////////////////////


                    string dataDM = ImagesDm.Uploadimage(filePath);
                    if (!String.IsNullOrEmpty(dataDM))
                    {
                        img1 = "http://images.demotores.cl/post/tmp/siteposting/" + dataDM;
                    }
                    else {

                        img1 = null;

                    }
                    img2 = ImagesCa.Uploadimage(filePath);

                    docfiles.Add(img1);
                    docfiles.Add(img2);
                    taskDocumento = Task.Factory.StartNew(() => eliminaDoc(filePath));
                }

                if (docfiles[0] != null && docfiles[1] != null)
                {

                    result = Request.CreateResponse(HttpStatusCode.OK, docfiles);

                }
                else {

                    result = Request.CreateResponse(HttpStatusCode.BadRequest, "imágenes no cargadas");

                }

                
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            result.Headers.Add("X-Custom-Header", "hello");
            return result;

        }
        
        [Route("Administracion/aviso")]
        [HttpPost]
        public async Task<HttpResponseMessage> AdministraAvisoDm(string codauto)
        {
            PublicarDM dpublicacion = new PublicarDM();
            HttpResponseMessage respuesta = null;
            var httpRequest = HttpContext.Current.Request;

            if (!String.IsNullOrEmpty(codauto))
            {
                string datarespuesta = dpublicacion.InsertarPublicacion(codauto);
                respuesta = Request.CreateResponse(HttpStatusCode.OK, datarespuesta);

            }
            else
            {

                respuesta = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Código no existe.");

            }

            return respuesta;
        }

        [Route("Administracion/aviso/Eliminar")]
        [HttpPost]
        public async Task<HttpResponseMessage> EliminarAvisoDm(string codauto)
        {
            PublicarDM dpublicacion = new PublicarDM();
            HttpResponseMessage respuesta = null;
            var httpRequest = HttpContext.Current.Request;

            if (!String.IsNullOrEmpty(codauto))
            {
                string datarespuesta = dpublicacion.EliminarPublicacion(codauto);
                respuesta = Request.CreateResponse(HttpStatusCode.OK, datarespuesta);

            }
            else
            {

                respuesta = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Código no existe.");

            }

            return respuesta;
        }

        private static bool SiArchivoEstaBloqueado(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //archivo en uso
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //archivo liberado
            return false;
        }

        private static Boolean DetectaArchivoEnUso(string targetDirectory)
        {
            Boolean eliminado = true;


                FileInfo file = new FileInfo(targetDirectory);

                while (eliminado == true)
                {

                    eliminado = SiArchivoEstaBloqueado(file);

                }


            return eliminado;
        }

        public void eliminaDoc(string urlfile) {


            if (File.Exists(urlfile)) {

                if (DetectaArchivoEnUso(urlfile) == false) {

                    File.Delete(@urlfile);
                    if (taskDocumento.IsCompleted)
                    {

                        taskDocumento.Dispose();

                    }


                }

            }

        }


        private static string CambiaOrientacion(string urlfile) {

            Image img1 = Image.FromFile(urlfile);
            Image img = OrientImage(img1);

            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            ms.ToArray();
            File.WriteAllBytes(urlfile, (Byte[])ms.ToArray()); // Requires System.IO
            return urlfile;

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
