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
            /*     string[] args = System.Environment.GetCommandLineArgs();

            // If a directory is not specified, exit program.
           if (args.Length != 2)
                {
                    // Display the proper way to call the program.
                    Console.WriteLine("Usage: Watcher.exe (directory)");
                    return;
                }
*/
                // Create a new FileSystemWatcher and set its properties.
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.Path = "C:\\github\\PowerShell";
            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */

                watcher.NotifyFilter = 
                NotifyFilters.LastAccess |
                NotifyFilters.CreationTime |
                NotifyFilters.LastWrite | 
                NotifyFilters.FileName | 
                NotifyFilters.DirectoryName;
                
            // Only watch text files.
                //watcher.Filter = "*.txt";
                watcher.Filter = "*.*";

                // Add event handlers.
               //watcher.Changed += new FileSystemEventHandler(OnChanged);
               watcher.Created += new FileSystemEventHandler(OnChanged);
               //watcher.Deleted += new FileSystemEventHandler(OnChanged);
               //watcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press \'q\' to quit the sample.");
                while (Console.Read() != 'q') ;
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

            string sourcePath = @"C:\github\PowerShell";
            string targetPath = @"C:\mails_prueba";

            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Move(sourceFile, destFile);

            Console.WriteLine("sourceFile"+ sourceFile+", destfile"+destFile);
            insertondatabase();
            }


            public static void insertondatabase() {

            SqlCommand mycommand = new SqlCommand();

            mycommand.CommandType = System.Data.CommandType.Text;
            mycommand.CommandText = "INSERT INTO [dbo].[documentos] (fnombre , estado ) VALUES ( '"+resultfilename+"' ,  0 );";
            mycommand.Connection = myConnection.GetConnection();


            mycommand.ExecuteNonQuery();
 //           mycommand.Connection.Close();



        }

        }

}
