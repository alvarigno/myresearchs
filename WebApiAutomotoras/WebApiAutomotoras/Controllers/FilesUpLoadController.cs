using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApiAutomotoras.Infrastructure;
using UpLoadServicesRestWebApiModel;

namespace WebApiAutomotoras.Controllers
{
    [RoutePrefix("API-CLAAutomotora/Upload")]
    public class FilesUpLoadController : ApiController
    {

        string nombrearchivosubido;
        int sitioprocedencia;
        string nombrearchivo2;
        string nombrerealarchivo;
        string uploadFolderPath = "C:\\Users\\Álvaro\\Source\\Repos\\myresearchs\\WebApiAutomotoras\\WebApiAutomotoras\\fileLoaded\\";

        private const string UploadFolder = "uploads";

        [HttpPost]
        [Route("")]
        public Task<IQueryable<FilesUpLoad>> Post(string nombrearchivo, int sitio)
        {

            nombrearchivosubido = nombrearchivo;
            sitioprocedencia = sitio;

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
                            var info = new FileInfo(i.LocalFileName);
                            //var info = new FileInfo(i.Headers.ContentDisposition.FileName);
                            nombrerealarchivo = info.Name;
                            Rename(nombrerealarchivo, nombrearchivosubido, sitioprocedencia);
                            string nuevoarchivo = uploadFolderPath + nombrearchivosubido;
                            return new FilesUpLoad(uploadFolderPath + nombrearchivosubido, Request.RequestUri.AbsoluteUri + "?filename=" + nombrearchivosubido, (nuevoarchivo.Length / 1024).ToString());

                        });

                        return fileInfo.AsQueryable();

                    });

                    return task;
                }
                else
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
                }


            }
            catch (Exception ex)
            {
                //log.Error(ex);
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message));
            }

        }

        public void Rename(string direarchivo, string nombrearchivo, int sitio)
        {

            if (File.Exists(uploadFolderPath + direarchivo) && validaextension(uploadFolderPath + direarchivo))
            {
                File.Move(uploadFolderPath + direarchivo, uploadFolderPath + nombrearchivo);
               // insertaDocumentoBD(nombrearchivo, sitio);
                File.Delete(uploadFolderPath + direarchivo);
            }
            else
            {

                File.Delete(uploadFolderPath + direarchivo);

            }


        }

        public bool validaextension(string fileName)
        {
            if (fileName.Contains(".xml"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
