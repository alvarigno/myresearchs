using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AppMakeXMLToIntegrador.Data;
using System.Globalization;
using System.IO;

namespace AppMakeXMLToIntegrador
{
    class Program
    {

        public static Dictionary<string, string> DicTecho = new Dictionary<string, string>();
        public static Dictionary<string, string> DicTipoDireccion = new Dictionary<string, string>();
        public static Dictionary<string, string> DicEquipamiento = new Dictionary<string, string>();

        public static String cultureNames = "es-ES";
        public static int id = 1093;

        static void Main(string[] args)
        {

            baseprod2Entities BD = new baseprod2Entities();
            Console.WriteLine("Comienza la carga");
            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo(cultureNames);
            string fecha = localDate.ToString(culture).ToString();


            Console.WriteLine("ingrese Cód. Cliente:");
            string codigo = Console.ReadLine();
            Console.WriteLine("Código de aviso ingresado: " + codigo);

            id = int.Parse(codigo);

            try
            {

                CargaDiccionarios();
                Console.WriteLine(" - Procesando Cod_cliente: " + id);
                XElement Registros = new XElement("publicacion",
                    

                    new XElement("identificacion",

                        new XAttribute("name", "nombre sucursal"),
                        new XAttribute("fecha-creacion", fecha),
                        new XElement("x-key", "B451AA1ACC6275A9DED6C799722B50CB")


                    ),
                    new XElement("listado",

                        new XElement("publicaciones", "10"),

                        (from tbl in BD.tabautos
                            where tbl.COD_CLIENTE == id
                            select new
                            {

                                tbl.COD_AUTO,
                                tbl.COD_CLIENTE,
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
                                tbl.motor,
                                tbl.Potencia,
                                tbl.color,
                                tbl.PESOS,
                                tbl.Cilindrada,
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
                                tbl.Techo,
                                tbl.consignacion

                            }).ToList().Select(
                                                            
                                x => new XElement("aviso", 
                                    new XAttribute("id", x.COD_AUTO),
                                    new XAttribute("patente", x.patente ?? string.Empty),
                                    new XAttribute("rdiccionario", "S"),
                                        
                                        new XElement("vehiculo",
                                            new XElement("sucursal",
                                                new XAttribute("id", x.COD_CLIENTE),
                                                (from cliente in BD.tabclientes where cliente.COD_CLIENTE == x.COD_CLIENTE select new { cliente.nombre_fantasia }).ToList().Select(y => y.nombre_fantasia.Trim().ToString())
                                            ),
                                            new XElement("categoria", 
                                                new XAttribute("id", (from cat in BD.tabCategoria_Tipo where cat.idTipoVeh == x.Tipoveh select new {  cat.idCategoria }).ToList().Select(catid => catid.idCategoria).FirstOrDefault()),
                                                (from cat in BD.tabCategoria_Tipo where cat.idTipoVeh == x.Tipoveh select new { cat.idCategoria }).ToList().Select(ca => (from descat in BD.tabCategorias where descat.idCategoria == ca.idCategoria select new { descat.Categoria }).ToList().Select(d => d.Categoria.ToString()))
                                            ),
                                            new XElement("tipo",
                                                new XAttribute("id", x.Tipoveh),
                                                (from tipovehi in BD.tipos where tipovehi.tveh == x.Tipoveh select new { tipovehi.nveh}).ToList().Select(t => t.nveh.Trim())
                                            ),
                                            new XElement("marca",
                                                new XAttribute("id", x.COD_MARCA),
                                                (from marcas in BD.tabmarcas where marcas.COD_MARCA == x.COD_MARCA select new { marcas.DES_MARCA}).ToList().Select(marca => marca.DES_MARCA.Trim())
                                            ),
                                            new XElement("modelo", x.MODELO),
                                            new XElement("version", x.Version),
                                            new XElement("carroceria",
                                                new XAttribute("id", x.Carroceria ?? string.Empty),
                                                (from carrocerias in BD.tabcarroceria where carrocerias.inicarr == x.Carroceria select new { carrocerias.carroceria}).ToList().Select(carroceria => carroceria.carroceria.Trim())
                                            ),
                                            new XElement("ano", x.ANO),
                                            new XElement("precio", x.PESOS),
                                            new XElement("color", x.color ?? string.Empty),
                                            new XElement("km", x.km),
                                            new XElement("motor", x.motor ?? string.Empty),
                                            new XElement("potencia", x.Potencia.Trim()),
                                            new XElement("combustible",
                                                new XAttribute("id", x.combustible),
                                                (from fuels in BD.tbl_combustible where fuels.idcomb == x.combustible select new { fuels.ncombustible }).ToList().Select(fuel => fuel.ncombustible.Trim())
                                            ),
                                            new XElement("cilindrada", x.Cilindrada),
                                            new XElement("tipodireccion",
                                                new XAttribute("id", x.tipo_direccion),
                                                DicTipoDireccion[x.tipo_direccion ?? string.Empty]
                                            ),
                                            new XElement("techo",
                                                new XAttribute("id", ObitneValorTecho(x.Techo ?? string.Empty)),
                                                DicTecho[x.Techo ?? string.Empty]
                                            ),
                                            new XElement("puertas", x.Puertas),
                                            new XElement("comentarios", x.otros.Trim() ?? string.Empty.Trim())
                                        ),
                                        new XElement("equipamiento",
                                            new XElement("nuevo", DicEquipamiento[x.nuevo ?? string.Empty]),
                                            new XElement("transmision", DicEquipamiento[x.tipo_cambio ?? string.Empty]),
                                            new XElement("aireacondicionado", DicEquipamiento[x.aire_acondicionado ?? string.Empty]),
                                            new XElement("radio", DicEquipamiento[x.radio ?? string.Empty]),
                                            new XElement("alzavidrios", DicEquipamiento[x.alzavidrios_electricos ?? string.Empty]),
                                            new XElement("espejoselectricos", DicEquipamiento[x.espejos_electricos ?? string.Empty]),
                                            new XElement("frenosabs", DicEquipamiento[x.frenos_ABS ?? string.Empty]),
                                            new XElement("airbag", DicEquipamiento[x.airbag ?? string.Empty]),
                                            new XElement("unicodueno", DicEquipamiento[x.unico_dueno ?? string.Empty]),
                                            new XElement("cierrecentralizado", DicEquipamiento[x.cierre_centralizado ?? string.Empty]),
                                            new XElement("catalitico", DicEquipamiento[x.catalitico ?? string.Empty]),
                                            new XElement("fwd", DicEquipamiento[x.fwd ?? string.Empty]),
                                            new XElement("llantas", DicEquipamiento[x.Llantas ?? string.Empty]),
                                            new XElement("alarma", DicEquipamiento[x.Alarma ?? string.Empty]),
                                            new XElement("consignacion", DicEquipamiento[x.consignacion ?? string.Empty])
                                        ),
                                        new XElement("fotos",
                                            new XAttribute("cantidad", (from countfotos in BD.tbl_fotosNuevoServer where countfotos.cod_auto == x.COD_AUTO select new { }).Count()),
                                            (from fotos in BD.tbl_fotosNuevoServer where fotos.cod_auto == x.COD_AUTO select new { fotos.foto }).ToList().Select(img => new XElement("image", new XAttribute("source", img.foto), Path.GetFileName(img.foto)) )
                                        )
                                )

                            ).Take(10)
                        
                    )
                    
                );

                LimpiaDiccionarios();

                XmlReader reader2 = Registros.CreateReader();
                reader2.MoveToContent();
                XmlDocument doc2 = new XmlDocument();
                XmlNode cd2 = doc2.ReadNode(reader2);
                doc2.AppendChild(cd2);
                doc2.Save(id + "_" + fecha.Replace("/", "").Replace(":","").Replace(" ","") + ".xml");





            }
            catch (Exception ex)
            {

                Console.WriteLine("error: " + ex);

            }

            Console.WriteLine("Finaló la carga");
            Console.ReadLine();


        }


        public static void CargaDiccionarios()
        {

            //Diccionarios de Techo
            DicTecho.Add("", "Ninguno");
            DicTecho.Add("ST","Ninguno");
            DicTecho.Add("TE","Eléctrico");
            DicTecho.Add("TM","Manual");
            DicTecho.Add("TP","Panorámico");

            //Diccionarios de Tipo Direcciones
            DicTipoDireccion.Add("", "Mecanica");
            DicTipoDireccion.Add("H","Hidraulica");
            DicTipoDireccion.Add("M","Mecanica");
            DicTipoDireccion.Add("A","Asistida");

            //Diccionarios de Equipamiento
            DicEquipamiento.Add("", "N");
            DicEquipamiento.Add("N", "N");
            DicEquipamiento.Add("S", "S");


        }

        public static void LimpiaDiccionarios()
        {
            DicTecho.Clear();
            DicTipoDireccion.Clear();
            DicEquipamiento.Clear();

        }

        public static string ObitneValorTecho(string data) {

            string dato = "";

            if (String.IsNullOrEmpty(data))
            {

                dato = "ST";

            }
            else {

                dato = data;

            }
            
            return dato;
        }

    }
}
