using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcesaDocumento.Models
{
    public class PublicarChileautosModel
    {

        public int codCliente { get; set; }
        public string ip { get; set; }

	    //datos vehiculo
        public string patente { get; set; }
        public int categoria { get; set; }
        public string tipo { get; set; }
        public int marca { get; set; }
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
        public int combustible { get; set; }
        public string comentario { get; set; }
        public long uidJato { get; set; }

        //equipamiento

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
	    public string listaFotos { get; set; }
        public string plataforma { get; set; } = "SIA";
	    public string consignacion { get; set; }

    }
}