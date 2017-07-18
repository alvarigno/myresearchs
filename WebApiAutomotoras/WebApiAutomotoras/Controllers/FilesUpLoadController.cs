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

        [HttpPost]
        [Route("")]
        [CustomCheckLogin]
        public Task<IQueryable<FilesUpLoad>> Upload(string nombrearchivo, int sitio)
        {
            string hash = Util.getValueFromHeader("X-KEY");
            string ipregistrada = VerificaIpAddress(hash);
            string ipqueaccesa = GetIPAddress();
            nombrearchivosubido = nombrearchivo;
            sitioprocedencia = sitio;

            if (ipqueaccesa == ipregistrada)
            {

                if (validaextension(nombrearchivo))
                {

                    /** validación que archivo.xml sea distinto **/
                    string rutadirectaarchivo = uploadFolderPath + nombrearchivo;

                    if (!File.Exists(rutadirectaarchivo))
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
                                        var info = new FileInfo(i.LocalFileName);
                                        nombrerealarchivo = info.Name;
                                        Renombra(nombrerealarchivo, nombrearchivosubido, sitioprocedencia);
                                        string nuevoarchivo = uploadFolderPath + nombrearchivosubido;
                                        PasaDocumentoXml(uploadFolderPath + nombrearchivosubido);
                                        return new FilesUpLoad(uploadFolderPath + nombrearchivosubido, Request.RequestUri.AbsoluteUri + "?filename=" + nombrearchivosubido, (nuevoarchivo.Length / 1024).ToString());
                                        
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

                        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Archivo " + nombrearchivo + ", ya existe."));

                    }

                } else {

                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "Archivo " + nombrearchivo + ", debe ser extensión '.xml'."));

                }

            }
            else {

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

        public static void PasaDocumentoXml(string rutadocumento) {


            Program procesa = new Program();
            procesa.ObtieneDocumentoXml(rutadocumento);

        }

    }
}
