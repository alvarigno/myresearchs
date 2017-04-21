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
using System.Security.Permissions;

namespace ReadExcelFiles
{
    class Program
    {

        public static string PosicionDocumento = @"C:\\Users\\Álvaro\\Desktop\\doc excel de ejemplo\\";
        public static string listadofotos = "";
        public static string resultfilename;

        public static Dictionary<string, string> objetoditec = new Dictionary<string, string>();

        public static Dictionary<string, string> objetotipovehiculo = new Dictionary<string, string>();

        public static Dictionary<string, string> objetocombustible = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            //Diccionario DITEC
            objetoditec.Add("codigo_auto_DITEC", "");
            objetoditec.Add("codigo_auto_chileautos", "0");
            objetoditec.Add("Nuevo_o_usado", "");
            objetoditec.Add("categoria", "");
            objetoditec.Add("Tipo_vehiculo", "");
            objetoditec.Add("Carroceria", "");
            objetoditec.Add("Marca", "");
            objetoditec.Add("Modelo", "");
            objetoditec.Add("Ano", "");
            objetoditec.Add("Precio", "");
            objetoditec.Add("Color", "");
            objetoditec.Add("KM", "");
            objetoditec.Add("motor", "");
            objetoditec.Add("combustible", "");
            objetoditec.Add("Cilindrada", "");
            objetoditec.Add("tipo_cambio", "N");
            objetoditec.Add("aire_acondicionado", "N");
            objetoditec.Add("tipo_direccion", "");
            objetoditec.Add("radio", "N");
            objetoditec.Add("alzavidrios_electricos", "N");
            objetoditec.Add("espejos_electricos", "N");
            objetoditec.Add("frenos_ABS", "N");
            objetoditec.Add("airbag", "N");
            objetoditec.Add("unico_dueno", "N");
            objetoditec.Add("cierre_centralizado", "N");
            objetoditec.Add("catalitico", "N");
            objetoditec.Add("fwd", "N");
            objetoditec.Add("Llantas", "N");
            objetoditec.Add("Puertas", "");
            objetoditec.Add("Alarma", "N");
            objetoditec.Add("Techo", "");
            objetoditec.Add("comentarios", "");
            objetoditec.Add("patente", "");
            objetoditec.Add("fotos", "");
            objetoditec.Add("fecha_i_data", "");
            objetoditec.Add("fecha_update_data", "");


            //Diccionario TipoVehículo
            objetotipovehiculo.Add("Automóvil", "A");
            objetotipovehiculo.Add("Ambulancia", "AM");
            objetotipovehiculo.Add("Camioneta", "C");
            objetotipovehiculo.Add("Clasicos o de Coleccion", "CL");
            objetotipovehiculo.Add("Furgón", "F"); 
            objetotipovehiculo.Add("Todo Terreno", "T");
            objetotipovehiculo.Add("SUV", "T");
            objetotipovehiculo.Add("Taxi", "TX");
            objetotipovehiculo.Add("Van", "V");

            //Diccionario de Combustible
            objetocombustible.Add("GASOLINA","1");
            objetocombustible.Add("DIESEL", "2");
            objetocombustible.Add("Gasolina", "1");
            objetocombustible.Add("Diesel", "2");

            Run();
            Console.ReadLine();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run() {

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = PosicionDocumento;

            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.xlsx";

            // Add event handlers. inside of FileSystemWatcher DLL
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType+", name file: "+e.Name);
            resultfilename = e.FullPath;

            if (resultfilename.Contains("~$")) { } else {
                ReadExcelFile(resultfilename);
            }
            
        }

        private static void ReadExcelFile(string pathsourcefile) {

            try
            {

                //string[] filesource = Directory.GetFiles(PosicionDocumento, "*.xlsx");

                //Create COM Objects. Create a COM object for everything that is referenced
                Application xlApp = new Application();
                //Workbook xlWorkbook = xlApp.Workbooks.Open(filesource[0].ToString());
                Workbook xlWorkbook = xlApp.Workbooks.Open(pathsourcefile);
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
                                        //Console.Write("Listado de fotografías: " + listadofotos + "\r\n");
                                        objetoditec["fotos"] = listadofotos;
                                        if (objetoditec["KM"] == "") { objetoditec["KM"] = "0"; }
                                        showdata();
                                        if (ConsultaSiIdDitecExiste(int.Parse(objetoditec["codigo_auto_DITEC"]))) {

                                            if (updatedataditec(int.Parse(objetoditec["codigo_auto_DITEC"]))) {

                                                Console.Write("Proceso de Actualización, funcionó!!!");
                                                ClearDictionary();

                                            } else {

                                                Console.Write("Proceso de Actualización, No funcionó!!!");
                                                ClearDictionary();

                                            }

                                        } else if (insertdataditec()) {

                                            Console.Write("Proceso de inserción, funcionó!!!");
                                            ClearDictionary();

                                        }
                                        else {

                                            Console.Write("Proceso de inserción o actualización, No funcionó!!!");
                                            ClearDictionary();
                                        }
                                        //Console.Write("Listado de fotografías diccionario: " + objetoditec["fotos"]+"\r\n");
                                        listadofotos = "";

                                    }

                                }


