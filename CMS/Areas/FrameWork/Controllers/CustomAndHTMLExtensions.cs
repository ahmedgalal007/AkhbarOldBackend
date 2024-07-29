using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using PagedList.Mvc;
using PagedList;
using System.Web.Mvc.Ajax;
using System.Web.SessionState;
using Akhbar.DBEntities;

namespace CMS.Areas.FrameWork.Controllers
{
    public static class CustomAndHTMLExtensions
    {

        #region ActionLinks
        public static MvcHtmlString CustomBackToListAjaxActionLink(this AjaxHelper ajaxHelper)
        {
            RouteValueDictionary routeValues = GeneralHellper.GetSystemRouteValues((Controller)ajaxHelper.ViewContext.Controller);
            AjaxOptions option = new AjaxOptions()
            {
                LoadingElementId = "waitimage",
                UpdateTargetId = (string)routeValues["RelatedTarget"],
                InsertionMode = InsertionMode.ReplaceWith,
                HttpMethod = "Post"
            };
            List<string> keys = new List<string>(routeValues.Keys);
            foreach (var key in keys)
            {
                routeValues[key] = routeValues[key] != null ? GeneralHellper.EncryptStringToBytes(routeValues[key].ToString()) : null;
            }
            routeValues.Add("area", ajaxHelper.ViewBag.Area);
            return ajaxHelper.ActionLink("Back To List", "AsRelated", (string)ajaxHelper.ViewBag.Controller, routeValues, option);
        }

