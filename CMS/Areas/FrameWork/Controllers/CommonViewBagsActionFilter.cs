using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMS.Areas.FrameWork.Controllers
{
    public class CommonViewBagsActionFilter : ActionFilterAttribute
    {
       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Action = filterContext.HttpContext.Request.RequestContext.RouteData.GetRequiredString("action");
            filterContext.Controller.ViewBag.Controller = filterContext.HttpContext.Request.RequestContext.RouteData.GetRequiredString("Controller");

            //get the area section in view bag to be used in the views
            if (filterContext.HttpContext.Request.RequestContext.RouteData.DataTokens.ContainsKey("area") == true)
                filterContext.Controller.ViewBag.Area = filterContext.HttpContext.Request.RequestContext.RouteData.DataTokens["area"];

            base.OnActionExecuting(filterContext);
        }
    }
}