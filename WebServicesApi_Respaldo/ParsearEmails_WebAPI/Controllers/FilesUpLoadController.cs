﻿using ProcesaDocumentos;
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
using ParsearEmails_WebAPI.Infrastructure;
//Enlace a modelos creados para procesar documentos, esto creado por la posibilidad de usos en distintos proyectos o procesos.
using UpLoadServicesRestWebApiModel;
using System.Web.Hosting;

namespace ParsearEmails_WebAPI.Controllers
{
        public class FilesUpLoadController : ApiController
        {

        private static SqlConnection _cnx;

            string nombrearchivosubido;
            int sitioprocedencia;
            string nombrearchivo2;
            string nombrerealarchivo;
        //private const string UploadFolder = "uploads";
               string uploadFolderPath = "D:\\mailparser\\documentos\\";
        //string uploadFolderPath = HostingEnvironment.MapPath("~/App_Data/");

        

            /// <summary>
            /// Servicio que carga un archivo en el sertvidor, desde el archivo de registro del multipart/from-data, siendo el archivo de forma nativa.
            /// Este servicio entrega mensaje de respuesta.
            /// </summary>
            /// <param name="nombrearchivo"></param>
            /// <param name="sitio"></param>
            /// <returns></returns>
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

            /// <summary>
            /// Métido que renomabra el archivo con el nombre original del archivo que fue subido. De esta forma que 
            /// </summary>
            /// <param name="direarchivo"></param>
            /// <param name="nombrearchivo"></param>
            /// <param name="sitio"></param>
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

            /// <summary>
            /// Verifica que la extensión del archivo a procesar o subir, sea ".eml".
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
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

            /// <summary>
            /// Métido que inserta el nombre del archivo en la base de datos (dbo.documentos)
            /// </summary>
            /// <param name="filename"></param>
            /// <param name="sitio"></param>
            /// <returns></returns>
            public bool insertaDocumentoBD(string filename, int sitio)
            {
                string format = "yyyy-MM-dd HH:MM:ss";
                int a = 0;

                ConexionSql con = new ConexionSql();
                _cnx = con.Conexion();

                string queryString = " INSERT INTO [dbo].[documentos] ([fnombre] ,[estado] ,[fecha_insert] ,[sitio] ,[idemail])  VALUES ('" + filename + "',0,CONVERT(DATETIME, '" + DateTime.Now.ToString(format) + "', 120)," + sitio + ",null)";

                    using (con.Conexion())
                    {
                        SqlCommand cmd = new SqlCommand();
                        SqlCommand command = new SqlCommand(queryString, con.Conexion());
                        con.Abrir();
                        a = command.ExecuteNonQuery();
                        con.Cerrar();

                    }

                //SqlCommand mycommand = new SqlCommand();

                //mycommand.CommandType = System.Data.CommandType.Text;
                //mycommand.Connection = myConnection.GetConnection();
                //mycommand.CommandText = " INSERT INTO [dbo].[documentos] ([fnombre] ,[estado] ,[fecha_insert] ,[sitio] ,[idemail])  VALUES ('" + filename + "',0,'" + DateTime.Now.ToString(format) + "'," + sitio + ",null)";

                //int a = mycommand.ExecuteNonQuery();
                //mycommand.Connection.Close();

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

            /// <summary>
            /// Métido que traspasada el nombre del documento y sitio, al proyecto ProcesarDocumentos, donde serán separtados los contenidos en:
            /// Form, To, Cc, BodyText, BodyHtml and Header
            /// </summary>
            /// <param name="filename"></param>
            /// <param name="sitio"></param>
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