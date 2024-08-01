using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;

namespace AkhbarElyoum.Models
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString MenuLink(
    this HtmlHelper htmlHelper,
    string linkText,
    string actionName,
    string controllerName,
            int SectionID,int currentsectionid
)
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentSection = (htmlHelper.ViewContext.RouteData.DataTokens["SecID"] != null) ? htmlHelper.ViewContext.RouteData.DataTokens["SecID"].ToString() : "";
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            if (SectionID == currentsectionid && controllerName == currentController)
            {
                return htmlHelper.ActionLink(
                    linkText,
                    actionName,
                    controllerName,
                    new { SecID = SectionID },
                    new
                    {
                        @class = "current"
                    });
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, new { SecID = SectionID },null);
        }
        private static bool IsInCurrentArea(object routeValues, string currentArea)
        {
            if (routeValues == null)
                return true;

            var rvd = new RouteValueDictionary(routeValues);
            string area = rvd["Area"] as string ?? rvd["area"] as string;
            return area == currentArea;
        }
    }
}