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

namespace WebApiUpLoadImageDM.Controllers
{
    [RoutePrefix("API-CLAImagDM/Upload")]
    public class UpLoadImageController : ApiController
    {

        string nombrerealarchivo;
        string uploadFolderPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded\\";
        string archivolocal = "";
        Task taskDocumento;


        [HttpPost]
        [Route("")]
        public Task<IQueryable<FilesUpLoad>> Upload()
        {
            string fileDM = "";

            if (Request.Content.IsMimeMultipartContent())
            {
                
                try
                {

                    if (Request.Content.IsMimeMultipartContent())
                    {
                        var streamProvider = new WithExtensionMultipartFormDataStreamProvider(uploadFolderPath);

                        var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IQueryable<FilesUpLoad>>(t =>
                        {
                            if (t.IsFaulted || t.IsCanceled)
                            {
                                throw new HttpResponseException(HttpStatusCode.InternalServerError);
                            }

                            var fileInfo = streamProvider.FileData.Select(i =>
                            {
                                DM_ImgUploadServer dataDm = new DM_ImgUploadServer();
                                var info = new FileInfo(i.LocalFileName);
                                nombrerealarchivo = info.Name;
                                
                                archivolocal = uploadFolderPath + nombrerealarchivo;
                                string data = dataDm.Uploadimage(uploadFolderPath + nombrerealarchivo);
                                data = "http://images.demotores.cl/post/tmp/siteposting/" + data;
                                string valoretorno = archivolocal;
                                taskDocumento = Task.Factory.StartNew(() => eliminaDoc(archivolocal));
                                return new FilesUpLoad(data);


                            });

                            return fileInfo.AsQueryable();

                        });

                        

                        return task;


                    }
                    else
                    {
                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Los datos requeridos no poseen la forma correcta."));
                    }


                }
                catch (Exception ex)
                {

                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
                }



            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Los datos requeridos no poseen la forma correcta."));
            }

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

    }
}
