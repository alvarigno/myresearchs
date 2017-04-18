using System;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Threading;
using System.ComponentModel;
using Transbank.NET;
using System.Text.RegularExpressions;

namespace ReadExcelFiles
{
    class Program
    {

        public static string PosicionDocumento = @"C:\\Users\\Álvaro\\Desktop\\doc excel de ejemplo\\";
        public static string listadofotos = "";


        static void Main(string[] args)
        {


            try
            {

                string[] filesource = Directory.GetFiles(PosicionDocumento, "*.xlsx");

                //Create COM Objects. Create a COM object for everything that is referenced
                Application xlApp = new Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(filesource[0].ToString());
                _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                int contador = 1;
                
                string codditect = "";

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                for (int i = 1; i <= rowCount; i++)
                {

                    for (int j = 1; j <= colCount; j++)
                    {

                        //new line
                        if (j == 1)
                        {

                            //Console.Write("\r\n");
                            //write the value to the console
                            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null && xlRange.Cells[i, j + 1].Value2 != null)
                            {

                                string nomfoto = "foto_" + contador;

                                if (xlRange.Cells[i, j].Value2.ToString() == nomfoto)
                                {
                                    //Console.Write("\r\n");
                                    //Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                                    //Console.Write(xlRange.Cells[i, j + 1].Value2.ToString() + "\t");


                                    if (BtnDownload_Click(xlRange.Cells[i, j + 1].Value2.ToString(), codditect) == true)
                                    {


                                    }

                                    contador = contador + 1;

                                }
                                else
                                {

                                    contador = 1;
                                    if (listadofotos != "")
                                    {
                                        listadofotos = listadofotos.Remove(listadofotos.Length - 1);
                                        Console.Write("Listado de fotografías: " + listadofotos + "\r\n");
                                        listadofotos = "";

                                    }


                                }


                                if (xlRange.Cells[i, j].Value2.ToString() == "codigo_auto_DITEC")
                                {
                                    Console.Write("\r\n");
                                    Console.Write("------------------------------------------------ \n");
                                    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                                    Console.Write(xlRange.Cells[i, j + 1].Value2.ToString() + "\t\n");
                                    codditect = xlRange.Cells[i, j + 1].Value2.ToString();
                                    //Console.Write("\r\n");
                                }
                                else if (xlRange.Cells[i, j].Value2.ToString() != nomfoto)
                                {
                                    Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                                    Console.Write(xlRange.Cells[i, j + 1].Value2.ToString() + "\t\n");

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Marca") {

                                        Console.Write("Cod marca: "+ GetCodMarca(xlRange.Cells[i, j + 1].Value2.ToString())+"\t\n");

                                    }


                                }
                                Getlocalconnect();
                            }
                        }
                    }

                }

                //cleanup
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //rule of thumb for releasing com objects:
                //  never use two dots, all COM objects must be referenced and released individually
                //  ex: [somthing].[something].[something] is bad

                //release com objects to fully kill excel process from running in the background
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);

                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
                Console.Write("\r\n ----------------------------------------------\r\n");
                Console.Write("Finalización de lectura de archivo: "+ filesource[0].ToString());
                Console.Write("\r\n ----------------------------------------------\r\n");
                Console.ReadLine();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());

            }

        }
        
        private static Boolean BtnDownload_Click(string urlfilename, string namefolder)
        {
            Boolean cargo = false;


            try
            {

                string pathfile = System.IO.Path.Combine(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory) + "\\carga", namefolder);
                System.IO.Directory.CreateDirectory(pathfile);

                string filename = "";
                Uri uri = new Uri(urlfilename);

                filename = System.IO.Path.GetFileName(uri.Segments.Last(seg => seg.Contains(".")));

                using (WebClient webClient = new WebClient())
                {

                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate (object sender, DownloadProgressChangedEventArgs e)
                    {
                        Console.WriteLine("Downloaded:" + e.ProgressPercentage.ToString());
                    });

                    webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler
                        (delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                        {
                            if (e.Error == null && !e.Cancelled)
                            {
                                Console.WriteLine("Download completed!");
                                
                            }
                        });
                    //webClient.DownloadFileAsync(new System.Uri(urlfilename), pathfile + "\\" + filename);
                    webClient.DownloadFile(new System.Uri(urlfilename), pathfile + "\\" + filename);
                    cargo = true;
                    listadofotos = listadofotos + pathfile + "\\" + filename + "*";
                }

            }
            catch (Exception e) {

                Console.Write(e.Message);
                cargo = false;
            }

            return cargo;
        }
        
        private static int GetCodMarca(string namemarca)
        {

            int codmarca = 0;
            myConnection myConn = new myConnection();

            if (namemarca == "MERCEDES-BENZ") {
                namemarca = Regex.Replace(namemarca, @"-", " ", RegexOptions.Multiline).Trim();
            }

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myConnection.GetConnection();
                    connection.CommandText = "select COD_MARCA from tabmarcas where DES_MARCA like '%"+ namemarca +"%'";

                    using (var reader = connection.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            if (reader.Read())
                            {

                                codmarca = int.Parse(reader["COD_MARCA"].ToString());

                            }

                        }

                    }
                    connection.Connection.Close();
                    connection.Connection.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }


            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex.Message);

            }

            return codmarca;
        }


        private static void Getlocalconnect()
        {

            int codmarca = 0;
            myLocalConnection myLocalConn = new myLocalConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myLocalConnection.GetLocalConnection();
                    connection.CommandText = "select * from tabautos";

                    using (var reader = connection.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            if (reader.Read())
                            {

                                codmarca = int.Parse(reader["COD_MARCA"].ToString());

                            }

                        }

                    }
                    connection.Connection.Close();
                    connection.Connection.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }


            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex.Message);

            }

        }

    }
}
