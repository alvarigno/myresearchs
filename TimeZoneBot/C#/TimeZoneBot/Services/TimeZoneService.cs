using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TimeZoneBot.Models;

namespace TimeZoneBot.Services
{
    public class TimeZoneService
    {
        /// <summary>
        ///     Gets the value from the "timezonedb" service as per the zoneName.
        /// </summary>
        /// <param name="zoneName"></param>
        /// <returns>TimeZoneModel</returns>
        public static async Task<TimeZoneModel> GetWeatherInfo(string zoneName)
        {
            using (var client = new HttpClient())
            {
                //Key will be generated after Registration http://timezonedb.com/register
                const string authKey = "12345";

                var url = string.Format("http://api.timezonedb.com/?zone={0}&format=json&key={1}", zoneName, authKey);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode) return null;
                var result = await response.Content.ReadAsStringAsync();

                //De-Serialize
                /* var list = JsonConvert.DeserializeObject<TimeZoneModel>(result);*/

                var js = new DataContractJsonSerializer(typeof(TimeZoneModel));
                var ms = new MemoryStream(Encoding.ASCII.GetBytes(result));
                var list = (TimeZoneModel)js.ReadObject(ms);

                return list;
            }
        }
    }
}