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
using CMS.Areas.CoreHandler.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Drawing;

namespace CMS.Areas.CoreHandler.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: MainProfile
        public ActionResult Index(string searchStr, ProfileVM vm = null)
        {
            Session["CurrentModule"] = "الملفات";
            Session["CurrentService"] = "إدارة الملفات";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            ProfileBusiness EB = new ProfileBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderBy(d => d.ProfileId), e => e.ProfName.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderBy(d => d.ProfileId), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/Profile/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

        public ActionResult ProfileSort(ProfileVM vm = null)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            ProfileBusiness EB = new ProfileBusiness(this.DbContext);

            var resList = EB.Load(e => e.Hide == 0, null).OrderBy(e => e.DisplayOrder);

            vm.orderedlst = resList.ToList();

            return View(vm);

        }

        [HttpPost]
        public ActionResult SaveProfileSort(string Ids, ProfileVM vm = null)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            ProfileBusiness msB = new ProfileBusiness(this.DbContext);
            var secArray = JArray.Parse(Ids).Select(x => (int)x).ToArray();
            var resList = msB.Load(e => e.Hide == 0, null).OrderBy(e => e.DisplayOrder);
            vm.orderedlst = resList.ToList();


            for (int i = 0; i < secArray.Count(); i++)
            {
                vm.Profile = vm.orderedlst.FirstOrDefault(e => e.ProfileId == secArray[i]);
                vm.Profile.DisplayOrder = i + 1;
                if (vm.Profile.Description == null)
                    vm.Profile.Description = "Description";
                if (vm.Profile.Keyword == null)
                    vm.Profile.Keyword = "Keywords";
                msB.Edit(vm.Profile);
                this.SaveChanges();
            }

            vm.orderedlst = vm.orderedlst.OrderBy(e => e.DisplayOrder).ToList();

            return View("~/Areas/CoreHandler/Views/Profile/ProfileSort.cshtml", vm);
        }


        // GET: MainProfile/Details/5
        public ActionResult Details(int? MProfileID)
        {
            ViewBag.Title = "تفاصيل الملف";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (MProfileID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfileBusiness EB = new ProfileBusiness(this.DbContext);
            ProfileVM vm = new ProfileVM();
            vm.Profile = EB.Load(MProfileID);
            if (vm.Profile.Hide != null)
                vm.SHide = Convert.ToBoolean(vm.Profile.Hide);
            if (vm.Profile == null)
            {
                return HttpNotFound();
            }

            ViewBag.RProfileImgPath = this.RelativeProfileImgPath;
            return View("~/Areas/CoreHandler/Views/Profile/Details.cshtml", vm);
        }

        // GET: MainProfile/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            ProfileVM vm = new ProfileVM();
            return View("~/Areas/CoreHandler/Views/Profile/Create.cshtml", vm);
        }

        // POST: MainProfile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProfileVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Profile>(vm.Profile, new string[] { "ProfName", "MainPictureName", "Hide", "Keyword", "Description" });
            if (ModelState.IsValid)
            {
                vm.Profile.Hide = Convert.ToByte(vm.SHide);
                ProfileBusiness eB = new ProfileBusiness(this.DbContext);

                var img = this.HttpContext.Request.Files["PMainPicture"];
                string picName = "";
                bool imgFlag = SaveProfileImage(ref picName, img);

                if (imgFlag)
                    vm.Profile.MainPictureName = picName;

                eB.Add(vm.Profile);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }
            return View("~/Areas/CoreHandler/Views/Profile/Create.cshtml", vm);
        }

        // GET: MainProfile/Edit/5
        public ActionResult Edit(int? MProfileID, ProfileVM vm = null)
        {
            if (MProfileID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            ProfileBusiness eB = new ProfileBusiness(this.DbContext);
            vm.Profile = eB.Load(MProfileID);
            if (vm.Profile.Hide != null)
                vm.SHide = Convert.ToBoolean(vm.Profile.Hide);
            if (vm.Profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.RProfileImgPath = this.RelativeProfileImgPath;
            return View("~/Areas/CoreHandler/Views/Profile/Edit.cshtml", vm);
        }

        // POST: MainProfile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProfileVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Profile>(vm.Profile, new string[] { "ProfileId", "ProfName", "MainPictureName", "Hide", "Keyword", "Description" });

            if (ModelState.IsValid)
            {
                vm.Profile.Hide = Convert.ToByte(vm.SHide);
                ProfileBusiness eB = new ProfileBusiness(this.DbContext);

                var img = this.HttpContext.Request.Files["PMainPicture"];
                string picName = "";
                bool imgFlag = SaveProfileImage(ref picName, img);

                if (imgFlag)
                    vm.Profile.MainPictureName = picName;

                eB.Edit(vm.Profile);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }

            ViewBag.RProfileImgPath = this.RelativeProfileImgPath;
            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            return View("~/Areas/CoreHandler/Views/Profile/Edit.cshtml", vm);

        }

        // GET: MainProfile/Delete/5
        public ActionResult Delete(int? MProfileID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (MProfileID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProfileBusiness eB = new ProfileBusiness(this.DbContext);
            ProfileVM vm = new ProfileVM();
            vm.Profile = eB.Load(MProfileID);
            if (vm.Profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.RProfileImgPath = this.RelativeProfileImgPath;
            return View("~/Areas/CoreHandler/Views/Profile/Delete.cshtml", vm);
        }

        // POST: MainProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int MProfileID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            ProfileBusiness eB = new ProfileBusiness(this.DbContext);
            ProfileVM vm = new ProfileVM();
            vm.Profile = eB.Load(MProfileID);
            eB.Delete(vm.Profile);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("Index");
        }

        private bool SaveProfileImage(ref string picName, HttpPostedFileBase img)
        {
            if (img == null || img.ContentLength <= 0)
                return false;
            string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            picName = "" + DateTimePattern + ".jpg";
            string filename = this.ProfileImgPath + picName;

            try
            {
                Stream strm = img.InputStream;
                Size lImgSize = new Size(this.NewsImgLargeWidth, this.NewsImgLargeHeight);
                Bitmap bitmap = new Bitmap(Image.FromStream(strm));

                //Save Original image with normal size without stretching
                SaveImage(ref bitmap, lImgSize, filename, false);

                return true;
            }
            catch (Exception ex)
            {
                string e = ex.ToString();
                return false;
            }
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}