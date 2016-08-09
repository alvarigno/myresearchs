using ProcesaDocumentos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using UpLoadServicesRestWebApi.Infrastructure;
using UpLoadServicesRestWebApiModel;

namespace UpLoadServicesRestWebApi.Controllers
{
    public class FilesUpLoadController : ApiController
    {

        myConnection myConn = new myConnection();

        string nombrearchivosubido;
        int sitioprocedencia;
        string nombrearchivo2;
        string nombrerealarchivo;
        string uploadFolderPath = "C:\\mails_prueba\\";

        private const string UploadFolder = "uploads";


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
                insertaDocumentoBD(nombrearchivo, sitio);
                File.Delete(uploadFolderPath + direarchivo);
            }
            else
            {

                File.Delete(uploadFolderPath + direarchivo);

            }


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
            mycommand.CommandText = " INSERT INTO [dbo].[documentos] ([fnombre] ,[estado] ,[fecha_insert] ,[sitio] ,[idemail])  VALUES ('" + filename + "',0,CONVERT(DATETIME, '" + DateTime.Now.ToString(format) + "', 120)," + sitio + ",null)";

            int a = mycommand.ExecuteNonQuery();
            mycommand.Connection.Close();
            mycommand.Connection.Dispose();
            SqlConnection.ClearAllPools();

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