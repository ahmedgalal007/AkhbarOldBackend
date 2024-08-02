using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Domain.Akhbar.DBEntities;
using CMS.Contexts;

namespace CMS.Areas.FrameWork.Controllers
{
    public class SystemAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            bool IsAuthenticated = base.AuthorizeCore(httpContext);

            if (httpContext.Session["UserID"] == null)
            {
                IsAuthenticated = false;
            }

            return IsAuthenticated;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest() == false)
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        Error = "NotAuthorized",
                        LogOnUrl = FormsAuthentication.LoginUrl//urlHelper.Action("Login", "Home", new { area = "Home" })
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };


            }

        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    //var x = filterContext.Controller.ValueProvider;
        //    bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
        //    || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);


        //    if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && filterContext.HttpContext.Session["UserID"] != null)
        //    {
        //        base.OnAuthorization(filterContext);
        //    }
        //}


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //var x = filterContext.Controller.ValueProvider;
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);


            if (filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated && filterContext.HttpContext.Session["UserID"] != null)
            {
                var routeData = filterContext.RequestContext.RouteData;

                string controller = routeData.GetRequiredString("controller");

                string action = routeData.GetRequiredString("action");


                string area = "";
                if (routeData.DataTokens.ContainsKey("area"))
                    area = (string)routeData.DataTokens["area"];

                //string module;
                //string service;

                //module = GeneralHellper.DecryptStringFromBytes(filterContext.RequestContext.HttpContext.Request["Mod"]);
                //service = GeneralHellper.DecryptStringFromBytes(filterContext.RequestContext.HttpContext.Request["Ser"]);




                if (controller != "Shared")//ignore shared controller from authorization
                {
                    List<VUserPermissions> lstSF;
                    GeneralHellper _GeneralHellper = new GeneralHellper();
                    using (AkhbarDBContext db = new AkhbarDBContext(_GeneralHellper.AkhbarDbConnection))
                    {
                        lstSF = _GeneralHellper.UserPermissions(db);
                    }

                    bool canAccess = false;


                    canAccess = lstSF.Any(sf => sf.AreaName == area && sf.ControllerName == controller && sf.ActionName == action);
                    if (skipAuthorization == false)
                    {
                        if (canAccess == false)
                        {

                            PartialViewResult p = new PartialViewResult();
                            p.ViewName = "~/Areas/FrameWork/Views/Shared/_NotAuthorized.cshtml";
                            filterContext.Result = p;

                        }
                    }






                    //canAccess = lstSF.Any(sf => sf.ServiceEngName == service && sf.ModuleEngName == module && sf.Area == area && sf.Controller == controller && sf.Action == action);
                    //if (skipAuthorization == false)
                    //{
                    //    if (canAccess == false)
                    //    {
                    //        if (action != "AsRelated")
                    //        {
                    //            PartialViewResult p = new PartialViewResult();
                    //            p.ViewName = "~/Areas/FrameWork/Views/Shared/_NotAuthorized.cshtml";
                    //            filterContext.Result = p;
                    //        }
                    //        else
                    //        {
                    //            ContentResult p = new ContentResult();
                    //            p.Content = "";
                    //            filterContext.Result = p;
                    //        }
                    //    }

                    }
            }


            base.OnAuthorization(filterContext);

        }


    }
}
