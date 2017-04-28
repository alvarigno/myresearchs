using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicarDITEC
{
    class Program
    {

        public static List<publicacion> listOfDatos = new List<publicacion>();
        static int count = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Espera de script de ejecución");

            consultadatos();
            
            Console.Read();
        }


        public static void consultadatos() {

             myLocalConnection myLocalConn = new myLocalConnection();

                try
                {
                    using (var connection = new System.Data.SqlClient.SqlCommand())
                    {
                        connection.Connection = myLocalConnection.GetLocalConnection();
                        connection.CommandText = "select * from tabautosDITEC";

                        using (var reader = connection.ExecuteReader())
                        {
                            while (reader.HasRows)
                            {

                                if (reader.Read()) {

                                    //Console.Write(reader["codigo_auto_DITEC"] +"\t\n");

                                    var datosavisos = new publicacion();
                                    datosavisos.codigo_auto_DITEC = int.Parse(reader["codigo_auto_DITEC"].ToString());
                                    datosavisos.Nuevo_o_usado = reader["Nuevo_o_usado"].ToString();
                                    datosavisos.categoria = int.Parse(reader["categoria"].ToString());
                                    datosavisos.Tipo_vehiculo = reader["Tipo_vehiculo"].ToString();
                                    datosavisos.Carroceria = reader["Carroceria"].ToString();
                                    datosavisos.Marca = int.Parse(reader["Marca"].ToString());
                                    datosavisos.Modelo = reader["Modelo"].ToString();
                                    datosavisos.Ano = int.Parse(reader["Ano"].ToString());
                                    datosavisos.Precio = int.Parse(reader["Precio"].ToString());
                                    datosavisos.Color = reader["Color"].ToString();
                                    datosavisos.KM = int.Parse(reader["KM"].ToString());
                                    datosavisos.motor = reader["motor"].ToString();
                                    datosavisos.combustible = int.Parse(reader["combustible"].ToString());
                                    datosavisos.Cilindrada = int.Parse(reader["Cilindrada"].ToString());
                                    datosavisos.tipo_cambio = reader["tipo_cambio"].ToString();
                                    datosavisos.aire_acondicionado = reader["aire_acondicionado"].ToString();
                                    datosavisos.tipo_direccion = reader["tipo_direccion"].ToString();
                                    datosavisos.radio = reader["radio"].ToString();
                                    datosavisos.alzavidrios_electricos = reader["alzavidrios_electricos"].ToString();
                                    datosavisos.espejos_electricos = reader["espejos_electricos"].ToString();
                                    datosavisos.frenos_ABS = reader["frenos_ABS"].ToString();
                                    datosavisos.airbag = reader["airbag"].ToString();
                                    datosavisos.unico_dueno = reader["unico_dueno"].ToString();
                                    datosavisos.cierre_centralizado = reader["cierre_centralizado"].ToString();
                                    datosavisos.catalitico = reader["catalitico"].ToString();
                                    datosavisos.fwd = reader["fwd"].ToString();
                                    datosavisos.Llantas = reader["Llantas"].ToString();
                                    datosavisos.Puertas = reader["Puertas"].ToString();
                                    datosavisos.Alarma = reader["Alarma"].ToString();
                                    datosavisos.Techo = reader["Techo"].ToString();
                                    datosavisos.comentarios = reader["comentarios"].ToString();
                                    datosavisos.patente = reader["patente"].ToString();
                                    datosavisos.fotos = reader["fotos"].ToString();
                                    listOfDatos.Add(datosavisos);
                                    muestralista(listOfDatos, count);

                                }

                                count = count + 1;
                            //Console.WriteLine("Cuenta: "+ listOfDatos.Count);
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

        public static void muestralista(List<publicacion> datos, int i) {

            try {
                
                    Console.WriteLine("Num. " + i + ", Listado: " + datos[i].codigo_auto_DITEC);



            } catch(Exception e) { Console.WriteLine("Error: " + e.Message); }

        }


        public static void getcodnonjto() {


        }

    }
}
