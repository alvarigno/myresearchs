﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsearEmails_WebAPI.Models
{
    public class AccionDelimitador
    {
        public int Accion { get; set; }
        public string Delimitador { get; set; }
        public string Nombre { get; set; }
        public int IdRegla { get; set; }
    }
}
