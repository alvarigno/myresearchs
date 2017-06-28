using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppStatusConecctionInternet
{
    class Program
    {

        private static string resultado ="";

        static void Main(string[] args)
        {

            resultado = GetStatusConexion().ToString();
            
            Console.WriteLine("Estado conexion: "+resultado);

            Console.ReadLine();

        }


        public static Boolean GetStatusConexion() {

            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }

        }


    }
}
