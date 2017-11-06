using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.IO;
using System.Net;
using AppMake360ViewIMG;
using EliminaDocumentos;

namespace WebApiMake360View.Controllers
{
    [System.Web.Http.RoutePrefix("MakeView")]
    public class Make360InsideViewController : ApiController
    {

        string nombrearchivosubido;
        int sitioprocedencia;
        string nombrerealarchivo;
        string uploadFolderPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded\\";
        public static Task taskProcesaImg360;
        Task taskDocumentoEliminacion;

        [HttpPost]
        [Route("Inside360")]
        public async Task<HttpResponseMessage> Upload(string codauto) {

            try
            {

                if (Request.Content.IsMimeMultipartContent())
                {

                    HttpResponseMessage result = null;
                    var httpRequest = HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        foreach (string file in httpRequest.Files)
                        {
                            var postedFile = httpRequest.Files[file];
                            var filePath = uploadFolderPath + postedFile.FileName;


                                if (!File.Exists(filePath))
                                {

                                    postedFile.SaveAs(filePath);
                                    taskProcesaImg360 = Task.Factory.StartNew(() => ProcesaDocumento(filePath, uploadFolderPath));
                                    result = Request.CreateResponse(HttpStatusCode.OK, "Archivo " + filePath + ", cargado con éxito.");

                                }
                                else
                                {

                                    result = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Archivo " + postedFile.FileName + ", ya existe.");

                                }

                        }
                        //result = Request.CreateResponse(HttpStatusCode.OK, docfiles);

                    }
                    else
                    {
                        result = Request.CreateResponse(HttpStatusCode.BadRequest);
                    }

                    return result;

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

        public static void ProcesaDocumento(string filePath, string uploadFolderPath) {
            
            Program procesa = new Program();
            Eliminar Eliminacion = new Eliminar();
            string filelisto = procesa.ProcesaVista360(filePath, uploadFolderPath);

            if (!string.IsNullOrEmpty(filelisto)) {

                Eliminar.eliminaDoc(taskProcesaImg360, filePath);

            }

        }



    }
}