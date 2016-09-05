using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UsingHttpClient_BestPracties
{
    class Program
    {
        private static HttpClient Client = new HttpClient();
        static void Main(string[] args)
        {
            
            Console.WriteLine("Starting connections");
            for (int i = 0; i < 10; i++)
            {
                var result = Client.GetAsync("http://www.alvaroemparan.cl").Result;
                Console.WriteLine(result.StatusCode);
            }
            Console.WriteLine("Connections done");
            Console.ReadLine();

        }
    }
}
