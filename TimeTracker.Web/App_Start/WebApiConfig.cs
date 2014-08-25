using System.Web.Http;

namespace TimeTracker.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {action = RouteParameter.Optional, 
                            id = RouteParameter.Optional }
            );
        }
    }
}
