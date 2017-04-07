using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Transbank.NET
{
    public class myConnection
    {
        static private string bdconexion = ConfigurationManager.ConnectionStrings["ConnectionDBString"].ConnectionString;
        public static SqlConnection GetConnection()
        {

            //quitar el comentario
            //string str = "data source=10.0.0.158; initial catalog=desarrollo;Persist Security Info=True; user id=usdes;password=Su_4320$.x;";//Desarrollo
            //string str = "data source=172.16.0.241;initial catalog=baseprod2;user id=usrBDDesa;password=*pwdBD*;";//Base local
            string str = bdconexion;
            SqlConnection con = new SqlConnection(str);
            con.Open();
            return con;
        }

    }

}