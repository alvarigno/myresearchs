using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppReadFile
{
    class myConnection
    {

        public static SqlConnection GetConnection()
        {
            string str = "Data Source=ALVARO-PC\\SQLEXPRESS; Initial Catalog = Procesarmails; Integrated Security = True";
            //string str = "Data Source=10.0.0.158; Initial Catalog=bdmailparser;User ID=usdes; Password=Su_4320$.x";
            SqlConnection con = new SqlConnection(str);
            con.Open();
            return con;
        }

    }
}
