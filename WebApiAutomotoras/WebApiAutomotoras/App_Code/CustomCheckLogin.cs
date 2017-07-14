using WebApiAutomotoras.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using DataAccess;

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
            DataAccess.Data.baseprodEntities database = new DataAccess.Data.baseprodEntities();

            string token = Util.getValueFromHeader("X-KEY") ?? "";

            ObjectParameter esValidoParam = new ObjectParameter("esValido", typeof(bool));
            database.SP_apiCLA_CompruebaLogin(token, esValidoParam);

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