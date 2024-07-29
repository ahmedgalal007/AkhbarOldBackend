using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Akhbar.DBContext;
using Akhbar.DBEntities;
using Akhbar.DBBusiness;
using PagedList;
using CMS.Areas.FrameWork.Controllers;
using CMS.Areas.FrameWork.ViewModels;
using System.Web.Routing;
using CMS.Areas.CoreHandler.Models;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Validation;

namespace CMS.Areas.CoreHandler.Controllers
{
    [SystemAuthorizationAttribute]
    public class UsersOpinionsController : BaseController
    {

        // GET: UsersOpinions
        public ActionResult Index(string searchStr, UsersOpinionVM vm = null)
        {
            Session["CurrentModule"] = "تعليقات الاخبار";
            Session["CurrentService"] = "التعليقات";
          

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UsersOpinionBusiness EB = new UsersOpinionBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;


            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderBy(d => d.CommentID), e => e.Subject.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderBy(d => d.CommentID), null, vm.page, PageSize, null);
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/UsersOpinions/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }
              

        // GET: UsersOpinions/Details/5
        public ActionResult Details(int? UsersOpinionId)
        {
            ViewBag.Title = "تفاصيل التعليق";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (UsersOpinionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersOpinionBusiness EB = new UsersOpinionBusiness(this.DbContext);
            UsersOpinionVM vm = new UsersOpinionVM();
            vm.UsersOpinion= EB.Load(UsersOpinionId);
           
            if (vm.UsersOpinion == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/UsersOpinions/Details.cshtml", vm);
        }


    
        // GET: UsersOpinions/Edit/5
        public ActionResult Edit(int? UsersOpinionId, UsersOpinionVM vm = null)
        {
            if (UsersOpinionId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UsersOpinionBusiness eB = new UsersOpinionBusiness(this.DbContext);
            vm.UsersOpinion = eB.Load(UsersOpinionId);
            
            if (vm.UsersOpinion == null)
            {
                return HttpNotFound();
            }
 
            return PartialView("~/Areas/CoreHandler/Views/UsersOpinions/Edit.cshtml", vm);
        }

        // POST: UsersOpinions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UsersOpinionVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<UsersOpinion>(vm.UsersOpinion, new string[] {"UsersOpinionID", "UserName", "Subject", "messagebody", "Approved", "OperationalUser" });
            if (ModelState.IsValid)
            {
                UsersOpinionBusiness eB = new UsersOpinionBusiness(this.DbContext);
                eB.Edit(vm.UsersOpinion);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }
            ViewBag.alert = GetMessage(TempData["Action"].ToString());
            
            return View("~/Areas/CoreHandler/Views/UsersOpinions/Edit.cshtml", vm);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
