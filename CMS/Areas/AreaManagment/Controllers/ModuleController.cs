using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Akhbar.DBEntities;
using Domain.Akhbar.Contexts;
using Domain.Akhbar.DBBusiness;
using PagedList;
using CMS.Areas.FrameWork.Controllers;
using CMS.Areas.FrameWork.ViewModels;
using System.Web.Routing;
using CMS.Areas.AreaManagment.Models;

namespace CMS.Areas.AreaManagment.Controllers
{
    public class ModuleController : BaseController
    {



        // GET: Roles
        public ActionResult Index(string searchStr, ModuleVM vm = null)
        {
            Session["CurrentModule"] = "الادارة";
            Session["CurrentService"] = "القطاعات";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UMModulesBusiness EB = new UMModulesBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderBy(d => d.ModuleId), e => e.ModuleName.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderBy(d => d.ModuleId), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/AreaManagment/Views/Module/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

        


        // GET: Roles/Details/5
        public ActionResult Details(int? ModuleID)
        {
            ViewBag.Title = "تفاصيل القطاع";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (ModuleID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UMModulesBusiness EB = new UMModulesBusiness(this.DbContext);
            ModuleVM vm = new ModuleVM();
            vm.Module= EB.Load(ModuleID);
           
            if (vm.Module == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/AreaManagment/Views/Module/Details.cshtml", vm);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
           
            ModuleVM vm = new ModuleVM();
            return View("~/Areas/AreaManagment/Views/Module/Create.cshtml",vm);
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModuleVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<UMModules>(vm.Module, new string[] { "ModuleName", "AreaName" });
           if (ModelState.IsValid)
            {
                UMModulesBusiness eB = new UMModulesBusiness(this.DbContext);
                eB.Add(vm.Module);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }
            
            return View("~/Areas/AreaManagment/Views/Module/Create.cshtml", vm);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? ModuleID, ModuleVM vm = null)
        {
            if (ModuleID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UMModulesBusiness eB = new UMModulesBusiness(this.DbContext);
            vm.Module = eB.Load(ModuleID);
           
            if (vm.Module == null)
            {
                return HttpNotFound();
            }

            return PartialView("~/Areas/AreaManagment/Views/Module/Edit.cshtml", vm);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModuleVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<UMModules>(vm.Module, new string[] { "ModuleId", "ModuleName", "AreaName" });
            if (ModelState.IsValid)
            {
                UMModulesBusiness eB = new UMModulesBusiness(this.DbContext);
                eB.Edit(vm.Module);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }
          
            return View("~/Areas/AreaManagment/Views/Module/Edit.cshtml", vm);

        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? ModuleID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (ModuleID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UMModulesBusiness eB = new UMModulesBusiness(this.DbContext);
            ModuleVM vm = new ModuleVM();
            vm.Module = eB.Load(ModuleID);
            if (vm.Module == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/AreaManagment/Views/Module/Delete.cshtml", vm); 
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ModuleID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UMModulesBusiness eB = new UMModulesBusiness(this.DbContext);
            ModuleVM vm = new ModuleVM();
            vm.Module = eB.Load(ModuleID);
            eB.Delete(vm.Module);
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
