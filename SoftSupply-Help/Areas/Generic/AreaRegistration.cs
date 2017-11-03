using System.Web.Mvc;

namespace SoftSupply.Help.Areas.Generic
{
    public class GenericAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Generic";
            }
        }
        
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                AreaName,
                AreaName + "/{controller}/{action}/{id}",
                new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}