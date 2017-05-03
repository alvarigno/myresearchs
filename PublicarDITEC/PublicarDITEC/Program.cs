using System;
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

                PublicacionChileautos datopublicacion = new PublicacionChileautos();

                int codigo = getCodigoJajoNoJato(GetDescMarca(datos[i].Marca), datos[i].Modelo, datos[i].Version, datos[i].Carroceria, int.Parse(datos[i].Puertas), datos[i].Ano, datos[i].tipo_cambio, datos[i].combustible, "", datos[i].categoria);

                //Datos del vehículo

                datopublicacion.codCliente = 1028;
                datopublicacion.ip = "localhost";
                datopublicacion.datosVehiculo.patente = datos[i].patente;
                datopublicacion.datosVehiculo.tipo = datos[i].Tipo_vehiculo;
                datopublicacion.datosVehiculo.marca = datos[i].Marca;
                datopublicacion.datosVehiculo.modelo = datos[i].Modelo;
                datopublicacion.datosVehiculo.ano = datos[i].Ano;
                datopublicacion.datosVehiculo.version = datos[i].Version;
                datopublicacion.datosVehiculo.carroceria = datos[i].Carroceria;
                datopublicacion.datosVehiculo.puertas = int.Parse(datos[i].Puertas);
                datopublicacion.datosVehiculo.tipoDireccion = datos[i].tipo_direccion;
                datopublicacion.datosVehiculo.precio = datos[i].Precio;
                datopublicacion.datosVehiculo.cilindrada = datos[i].Cilindrada;
                datopublicacion.datosVehiculo.potencia = "";
                datopublicacion.datosVehiculo.color = datos[i].Color;
                datopublicacion.datosVehiculo.kilom = datos[i].KM;
                datopublicacion.datosVehiculo.motor = datos[i].motor;
                datopublicacion.datosVehiculo.techo = datos[i].Techo;
                datopublicacion.datosVehiculo.combustible = datos[i].combustible;
                datopublicacion.datosVehiculo.comentario = datos[i].comentarios;
                datopublicacion.datosVehiculo.uidJato = codigo;

                //datos equipamiento
                datopublicacion.datosEquipamiento.airbag = datos[i].airbag;
                datopublicacion.datosEquipamiento.aireAcon = datos[i].aire_acondicionado;
                datopublicacion.datosEquipamiento.alarma = datos[i].Alarma;
                datopublicacion.datosEquipamiento.alzaVidrios = datos[i].alzavidrios_electricos;
                datopublicacion.datosEquipamiento.nuevo = datos[i].Nuevo_o_usado;
                datopublicacion.datosEquipamiento.transmision = datos[i].tipo_cambio;
                datopublicacion.datosEquipamiento.radio = datos[i].radio;
                datopublicacion.datosEquipamiento.espejos = datos[i].espejos_electricos;
                datopublicacion.datosEquipamiento.frenosAbs = datos[i].frenos_ABS;
                datopublicacion.datosEquipamiento.unicoDueno = datos[i].unico_dueno;
                datopublicacion.datosEquipamiento.cierreCentral = datos[i].cierre_centralizado;
                datopublicacion.datosEquipamiento.catalitico = datos[i].catalitico;
                datopublicacion.datosEquipamiento.fwd = datos[i].fwd;
                datopublicacion.datosEquipamiento.llantas = datos[i].Llantas;
                datopublicacion.datosEquipamiento.fotos = datos[i].fotos;
                datopublicacion.datosEquipamiento.plataforma = "DTC";



                Console.WriteLine("Codigo jato: " + codigo);


            } catch(Exception e) { Console.WriteLine("Error: " + e.Message); }

        }


        public static int getCodigoJajoNoJato(string marca, string modelo,  string version,  string carroceria,  int puertas, int ano,  string transmision, int combustible, string edicion, int categoria) {

            int codigo = 0;

            bdToolsEntities bdTools = new bdToolsEntities();

            var uidJato = bdTools.bdj_idJato_SEL_marca_modelo_version_carroceria_ptas_ano_trans_ltl(marca, modelo, version, carroceria, puertas, ano, transmision, edicion);

            codigo = uidJato;

            if (uidJato == null) { 
                uidJato = bdTools.SP_bdj_getNonJatoID(categoria, marca, modelo, ano, carroceria, transmision, combustible.ToString());
                codigo = uidJato;
            }

            return codigo;
        }

        public static int codigojato(string marca, string modelo, string version, string carroceria, int puertas, int ano, string transmision, string edicion) {
            
            int codjato = 0;

            if (edicion == "") {

                edicion = "-";
            }

            if (transmision == "S")
            {

                transmision = "A";

            }
            else {

                transmision = "M";

            }

            try {

                myConnection myConn = new myConnection();

                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myConnection.GetConnection();
                    connection.CommandText = "with c as (";
                    connection.CommandText = connection.CommandText + "select id_108, id_111, id_112, id_302, id_404, id_602, id_603, id_20602, vehicle_id, Ch_make, Ch_model from bdJato_NSCRCH_CS2002.dbo.version";
                    connection.CommandText = connection.CommandText + "union";
                    connection.CommandText = connection.CommandText + "select id_108, id_111, id_112, id_302, id_404, id_602, id_603, id_20602, vehicle_id, Ch_make, Ch_model from bdJato_SSCRCH_CS2002.dbo.version";
                    connection.CommandText = connection.CommandText + "union";
                    connection.CommandText = connection.CommandText + "select id_108, id_111, id_112, id_302, id_404, id_602, id_603, id_20602, vehicle_id, Ch_make, Ch_model from bdJatoH_NSCRCH_CS2002.dbo.version";
                    connection.CommandText = connection.CommandText + "union";
                    connection.CommandText = connection.CommandText + "select id_108, id_111, id_112, id_302, id_404, id_602, id_603, id_20602, vehicle_id, Ch_make, Ch_model from bdJatoH_SSCRCH_CS2002.dbo.version)";
                    connection.CommandText = connection.CommandText + "select min(vehicle_id) as idjato from c WHERE Ch_make = '"+marca+"' AND Ch_model = '"+modelo+"' AND id_302 = '"+version+"' AND id_603 = '"+carroceria+"' AND id_602 = "+puertas+" AND id_108 = "+ano+" AND id_20602 = '"+transmision+"' AND id_404 = '"+edicion+"'";

                    using (var reader = connection.ExecuteReader())
                    {
                        while (reader.HasRows)
                        {
                            if (reader.Read()) {

                                codjato = int.Parse(reader["c"].ToString());

                            }
                        }
                    }
                }


            } catch (Exception e) {

                Console.WriteLine("Error: "+e.Message);

            }
            
            return codjato;

        }

        public static int codigoNonjato(string marca, string modelo, string carroceria, int ano, string transmision, int combustible, int categoria )
        {

            int codNojato = 0;

            if (transmision == "S")
            {

                transmision = "A";

            }
            else
            {

                transmision = "M";

            }

            try
            {
                myConnection myConn = new myConnection();

                using (var connection = new System.Data.SqlClient.SqlCommand())
                {
                    connection.Connection = myConnection.GetConnection();
                    connection.CommandText = "SELECT nj_id FROM [bdTools].[dbo].[tbl_NONJato] WHERE marca = '" + marca+"' and modelo = '"+modelo+"' and ano = "+ano+" and(trans = '"+transmision+"' OR '"+transmision+"' IS NULL) and(carr = '"+carroceria+"' OR '"+carroceria+"' IS NULL) and comb = ISNULL("+combustible+", 10)  and idCat = " +categoria;

                    using (var reader = connection.ExecuteReader())
                    {
                        while (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                codNojato = int.Parse(reader["c"].ToString());
                            }
                        }
                    }

                }


            } catch (Exception e) {

                Console.WriteLine("Error: "+e.Message);

            }



            return codNojato;

        }


        public static int GetIdeAutomotora() {

            int idclient = 0;

         //   select* from Tabclientes where nombre_fantasia like '%ditec%'


            return idclient;
        }

        public static Boolean InserTtblJatoTabautos() {

            Boolean proceso = false;


                //        INSERT INTO tbl_jato_tabautos(cod_auto, uidJato, uidnonJato, ope) VALUES(@codAuto, @uidJato, 0, @plataforma)
                //
                //    
                //        INSERT INTO tbl_jato_tabautos(cod_auto, uidJato, uidnonJato, ope) VALUES(@codAuto, 0, @uidJato, @plataforma)
                //
            return proceso;

        }


        public static Boolean PublicaAviso()
        {

            Boolean publico = false;

//                    INSERT INTO tabautos(
//                fecha_ingreso,
//                cod_cliente, nuevo, tipoveh, carroceria, cod_marca, modelo, version,
//                ano, pesos, potencia, color, km, milla, motor, cilindrada,
//                foto_chica, foto_grande, tipo_cambio, aire_acondicionado, tipo_direccion,
//                radio, alzavidrios_electricos, espejos_electricos, frenos_ABS, airbag, unico_dueno,
//                cierre_centralizado, catalitico, fwd, llantas, puertas, combustible, alarma, techo,
//                otros, patente, destacado) VALUES(
//               GETDATE(),
//               @codCliente, @nuevo, @tipo, @carroceria, @marca, @modelo, @version,
//               @ano, @precio, @potencia, @color, @kilom, @milla, @motor, @cilindrada,
//               @foto, @foto, @transmision, @aireAcon, @tipoDireccion,
//               @radio, @alzaVidrios, @espejos, @frenosAbs, @airbag, @unicoDueno,
//               @cierreCentral, @catalitico, @fwd, @llantas, @puertas, @combustible, @alarma, @techo,
//               @comentario, @patente, 'N'
//                           );
//            SELECT @codAuto = SCOPE_IDENTITY();


            return publico;
        }


        public static Boolean InsertaFotos() {

            Boolean insertofotos = false;

          //  DECLARE @tmp TABLE(orden INT IDENTITY, foto VARCHAR(100));
          //  INSERT INTO @tmp SELECT* FROM dbo.fn_Split(@listaFotos, ',');
          //  INSERT INTO tbl_fotosNuevoServer SELECT @codAuto, foto, orden FROM @tmp;


            return insertofotos;

        }

        public static Boolean accionescv() {

            Boolean inserto = false;

//            SELECT @accioncvId = (SELECT MAX(accioncv) + 1 FROM accionescv)
//				SELECT @actcv = ISNULL((SELECT actop FROM operaciones WHERE clieop = @codCliente), 0)
//				SET @tipocv = 1
//
//                INSERT INTO accionescv(
//                    accioncv, codautocv, codclientecv, fechacv,
//                    tipocv, actcv, marcacv, modelocv,
//                    anocv, preciocv, ipcv, patentecv, versioncv
//                ) VALUES(
//                    @accioncvId, @codAuto, @codCliente, GETDATE(),
//                    @tipocv, @actcv, @marca, @modelo,
//                    @ano, @precio, @ip, @patente, @version
//                )

            return inserto;

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

    }
}
