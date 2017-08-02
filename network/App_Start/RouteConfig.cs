using System.Web.Mvc;
using System.Web.Routing;

namespace network
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Users", action = "Page", id = UrlParameter.Optional }
            );
        }
    }
}
