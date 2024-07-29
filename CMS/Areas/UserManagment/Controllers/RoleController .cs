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
using CMS.Areas.UserManagment.Models;

namespace CMS.Areas.UserManagment.Controllers
{
    public class RoleController : BaseController
    {



        // GET: Roles
        public ActionResult Index(string searchStr, RoleVM vm = null)
        {
            Session["CurrentModule"] = "المستخدمين";
            Session["CurrentService"] = "المجموعات";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            RoleBusiness EB = new RoleBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderBy(d => d.NewID), e => e.RoleName.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderBy(d => d.NewID), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/UserManagment/Views/Role/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }


        // GET: Roles/Details/5
        public ActionResult Details(int? RoleID)
        {
            ViewBag.Title = "تفاصيل المجموعة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (RoleID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleBusiness EB = new RoleBusiness(this.DbContext);
            RoleVM vm = new RoleVM();
            vm.Role= EB.Load(RoleID);
           
            if (vm.Role == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/UserManagment/Views/Role/Details.cshtml", vm);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
           
            RoleVM vm = new RoleVM();
            return View("~/Areas/UserManagment/Views/Role/Create.cshtml",vm);
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoleVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Role>(vm.Role, new string[] { "RoleName" });
           if (ModelState.IsValid)
            {
                RoleBusiness eB = new RoleBusiness(this.DbContext);
                eB.Add(vm.Role);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }
            
            return View("~/Areas/UserManagment/Views/Role/Create.cshtml", vm);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? RoleID, RoleVM vm = null)
        {
            if (RoleID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            RoleBusiness eB = new RoleBusiness(this.DbContext);
            vm.Role = eB.Load(RoleID);
           
            if (vm.Role == null)
            {
                return HttpNotFound();
            }

            return PartialView("~/Areas/UserManagment/Views/Role/Edit.cshtml", vm);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoleVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Role>(vm.Role, new string[] { "NewID", "RoleName"});
            if (ModelState.IsValid)
            {
                RoleBusiness eB = new RoleBusiness(this.DbContext);
                eB.Edit(vm.Role);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }
            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            ViewBag.Sec_ID = new SelectList(secB.Load(null, null), "SectionID", "SecTitle");
            return View("~/Areas/UserManagment/Views/Role/Edit.cshtml", vm);

        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? RoleID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (RoleID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleBusiness eB = new RoleBusiness(this.DbContext);
            RoleVM vm = new RoleVM();
            vm.Role = eB.Load(RoleID);
            if (vm.Role == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/UserManagment/Views/Role/Delete.cshtml", vm); 
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int RoleID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            RoleBusiness eB = new RoleBusiness(this.DbContext);
            RoleVM vm = new RoleVM();
            vm.Role = eB.Load(RoleID);
            eB.Delete(vm.Role);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }



        public ActionResult MangeRoleServices(RoleVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (vm.Role != null)
            {
                RoleServiceBusiness rSB = new RoleServiceBusiness(this.DbContext);
                Expression<Func<RoleServices, bool>> _filter = e => e.RoleId == vm.Role.NewID;
                vm.roleServiceLst = rSB.Load(_filter, null);
            }
            UMServicesBusiness serviceB = new UMServicesBusiness(this.DbContext);
            ViewBag.serviceID = new SelectList(serviceB.Load(e=>e.ModuleId!=13 /*&& e.ModuleId != 10*/ && e.ModuleId != 15 && e.ServiceHide==false, null), "ServiceId", "ServiceName");

            return PartialView("~/Areas/UserManagment/Views/Role/MangeRoleServices.cshtml", vm);
        }

        public ActionResult AddToServices(RoleVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            vm.RoleService.RoleId = vm.Role.NewID;
            RoleServiceBusiness rSB = new RoleServiceBusiness(this.DbContext);
            UMServicesBusiness sB = new UMServicesBusiness(this.DbContext);
            UMServices currentServ = sB.Load(vm.RoleService.ServiceId);

            // Add Service
            Func<RoleServices, bool> expr1 = (x => x.ServiceId == vm.RoleService.ServiceId);
            Func<RoleServices, bool> expr2 = (x => x.RoleId == vm.RoleService.RoleId);
            Func<RoleServices, bool> expr1ANDexpr2 = (x => expr1(x) && expr2(x));
            rSB.AddIfNotExists(vm.RoleService, expr1ANDexpr2);
            this.SaveChanges();

            //Add BaseError Service
            RoleServices tempRoleServices = new RoleServices();
            tempRoleServices.RoleId = vm.RoleService.RoleId;
            tempRoleServices.ServiceId = 124;
            expr1 = (x => x.ServiceId == 124); 
            expr1ANDexpr2 = (x => expr1(x) && expr2(x));
            rSB.AddIfNotExists(tempRoleServices, expr1ANDexpr2);
            this.SaveChanges();

            //Add SharedMenu Service
            tempRoleServices = new RoleServices();
            tempRoleServices.RoleId = vm.RoleService.RoleId;
            tempRoleServices.ServiceId = 125;
            expr1 = (x => x.ServiceId == 125); 
            expr1ANDexpr2 = (x => expr1(x) && expr2(x));
            rSB.AddIfNotExists(tempRoleServices, expr1ANDexpr2);
            this.SaveChanges();

            //Add UploadImages Service
            tempRoleServices = new RoleServices();
            tempRoleServices.RoleId = vm.RoleService.RoleId;
            tempRoleServices.ServiceId = 112;
            expr1 = (x => x.ServiceId == 112);
            expr1ANDexpr2 = (x => expr1(x) && expr2(x));
            rSB.AddIfNotExists(tempRoleServices, expr1ANDexpr2);
            this.SaveChanges();

            //Add UploadFiles Service
            tempRoleServices = new RoleServices();
            tempRoleServices.RoleId = vm.RoleService.RoleId;
            tempRoleServices.ServiceId = 146;
            expr1 = (x => x.ServiceId == 146);
            expr1ANDexpr2 = (x => expr1(x) && expr2(x));
            rSB.AddIfNotExists(tempRoleServices, expr1ANDexpr2);
            this.SaveChanges();

            //Add ArchiveImage Service
            tempRoleServices = new RoleServices();
            tempRoleServices.RoleId = vm.RoleService.RoleId;
            tempRoleServices.ServiceId = 150;
            expr1 = (x => x.ServiceId == 150);
            expr1ANDexpr2 = (x => expr1(x) && expr2(x));
            rSB.AddIfNotExists(tempRoleServices, expr1ANDexpr2);
            this.SaveChanges();

            tempRoleServices = new RoleServices();
            tempRoleServices.RoleId = vm.RoleService.RoleId;
            tempRoleServices.ServiceId = 151;
            expr1 = (x => x.ServiceId == 151);
            expr1ANDexpr2 = (x => expr1(x) && expr2(x));
            rSB.AddIfNotExists(tempRoleServices, expr1ANDexpr2);
            this.SaveChanges();

            tempRoleServices = new RoleServices();
            tempRoleServices.RoleId = vm.RoleService.RoleId;
            tempRoleServices.ServiceId = 152;
            expr1 = (x => x.ServiceId == 152);
            expr1ANDexpr2 = (x => expr1(x) && expr2(x));
            rSB.AddIfNotExists(tempRoleServices, expr1ANDexpr2);
            this.SaveChanges();


            // Add Related Services

            if (currentServ.RelatedService != "0")
            {
                string[] relatedServices = currentServ.RelatedService.Split(',');
                if (relatedServices.Count() > 0)
                {
                    int rSId;
                    foreach (var item in relatedServices)
                    {
                        rSId = int.Parse(item);
                        tempRoleServices = new RoleServices();
                        tempRoleServices.RoleId = vm.RoleService.RoleId;
                        tempRoleServices.ServiceId = rSId;
                        expr1 = (x => x.ServiceId == rSId);
                        expr1ANDexpr2 = (x => expr1(x) && expr2(x));
                        rSB.AddIfNotExists(tempRoleServices, expr1ANDexpr2);
                        this.SaveChanges();
                    }
                }
            }
           
            return MangeRoleServices(vm);
        }

        public ActionResult DeleteFromServices(RoleVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            vm.RoleService.RoleId = vm.Role.NewID;
            RoleServiceBusiness rSB = new RoleServiceBusiness(this.DbContext);
            rSB.Delete(vm.RoleService);
            this.SaveChanges();

            UMServicesBusiness sB = new UMServicesBusiness(this.DbContext);
            UMServices currentServ = sB.Load(vm.RoleService.ServiceId);
            RoleServices tempRoleServices = new RoleServices();
            if (currentServ.RelatedService != "0")
            {
                string[] relatedServices = currentServ.RelatedService.Split(',');
                if (relatedServices.Count() > 0)
                {
                    int rSId;
                    foreach (var item in relatedServices)
                    {
                        rSId = int.Parse(item);
                        tempRoleServices = new RoleServices();
                        tempRoleServices.RoleId = vm.RoleService.RoleId;
                        tempRoleServices.ServiceId = rSId;
                        rSB.Delete(tempRoleServices);
                        this.SaveChanges();
                    }
                }
            }

            return MangeRoleServices(vm);
        }
    }
}
