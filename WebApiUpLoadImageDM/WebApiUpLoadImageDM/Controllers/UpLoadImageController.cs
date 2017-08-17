﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApiUpLoadImageDM.Models;
using WebApiUpLoadImageDM.Infrastructure;
using UpLoadFile;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace WebApiUpLoadImageDM.Controllers
{

    [RoutePrefix("API-CLAImagDM/Upload")]
    public class UpLoadImageController : ApiController
    {

        string nombrerealarchivo;
        string uploadFolderPath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\fileLoaded\\";
        string archivolocal = "";
        Task taskDocumento;
        string img1;
        string img2;


        [HttpPost]
        [EnableCors("*", "*", "POST,OPTIONS")]
        public async Task<HttpResponseMessage> Upload()
        {

            DM_ImgUploadServer ImagesDm = new DM_ImgUploadServer();
            CA_ImgUploadServer ImagesCa = new CA_ImgUploadServer();

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = uploadFolderPath+postedFile.FileName;
                    postedFile.SaveAs(filePath);

                    string dataDM = ImagesDm.Uploadimage(filePath);
                    img1 = "http://images.demotores.cl/post/tmp/siteposting/" + dataDM;
                    img2 = ImagesCa.Uploadimage(filePath);

                    docfiles.Add(img1);
                    docfiles.Add(img2);
                    taskDocumento = Task.Factory.StartNew(() => eliminaDoc(filePath));
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;

        }

        private static bool SiArchivoEstaBloqueado(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //archivo en uso
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //archivo liberado
            return false;
        }

        private static Boolean DetectaArchivoEnUso(string targetDirectory)
        {
            Boolean eliminado = true;


                FileInfo file = new FileInfo(targetDirectory);

                while (eliminado == true)
                {

                    eliminado = SiArchivoEstaBloqueado(file);

                }


            return eliminado;
        }

        public void eliminaDoc(string urlfile) {


            if (File.Exists(urlfile)) {

                if (DetectaArchivoEnUso(urlfile) == false) {

                    File.Delete(@urlfile);
                    if (taskDocumento.IsCompleted)
                    {

                        taskDocumento.Dispose();

                    }


                }

            }

        }

    }
}