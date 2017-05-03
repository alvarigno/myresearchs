using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicarDITEC
{


    public class PublicacionChileautos
    {

        public int codAuto { get; set; }
        public datosUsuario datosUsuario { get; set; }
        public datosVehiculo datosVehiculo { get; set; }
        public datosEquipamiento datosEquipamiento { get; set; }

    }

    public class datosUsuario
    {
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string fonos { get; set; }
        public string email { get; set; }
        public string rut { get; set; }
        public string clave { get; set; }
        public int codCliente { get; set; }
        public int region { get; set; }
        public string nomRegion { get; set; }
        public string direccion { get; set; }
        public string comuna { get; set; }
        public string nomComuna { get; set; }
        public int ciudad { get; set; }
        public string nomCiudad { get; set; }

    }

    public class datosVehiculo
    {

        public string patente { get; set; }
        public string tipo { get; set; }
        public int marca { get; set; }
        public string nomMarca { get; set; }
        public string modelo { get; set; }
        public int ano { get; set; }
        public string version { get; set; }
        public string carroceria { get; set; }
        public int puertas { get; set; }
        public string tipoDireccion { get; set; }
        public int precio { get; set; }
        public int cilindrada { get; set; }
        public string potencia { get; set; }
        public string color { get; set; }
        public int kilom { get; set; }
        public string motor { get; set; }
        public string techo { get; set; }
        public string combustible { get; set; }
        public string comentario { get; set; }
        public List<String> fotos { get; set; }
        public string video { get; set; }
        public int idCategoria { get; set; }
        public string categoria { get; set; }
        public bool esJato { get; set; }
        public long uid { get; set; }
        public string edicion { get; set; }

    }
    public class datosEquipamiento
    {

        public string airbag { get; set; }

        public string aireAcon { get; set; }

        public string alarma { get; set; }

        public string alzaVidrios { get; set; }

        public string nuevo { get; set; }

        public string transmision { get; set; }

        public string radio { get; set; }

        public string espejos { get; set; }

        public string frenosAbs { get; set; }

        public string unicoDueno { get; set; }

        public string cierreCentral { get; set; }

        public string catalitico { get; set; }

        public string fwd { get; set; }

        public string llantas { get; set; }


    }

}
