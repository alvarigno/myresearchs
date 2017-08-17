using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ApiPublicacionDM.Controllers
{
    public class PublicacionDeMotoresController : ApiController
    {


        [HttpPost]
        public async Task<HttpResponseMessage> AdministraAvisoDm() {

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Form.Count > 0 && httpRequest.Form.Keys[0] == "codautoCA")
            {
                if (httpRequest.Form["codautoCA"] == "12345")
                {
                    result = Request.CreateResponse(HttpStatusCode.OK, httpRequest.Form["codautoCA"]);
                }
                else {

                    result = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Código no existe.");

                }
            }
            else {

                result = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Campo no existe.");
            }
            return result;
        }


    }
}