using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace PublicarDITEC
{
    class myConnection
    {




        public static SqlConnection GetConnection()
        {
            string str = "data source=172.16.0.241;initial catalog=baseprod2;user id=usrBDDesa;password=*pwdBD*;";//Base local
            
            SqlConnection con = new SqlConnection(str);
            con.Open();
            return con;

        }

    }
}
