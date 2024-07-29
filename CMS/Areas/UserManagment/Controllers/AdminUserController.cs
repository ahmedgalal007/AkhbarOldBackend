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
using CMS.Areas.UserManagment.Models;
using System.Data.Entity.Validation;
using System.IO;
using SimpleHashing;

namespace CMS.Areas.UserManagment.Controllers
{
    public class AdminUserController : BaseController
    {

        // GET: AdminUser
        public ActionResult Index(string searchStr, AdminUserVM vm = null)
        {
            Session["CurrentModule"] = "المستخدمين";
            Session["CurrentService"] = "المستخدمين";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            AdminUserBusiness EB = new AdminUserBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderByDescending(d => d.UserID), e => e.FullName.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderByDescending(d => d.UserID), null, vm.page, PageSize, null);
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }
            ViewBag.UsersImgPath = this.RelativeUsersImgPath;
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/UserManagment/Views/AdminUser/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }  


        // GET: AdminUser/Details/5
        public ActionResult Details(int? adminUserID)
        {
            ViewBag.Title = "تفاصيل المجموعة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (adminUserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminUserBusiness EB = new AdminUserBusiness(this.DbContext);
            AdminUserVM vm = new AdminUserVM();
            vm.adminUser = EB.Load(adminUserID);
            if (vm.adminUser.Type != null)
                vm.ManagerFlag = Convert.ToBoolean(vm.adminUser.Type);
            if (vm.adminUser.Active != null)
                vm.ActiveFlag = Convert.ToBoolean(vm.adminUser.Active);
            if (vm.adminUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersImgPath = this.RelativeUsersImgPath;
            return PartialView("~/Areas/UserManagment/Views/AdminUser/Details.cshtml", vm);
        }

        // GET: AdminUser/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            AdminUserVM vm = new AdminUserVM();
            return View("~/Areas/UserManagment/Views/AdminUser/Create.cshtml", vm);
        }

        // POST: AdminUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminUserVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<AdminUser>(vm.adminUser, new string[] { "FullName", "UserName", "Password", "Type", "Telephone", "UserPhoto", "Active" });
            if (ModelState.IsValid)
            {
                var img = this.HttpContext.Request.Files["AdminUser_Image"];
                if (img != null && img.ContentLength > 0)
                {
                    string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string fileName = DateTimePattern + ".jpg";
                    string filePath = this.UsersImgPath + fileName;
                    img.SaveAs(filePath);
                    vm.adminUser.UserPhoto = fileName;
                }
                vm.adminUser.Type = Convert.ToInt16(vm.ManagerFlag);
                vm.adminUser.Active = vm.ActiveFlag;

                SimpleHash hash = new SimpleHash();
                vm.adminUser.Password = hash.Compute(vm.adminUser.Password);

                AdminUserBusiness eB = new AdminUserBusiness(this.DbContext);
                eB.Add(vm.adminUser);
                this.SaveChanges();

                ByLineBusiness blB = new ByLineBusiness(this.DbContext);
                ByLine byline = new ByLine();
                byline.ByLineName = vm.adminUser.FullName.Trim();
                byline.Active = vm.adminUser.Active;
                blB.AddIfNotExists(byline, x => x.ByLineName == vm.adminUser.FullName);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }

            return View("~/Areas/UserManagment/Views/AdminUser/Create.cshtml", vm);
        }

        // GET: AdminUser/Edit/5
        public ActionResult Edit(int? adminUserID, AdminUserVM vm = null)
        {
            if (adminUserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            AdminUserBusiness eB = new AdminUserBusiness(this.DbContext);
            vm.adminUser = eB.Load(adminUserID);
            if (vm.adminUser.Type != null)
                vm.ManagerFlag = Convert.ToBoolean(vm.adminUser.Type);
            if (vm.adminUser.Active != null)
                vm.ActiveFlag = Convert.ToBoolean(vm.adminUser.Active);
            if (vm.adminUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersImgPath = this.RelativeUsersImgPath;

            NationalitesBusiness nB = new NationalitesBusiness(this.DbContext);
            ViewBag.Nationalities = new SelectList(nB.Load(null, null), "CountryID", "Name");

            return PartialView("~/Areas/UserManagment/Views/AdminUser/Edit.cshtml", vm);
        }

        // POST: AdminUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdminUserVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<AdminUser>(vm.adminUser, new string[] { "UserID", "FullName", "UserName", "Password", "Type", "Telephone", "UserPhoto", "Active", "UserEmail", "BirthDay", "BirthPlace", "UserTwitter", "UserFB", "Nationality", "EducationalQualification", "NickName", "AboutYourSelf" });
            if (ModelState.IsValid)
            {
                var img = this.HttpContext.Request.Files["AdminUser_Image"];
                if (img != null && img.ContentLength > 0)
                {
                    string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string fileName= DateTimePattern + ".jpg";
                    string filePath = this.UsersImgPath + fileName;
                    img.SaveAs(filePath);
                    vm.adminUser.UserPhoto = fileName;
                }

                vm.adminUser.Type = Convert.ToInt16(vm.ManagerFlag);
                vm.adminUser.Active = vm.ActiveFlag;

                SimpleHash hash = new SimpleHash();
                vm.adminUser.Password = hash.Compute(vm.adminUser.Password);

                AdminUserBusiness eB = new AdminUserBusiness(this.DbContext);
                eB.Edit(vm.adminUser);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }
            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            ViewBag.UsersImgPath = this.RelativeUsersImgPath;

            NationalitesBusiness nB = new NationalitesBusiness(this.DbContext);
            ViewBag.Nationalities = new SelectList(nB.Load(null, null), "CountryID", "Name");

            return View("~/Areas/UserManagment/Views/AdminUser/Edit.cshtml", vm);

        }

        // GET: AdminUser/Delete/5
        public ActionResult Delete(int? adminUserID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (adminUserID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminUserBusiness eB = new AdminUserBusiness(this.DbContext);
            AdminUserVM vm = new AdminUserVM();
            vm.adminUser = eB.Load(adminUserID);
            if (vm.adminUser == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/UserManagment/Views/AdminUser/Delete.cshtml", vm);
        }

        // POST: AdminUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int adminUserID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            AdminUserBusiness eB = new AdminUserBusiness(this.DbContext);
            AdminUserVM vm = new AdminUserVM();
            vm.adminUser = eB.Load(adminUserID);
            eB.Delete(vm.adminUser);
            this.SaveChanges();

            ByLineBusiness blB = new ByLineBusiness(this.DbContext);
           List<ByLine> bylines = blB.Load(e=>e.ByLineName==vm.adminUser.FullName);
            foreach (var item in bylines)
            {
                item.Active = false;
                blB.Edit(item);
                this.SaveChanges();
            }
            
            TempData["Action"] = "Delete";

            ViewBag.UsersImgPath = this.RelativeUsersImgPath;
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }    

        public ActionResult MangeUserRoles(AdminUserVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (vm.adminUser != null && vm.adminUser.UserID != 0)
            {
                UserRoleBusiness urB = new UserRoleBusiness(this.DbContext);
                Expression<Func<UserRole, bool>> _filter = e => e.AdminUserID == vm.adminUser.UserID;
                vm.userRolesLst = urB.Load(_filter, null);
            }
            RoleBusiness roleB = new RoleBusiness(this.DbContext);
            ViewBag.roleID = new SelectList(roleB.Load(null, null), "NewID", "RoleName");

            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            ViewBag.Sec_ID = new SelectList(secB.Load(null, null), "SectionID", "SecTitle");

            return PartialView("~/Areas/UserManagment/Views/AdminUser/MangeUserRoles.cshtml", vm);
        }

        public ActionResult AddToRoles(AdminUserVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            vm.UserRole.AdminUserID = vm.adminUser.UserID;
            vm.UserRole.JournalID = 1;
            UserRoleBusiness urB = new UserRoleBusiness(this.DbContext);
            if (vm.UserRole.SectionID == null)
                vm.UserRole.SectionID = 0;
            urB.Add(vm.UserRole);
            this.SaveChanges();
            return MangeUserRoles(vm);
        }

        public ActionResult DeleteFromRoles(AdminUserVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            vm.UserRole.AdminUserID = vm.adminUser.UserID;
            UserRoleBusiness urB = new UserRoleBusiness(this.DbContext);
            urB.Delete(vm.UserRole);
            this.SaveChanges();
            return MangeUserRoles(vm);
        }


        // GET: AdminUser/Edit/5
        public ActionResult UpdateMyData()
        {
            AdminUserVM vm = new AdminUserVM();   
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            AdminUserBusiness eB = new AdminUserBusiness(this.DbContext);
            vm.adminUser = eB.Load(LoginUserID);
          
            if (vm.adminUser == null)
            {
                return HttpNotFound();
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }
            ViewBag.UsersImgPath = this.RelativeUsersImgPath;

            NationalitesBusiness nB = new NationalitesBusiness(this.DbContext);
            ViewBag.Nationalities = new SelectList(nB.Load(null, null), "CountryID", "Name");
            return View("~/Areas/UserManagment/Views/AdminUser/UpdateMyData.cshtml", vm);
        }

        // POST: AdminUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMyData(AdminUserVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<AdminUser>(vm.adminUser, new string[] { "UserID", "UserName", "Password", "Telephone", "UserPhoto", "UserEmail", "BirthDay", "BirthPlace", "UserTwitter", "UserFB", "Nationality", "EducationalQualification", "NickName", "AboutYourSelf" });
            if (ModelState.IsValid)
            {
                var img = this.HttpContext.Request.Files["AdminUser_Image"];
                if (img != null && img.ContentLength > 0)
                {
                    string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                    string fileName = DateTimePattern + ".jpg";
                    string filePath = this.UsersImgPath + fileName;
                    img.SaveAs(filePath);
                    vm.adminUser.UserPhoto = fileName;

                }

                SimpleHash hash = new SimpleHash();
                vm.adminUser.Password = hash.Compute(vm.adminUser.Password);

                AdminUserBusiness eB = new AdminUserBusiness(this.DbContext);
                eB.Edit(vm.adminUser);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }

            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            NationalitesBusiness nB = new NationalitesBusiness(this.DbContext);
            ViewBag.Nationalities = new SelectList(nB.Load(null, null), "CountryID", "Name");
            return View("~/Areas/UserManagment/Views/AdminUser/UpdateMyData.cshtml", vm);
        }


        [HttpPost]
        public ActionResult JoinDeskShift(TempDeskShiftVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TempDeskShiftBusiness tempDeskBuss = new TempDeskShiftBusiness(this.DbContext);
            vm.lst = tempDeskBuss.Load(e => e.WaitingFlag == true, null).OrderBy(e => e.OrderID).ToList();
            vm.tempDeskShift = new TempDeskShift();
            vm.tempDeskShift.UserId = LoginUserID.Value;
            tempDeskBuss.AddIfNotExists(vm.tempDeskShift, x => x.UserId == vm.tempDeskShift.UserId);
            this.SaveChanges();
            this.SaveDeskLog(true);

            foreach (var item in vm.lst)
            {
                item.WaitingFlag = false;
                tempDeskBuss.Edit(item);
                this.SaveChanges();
            }
            vm.waitingTempDeskShift = tempDeskBuss.Load(LoginUserID.Value);
            vm.waitingTempDeskShift.WaitingFlag = true;
            tempDeskBuss.Edit(vm.waitingTempDeskShift);
            this.SaveChanges();

            //Session["DeskUser"]= "JoinDesk";
            TempData["Action"] = "JoinDesk";
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        [HttpPost]
        public ActionResult LeaveDeskShift(TempDeskShiftVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TempDeskShiftBusiness tempDeskBuss = new TempDeskShiftBusiness(this.DbContext);
            vm.tempDeskShift=tempDeskBuss.Load(LoginUserID);
            if (vm.tempDeskShift != null)
            {
                vm.lst = tempDeskBuss.Load(null, null).OrderBy(e => e.OrderID).ToList();

                for (int i = 0; i < vm.lst.Count; i++)
                {
                    if (vm.tempDeskShift.UserId == vm.lst[i].UserId)
                    {
                        if (vm.lst[i] == vm.lst.Last())
                        {
                            vm.lst[0].WaitingFlag = true;
                            tempDeskBuss.Edit(vm.lst[0]);
                        }
                        else
                        {
                            vm.lst[i + 1].WaitingFlag = true;
                            tempDeskBuss.Edit(vm.lst[i + 1]);
                        }
                        this.SaveChanges();
                    }
                }


                tempDeskBuss.Delete(vm.tempDeskShift);
                this.SaveChanges();
                this.SaveDeskLog(false);
            }

            //Session["DeskUser"] = "LeftDesk";
            TempData["Action"] = "LeftDesk";
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }



    }


}
