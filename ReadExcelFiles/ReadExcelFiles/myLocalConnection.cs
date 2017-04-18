using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Transbank.NET
{
    public class myLocalConnection
    {
        public static SqlConnection GetLocalConnection()
        {
            string str = "Data Source=ALVARO-PC\\SQLEXPRESS; Initial Catalog = dbo.DITEC; User ID=jesus; Password=12345";
            SqlConnection con = new SqlConnection(str);
            con.Open();
            return con;

        }

    }

}