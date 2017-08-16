using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApiUpLoadImageDM.Models
{
    public class FilesUpLoad
    {

        public FilesUpLoad(string name)
        {
            NameDM = name;
        }

        public string NameDM { get; set; }

        public string UrlLocal { get; set; }

    }
}