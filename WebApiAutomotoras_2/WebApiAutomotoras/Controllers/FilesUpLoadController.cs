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
using WebApiAutomotoras.App_Code;
using System.Web.Http.Cors;
using System.Data.Entity.Core.Objects;
using AccesoDatos.Data;
using ProcesaDocumento;

namespace WebApiAutomotoras.Controllers
{
    
    [RoutePrefix("API-CLAAutomotora/Upload")]
    public class FilesUpLoadController : ApiController
    {

        string nombrearchivosubido;
        int sitioprocedencia;
        string nombrerealarchivo;
        string uploadFolderPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded\\";
        Task taskDocumento;
        Task taskDocumentoEliminacion;

        [HttpPost]
        [Route("")]
        [CustomCheckLogin]
        /// <summary>
        /// Get text by ID
        /// </summary>
        /// <param name="id">ID used to get the result text.</param>
        public async Task<HttpResponseMessage> Upload()
        {

            VerificaAcceso va = new VerificaAcceso();

            string hash = Util.getValueFromHeader("X-KEY");
            string ipregistrada = va.VerificaIpAddress(hash);
            string ipqueaccesa = va.GetIPAddress();

            if (ipqueaccesa == ipregistrada)
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

                                if (validaextension(postedFile.FileName))
                                {
                                    if (!File.Exists(filePath))
                                    {

                                        postedFile.SaveAs(filePath);
                                        taskDocumento = Task.Factory.StartNew(() => PasaDocumentoXml(filePath, ipqueaccesa, "publica/modifica"));
                                        result = Request.CreateResponse(HttpStatusCode.OK, "Archivo " + postedFile.FileName + ", cargado con éxito.");

                                    }
                                    else
                                    {

                                        result = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Archivo " + postedFile.FileName + ", ya existe.");

                                    }

                                }
                                else
                                {

                                    result = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Archivo " + postedFile.FileName + ", debe ser extensión '.xml'.");

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
            else {

                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "ip no está registrada."));

            }
           
        }

        [HttpPost]
        [Route("Elimina/avisos")]
        [CustomCheckLogin]
        public async Task<HttpResponseMessage> EliminaUpload()
        {
            VerificaAcceso va = new VerificaAcceso();
            string hash = Util.getValueFromHeader("X-KEY");
            string ipregistrada = va.VerificaIpAddress(hash);
            string ipqueaccesa = va.GetIPAddress();

            if (ipqueaccesa == ipregistrada)
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

                                if (validaextension(postedFile.FileName))
                                {
                                    if (!File.Exists(filePath))
                                    {

                                        postedFile.SaveAs(filePath);
                                        taskDocumentoEliminacion = Task.Factory.StartNew(() => PasaDocumentoEliminacionXml(filePath, ipqueaccesa, "elimina"));
                                        result = Request.CreateResponse(HttpStatusCode.OK, "Archivo " + postedFile.FileName + ", cargado con éxito.");

                                    }
                                    else
                                    {

                                        result = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Archivo " + postedFile.FileName + ", ya existe.");

                                    }

                                }
                                else
                                {

                                    result = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Archivo " + postedFile.FileName + ", debe ser extensión '.xml'.");

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
            else
            {

                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "ip no está registrada."));

            }

        }

        protected void Renombra(string direarchivo, string nombrearchivo, int sitio)
        {

            if (File.Exists(uploadFolderPath + direarchivo) && validaextension(uploadFolderPath + direarchivo))
            {
                File.Move(uploadFolderPath + direarchivo, uploadFolderPath + nombrearchivo);
                File.Delete(uploadFolderPath + direarchivo);
            }
            else
            {

                File.Delete(uploadFolderPath + direarchivo);

            }


        }

        protected bool validaextension(string fileName)
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

        private async void PasaDocumentoXml(string rutadocumento, string iporigen, string tarea)
        {

            await Task.Run(() =>
            {

                Program procesa = new Program();
                procesa.ObtieneDocumentoXml(rutadocumento, iporigen, tarea);

            });
            
        }

        private async void PasaDocumentoEliminacionXml(string rutadocumento, string iporigen, string tarea)
        {

            await Task.Run(() =>
            {
                Program procesa = new Program();
                procesa.ObtieneDocumentoXml(rutadocumento, iporigen, tarea);

            });

        }


    }
}
