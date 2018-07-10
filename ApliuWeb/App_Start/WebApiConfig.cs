using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ApliuWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = "Main", id = RouteParameter.Optional }
            );
        }
    }
}
