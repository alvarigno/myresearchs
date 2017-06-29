using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiAutomotoras.App_Code
{
    public class CustomCheckReferer : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var referer = HttpContext.Current.Request.UrlReferrer.AbsolutePath.ToString();
            if (referer.IndexOf("chileautos.cl") > 0 || referer.IndexOf("172.16.0") > 0)
            {
                return true;
            }
            else
            {
                HttpContext.Current.Response.AddHeader("authenticationTokenStatus", "NotAuthorized");
                return false;
            }
        }
    }
}