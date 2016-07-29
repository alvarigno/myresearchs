using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COnsoleWahtSystem
{
    class myConnection
    {

        public static SqlConnection GetConnection()
        {
            string str = "Data Source=ALVARO-PC\\SQLEXPRESS; Initial Catalog = Procesarmails; Integrated Security = True";
            SqlConnection con = new SqlConnection(str);
            con.Open();
            return con;
        }

    }
}