                                if (xlRange.Cells[i, j].Value2.ToString() == "codigo_auto_DITEC")
                                {
                                    Console.Write("\r\n");
                                    Console.Write("------------------------------------------------ \n");
                                    //Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                                    //Console.Write(xlRange.Cells[i, j + 1].Value2.ToString() + "\t\n");

                                    objetoditec["codigo_auto_DITEC"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                    //Console.Write("codigo_auto_DITEC: " + objetoditec["codigo_auto_DITEC"] + "\t\n");

                                    codditect = xlRange.Cells[i, j + 1].Value2.ToString();
                                    //Console.Write("\r\n");
                                }
                                else if (xlRange.Cells[i, j].Value2.ToString() != nomfoto)
                                {
                                    //Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                                    //Console.Write(xlRange.Cells[i, j + 1].Value2.ToString() + "\t\n");

                                    if (xlRange.Cells[i, j].Value2.ToString() == "codigo_auto_chileautos")
                                    {
                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "")
                                        {
                                            objetoditec["codigo_auto_chileautos"] = "0";
                                        }
                                        else { objetoditec["codigo_auto_chileautos"] = xlRange.Cells[i, j + 1].Value2.ToString();  }
                                        //Console.Write("codigo_auto_chileautos: " + objetoditec["codigo_auto_chileautos"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Nuevo_o_usado")
                                    {
                                        objetoditec["Nuevo_o_usado"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("Nuevo_o_usado: " + objetoditec["Nuevo_o_usado"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Tipo_vehiculo")
                                    {
                                        int codigo = GetCategoriaId(xlRange.Cells[i, j + 1].Value2.ToString());
                                        objetoditec["Tipo_vehiculo"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        objetoditec["categoria"] = codigo.ToString();
                                        //Console.Write("Tipo_vehiculo: " + objetoditec["Tipo_vehiculo"] + ", Cat. Vehículo: "+ objetoditec["categoria"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Carroceria")
                                    {

                                        objetoditec["Carroceria"] = GetCodCarroceria(xlRange.Cells[i, j + 1].Value2.ToString(), int.Parse(objetoditec["categoria"]));
                                        //Console.Write("Carroceria: " + objetoditec["Carroceria"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Marca")
                                    {

                                        objetoditec["Marca"] = GetCodMarca(xlRange.Cells[i, j + 1].Value2.ToString()).ToString();
                                        //Console.Write("Marca: " + xlRange.Cells[i, j + 1].Value2.ToString() + ", cod: "+ objetoditec["Marca"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Modelo")
                                    {

                                        objetoditec["Modelo"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("Modelo: " + objetoditec["Modelo"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Ano")
                                    {

                                        objetoditec["Ano"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("Ano: " + objetoditec["Ano"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Precio")
                                    {

                                        objetoditec["Precio"] = xlRange.Cells[i, j + 1].Value2.ToString().Replace("$", "");
                                        objetoditec["Precio"] = objetoditec["Precio"].Replace(".", "");
                                        //Console.Write("Precio: " + objetoditec["Precio"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Color")
                                    {

                                        objetoditec["Color"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("Color: " + objetoditec["Color"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "KM")
                                    {
                                        objetoditec["KM"] = "";
                                        objetoditec["KM"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("KM: " + objetoditec["KM"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "motor")
                                    {

                                        objetoditec["motor"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("motor: " + objetoditec["motor"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "combustible")
                                    {
                                        if (objetocombustible.ContainsKey(xlRange.Cells[i, j + 1].Value2.ToString())) {

                                            objetoditec["combustible"] = objetocombustible[xlRange.Cells[i, j + 1].Value2.ToString()];

                                        }
                                        
                                        //Console.Write("combustible: " + objetoditec["combustible"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Cilindrada")
                                    {

                                        objetoditec["Cilindrada"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("Cilindrada: " + objetoditec["Cilindrada"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "tipo_cambio")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S") {

                                            objetoditec["tipo_cambio"] = "S";

                                        } else {

                                            objetoditec["tipo_cambio"] = "N";
                                            

                                        }
                                        //Console.Write("tipo_cambio: " + objetoditec["tipo_cambio"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "aire_acondicionado")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["aire_acondicionado"] = "S";
                                            
                                        }
                                        else
                                        {

                                            objetoditec["aire_acondicionado"] = "N";
                                            
                                        }
                                        //Console.Write("aire_acondicionado: " + objetoditec["aire_acondicionado"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "tipo_direccion")
                                    {

                                        objetoditec["tipo_direccion"] = xlRange.Cells[i, j].Value2.ToString();
                                        //Console.Write("tipo_direccion: " + objetoditec["tipo_direccion"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "radio")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["radio"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["radio"] = "N";

                                        }
                                        //Console.Write("radio: " + objetoditec["radio"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "alzavidrios_electricos")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["alzavidrios_electricos"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["alzavidrios_electricos"] = "N";

                                        }
                                        //Console.Write("alzavidrios_electricos: " + objetoditec["alzavidrios_electricos"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "espejos_electricos")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["espejos_electricos"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["espejos_electricos"] = "N";

                                        }
                                        //Console.Write("espejos_electricos: " + objetoditec["espejos_electricos"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "frenos_ABS")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["frenos_ABS"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["frenos_ABS"] = "N";

                                        }
                                        //Console.Write("frenos_ABS: " + objetoditec["frenos_ABS"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "airbag")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["airbag"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["airbag"] = "N";

                                        }
                                        //Console.Write("airbag: " + objetoditec["airbag"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "unico_dueno")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["unico_dueno"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["unico_dueno"] = "N";

                                        }
                                        //Console.Write("unico_dueno: " + objetoditec["unico_dueno"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "cierre_centralizado")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["cierre_centralizado"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["cierre_centralizado"] = "N";

                                        }
                                        //Console.Write("cierre_centralizado: " + objetoditec["cierre_centralizado"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "catalitico")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["catalitico"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["catalitico"] = "N";

                                        }
                                        //Console.Write("catalitico: " + objetoditec["catalitico"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "fwd")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["fwd"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["fwd"] = "N";

                                        }
                                        //Console.Write("fwd: " + objetoditec["fwd"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Llantas")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["Llantas"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["Llantas"] = "N";

                                        }
                                        //Console.Write("Llantas: " + objetoditec["Llantas"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Puertas")
                                    {

                                        objetoditec["Puertas"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("Puertas: " + objetoditec["Puertas"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Alarma")
                                    {

                                        if (xlRange.Cells[i, j + 1].Value2.ToString() == "S")
                                        {

                                            objetoditec["Alarma"] = "S";

                                        }
                                        else
                                        {

                                            objetoditec["Alarma"] = "N";

                                        }
                                        //Console.Write("Alarma: " + objetoditec["Alarma"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "Techo")
                                    {

                                        objetoditec["Techo"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("Techo: " + objetoditec["Techo"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "comentarios")
                                    {

                                        objetoditec["comentarios"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("comentarios: " + objetoditec["comentarios"] + "\t\n");

                                    }

                                    if (xlRange.Cells[i, j].Value2.ToString() == "patente")
                                    {

                                        objetoditec["patente"] = xlRange.Cells[i, j + 1].Value2.ToString();
                                        //Console.Write("patente: " + objetoditec["patente"] + "\t\n");

                                    }

                                    
                                }
                               
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
                //Console.Write("Finalización de lectura de archivo: " + filesource[0].ToString());
                Console.Write("Finalización de lectura de archivo: " + pathsourcefile); 
                Console.Write("\r\n ----------------------------------------------\r\n");
                

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

                //Console.Write(e.Message);
                listadofotos = listadofotos + e.Message +"*";
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

        private static string GetCodCarroceria(string namecarroceria, int codcategoria)
        {

            string codcarroceria = "";
            myConnection myConn = new myConnection();

            if (namecarroceria == "Sedan")
            {
                namecarroceria = "Sedán";
            }

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myConnection.GetConnection();
                    connection.CommandText = "select t2.inicarr as codcarroceria from tabcarroceria t1, tabcarroceriasXCategoria t2 where t2.inicarr = t1.inicarr and t2.idCategoria = "+ codcategoria + " and t1.carroceria = '"+ namecarroceria + "'";
                    
                    using (var reader = connection.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            if (reader.Read())
                            {

                                codcarroceria = reader["codcarroceria"].ToString();

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

            return codcarroceria;
        }

        private static int GetCategoriaId(string tipovehiculo) {

            int codcategoria = 0;
            myConnection myConn = new myConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myConnection.GetConnection();
                    connection.CommandText = "SELECT t2.idCategoria as categoria FROM[tabCategoria-Tipo] t1 JOIN tabCategorias t2 ON(t1.idCategoria = t2.idCategoria) JOIN tipos t3 ON(t1.idTipoVeh = t3.tveh) WHERE t1.idTipoVeh = '" + tipovehiculo + "'";
                    
                    using (var reader = connection.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            if (reader.Read())
                            {

                                codcategoria = int.Parse(reader["categoria"].ToString());

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

            return codcategoria;


        }

        private static void showdata() {

            foreach (KeyValuePair<string, string> kvp in objetoditec)
            {

                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

        }

        private static Boolean insertdataditec()
        {
            Boolean inserto = false;
            myLocalConnection myLocalConn = new myLocalConnection();
            
            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myLocalConnection.GetLocalConnection();
                    connection.CommandText = "INSERT INTO[dbo].[tabautosDITEC]([codigo_auto_DITEC] ,[codigo_auto_chileautos] ,[Nuevo_o_usado] ,[categoria] ,[Tipo_vehiculo] ,[Carroceria] ,[Marca] ,[Modelo] ,[Ano] ,[Precio] ,[Color], [KM] ,[motor] ,[combustible] ,[Cilindrada] ,[tipo_cambio] ,[aire_acondicionado] ,[tipo_direccion] ,[radio] ,[alzavidrios_electricos] ,[espejos_electricos] ,[frenos_ABS] ,[airbag] ,[unico_dueno] ,[cierre_centralizado] ,[catalitico] ,[fwd] ,[Llantas] ,[Puertas] ,[Alarma] ,[Techo] ,[comentarios] ,[patente] ,[fotos] ,[fecha_i_data],[fecha_update_data])";
                    connection.CommandText = connection.CommandText + "VALUES("+ objetoditec["codigo_auto_DITEC"] +", "+ objetoditec["codigo_auto_chileautos"] + ",'" + objetoditec["Nuevo_o_usado"]+"',"+ objetoditec["categoria"] +",'"+ objetoditec["Tipo_vehiculo"] +"','"+ objetoditec["Carroceria"]+"' ,"+ int.Parse(objetoditec["Marca"])+" ,'"+ objetoditec["Modelo"]+"' ,"+ objetoditec["Ano"]+" ,"+ objetoditec["Precio"]+" ,'"+ objetoditec["Color"] + "',"+ int.Parse(objetoditec["KM"])+" ,'"+ objetoditec["motor"]+"' ,'"+ objetoditec["combustible"]+"' ,'"+ objetoditec["Cilindrada"]+"' ,'"+ objetoditec["tipo_cambio"]+"' ,'"+ objetoditec["aire_acondicionado"]+"' ,'"+ objetoditec["tipo_direccion"]+"' ,'"+ objetoditec["radio"]+ "','"+ objetoditec["alzavidrios_electricos"]+"' ,'"+ objetoditec["espejos_electricos"]+"' ,'"+ objetoditec["frenos_ABS"]+"' ,'"+ objetoditec["airbag"]+"' ,'"+ objetoditec["unico_dueno"]+"' ,'"+ objetoditec["cierre_centralizado"]+"' ,'"+ objetoditec["catalitico"]+"' ,'"+ objetoditec["fwd"]+"' ,'"+ objetoditec["Llantas"]+"' ,'"+ objetoditec["Puertas"]+"' ,'"+ objetoditec["Alarma"]+"' ,'"+ objetoditec["Techo"]+"' ,'"+ objetoditec["comentarios"]+"' ,'"+ objetoditec["patente"]+"' ,'"+ objetoditec["fotos"]+ "' ,getdate(),'' )";
                    
                    connection.ExecuteNonQuery();

                    connection.Connection.Close();
                    connection.Connection.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }

                inserto = true;

            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex.Message);
            }

            return inserto;
        }

        private static Boolean updatedataditec(int codigoditec)
        {
            Boolean actualizo = false;
            myLocalConnection myLocalConn = new myLocalConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myLocalConnection.GetLocalConnection();
                    connection.CommandText = "UPDATE[dbo].[tabautosDITEC] SET[codigo_auto_chileautos] = " + objetoditec["codigo_auto_chileautos"] + ", [Nuevo_o_usado] ='" + objetoditec["Nuevo_o_usado"] + "',[categoria] = " + objetoditec["categoria"] + ",[Tipo_vehiculo] = '" + objetoditec["Tipo_vehiculo"] + "', [Carroceria] ='" + objetoditec["Carroceria"] + "' ,[Marca] = " + int.Parse(objetoditec["Marca"]) + " ,[Modelo] = '" + objetoditec["Modelo"] + "' ,[Ano] = " + objetoditec["Ano"] + " ,[Precio] = " + objetoditec["Precio"] + " ,[Color] = '" + objetoditec["Color"] + "',[KM] = " + int.Parse(objetoditec["KM"]) + " ,[motor] = '" + objetoditec["motor"] + "' ,[combustible] = '" + objetoditec["combustible"] + "' ,[Cilindrada] = '" + objetoditec["Cilindrada"] + "' ,[tipo_cambio] = '" + objetoditec["tipo_cambio"] + "' ,[aire_acondicionado] = '" + objetoditec["aire_acondicionado"] + "' ,[tipo_direccion] = '" + objetoditec["tipo_direccion"] + "' ,[radio] = '" + objetoditec["radio"] + "',[alzavidrios_electricos] = '" + objetoditec["alzavidrios_electricos"] + "' ,[espejos_electricos] = '" + objetoditec["espejos_electricos"] + "' ,[frenos_ABS] = '" + objetoditec["frenos_ABS"] + "' ,[airbag] = '" + objetoditec["airbag"] + "' ,[unico_dueno] = '" + objetoditec["unico_dueno"] + "' ,[cierre_centralizado] = '" + objetoditec["cierre_centralizado"] + "' ,[catalitico] = '" + objetoditec["catalitico"] + "' ,[fwd] = '" + objetoditec["fwd"] + "' ,[Llantas] = '" + objetoditec["Llantas"] + "' ,[Puertas] = '" + objetoditec["Puertas"] + "' ,[Alarma] = '" + objetoditec["Alarma"] + "' ,[Techo] = '" + objetoditec["Techo"] + "' ,[comentarios] = '" + objetoditec["comentarios"] + "' ,[patente] = '" + objetoditec["patente"] + "' ,[fotos] = '" + objetoditec["fotos"] + "' ,[fecha_update_data] = getdate() WHERE codigo_auto_DITEC = "+ codigoditec + "";

                    connection.ExecuteNonQuery();

                    connection.Connection.Close();
                    connection.Connection.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                }

                actualizo = true;

            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex.Message);
            }

            return actualizo;
        }

        private static Boolean ConsultaSiIdDitecExiste(int codditec)
        {

            Boolean existe = false;
            myLocalConnection myLocalConn = new myLocalConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myLocalConnection.GetLocalConnection();
                    connection.CommandText = "select * from [tabautosDITEC] where codigo_auto_DITEC = "+ codditec + "";

                    using (var reader = connection.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            existe = true;
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

            return existe;


        }

        private static void ClearDictionary() {

            //Diccionario DITEC
            objetoditec["codigo_auto_DITEC"] = "";
            objetoditec["codigo_auto_chileautos"] = "0";
            objetoditec["Nuevo_o_usado"] = "";
            objetoditec["categoria"] = "";
            objetoditec["Tipo_vehiculo"] = "";
            objetoditec["Carroceria"] = "";
            objetoditec["Marca"] = "";
            objetoditec["Modelo"] = "";
            objetoditec["Ano"] = "";
            objetoditec["Precio"] = "";
            objetoditec["Color"] = "";
            objetoditec["KM"] = "";
            objetoditec["motor"] = "";
            objetoditec["combustible"] = "";
            objetoditec["Cilindrada"] = "";
            objetoditec["tipo_cambio"] = "N";
            objetoditec["aire_acondicionado"] = "N";
            objetoditec["tipo_direccion"] = "";
            objetoditec["radio"] = "N";
            objetoditec["alzavidrios_electricos"] = "N";
            objetoditec["espejos_electricos"] = "N";
            objetoditec["frenos_ABS"] = "N";
            objetoditec["airbag"] = "N";
            objetoditec["unico_dueno"] = "N";
            objetoditec["cierre_centralizado"] = "N";
            objetoditec["catalitico"] = "N";
            objetoditec["fwd"] = "N";
            objetoditec["Llantas"] = "N";
            objetoditec["Puertas"] = "N";
            objetoditec["Alarma"] = "N";
            objetoditec["Techo"] = "";
            objetoditec["comentarios"] = "";
            objetoditec["patente"] = "";
            objetoditec["fotos"] = "";
            objetoditec["fecha_i_data"] = "";
            objetoditec["fecha_update_data"] = "";

        }

    }
}
