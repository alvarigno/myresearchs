using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParsearEmails_WebAPI.Models
{
    public class MailsSinParsearModel
    {
        public int uid_email { get; set; }
        public int uid_tipo { get; set; }
        public int uid_estado { get; set; }
        public int uid_automotora { get; set; }
        public int uid_fuente { get; set; }
        public string email { get; set; }
        public string asunto { get; set; }
        public string cabecera { get; set; }
        public string destinatarios { get; set; }
        public string remitente { get; set; }
        public string cc { get; set; }
    }
}