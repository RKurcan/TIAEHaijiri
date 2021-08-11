using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace RTech.Demo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "DefaultApiGet",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
              name: "DefaultApiP",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { action = "get", id = RouteParameter.Optional }
           );
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.EnableCors();

        }
    }
}
