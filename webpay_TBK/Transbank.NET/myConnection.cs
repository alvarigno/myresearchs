using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Transbank.NET
{
    public class myConnection
    {

        public static SqlConnection GetConnection()
        {
            string str = "data source=172.16.0.241;initial catalog=baseprod2;user id=usrBDDesa;password=*pwdBD*;";
            //string str = "Data Source=10.0.0.158; Initial Catalog=bdmailparser;User ID=usdes; Password=Su_4320$.x";
            SqlConnection con = new SqlConnection(str);
            con.Open();
            return con;
        }

    }
}