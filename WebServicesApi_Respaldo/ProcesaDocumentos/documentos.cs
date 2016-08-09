using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcesaDocumentos
{
    public class Documentos
    {

        public int id_num { get; set; }
        public string fnombre { get; set; }
        public int estado { get; set; }
        public int sitio { get; set; }
        public DateTime fecha_insert { get; set; }

    }
}
