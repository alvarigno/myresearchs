using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApiAutomotoras.Infrastructure;
using UpLoadServicesRestWebApiModel;
using WebApiAutomotoras.App_Code;
using System.Web.Http.Cors;
using System.Data.Entity.Core.Objects;
using AccesoDatos.Data;
using ProcesaDocumento;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WebApiAutomotoras.Controllers
{
    [RoutePrefix("API-CLAAutomotora/Diccionario")]
    public class DiccionarioController : ApiController
    {
        [HttpGet]
        [Route("techos")]
        [CustomCheckLogin]
        public HttpResponseMessage getTechos()
        {
            string releases ="";

            using (var httpClient = new HttpClient())
            {            

                 string response = httpClient.GetStringAsync(new Uri("https://ws.chileautos.cl/API-CLA/Caracteristicas/Techos")).Result;

                releases = response; 
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }

        [HttpGet]
        [Route("Puertas")]
        [CustomCheckLogin]
        public HttpResponseMessage getPuertas()
        {
            string releases = "";

            using (var httpClient = new HttpClient())
            {

                string response = httpClient.GetStringAsync(new Uri("https://ws.chileautos.cl/API-CLA/Caracteristicas/Puertas")).Result;

                releases = response;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }

        [HttpGet]
        [Route("TipoDireccion")]
        [CustomCheckLogin]
        public HttpResponseMessage getTipoDireccion()
        {
            string releases = "";

            using (var httpClient = new HttpClient())
            {

                string response = httpClient.GetStringAsync(new Uri("https://ws.chileautos.cl/API-CLA/Caracteristicas/Tiposdireccion")).Result;

                releases = response;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }

        [HttpGet]
        [Route("Cilindradas")]
        [CustomCheckLogin]
        public HttpResponseMessage getCilindradas()
        {
            string releases = "";

            using (var httpClient = new HttpClient())
            {

                string response = httpClient.GetStringAsync(new Uri("https://ws.chileautos.cl/API-CLA/Caracteristicas/Cilindradas")).Result;

                releases = response;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }

        [HttpPost]
        [Route("Combustible/")]
        [CustomCheckLogin]
        public HttpResponseMessage getCombustible(string marca, string modelo, string agno, string carroceria)
        {
            string releases = "";
            string url = "https://ws.chileautos.cl/API-CLA/Jato/CombustiblesWL";

            using (var httpClient = new HttpClient())
            {
                
                var customer = new { marca = "" + marca + "", modelo = "" + modelo + "", ano = "" + agno + "", carroceria=""+carroceria+"" };
                var json = JsonConvert.SerializeObject(customer);

                var response = httpClient.PostAsJsonAsync(new Uri(url), JToken.Parse(json));

                releases = response.Result.Content.ReadAsStringAsync().Result;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }
        
        [HttpGet]
        [Route("Agnos")]
        [CustomCheckLogin]
        public HttpResponseMessage getAnos()
        {
            string releases = "";

            using (var httpClient = new HttpClient())
            {

                string response = httpClient.GetStringAsync(new Uri("https://ws.chileautos.cl/API-CLA/Caracteristicas/Anos")).Result;

                releases = response;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }
        
        [HttpGet]
        [Route("Categorias")]
        [CustomCheckLogin]
        public HttpResponseMessage getCategorias()
        {
            string releases = "";

            using (var httpClient = new HttpClient())
            {

                string response = httpClient.GetStringAsync(new Uri("https://ws.chileautos.cl/API-CLA/Categorias")).Result;

                releases = response;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }

    //    [HttpGet]
    //    [Route("Marcas/Categoria/")]
    //    [CustomCheckLogin]
    //    public HttpResponseMessage getMarcaCategoria(string categoria)
    //    {
    //        string releases = "";
    //        string url = "https://ws.chileautos.cl/API-CLA/Marcas/Categoria/" + categoria;
    //
    //        using (var httpClient = new HttpClient())
    //        {
    //
    //            string response = httpClient.GetStringAsync(new Uri(url)).Result;
    //
    //            releases = response;
    //        }
    //
    //        return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
    //    }

        [HttpPost]
        [Route("Carrocerias/")]
        [CustomCheckLogin]
        public HttpResponseMessage getCarroceria(string marca, string modelo, string agno)
        {
            string releases = "";
            string url = "https://ws.chileautos.cl/API-CLA/Jato/CarroceriasWL";

            using (var httpClient = new HttpClient())
            {

                var customer = new { marca = ""+marca+"", modelo =""+modelo+"", ano=""+agno+"" };
                var json = JsonConvert.SerializeObject(customer);

                var response = httpClient.PostAsJsonAsync(new Uri(url), JToken.Parse(json));

                releases = response.Result.Content.ReadAsStringAsync().Result;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));

        }

        [HttpGet]
        [Route("TipoVehiculo/Categoria/")]
        [CustomCheckLogin]
        public HttpResponseMessage getTipoVehiculoCategoria(string categoria)
        {
            string releases = "";
            string url = "https://ws.chileautos.cl/API-CLA/TiposVehiculo/Categoria/" + categoria;

            using (var httpClient = new HttpClient())
            {

                string response = httpClient.GetStringAsync(new Uri(url)).Result;

                releases = response;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }

        [HttpPost]
        [Route("Marcas/")]
        [CustomCheckLogin]
        public HttpResponseMessage getMarcas(string categoria)
        {
            string releases = "";
            string url = "https://ws.chileautos.cl/API-CLA/Jato/MarcasWL";

            using (var httpClient = new HttpClient())
            {

                var customer = new { ppu = "", idCat = ""+categoria+"" };
                var json = JsonConvert.SerializeObject(customer);

                var response = httpClient.PostAsJsonAsync(new Uri(url), JToken.Parse(json));

                releases = response.Result.Content.ReadAsStringAsync().Result;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }


        [HttpPost]
        [Route("Modelos/")]
        [CustomCheckLogin]
        public HttpResponseMessage getModelos(string categoria, string marca, string agno)
        {
            string releases = "";
            string url = "https://ws.chileautos.cl/API-CLA/Jato/ModelosWL";

            using (var httpClient = new HttpClient())
            {
               
                var customer = new { idCat = "" + categoria + "", marca = ""+marca+"", ano= ""+agno+"" };
                var json = JsonConvert.SerializeObject(customer);

                var response = httpClient.PostAsJsonAsync(new Uri(url), JToken.Parse(json));

                releases = response.Result.Content.ReadAsStringAsync().Result;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }

        [HttpPost]
        [Route("Version/")]
        [CustomCheckLogin]
        public HttpResponseMessage getVersion(string marca, string modelo, string agno)
        {
            string releases = "";
            string url = "https://ws.chileautos.cl/API-CLA/Jato/Versiones";

            using (var httpClient = new HttpClient())
            {
               
                var customer = new { marca = ""+marca+"", modelo =""+modelo+"", ano= ""+agno+"" };
                var json = JsonConvert.SerializeObject(customer);

                var response = httpClient.PostAsJsonAsync(new Uri(url), JToken.Parse(json));

                releases = response.Result.Content.ReadAsStringAsync().Result;
            }

            return Request.CreateResponse(HttpStatusCode.OK, JToken.Parse(releases));
        }



    }

}