using System.Web.Mvc;

namespace CMS.Areas.CoreHandler
{
    public class CoreHandlerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CoreHandler";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CoreHandler_default",
                "CoreHandler/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}