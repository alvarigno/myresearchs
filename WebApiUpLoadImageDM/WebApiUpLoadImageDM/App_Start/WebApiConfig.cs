using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace WebApiUpLoadImageDM
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
