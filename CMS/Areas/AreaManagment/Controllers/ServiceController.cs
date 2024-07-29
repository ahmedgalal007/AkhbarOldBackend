using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Akhbar.DBContext;
using Domain.Akhbar.DBEntities;
using Domain.Akhbar.DBBusiness;
using PagedList;
using CMS.Areas.FrameWork.Controllers;
using CMS.Areas.FrameWork.ViewModels;
using System.Web.Routing;
using CMS.Areas.AreaManagment.Models;

namespace CMS.Areas.AreaManagment.Controllers
{
    public class ServiceController : BaseController
    {



        // GET: Roles
        public ActionResult Index(string searchStr, ServiceVM vm = null)
        {
            Session["CurrentModule"] = "الادارة";
            Session["CurrentService"] = "خدمات القطاعات";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UMServicesBusiness EB = new UMServicesBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderBy(d => d.ServiceId), e => e.ServiceName.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderBy(d => d.ServiceId), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/AreaManagment/Views/Service/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

      


        // GET: Roles/Details/5
        public ActionResult Details(int? ServiceID)
        {
            ViewBag.Title = "تفاصيل الخدمة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (ServiceID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UMServicesBusiness EB = new UMServicesBusiness(this.DbContext);
            ServiceVM vm = new ServiceVM();
            vm.Service= EB.Load(ServiceID);
            if (vm.Service.ModuleId != null)
            {
                vm.Service.UMModule = new UMModules();
                UMModulesBusiness mB = new UMModulesBusiness(this.DbContext);
                vm.Service.UMModule = mB.Load(vm.Service.ModuleId);
                //vm.editor.Section.SectionID = vm.editor.SectionID ?? 0;
            }
            if (vm.Service == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/AreaManagment/Views/Service/Details.cshtml", vm);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UMModulesBusiness modB = new UMModulesBusiness(this.DbContext);
            ViewBag.mod_ID = new SelectList(modB.Load(null, null), "ModuleId", "ModuleName");
            ServiceVM vm = new ServiceVM();
            return View("~/Areas/AreaManagment/Views/Service/Create.cshtml",vm);
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiceVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<UMServices>(vm.Service, new string[] { "ServiceName", "ModuleId", "ControllerName" });
           if (ModelState.IsValid)
            {
                UMServicesBusiness eB = new UMServicesBusiness(this.DbContext);
                vm.Service.ActionName = "Index";
                eB.Add(vm.Service);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }
            UMModulesBusiness modB = new UMModulesBusiness(this.DbContext);
            ViewBag.mod_ID = new SelectList(modB.Load(null, null), "ModuleId", "ModuleName");
            return View("~/Areas/AreaManagment/Views/Service/Create.cshtml", vm);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? ServiceID, ServiceVM vm = null)
        {
            if (ServiceID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UMServicesBusiness eB = new UMServicesBusiness(this.DbContext);
            vm.Service = eB.Load(ServiceID);
           
            if (vm.Service == null)
            {
                return HttpNotFound();
            }

            UMModulesBusiness modB = new UMModulesBusiness(this.DbContext);
            ViewBag.mod_ID = new SelectList(modB.Load(null, null), "ModuleId", "ModuleName");
            return PartialView("~/Areas/AreaManagment/Views/Service/Edit.cshtml", vm);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ServiceVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<UMServices>(vm.Service, new string[] { "ServiceID", "ServiceName", "ModuleId", "ControllerName", "ActionName" });
            if (ModelState.IsValid)
            {
                UMServicesBusiness eB = new UMServicesBusiness(this.DbContext);
                vm.Service.ActionName = "Index";
                eB.Edit(vm.Service);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }
            UMModulesBusiness modB = new UMModulesBusiness(this.DbContext);
            ViewBag.mod_ID = new SelectList(modB.Load(null, null), "ModuleId", "ModuleName");
            return View("~/Areas/AreaManagment/Views/Service/Edit.cshtml", vm);

        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? ServiceID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (ServiceID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UMServicesBusiness eB = new UMServicesBusiness(this.DbContext);
            ServiceVM vm = new ServiceVM();
            vm.Service = eB.Load(ServiceID);
            if (vm.Service == null)
            {
                return HttpNotFound();
            }
            vm.Service.UMModule = new UMModules();
            UMModulesBusiness mB = new UMModulesBusiness(this.DbContext);
            vm.Service.UMModule = mB.Load(vm.Service.ModuleId);

            return PartialView("~/Areas/AreaManagment/Views/Service/Delete.cshtml", vm); 
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ServiceID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            UMServicesBusiness eB = new UMServicesBusiness(this.DbContext);
            ServiceVM vm = new ServiceVM();
            vm.Service = eB.Load(ServiceID);
            eB.Delete(vm.Service);
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
