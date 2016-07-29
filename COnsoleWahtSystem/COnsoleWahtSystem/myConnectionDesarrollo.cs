using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COnsoleWahtSystem
{
    class myConnectionDesarrollo
    {

        public static SqlConnection GetConnection()
        {
            string str = "Data Source=10.0.0.158; Initial Catalog = bdmailparser;User ID= usdes; Password= Su_4320$.x; Integrated Security = True";
            SqlConnection con = new SqlConnection(str);
            con.Open();
            return con;
        }


        //        nombre_base = "desarrollo"
        //nombre_server = "10.0.0.158"
        //nombre_user = "usdes"
        //pass = "Su_4320$.x"

    }

}
