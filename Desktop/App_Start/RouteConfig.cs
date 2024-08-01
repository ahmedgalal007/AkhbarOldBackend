using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Desktop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // routes.MapRoute(
            //     name: "Default",
            //     url: "{controller}/{action}/{id}",
            //     defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            // );


            routes.MapRoute(
                name: "YearSiteMap",
                url: "{controller}/{year}",
                defaults: new { controller = "Sitemap", action = "Index" },
                constraints: new { year = @"\d+" }
            );

            routes.MapRoute(
                name: "StoriesSiteMap",
                url: "{controller}/{year}/{month}/{day}",
                defaults: new { controller = "Sitemap", action = "Stories" },
                constraints: new { year = @"\d+", month = @"\d+", day = @"\d+" }
            );
            routes.MapRoute(
                name: "AMPStoriesSiteMap",
                url: "{controller}/{Action}/{year}/{month}/{day}",
                defaults: new { controller = "Sitemap", action = "AmpStories" },
                constraints: new { year = @"\d+", month = @"\d+", day = @"\d+" }
            );

            routes.MapRoute(
                name: "Default3",
                url: "{controller}/{action}/{ID}/{JournalID}/{name}",
                //defaults: new { controller = "News", action = "NewDetails", name = UrlParameter.Optional, JournalID = UrlParameter.Optional }
                defaults: new { controller = "News", action = "NewDetails", name = "", JournalID = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Default2",
               url: "{controller}/{action}/{JournalID}/{name}",
               defaults: new { controller = "Home", action = "Index", name = UrlParameter.Optional, JournalID = 1 }
            );

            routes.MapRoute(
                name: "Default1",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
            
            routes.MapRoute(
                name: "Default33",
                url: "{controller}/{action}/{ID}/{JournalID}/{name}/amp",
                defaults: new { controller = "News", action = "NewDetails", name = UrlParameter.Optional, JournalID = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Default44",
                url: "{controller}/{action}/{name}",
                defaults: new { controller = "Editors", action = "GateEditors", name = UrlParameter.Optional }
            );
        }
    }
}
