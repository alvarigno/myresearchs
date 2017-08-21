using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace PublicacionDM
{
    public class PublicarDM
    {

        public string InsertarPublicacion() {

            string result ="";

            var data = "vehicleType=CAR&brand=Suzuki&model=Aerio&version=GLX&year=2008&fuel=Bencina&transmission=Manual&steering=Asistida&doors=4&segment=Sedán&color=Otro Color&mileage=150000&price=5800000&subtitle=Suzuki Aerio GLX 2008&description=Un+modelo+pionero+que+nunca+perdi%C3%B3+su+liderazgo&provider=GRUPO_CCA&key=3453ceda832a83687d0905c2fcdfbe9c&userId=11053485&currency=CLP&image[0]=http://chileautos.li.csnstatic.com/chileautos/auto/particular/ap5655451225218551194.jpg&providerVehicleId=4050906";
            string url = "http://www.demotores.cl/frontend/rest/post.service";
            string responseString;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var action = Uri.EscapeUriString(url);

                var content = new StringContent(data.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                
                var response = client.PostAsync(action, content).Result;
                var responseContent = response.Content;
                responseString = responseContent.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                {
                    result = responseString;
                }
                else
                {
                    result = responseString;
                }

            }

            return result;
        }


    }
}