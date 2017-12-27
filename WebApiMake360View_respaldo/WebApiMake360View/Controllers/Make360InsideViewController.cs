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
using Newtonsoft.Json;
using UpLoadFile;

namespace WebApiMake360View.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [System.Web.Http.RoutePrefix("MakeView")]
    public class Make360InsideViewController : ApiController
    {

        public static DateTime datetime = DateTime.Now;
        public static DateTime dateOnly = datetime;
        public static string dirname = dateOnly.ToString("d").Replace("/", "").Replace(" ", "").Replace(":", "");
        public static string listado = "";

        string nombrearchivosubido;
        int sitioprocedencia;
        string nombrerealarchivo;
        string uploadFolderPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded\\";
        public static Task taskProcesaImg360;
        public static Task taskProcesaImg360OutSide;
        public static Task taskProcesa360OutSideImg;

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
                            var filePath = uploadFolderPath+ "\\inside\\" + postedFile.FileName;


                                if (!File.Exists(filePath))
                                {

                                    postedFile.SaveAs(filePath);

                                    CA_ImgUploadServer subearchivo = new CA_ImgUploadServer();
                                    string imglista = subearchivo.Uploadimage(filePath);

                                    taskProcesaImg360 = Task.Factory.StartNew(() => ProcesaInsideView(filePath, uploadFolderPath));
                                    result = Request.CreateResponse(HttpStatusCode.OK, imglista);

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
        [Route("view360Video")]
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

        [HttpPost]
        [Route("Outside360")]
        public async Task<HttpResponseMessage> UploadImgeArray(string codauto)
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

//                            DirectoryInfo di = Directory.CreateDirectory(uploadFolderPath + "outside\\" +  "_" + dirname);

                            var filePath = uploadFolderPath + "outside\\"+ postedFile.FileName;

                            listado = listado + postedFile.FileName + ", ";
                            

                            if (!File.Exists(filePath))
                            {

                                postedFile.SaveAs(filePath);

                                CA_ImgUploadServer subearchivo = new CA_ImgUploadServer();
                                docfiles.Add(subearchivo.UploadOutsideView(filePath));
                                Images customer = new Images();
                                customer.images = docfiles.ToArray();
                                var json = JsonConvert.SerializeObject(customer);

                                taskProcesa360OutSideImg = Task.Factory.StartNew(() => ProcesaOutsideView360(filePath, uploadFolderPath));
                                result = Request.CreateResponse(HttpStatusCode.OK, customer.images);

                            }
                            else
                            {

                                result = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Archivo " + listado + " ya existe(n).");

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
            

            Eliminar Eliminacion = new Eliminar();

            if (!string.IsNullOrEmpty(filePath)) {

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

        public static void ProcesaOutsideView360(string filePath, string uploadFolderPath)
        {

            Eliminar Eliminacion = new Eliminar();

            if (!string.IsNullOrEmpty(filePath))
            {

                Eliminar.eliminaDoc(taskProcesa360OutSideImg, filePath);

            }

        }




    }

    internal class Images
    {
        public string[] images { get; set; }
    }
}