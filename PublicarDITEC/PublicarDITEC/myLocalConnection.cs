using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicarDITEC
{
    class myLocalConnection
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
