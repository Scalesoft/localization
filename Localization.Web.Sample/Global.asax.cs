using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Localization.Web.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var container = Container.Current; //Create container on startup
            container.Resolve<LocalizationManager>();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
