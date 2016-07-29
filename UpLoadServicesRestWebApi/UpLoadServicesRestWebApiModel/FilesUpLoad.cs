using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UpLoadServicesRestWebApiModel
{
    public class FilesUpLoad
    {

        public FilesUpLoad(string name, string url, string size)
        {
            Name = name;
            Url = url;
            Size = size;
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Size { get; set; }

    }
}