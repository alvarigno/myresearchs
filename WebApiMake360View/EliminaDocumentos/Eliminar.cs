using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EliminaDocumentos
{
    public class Eliminar
    {

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

        public static void eliminaDoc(Task data, string urlfile)
        {
            Task proceso = data;

            if (File.Exists(urlfile))
            {

                if (DetectaArchivoEnUso(urlfile) == false)
                {

                    File.Delete(@urlfile);
                    if (proceso.IsCompleted)
                    {

                        proceso.Dispose();

                    }


                }

            }

        }


    }
}