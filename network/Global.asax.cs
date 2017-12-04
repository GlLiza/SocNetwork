using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using network;
using System;

namespace network
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;

            Bootstrapper.Initialise();
         
        }
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            //log the error!
            Console.WriteLine(ex.Message);
        }
        //protected void Application_EndRequest()
        //{

        //}
    }
}
