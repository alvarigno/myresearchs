using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApiUpLoadImageDM.Models
{
    public class FilesUpLoad
    {

        public FilesUpLoad(string urlDM, string urlCA)
        {
            FileDM = urlDM;
            FileCA = urlCA;
        }

        public string FileDM { get; set; }

        public string FileCA { get; set; }

    }
}