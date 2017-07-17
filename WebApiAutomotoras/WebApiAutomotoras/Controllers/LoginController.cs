using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using WebApiAutomotoras.Models.Parametros;
using System.Data.Entity.Core.Objects;
using System.Net;
using AccesoDatos.Data;


namespace WebApiAutomotoras.Controllers
{
    [RoutePrefix("API-CLAAutomotora/Login")]
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("")]
        public HttpResponseMessage login([FromBody] LoginUploadParamModel  param)
        {
            baseprod2Entities database = new baseprod2Entities();
            ObjectParameter keyParam = new ObjectParameter("key", typeof(string));

            try
            {
                database.SP_apiCLA_LoginModificacion(param.rut, param.clave, keyParam);

                if (keyParam.Value.ToString() == "notfound")
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { msg = "No existen registros con esos datos" });
                }
                else { 

                    return Request.CreateResponse(HttpStatusCode.OK, new { msg = "Exito", key = keyParam.Value.ToString() });
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { msg = "No es posible procesar la petición" });
            }
        }
    }
}
