using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SoftSupply.Help.Models;
using System.IO;
using System.Web.Helpers;

namespace SoftSupply.Help
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Dictionary<string, Manifest> Areas { get; internal set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            Areas = (from item in RouteTable.Routes.OfType<Route>()
                     where item.DataTokens != null && item.DataTokens.ContainsKey("area")
                     let area = item.DataTokens["area"]
                     orderby area
                     select Json.Decode<Manifest>(File.ReadAllText(Server.MapPath($"areas/{area}/content/manifest.json"))))
                     .ToDictionary(k => k.Name, v => v);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}