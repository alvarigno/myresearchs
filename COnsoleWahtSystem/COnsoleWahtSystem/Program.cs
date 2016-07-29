using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace COnsoleWahtSystem
{

public class Watcher
        {


        public static string resultfilename;
        myConnection myConnection = new myConnection();
        

        public static void Main()
            {

               myConnection.GetConnection();

                Run();
               Console.ReadLine();
            }

            [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
            public static void Run()
            {
                // Create a new FileSystemWatcher and set its properties.
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = "C:\\github\\PowerShell";
                //watcher.Path = "D:\\git\\mailparser\\emailprospectos";

                /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */

                watcher.NotifyFilter = 
                NotifyFilters.LastAccess |
                NotifyFilters.CreationTime |
                NotifyFilters.LastWrite | 
                NotifyFilters.FileName | 
                NotifyFilters.DirectoryName;
                
                // Only watch text files.
                //watcher.Filter = "*.txt";
                watcher.Filter = "*.eml*";

                // Add event handlers.
               //watcher.Changed += new FileSystemEventHandler(OnChanged);
               watcher.Created += new FileSystemEventHandler(OnChanged);
               //watcher.Deleted += new FileSystemEventHandler(OnChanged);
               //watcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press \'q\' to quit the sample.");
               // while (Console.Read() != 'q') ;

            }

            // Define the event handlers.
            public static void OnChanged(object source, FileSystemEventArgs e)
            {
            // Specify what is done when a file is changed, created, or deleted.
                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                resultfilename = Path.GetFileName(e.FullPath);
                muevearchivo(resultfilename);
            }

            private static void OnRenamed(object source, RenamedEventArgs e)
            {
                // Specify what is done when a file is renamed.
                Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
            }

            private static void muevearchivo(String fileName) {

                    if (IsFileLocked(fileName)) {

                string sourcePath = @"C:\github\PowerShell";
                string targetPath = @"C:\mails_prueba";
                //string sourcePath = @"D:\git\mailparser\emailprospectos";
                //string targetPath = @"D:\git\mailparser\documentos";

                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                        string destFile = System.IO.Path.Combine(targetPath, fileName);

                        if (File.Exists(destFile))
                        {
                            Console.WriteLine("Archivo ya existe");
                        }
                        else { 

                            System.IO.File.Move(sourceFile, destFile);
                            Console.WriteLine("sourceFile" + sourceFile + ", destfile" + destFile);
                            insertondatabase();

                        }

                    } else {

                        Console.WriteLine("Archivo: "+fileName+", no logró ser procesado");

                    }
            }

            static bool validaarchivo(string filepruba) {

                FileStream fs = new FileStream(filepruba, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                if (fs.CanRead)
                {
                    Console.WriteLine("Archivo: "+filepruba+" pruede ser procesado.");
                    return true;
                }
                return false;
                //string mensaje = "Archivo: " + filepruba + " pruede ser procesado. esta en uso por algun otro proceso.";
            
            }

            public static void insertondatabase() {

                SqlCommand mycommand = new SqlCommand();

                mycommand.CommandType = System.Data.CommandType.Text;
                mycommand.CommandText = "INSERT INTO [dbo].[documentos] (fnombre , estado, sitio ) VALUES ( '"+resultfilename+"' ,  0, 1 );";
                mycommand.Connection = myConnection.GetConnection();


                mycommand.ExecuteNonQuery();
     //           mycommand.Connection.Close();



            }

            static bool IsFileLocked(string filemio)
            {
                FileInfo filepruba = new FileInfo(filemio);
                FileStream stream = null;
                string registroerror ="";

                try
                {
                    stream = filepruba.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (IOException e)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)
                    Console.WriteLine("Archivo: " + filepruba + " pruede ser procesado. ");
                    registroerror = e.ToString();
                    return true;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }

                DateTime localDate = DateTime.Now;
                //file is not locked
                string log = "Archivo: " + filepruba + " - Fecha: " + localDate + ", no puede ser procesado. "+ registroerror;
                Console.WriteLine("Archivo: " + filepruba +" - Fecha: "+ localDate + ", no puede ser procesado. "+ registroerror);
                File.WriteAllText(localDate + "log.txt", log);
                return false;
            }

        }

}
