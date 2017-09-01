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
        public async Task<HttpResponseMessage> Upload()
        {

            string hash = Util.getValueFromHeader("X-KEY");
            string ipregistrada = VerificaIpAddress(hash);
            string ipqueaccesa = GetIPAddress();

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

            string hash = Util.getValueFromHeader("X-KEY");
            string ipregistrada = VerificaIpAddress(hash);
            string ipqueaccesa = GetIPAddress();

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

        /// <summary>
        /// Modifica el nombre del archivo obtenido del directorio temp de windows.
        /// </summary>
        /// <param name="direarchivo"></param>
        /// <param name="nombrearchivo"></param>
        /// <param name="sitio"></param>
        public void Renombra(string direarchivo, string nombrearchivo, int sitio)
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

        /// <summary>
        /// obtiene la ip registrada en base de datos a través del SP_Valida_ip_x_xkey 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string VerificaIpAddress(string token) {

            Object ip = "0";

            baseprod2Entities database = new baseprod2Entities();
            
            ObjectParameter respuestaParam = new ObjectParameter("respuesta", typeof(bool));
            ObjectParameter ipaddressParam = new ObjectParameter("ipaddress", typeof(string));
            database.SPR_Valida_ip_x_xkey(token, respuestaParam, ipaddressParam);

            if (Boolean.Parse(respuestaParam.Value.ToString()) == true)
            {
                ip = ipaddressParam.Value;
            }
            else
            {
                HttpContext.Current.Response.AddHeader("authenticationToken", token);
                HttpContext.Current.Response.AddHeader("authenticationTokenStatus", "NotAuthorized");
                ip = "0";
            }

            return ip.ToString();
        }

        /// <summary>
        /// obtiene la ip del cliente que accesa al servicio.
        /// </summary>
        /// <returns></returns>
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
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
