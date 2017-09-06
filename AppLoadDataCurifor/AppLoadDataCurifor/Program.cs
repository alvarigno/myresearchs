using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AppLoadDataCurifor.Data;

namespace AppLoadDataCurifor
{
    class Program
    {

        public static Dictionary<string, string> DicCarroceria = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCombustibles = new Dictionary<string, string>();
        public static Dictionary<string, string> DicTipoCambio = new Dictionary<string, string>();
        public static Dictionary<string, string> DicNuevousado = new Dictionary<string, string>();
        public static Dictionary<string, string> DicTecho = new Dictionary<string, string>();
        public static Dictionary<string, string> DicComodin = new Dictionary<string, string>();
        public static Dictionary<string, string> DicCategoriasCurifor = new Dictionary<string, string>();
        public static Dictionary<string, string> DicSucursalesCurifor = new Dictionary<string, string>();
        public static Dictionary<string, string> DicColoresCurifor = new Dictionary<string, string>();
        public static Dictionary<string, string> DicOpcionalesCurifor = new Dictionary<string, string>();
        public static int[] idCurifor = new int[15];


        static void Main(string[] args)
        {


            idCurifor[0] = 2444;
            idCurifor[1] = 610;
            idCurifor[2] = 897;
            idCurifor[3] = 1454;
            idCurifor[4] = 895;
            idCurifor[5] = 2393;
            idCurifor[6] = 2353;
            idCurifor[7] = 893;
            idCurifor[8] = 894;
            idCurifor[9] = 896;
            idCurifor[10] = 929;
            idCurifor[11] = 1556;
            idCurifor[12] = 582;
            idCurifor[13] = 2090;
            idCurifor[14] = 1608;


            baseprod2Entities BD = new baseprod2Entities();
            Console.WriteLine("Comienza la carga");

            try
            {
                
                for (int i = 0; i< idCurifor.Length;i++) {

                    int valor = idCurifor[i];
                    Console.WriteLine((i+1)+" - Procesando Cod_cliente: "+valor);
                    CargaDiccionarios();
                    XElement Registros = new XElement("stock",

                            (from tbl in BD.tabautos
                             where ((tbl.COD_CLIENTE == valor) && (tbl.nuevo == "N"))
                             select new
                             {

                                 tbl.COD_AUTO,
                                 tbl.nuevo,
                                 tbl.COD_MARCA,
                                 tbl.MODELO,
                                 tbl.Version,
                                 tbl.ANO,
                                 tbl.patente,
                                 tbl.Tipoveh,
                                 tbl.Carroceria,
                                 tbl.combustible,
                                 tbl.tipo_cambio,
                                 tbl.Puertas,
                                 tbl.km,
                                 tbl.color,
                                 tbl.PESOS,
                                 tbl.otros,
                                 tbl.unico_dueno,
                                 tbl.aire_acondicionado,
                                 tbl.tipo_direccion,
                                 tbl.radio,
                                 tbl.alzavidrios_electricos,
                                 tbl.espejos_electricos,
                                 tbl.frenos_ABS,
                                 tbl.airbag,
                                 tbl.cierre_centralizado,
                                 tbl.catalitico,
                                 tbl.fwd,
                                 tbl.Llantas,
                                 tbl.Alarma,
                                 tbl.Techo

                             }).ToList().Select(
                                x => new XElement("item",
                                new XElement("code", x.COD_AUTO),
                                //new XElement("new", new XAttribute("Descripcion", "nuevo/usado"), DicNuevousado[x.nuevo]),
                                new XElement("title",

                                    (from mar in BD.tabmarcas where mar.COD_MARCA == x.COD_MARCA select new { mar.DES_MARCA }).ToList().Select(m => x.ANO + " " + (m.DES_MARCA).Trim() + " " + x.MODELO + " " + x.Version)

                                    ),
                                new XElement("brand", x.COD_MARCA),
                                new XElement("model", x.MODELO),
                                new XElement("version", x.Version),
                                new XElement("year", x.ANO),
                                new XElement("year_model", x.ANO),
                                new XElement("plate", x.patente ?? string.Empty),
                                new XElement("category", (
                                    from cat in BD.tabCategoria_Tipo where cat.idTipoVeh == x.Tipoveh select new { cat.idCategoria }).ToList().Select(y => DicCategoriasCurifor[y.idCategoria.ToString()]
                                    )
                                    ),
                                new XElement("segment", DicCarroceria[x.Carroceria ?? string.Empty]),
                                new XElement("fuel", x.combustible),
                                new XElement("transmission", DicTipoCambio[x.tipo_cambio ?? string.Empty]),
                                new XElement("doors", x.Puertas),
                                new XElement("mileage", x.km),
                                new XElement("color", CompruebaColores(x.color ?? string.Empty, DicColoresCurifor)),
                                new XElement("value", x.PESOS),
                                new XElement("description", x.otros),
                                new XElement("optionals",
                                
                                    new XElement(GetOpcion("aire_acondicionado", DicComodin[x.aire_acondicionado ?? string.Empty])),
                                    new XElement(GetOpcion("unico_dueño", DicComodin[x.unico_dueno ?? string.Empty])),
                                    new XElement(GetOpcion("tipo_direccion", x.tipo_direccion ?? string.Empty)),
                                    new XElement(GetOpcion("radio", DicComodin[x.radio ?? string.Empty])),
                                    new XElement(GetOpcion("alzavidrios_electricos", DicComodin[x.alzavidrios_electricos ?? string.Empty])),
                                    new XElement(GetOpcion("espejos_electricos", DicComodin[x.espejos_electricos ?? string.Empty])),
                                    new XElement(GetOpcion("frenos_ABS", DicComodin[x.frenos_ABS ?? string.Empty])),
                                    new XElement(GetOpcion("airbag", DicComodin[x.airbag ?? string.Empty])),
                                    new XElement(GetOpcion("cierre_centralizado", DicComodin[x.cierre_centralizado ?? string.Empty])),
                                    new XElement(GetOpcion("catalitico", DicComodin[x.catalitico ?? string.Empty])),
                                    new XElement(GetOpcion("fwd", DicComodin[x.fwd ?? string.Empty])),
                                    new XElement(GetOpcion("Llantas", DicComodin[x.Llantas ?? string.Empty])),
                                    new XElement(GetOpcion("Alarma", DicComodin[x.Alarma ?? string.Empty])),
                                    new XElement(GetOpcion("Techo", DicTecho[x.Techo ?? string.Empty]))

                                ),
                                new XElement("images", (

                                    from img in BD.tbl_fotosNuevoServer where img.cod_auto == x.COD_AUTO select new { img.foto, img.orden }).ToList().Select(j => new XElement("image", j.foto)

                                    )
                                )


                            )
                        )

                    );
                    
                    XmlReader reader2 = Registros.CreateReader();
                    reader2.MoveToContent();
                    XmlDocument doc2 = new XmlDocument();
                    XmlNode cd2 = doc2.ReadNode(reader2);
                    doc2.AppendChild(cd2);
                    doc2.Save("doc_"+idCurifor[i]+"_"+ DicSucursalesCurifor[idCurifor[i].ToString()] +".xml");

                    LimpiaDiccionario();


                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("error: "+ex);

            }

            Console.WriteLine("Finaló la carga");
            Console.ReadLine();
        }

        public static void CargaDiccionarios() {


            //Diccionario de Carrocerías
            DicCarroceria.Add("OC", "COMERCIAL");//4x4 Comercial
            DicCarroceria.Add("BA", "A motor");
            DicCarroceria.Add("CV", "COMERCIAL");//Auto comercial
            DicCarroceria.Add("BX", "BMX");
            DicCarroceria.Add("CS", "CABINE_SIMPLES");//Cabina Simple
            DicCarroceria.Add("CM", "CABINE_MEIA");//Cabina y media
            DicCarroceria.Add("CL", "Camión Plano");
            DicCarroceria.Add("CU", "Camión Pluma");
            DicCarroceria.Add("CT", "Camión Tolva");
            DicCarroceria.Add("BH", "Chasis Cabina");
            DicCarroceria.Add("CH", "Chopper");
            DicCarroceria.Add("MC", "Citycar");
            DicCarroceria.Add("CC", "Combi");
            DicCarroceria.Add("CA", "CABRIOLET");//Convertible
            DicCarroceria.Add("CO", "Coupé");
            DicCarroceria.Add("CP", "Coupé");
            DicCarroceria.Add("CD", "Cuadrimoto");
            DicCarroceria.Add("MU", "Custom");
            DicCarroceria.Add("MD", "Deportivas");
            DicCarroceria.Add("DC", "CABINE_DUPLA");//Doble Cabina
            DicCarroceria.Add("DH", "Down hill");
            DicCarroceria.Add("BE", "Enduro");
            DicCarroceria.Add("ME", "Enduro - cross");
            DicCarroceria.Add("BS", "Estática");
            DicCarroceria.Add("BF", "Freestyle");
            DicCarroceria.Add("PV", "FURGAO");//Furgón
            DicCarroceria.Add("WV", "Furgón de pasajeros");
            DicCarroceria.Add("RV", "Furgonado");
            DicCarroceria.Add("HA", "HATCHA");//Hatchback
            DicCarroceria.Add("BI", "Infantiles");
            DicCarroceria.Add("MB", "Media Barandas");
            DicCarroceria.Add("MM", "Mini MPV");
            DicCarroceria.Add("FW", "VAN");//Minivan
            DicCarroceria.Add("MN", "Moto de nieve");
            DicCarroceria.Add("BM", "Mountainbike");
            DicCarroceria.Add("PU", "PICAPE");//Pick-up
            DicCarroceria.Add("BP", "Plegables");
            DicCarroceria.Add("PC", "Portacontenedor");
            DicCarroceria.Add("RA", "Racing");
            DicCarroceria.Add("R", "Rampla");
            DicCarroceria.Add("MR", "Retro");
            DicCarroceria.Add("BR", "Rutera");
            DicCarroceria.Add("SC", "Scooter");
            DicCarroceria.Add("SA", "SEDAN");//Sedán
            DicCarroceria.Add("NN", "Sin carroceria");
            DicCarroceria.Add("MS", "Sport calle - urbanas");
            DicCarroceria.Add("ES", "PERUA");//Station Wagon
            DicCarroceria.Add("OD", "SUV");
            DicCarroceria.Add("TN", "Tandem");
            DicCarroceria.Add("TA", "Targa");
            DicCarroceria.Add("MO", "4x4");//Todo terrenos
            DicCarroceria.Add("MT", "Trabajo - calle");
            DicCarroceria.Add("TC", "Tracto Camión");
            DicCarroceria.Add("BT", "Trail");
            DicCarroceria.Add("BK", "Trekking");
            DicCarroceria.Add("TL", "Triciclo");
            DicCarroceria.Add("BU", "Van");
            DicCarroceria.Add("", "NN");


            //Diccionarios Combustibles
            DicCombustibles.Add("1", "2");//Bencina
            DicCombustibles.Add("2", "6");//Diesel (petróleo)
            DicCombustibles.Add("5", "8");//Eléctrico
            DicCombustibles.Add("3","Gas");
            DicCombustibles.Add("4", "8");//Híbrido
            DicCombustibles.Add("10","Otros");

            //Diccionario TIpo Cambio
            DicTipoCambio.Add("S","2");
            DicTipoCambio.Add("", "1");
            DicTipoCambio.Add("N", "1");

            //Diccionario Nuevo o usado
            DicNuevousado.Add("S", "1");
            DicNuevousado.Add("N", "3");

            //Diccionario de Techos
            DicTecho.Add("ST","Ninguno");
            DicTecho.Add("", "Ninguno");
            DicTecho.Add("TE","Eléctrico");
            DicTecho.Add("TM","Manual");
            DicTecho.Add("TP","Panorámico");

            //Diccionario Comodín
            DicComodin.Add("","N");
            DicComodin.Add("S", "S");
            DicComodin.Add("N", "N");

            //Diccionario Categorias Curifor
            DicCategoriasCurifor.Add("1","1");//Autos, camionetas y 4x4
            DicCategoriasCurifor.Add("2","2");//motos
            DicCategoriasCurifor.Add("4","3");//Camiones
            DicCategoriasCurifor.Add("5","5");//Buses
            DicCategoriasCurifor.Add("6","6");//Maquinarias
            DicCategoriasCurifor.Add("8","9");//Acuaticos
            DicCategoriasCurifor.Add("9","0");//Aereos
            DicCategoriasCurifor.Add("11","8");//MotorHomes
            DicCategoriasCurifor.Add("10","7");//Otros

            //Diccionario Sucursales
            DicSucursalesCurifor.Add("1454","COQUIMBO");
            DicSucursalesCurifor.Add("2090","LA FLORIDA");
            DicSucursalesCurifor.Add("610", "LA FLORIDA");
            DicSucursalesCurifor.Add("1556", "LA FLORIDA");
            DicSucursalesCurifor.Add("582", "LA FLORIDA");
            DicSucursalesCurifor.Add("2393","LINDEROS");
            DicSucursalesCurifor.Add("2353","PLACILLA");
            DicSucursalesCurifor.Add("893","RANCAGUA");
            DicSucursalesCurifor.Add("894","SAN FERNANDO");
            DicSucursalesCurifor.Add("895","CURICO");
            DicSucursalesCurifor.Add("896","TALCA");
            DicSucursalesCurifor.Add("1608","TALCA");
            DicSucursalesCurifor.Add("897", "CHILLAN");
            DicSucursalesCurifor.Add("2444", "MACUL");
            DicSucursalesCurifor.Add("929", "SANTIAGO");

            //Diccionario Colores Curifor
            DicColoresCurifor.Add("Amarillo", "1");
            DicColoresCurifor.Add("AMARILLO", "1");
            DicColoresCurifor.Add("Beige", "4");
            DicColoresCurifor.Add("BEIGE", "4");
            DicColoresCurifor.Add("Blanco", "5");
            DicColoresCurifor.Add("BLANCO", "5");
            DicColoresCurifor.Add("Plata", "7");
            DicColoresCurifor.Add("PLATA", "7");
            DicColoresCurifor.Add("Negro", "8");
            DicColoresCurifor.Add("NEGRO", "8");
            DicColoresCurifor.Add("Verde", "9");
            DicColoresCurifor.Add("VERDE", "9");
            DicColoresCurifor.Add("Rojo", "10");
            DicColoresCurifor.Add("ROJO", "10");
            DicColoresCurifor.Add("Azul", "11");
            DicColoresCurifor.Add("AZUL", "11");
            DicColoresCurifor.Add("Gris", "12");
            DicColoresCurifor.Add("GRIS", "12");
            DicColoresCurifor.Add("Dorado", "13");
            DicColoresCurifor.Add("DORADO", "13");
            DicColoresCurifor.Add("Marrón", "14");
            DicColoresCurifor.Add("MARRON", "14");
            DicColoresCurifor.Add("Vino", "15");
            DicColoresCurifor.Add("VINO", "15");
            DicColoresCurifor.Add("Púrpura", "16");
            DicColoresCurifor.Add("PURPURA", "16");
            DicColoresCurifor.Add("Naranja", "17");
            DicColoresCurifor.Add("NARANJA", "17");
            DicColoresCurifor.Add("Rosa", "25");
            DicColoresCurifor.Add("ROSA", "25");
            DicColoresCurifor.Add("Bronce", "26");
            DicColoresCurifor.Add("BRONCE", "26");
            DicColoresCurifor.Add("","24");


            //Diccionario de Opcionales
            DicOpcionalesCurifor.Add("aire_acondicionado","2");
            DicOpcionalesCurifor.Add("tipo_direccion","3");
            DicOpcionalesCurifor.Add("radio","37");
            DicOpcionalesCurifor.Add("alzavidrios_electricos","29");
            DicOpcionalesCurifor.Add("espejos_electricos","6");
            DicOpcionalesCurifor.Add("frenos_ABS","8");
            DicOpcionalesCurifor.Add("airbag","28");
            DicOpcionalesCurifor.Add("cierre_centralizado","");
            DicOpcionalesCurifor.Add("catalitico","");
            DicOpcionalesCurifor.Add("fwd","16");
            DicOpcionalesCurifor.Add("Llantas","15");
            DicOpcionalesCurifor.Add("Alarma","7");
            DicOpcionalesCurifor.Add("Techo","20");



        }

        public static void LimpiaDiccionario() {

            DicCarroceria.Clear();
            DicCombustibles.Clear();
            DicTipoCambio.Clear();
            DicNuevousado.Clear();
            DicTecho.Clear();
            DicComodin.Clear();
            DicCategoriasCurifor.Clear();
            DicSucursalesCurifor.Clear();
            DicColoresCurifor.Clear();
            DicOpcionalesCurifor.Clear();


        }

        public static string CompruebaColores(string dato, Dictionary<string,string> Dicc) {

            string valor = "";

            if (Dicc.ContainsKey(dato))
            {

                valor = Dicc[dato];

            }
            else {

                valor = "23";

            }



            return valor;
        }


        public static XElement GetOpcion(string etiqueta, string valor) {
            
           XElement data = null;
            data = new XElement("optional", "0");

            if (etiqueta == "unico_dueño" && valor == "S") {

                data = new XElement("optional", "1003");
            }

            if (etiqueta == "aire_acondicionado" && valor == "S")
            {

                data = new XElement("optional", "2");

            }

            if (etiqueta == "radio" && valor == "S")
            {

                data = new XElement("optional", "37");

            }

            if (etiqueta == "alzavidrios_electricos" && valor == "S")
            {

                data = new XElement("optional", "29");

            }

            if (etiqueta == "espejos_electricos" && valor == "S")
            {

                data = new XElement("optional", "6");

            }

            if (etiqueta == "frenos_ABS" && valor == "S")
            {

                data = new XElement("optional", "8");

            }

            if (etiqueta == "airbag" && valor == "S")
            {

                data = new XElement("optional", "28");

            }

            if (etiqueta == "cierre_centralizado" && valor == "S")
            {

                data = new XElement("optional", "0");

            }

            if (etiqueta == "catalitico" && valor == "S")
            {

                data = new XElement("optional", "0");

            }

            if (etiqueta == "fwd" && valor == "S")
            {

                data = new XElement("optional", "16");

            }

            if (etiqueta == "Llantas" && valor == "S")
            {

                data = new XElement("optional", "15");

            }

            if (etiqueta == "Alarma" && valor == "S")
            {

                data = new XElement("optional", "7");

            }

            if (etiqueta == "Techo" && valor != "")
            {
                data = new XElement("optional", "20");

                if (valor == "Ninguno") {

                    data = new XElement("optional", "0");

                }

                if(valor == "ST")
                {

                    data = new XElement("optional", "0");

                }


            }



            return data;
        }


        public static XElement GetOptional(XObject x) {


                XElement data = new XElement("optional",x);


                    //data = data + new XElement("aire_acondicionado", DicComodin[x.aire_acondicionado ?? string.Empty]);
                    //data = data + new XElement("tipo_direccion", x.tipo_direccion ?? string.Empty);
                    //data = data + new XElement("radio", DicComodin[x.radio ?? string.Empty]);
                    //data = data + new XElement("alzavidrios_electricos", DicComodin[x.alzavidrios_electricos ?? string.Empty]);
                    //data = data + new XElement("espejos_electricos", DicComodin[x.espejos_electricos ?? string.Empty]);
                    //data = data + new XElement("frenos_ABS", DicComodin[x.frenos_ABS ?? string.Empty]);
                    //data = data + new XElement("airbag", DicComodin[x.airbag ?? string.Empty]);
                    //data = data + new XElement("cierre_centralizado", DicComodin[x.cierre_centralizado ?? string.Empty]);
                    //data = data + new XElement("catalitico", DicComodin[x.catalitico ?? string.Empty]);
                    //data = data + new XElement("fwd", DicComodin[x.fwd ?? string.Empty]);
                    //data = data + new XElement("Llantas", DicComodin[x.Llantas ?? string.Empty]);
                    //data = data + new XElement("Alarma", DicComodin[x.Alarma ?? string.Empty]);
                    //data = data + new XElement("Techo", DicTecho[x.Techo ?? string.Empty]);


            return data;


        }


    }
}
