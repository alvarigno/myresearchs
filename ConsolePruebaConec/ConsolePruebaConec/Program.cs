using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePruebaConec
{
    class Program
    {
       myConnection con = new myConnection();

        public static void Main(string[] args)
        {
            Conectar();
            Console.ReadLine();

        }

        public static void Conectar() {

            //SqlConnection myConnection = new SqlConnection("Data Source=10.0.0.158; Initial Catalog = bdmailparser; User ID= usdes; Password= Su_4320$.x; Integrated Security = True");

            try
            {
                SqlCommand mycommand = new SqlCommand();
                mycommand.Connection = myConnection.GetConnection();
                mycommand.Connection.Open();
                Console.WriteLine("Well done!");
                mycommand.Connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("You failed!" + ex.Message);
            }

        }


    }
}