        public static MvcHtmlString CustomRowFunctionsAjaxActionLink(this AjaxHelper ajaxHelper, string linkText
                   , string area, string controller
                   , string action
               , int id
                   //, string HttpMethod
                   , RouteValueDictionary AdditionalRouteValues
           , AjaxOptions ajaxOption
                   , IDictionary<string, object> htmlAttributes
          )
        {
            RouteValueDictionary routeValues = GeneralHellper.GetSystemRouteValues((Controller)ajaxHelper.ViewContext.Controller);

            //if (action == "Delete") routeValues.Add("__RequestVerificationToken", "_");



            AjaxOptions Option = ajaxOption;
            if (Option == null)
            {
                if (action != "Delete")
                {

                    Option = new AjaxOptions
                    {
                        LoadingElementId = "waitimage",
                        OnSuccess = routeValues["RelatedTarget"] == null ? "OpenModal();" : "", //routeValues["RelatedTarget"] == null ? "OpenModal();InitAccordionMain(1);InitAccordion();" : "InitAccordionMain();InitAccordion();",
                        UpdateTargetId = (string)routeValues["RelatedTarget"] ?? "ModalContent",//ViewBag.UpdateTarget,
                        InsertionMode = routeValues["RelatedTarget"] == null ? InsertionMode.Replace : InsertionMode.ReplaceWith,
                        HttpMethod = "Get"
                    };
                }
                else
                {
                    Option = new AjaxOptions
                    {
                        LoadingElementId = "waitimage",
                        // OnSuccess = "InitAccordion();",
                        Confirm = "Are you sure you wish to delete this item ?",
                        //  OnBegin = "confirm('Are you sure you wish to delete this item ?');",
                        UpdateTargetId = (string)routeValues["RelatedTarget"] ?? "Content",
                        InsertionMode = routeValues["RelatedTarget"] != null ? InsertionMode.ReplaceWith : InsertionMode.Replace,
                        //OnSuccess = routeValues["RelatedTarget"] != null ? "InitAccordion();InitAccordionMain(false);" : "InitAccordion();InitAccordionMain();",
                        HttpMethod = "Post"
                    };
                }
            }

            if (AdditionalRouteValues != null)
            {
                foreach (var item in AdditionalRouteValues)
                {
                    routeValues[item.Key] = item.Value;
                }
            }
            List<string> keys = new List<string>(routeValues.Keys);
            foreach (var key in keys)
            {
                routeValues[key] = routeValues[key] != null ? GeneralHellper.EncryptStringToBytes(routeValues[key].ToString()) : null;
            }
            routeValues.Add("id", id);
            routeValues.Add("area", area);
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, action, controller, routeValues, Option, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));

        }

        public static MvcHtmlString CustomAjaxActionLink(this AjaxHelper ajaxHelper, string linkText
                   , string area, string controller
                   , string action
              //, int id
              , RouteValueDictionary AdditionalRouteValues
           , AjaxOptions ajaxOption
                   , IDictionary<string, object> htmlAttributes
          )
        {
            RouteValueDictionary routeValues = GeneralHellper.GetSystemRouteValues((Controller)ajaxHelper.ViewContext.Controller);


            if (AdditionalRouteValues != null)
            {
                foreach (var item in AdditionalRouteValues)
                {
                    routeValues[item.Key] = item.Value;
                }
            }
            List<string> keys = new List<string>(routeValues.Keys);
            foreach (var key in keys)
            {
                routeValues[key] = routeValues[key] != null ? GeneralHellper.EncryptStringToBytes(routeValues[key].ToString()) : null;
            }
            //routeValues.Add("id", id);
            routeValues.Add("area", area);
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, action, controller, routeValues, ajaxOption, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));

        }


        #endregion

        #region PagingActionLinks

        public static MvcHtmlString CustomAjaxPagedListPager(this System.Web.Mvc.HtmlHelper html, IPagedList list, UrlHelper Url)//, RouteValueDictionary SerachValues = null
        {
            RouteValueDictionary routeValues = GeneralHellper.GetSystemRouteValues((Controller)html.ViewContext.Controller);


            AjaxOptions option = new AjaxOptions
            {
                LoadingElementId = "waitimage",
                InsertionMode = routeValues["RelatedTarget"] != null ? InsertionMode.ReplaceWith : InsertionMode.Replace,
                HttpMethod = html.ViewContext.TempData["SearchRout"] != null ? "Post" : "Get",
                //OnSuccess = routeValues["RelatedTarget"] != null ? "InitAccordion();InitAccordionMain(false);" : "InitAccordion();InitAccordionMain();",
                UpdateTargetId = (string)routeValues["RelatedTarget"] ?? "Content"
            };



            List<string> keys = new List<string>(routeValues.Keys);
            foreach (var key in keys)
            {
                routeValues[key] = routeValues[key] != null ? GeneralHellper.EncryptStringToBytes(routeValues[key].ToString()) : null;
            }
            routeValues.Remove("page");
            if (routeValues["RelatedID"] != null)
            {
                routeValues["id"] = null;
            }
            return html.PagedListPager(
                list
                , page =>//?? "1"
                    Url.Action(routeValues["RelatedID"] == null ? "index" : "AsRelated", routeValues) + "&page=" + GeneralHellper.EncryptStringToBytes(page.ToString())
                    , PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(
                    PagedListRenderOptions.Classic, option));


        }

        public static MvcHtmlString CustomAjaxPagedListPager(this System.Web.Mvc.HtmlHelper html, string Area, string Controller, string Action, AjaxOptions ajaxOption, IPagedList list, UrlHelper Url)//, RouteValueDictionary SerachValues = null
        {
            RouteValueDictionary routeValues = GeneralHellper.GetSystemRouteValues((Controller)html.ViewContext.Controller);

            List<string> keys = new List<string>(routeValues.Keys);
            foreach (var key in keys)
            {
                routeValues[key] = routeValues[key] != null ? GeneralHellper.EncryptStringToBytes(routeValues[key].ToString()) : null;
            }
            routeValues.Remove("page");
            routeValues.Add("area", Area);
            return html.PagedListPager(
                list
                , page =>
                    Url.Action(Action, Controller, routeValues) + "&page=" + GeneralHellper.EncryptStringToBytes(page.ToString())
                    , PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(
                    PagedListRenderOptions.Classic, ajaxOption));


        }

        #endregion

        #region AjaxBeginForm
        public static MvcForm CustomBeginForm(this AjaxHelper ajaxHelper, string Action, string Controller, string area, RouteValueDictionary AdditionalRouteValues, AjaxOptions ajaxOption, IDictionary<string, object> htmlAttributes = null)
        {
            RouteValueDictionary routeValues = GeneralHellper.GetSystemRouteValues((Controller)ajaxHelper.ViewContext.Controller);


            AjaxOptions option = ajaxOption;

            if (option == null)
            {
                option = new AjaxOptions()
                {
                    LoadingElementId = "waitimage",
                    UpdateTargetId = (string)routeValues["RelatedTarget"] ?? "ModalContent",// ???? "Content",
                                                                                            // OnSuccess = routeValues["RelatedTarget"] == null ? "CloseModal();" : "",
                    InsertionMode = routeValues["RelatedTarget"] != null ? InsertionMode.ReplaceWith : InsertionMode.Replace,
                    HttpMethod = "Post"
                };
            }


            if (AdditionalRouteValues != null)
            {
                foreach (var item in AdditionalRouteValues)
                {
                    routeValues[item.Key] = item.Value;
                }
            }
            List<string> keys = new List<string>(routeValues.Keys);
            foreach (var key in keys)
            {
                routeValues[key] = routeValues[key] != null ? GeneralHellper.EncryptStringToBytes(routeValues[key].ToString()) : null;
            }
            routeValues.Add("area", area);
            return ajaxHelper.BeginForm(Action, Controller, routeValues, option, htmlAttributes);
        }


        #endregion


        #region SessionExtensions
        /// <summary>
        /// this class saves something to the Session object
        /// but with an EXPIRATION TIMEOUT
        /// (just like the ASP.NET Cache)
        /// (c) Jitbit 2011. Feel free to use/modify/whatever
        /// usage sample:
        ///  Session.AddWithTimeout(
        ///   "key",
        ///   "value",
        ///   TimeSpan.FromMinutes(5));
        /// </summary>
        public static void AddWithTimeout(this HttpSessionState session, string name, object value, TimeSpan expireAfter)
        {
            session[name] = value;
            session[name + "ExpDate"] = DateTime.Now.Add(expireAfter);
        }

        public static object GetWithTimeout(this HttpSessionState session, string name)
        {
            object value = session[name];
            if (value == null) return null;

            DateTime? expDate = session[name + "ExpDate"] as DateTime?;
            if (expDate == null) return null;

            if (expDate < DateTime.Now)
            {
                session.Remove(name);
                session.Remove(name + "ExpDate");
                return null;
            }

            return value;
        }

        public static void RemoveTimeout(this HttpSessionState session, string name)
        {
            session.Remove(name);
            session.Remove(name + "ExpDate");
        }

        public static bool ContainKey(this HttpSessionState session, string name)
        {
            foreach (string item in session.Keys)
            {
                if (item == name)
                    return true;
            }
            return false;
        }
        #endregion

       
    }
}