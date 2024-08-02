using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.Contexts;
using Domain.Akhbar.DBEntities;
using Domain.Akhbar.DBBusiness;
using PagedList;
using CMS.Areas.FrameWork.Controllers;
using CMS.Areas.FrameWork.ViewModels;
using System.Web.Routing;
using CMS.Areas.CoreHandler.Models;

namespace CMS.Areas.CoreHandler.Controllers
{

    public class GalleryTypeController : BaseController
    {
        // GET: GalleryType
        public ActionResult Index(string searchStr, GalleryTypeVM vm = null)
        {
            Session["CurrentModule"] = "البومات الصور";
            Session["CurrentService"] = "انواع الالبومات";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            GalleryTypeBusiness EB = new GalleryTypeBusiness(this.DbContext);
      
            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderBy(d => d.GalleryTypeId), e => e.GalleryTypeName.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderBy(d => d.GalleryTypeId), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/GalleryType/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

      
        // GET: GalleryType/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            GalleryTypeVM vm = new GalleryTypeVM();
            return View("~/Areas/CoreHandler/Views/GalleryType/Create.cshtml", vm);
        }

        // POST: GalleryType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GalleryTypeVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<GalleryType>(vm.GalleryType, new string[] { "GalleryTypeName" });
            if (ModelState.IsValid)
            {
                GalleryTypeBusiness eB = new GalleryTypeBusiness(this.DbContext);
                eB.Add(vm.GalleryType);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }

            return View("~/Areas/CoreHandler/Views/GalleryType/Create.cshtml", vm);
        }

        // GET: GalleryType/Edit/5
        public ActionResult Edit(string GalleryTypeName, GalleryTypeVM vm = null)
        {
            if (GalleryTypeName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            GalleryTypeBusiness gTB = new GalleryTypeBusiness(this.DbContext);
            vm.GalleryType = gTB.Load(GalleryTypeName);

            if (vm.GalleryType == null)
            {
                return HttpNotFound();
            }

            return PartialView("~/Areas/CoreHandler/Views/GalleryType/Edit.cshtml", vm);
        }

        // POST: GalleryType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GalleryTypeVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<GalleryType>(vm.GalleryType, new string[] { "GalleryTypeId", "GalleryTypeName" });
            if (ModelState.IsValid)
            {
                GalleryTypeBusiness eB = new GalleryTypeBusiness(this.DbContext);
                eB.Edit(vm.GalleryType);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }

            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            return View("~/Areas/CoreHandler/Views/GalleryType/Edit.cshtml", vm);

        }

        // GET: GalleryType/Delete/5
        public ActionResult Delete(string GalleryTypeName)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (GalleryTypeName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GalleryTypeBusiness eB = new GalleryTypeBusiness(this.DbContext);
            GalleryTypeVM vm = new GalleryTypeVM();
            vm.GalleryType = eB.Load(GalleryTypeName);
            if (vm.GalleryType == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/GalleryType/Delete.cshtml", vm);
        }

        // POST: GalleryType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string GalleryTypeName)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            GalleryTypeBusiness eB = new GalleryTypeBusiness(this.DbContext);
            GalleryTypeVM vm = new GalleryTypeVM();
            vm.GalleryType = eB.Load(GalleryTypeName);
            eB.Delete(vm.GalleryType);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
