﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PublicarDITEC.Data;

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
                    //connection.CommandText = "select * from tabautosDITEC where cast(fecha_update_data as date) = CONVERT (date, SYSDATETIMEOFFSET())  ";

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
                                    datosavisos.Version = reader["Version"].ToString();
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

                if (ConsultaEstadoActualizacion(datos[i].codigo_auto_DITEC))
                {

                    PublicacionChileautos datoparaactualizar = new PublicacionChileautos();

                    datoparaactualizar = llenaavisoautomotora(datos, i);

                }
                else {

                    PublicacionChileautos datoparapublicacion = new PublicacionChileautos();

                    datoparapublicacion = llenaavisoautomotora(datos,i);

                    var vars = publicaavisoautomotora(datoparapublicacion);

                    SP_PublicarAviso_Automotoras_Result result = (SP_PublicarAviso_Automotoras_Result)vars;

                    if (updateregistro(datos[i].codigo_auto_DITEC, (int)result.codauto))
                    {

                        Console.WriteLine("ingreso codigo chileautos");

                    }
                    else {

                        Console.WriteLine("Falló el ingreso código chileautos");

                    }

                    Console.WriteLine("Codigo jato: " + datoparapublicacion.datosVehiculo.uidJato+", resultado: "+result.error);
                }

            } catch(Exception e) {
                Console.WriteLine("Error: " + e.Message);
            }

        }




        public static PublicacionChileautos llenaavisoautomotora(List<publicacion> datos, int i) {

            PublicacionChileautos datopublicacion = new PublicacionChileautos();
            datosVehiculo dv = new datosVehiculo();
            datosEquipamiento de = new datosEquipamiento();

            int codigo = getCodigoJajoNoJato(GetDescMarca(datos[i].Marca), datos[i].Modelo, datos[i].Version, datos[i].Carroceria, int.Parse(datos[i].Puertas), datos[i].Ano, datos[i].tipo_cambio, datos[i].combustible, "", datos[i].categoria);

            //Datos del vehículo

            datopublicacion.codCliente = 1028;
            datopublicacion.ip = "localhost";
            dv.patente = datos[i].patente;
            dv.tipo = datos[i].Tipo_vehiculo;
            dv.marca = datos[i].Marca;
            dv.modelo = datos[i].Modelo;
            dv.ano = datos[i].Ano;
            dv.version = datos[i].Version;
            dv.carroceria = datos[i].Carroceria;
            dv.puertas = int.Parse(datos[i].Puertas);
            dv.tipoDireccion = datos[i].tipo_direccion;
            dv.precio = datos[i].Precio;
            dv.cilindrada = datos[i].Cilindrada;
            dv.potencia = "";
            dv.color = datos[i].Color;
            dv.kilom = datos[i].KM;
            dv.motor = datos[i].motor;
            dv.techo = datos[i].Techo;
            dv.combustible = datos[i].combustible;
            dv.comentario = datos[i].comentarios;
            dv.uidJato = codigo;

            datopublicacion.datosVehiculo = dv;

            //datos equipamiento
            de.airbag = datos[i].airbag;
            de.aireAcon = datos[i].aire_acondicionado;
            de.alarma = datos[i].Alarma;
            de.alzaVidrios = datos[i].alzavidrios_electricos;
            de.nuevo = datos[i].Nuevo_o_usado;
            de.transmision = datos[i].tipo_cambio;
            de.radio = datos[i].radio;
            de.espejos = datos[i].espejos_electricos;
            de.frenosAbs = datos[i].frenos_ABS;
            de.unicoDueno = datos[i].unico_dueno;
            de.cierreCentral = datos[i].cierre_centralizado;
            de.catalitico = datos[i].catalitico;
            de.fwd = datos[i].fwd;
            de.llantas = datos[i].Llantas;
            de.fotos = datos[i].fotos;
            de.plataforma = "DTC";

            datopublicacion.datosEquipamiento = de;


            return datopublicacion;
        }


        public static int getCodigoJajoNoJato(string marca, string modelo,  string version,  string carroceria,  int puertas, int ano,  string transmision, int combustible, string edicion, int categoria) {

            if (edicion == "")
            {

                edicion = "-";
            }

            if (transmision == "S")
            {

                transmision = "A";

            }
            else
            {

                transmision = "M";

            }

            int uid =0;
            try {

                bdToolsEntities bdTools = new bdToolsEntities();

                var uidJato = bdTools.bdj_idJato_SEL_marca_modelo_version_carroceria_ptas_ano_trans_ltl(marca, modelo, version, carroceria, puertas, ano, transmision, edicion);

                uid = (int)uidJato;

                if (uidJato == null || uidJato == -1)
                {
                    uidJato = bdTools.SP_bdj_getNonJatoID(categoria, marca, modelo, ano, carroceria, transmision, combustible.ToString());
                    uid = (int)uidJato;
                }

            } catch (Exception e) {

                Console.WriteLine("Erro: "+ e.Message);

            }

            return uid;
        }

        public static object publicaavisoautomotora(PublicacionChileautos dato) {
            
            baseprodEntities baseprod = new baseprodEntities();

            //var logrado = baseprod.SP_PublicarAviso_Automotoras(dato.codCliente, dato.ip, dato.datosVehiculo.patente, dato.datosVehiculo.tipo, dato.datosVehiculo.marca, dato.datosVehiculo.modelo, dato.datosVehiculo.ano, dato.datosVehiculo.version, dato.datosVehiculo.carroceria, dato.datosVehiculo.puertas, dato.datosVehiculo.tipoDireccion, dato.datosVehiculo.precio, dato.datosVehiculo.cilindrada, dato.datosVehiculo.potencia, dato.datosVehiculo.color, dato.datosVehiculo.kilom, dato.datosVehiculo.motor, dato.datosVehiculo.techo, dato.datosVehiculo.combustible, dato.datosVehiculo.comentario, dato.datosVehiculo.uidJato, dato.datosEquipamiento.airbag, dato.datosEquipamiento.aireAcon, dato.datosEquipamiento.alarma, dato.datosEquipamiento.alzaVidrios, dato.datosEquipamiento.nuevo, dato.datosEquipamiento.transmision, dato.datosEquipamiento.radio, dato.datosEquipamiento.espejos, dato.datosEquipamiento.frenosAbs, dato.datosEquipamiento.unicoDueno, dato.datosEquipamiento.cierreCentral, dato.datosEquipamiento.catalitico, dato.datosEquipamiento.fwd, dato.datosEquipamiento.llantas, dato.datosEquipamiento.fotos, dato.datosEquipamiento.plataforma);
            //objeto de prueba
            SP_PublicarAviso_Automotoras_Result prueba = new SP_PublicarAviso_Automotoras_Result();
            prueba.codauto = 12345600;
            prueba.error = 1;
            var logrado = prueba; 
            return logrado;
        }

        private static string GetDescMarca(int namemarca)
        {

            string descmarca = "";
            myConnection myConn = new myConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myConnection.GetConnection();
                    connection.CommandText = "select DES_MARCA from tabmarcas where cod_marca=" + namemarca;

                    using (var reader = connection.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            if (reader.Read())
                            {

                                descmarca = reader["DES_MARCA"].ToString();

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

            return descmarca;
        }

        private static Boolean ConsultaEstadoActualizacion(int codditec)
        {

            Boolean existe = false;
            myLocalConnection myLocalConn = new myLocalConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myLocalConnection.GetLocalConnection();
                    connection.CommandText = "select [cod_auto] from [tabautosDITEC] where codigo_auto_DITEC = " + codditec + " and cod_auto is not null";

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

        private static Boolean updateregistro(int codditec, int cod_auto) {

            Boolean actualizo = false;
            myLocalConnection myLocalConn = new myLocalConnection();

            try
            {
                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myLocalConnection.GetLocalConnection();
                    connection.CommandText = "UPDATE[dbo].[tabautosDITEC] SET[cod_auto] = " + cod_auto + " WHERE codigo_auto_DITEC = " + codditec + "";

                    connection.ExecuteNonQuery();

                    connection.Connection.Close();
                    connection.Connection.Dispose();
                    System.Data.SqlClient.SqlConnection.ClearAllPools();
                    actualizo = true;
                }

                

            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex.Message);
                actualizo = false;
            }

            return actualizo;

        }

    }
}
