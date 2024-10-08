﻿using System.Web.Mvc;

namespace CMS.Areas.FrameWork
{
    public class FrameWorkAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FrameWork";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FrameWork_default",
                "FrameWork/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}