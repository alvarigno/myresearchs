using WebApiAutomotoras.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using AccesoDatos.Data;

namespace WebApiAutomotoras.App_Code
{
    public class CustomCheckLogin : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            baseprod2Entities database = new baseprod2Entities();

            string token = Util.getValueFromHeader("X-KEY") ?? "";

            ObjectParameter esValidoParam = new ObjectParameter("esValido", typeof(bool));
            database.SPR_CompruebaLogin_Automotora(token, esValidoParam);

            if ((bool)esValidoParam.Value)
            {
                return true;
            }
            else
            {
                HttpContext.Current.Response.AddHeader("authenticationToken", token);
                HttpContext.Current.Response.AddHeader("authenticationTokenStatus", "NotAuthorized");
                return false;
            }
        }
    }
}