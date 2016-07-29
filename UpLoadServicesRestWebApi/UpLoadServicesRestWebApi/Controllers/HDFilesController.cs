using ProcesaDocumentos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using UpLoadServicesRestWebApi.Infrastructure;
using UpLoadServicesRestWebApiModel;
using System.Data.SqlClient;

namespace UpLoadServicesRestWebApi.Controllers
{

    public class HDFilesController : ApiController
    {
        myConnection myConn = new myConnection();

        string nombrearchivosubido;
        int sitioprocedencia;
        string nombrearchivo2;
        string nombrerealarchivo;
        string uploadFolderPath = "C:\\mails_prueba\\";

        private const string UploadFolder = "uploads";


        public HttpResponseMessage Get(string fileName)
        {
            HttpResponseMessage result = null;

            //DirectoryInfo directoryInfo = new DirectoryInfo(HostingEnvironment.MapPath(uploadFolderPath));
            DirectoryInfo directoryInfo = new DirectoryInfo(uploadFolderPath);
            FileInfo foundFileInfo = directoryInfo.GetFiles().Where(x => x.Name == fileName).FirstOrDefault();
            if (foundFileInfo != null)
            {
                FileStream fs = new FileStream(foundFileInfo.FullName, FileMode.Open);

                result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(fs);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = foundFileInfo.Name;
            }
            else
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return result;
        }

//        public Task<IQueryable<HDFile>> Post(string nombrearchivo, int sitio)
        public void Post(string nombrearchivo, int sitio)
        {

            nombrearchivosubido = nombrearchivo;
            sitioprocedencia = sitio;

            try
            {

                if (Request.Content.IsMimeMultipartContent())
                {
                    var streamProvider = new WithExtensionMultipartFormDataStreamProvider(uploadFolderPath);

                    var task = Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith<IQueryable<HDFile>>(t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                        {
                            throw new HttpResponseException(HttpStatusCode.InternalServerError);
                        }

                        var fileInfo = streamProvider.FileData.Select(i =>
                        {
                            var info = new FileInfo(i.LocalFileName);
                            //nombrerealarchivo = info.Name;
                            //Rename(nombrerealarchivo, nombrearchivosubido, sitioprocedencia);
                            //return new HDFile(info.Name, Request.RequestUri.AbsoluteUri + "?filename=" + info.Name, (info.Length / 1024).ToString());
                            //string nuevoarchivo = uploadFolderPath + nombrearchivosubido;
                            // return new HDFile(uploadFolderPath+nombrearchivosubido, Request.RequestUri.AbsoluteUri + "?filename=" + nombrearchivosubido, (nuevoarchivo.Length / 1024).ToString());
                            //return new HDFile(uploadFolderPath + nombrearchivosubido, Request.RequestUri.AbsoluteUri + "?filename=" + nombrearchivosubido, (nuevoarchivo.Length / 1024).ToString());
                            return new HDFile("", "", "");
                        });

                        foreach (var file in streamProvider.FileData)
                        {
                            var fi = new FileInfo(file.LocalFileName);
                            nombrerealarchivo = fi.Name;
                            Rename(nombrerealarchivo, nombrearchivosubido, sitioprocedencia);
                            string miodatos = nombrearchivo + "/" + nombrearchivosubido;

                        }

                        return fileInfo.AsQueryable();

                    });



                    //return task;
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
            //Rename(nombrearchivo, nombrearchivosubido, sitioprocedencia);
        }

        public void Rename(string direarchivo , string nombrearchivo, int sitio) {

            if (File.Exists(@"C:\mails_prueba\" + direarchivo) && validaextension(@"C:\mails_prueba\" + direarchivo))
            {
                //File.Move(uploadFolderPath+direarchivo, uploadFolderPath+nombrearchivo);
                File.Move(uploadFolderPath + direarchivo, uploadFolderPath+nombrearchivo);
                insertaDocumentoBD(nombrearchivo, sitio);
                File.Delete(uploadFolderPath+direarchivo);
            }
            else {

                File.Delete(@"C:\mails_prueba\" + direarchivo);

            }

            //File.Move(direarchivo, nombrearchivo);

        }

        public bool validaextension(string fileName)
        {
            if (fileName.Contains(".eml"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool insertaDocumentoBD(string filename, int sitio)
        {
            string format = "yyyy-MM-dd HH:MM:ss";

            SqlCommand mycommand = new SqlCommand();

            mycommand.CommandType = System.Data.CommandType.Text;
            mycommand.Connection = myConnection.GetConnection();
            mycommand.CommandText = " INSERT INTO [dbo].[documentos] ([fnombre] ,[estado] ,[fecha_insert] ,[sitio] ,[idemail])  VALUES ('"+filename+ "',0,CONVERT(DATETIME, '" + DateTime.Now.ToString(format) + "', 120),"+sitio+",null)";

            int a = mycommand.ExecuteNonQuery();
            mycommand.Connection.Close();

            //int a = 0;
            if (a == 0)
            {
                //Not updated.
                return false;
            }
            else
            {
                //Updated.
                ProcesaDocumentoSeleccionado(filename, sitio);
                return true;
            }
        }

        public void ProcesaDocumentoSeleccionado(string filename, int sitio)
        {

            ProcesaDocumentos.Program pm = new Program();
            string hola = pm.Comienzaproceso(filename, uploadFolderPath);
           
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}