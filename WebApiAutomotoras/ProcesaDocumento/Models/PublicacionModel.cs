﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcesaDocumento.Models
{
    public class PublicacionModel
    {

        public int codCliente { get; set; }//cliente sucursal
        public string idfuente { get; set; }//codigo auto de la automotora
        public string ip { get; set; }
        public string revision { get; set; }
        public datosVehiculo dVehiculo { get; set; }
        public datosEquipamiento dEquipamiento { get; set; }
        public List<fotos> dfotos { get; set; }

    }

    public class datosVehiculo
    {

        public string patente { get; set; }
        public string tipo { get; set; }
        public int categoria { get; set; }
        public int marca { get; set; }
        public string txtmarca { get; set; }
        public string modelo { get; set; }
        public string version { get; set; }
        public string edicion { get; set; }
        public string carroceria { get; set; }
        public int ano { get; set; }
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
        public string[] listadofotos { get; set; }
        public long uidJato { get; set; }

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

        public string consignacion { get; set; }

        public string plataforma { get; set; } = "SIA"; //Sistema integración Automotoras
      
    }

    public class fotos
    {

        public string url { get; set; }

    }


}