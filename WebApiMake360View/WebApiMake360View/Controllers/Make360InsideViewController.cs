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
        public static Task taskProcesaImg360OutSide;


        [HttpPost]
        [Route("Inside360")]
        public async Task<HttpResponseMessage> UploadImg(string codauto) {

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
                                    taskProcesaImg360 = Task.Factory.StartNew(() => ProcesaInsideView(filePath, uploadFolderPath));
                                    result = Request.CreateResponse(HttpStatusCode.OK, "Archivo " + postedFile.FileName + ", cargado con éxito.");

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

        [HttpPost]
        [Route("Outside360")]
        public async Task<HttpResponseMessage> UploadVideo(string codauto)
        {

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
                                taskProcesaImg360OutSide = Task.Factory.StartNew(() => ProcesaOutsideView(filePath, uploadFolderPath));
                                result = Request.CreateResponse(HttpStatusCode.OK, "Archivo " + postedFile.FileName + ", cargado con éxito.");

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


        public static void ProcesaInsideView(string filePath, string uploadFolderPath) {
            
            Program procesa = new Program();
            Eliminar Eliminacion = new Eliminar();
            string filelisto = procesa.ProcesaVista360Interna(filePath, uploadFolderPath);

            if (!string.IsNullOrEmpty(filelisto)) {

                Eliminar.eliminaDoc(taskProcesaImg360, filePath);

            }

        }

        public static void ProcesaOutsideView(string filePath, string uploadFolderPath)
        {

            Program procesa = new Program();
            Eliminar Eliminacion = new Eliminar();
            string filelisto = procesa.ProcesaVista360Externa(filePath, uploadFolderPath);

            if (!string.IsNullOrEmpty(filelisto))
            {

                Eliminar.eliminaDoc(taskProcesaImg360OutSide, filePath);

            }

        }



    }
}