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
using System.Data.Entity.Validation;
using System.IO;
using System.Web.Helpers;
using System.Drawing;

namespace CMS.Areas.CoreHandler.Controllers
{
   
    public class GalleryController : BaseController
    {
        // GET: Gallery
        public ActionResult Index(string searchStr, GalleryVM vm = null)
        {
            Session["CurrentModule"] = "البومات الصور";
            Session["CurrentService"] = "الالبومات";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            GalleryBusiness gb = new GalleryBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = gb.Load(q => q.Take(100).OrderByDescending(d => d.GalleryID), e => e.Title.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = gb.Load(q => q.Take(100).OrderByDescending(d => d.GalleryID), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/Gallery/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }


        // GET: Gallery/Details/5
        public ActionResult Details(int? MGalleryID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (MGalleryID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GalleryBusiness gb = new GalleryBusiness(this.DbContext);
            GalleryVM vm = new GalleryVM();
            vm.Gallery = gb.Load(MGalleryID);
            if (vm.Gallery.IsHome != null)
                vm.HomePageFlag = Convert.ToBoolean(vm.Gallery.IsHome);
           
            if (vm.Gallery == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Gallery/Details.cshtml", vm);
        }

        // GET: Gallery/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            GalleryTypeBusiness gTB = new GalleryTypeBusiness(this.DbContext);
            ViewBag.GalleryTypes = new SelectList(gTB.Load(null, null), "GalleryTypeId", "GalleryTypeName");
            GalleryVM vm = new GalleryVM();
            return View("~/Areas/CoreHandler/Views/Gallery/Create.cshtml",vm);
        }

        // POST: Gallery/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GalleryVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Gallery>(vm.Gallery, new string[] { "Title", "Description", "Keywords", "GDate", "IsHome", "GalleryType", "JournalID" });

            vm.Gallery.IsHome = vm.HomePageFlag;
            vm.Gallery.GDate = DateTime.Now;
            vm.Gallery.JournalID = 1;
            GalleryBusiness gb = new GalleryBusiness(this.DbContext);
            gb.Add(vm.Gallery);
            this.SaveChanges();
            this.DbContext.Entry(vm.Gallery).GetDatabaseValues();
            SaveGalleryImages(vm, Request.Files);

            TempData["Action"] = "Create";

            return RedirectToAction("Index");
        }

        // GET: Gallery/Edit/5
        public ActionResult Edit(int? MGalleryID, GalleryVM vm = null)
        {
            if (MGalleryID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            GalleryBusiness eB = new GalleryBusiness(this.DbContext);
            vm.Gallery = eB.Load(MGalleryID);
            if (vm.Gallery.IsHome != null)
                vm.HomePageFlag = Convert.ToBoolean(vm.Gallery.IsHome);
            
            if (vm.Gallery == null)
            {
                return HttpNotFound();
            }
            GalleryTypeBusiness gTB = new GalleryTypeBusiness(this.DbContext);
            ViewBag.GalleryTypes = new SelectList(gTB.Load(null, null), "GalleryTypeId", "GalleryTypeName");
            return PartialView("~/Areas/CoreHandler/Views/Gallery/Edit.cshtml", vm);
        }

        // POST: Gallery/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GalleryVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Gallery>(vm.Gallery, new string[] { "GalleryID", "SecTitle", "Hide", "WeeklySection", "Description" });
          
            if (ModelState.IsValid)
            {
                vm.Gallery.IsHome = vm.HomePageFlag;
              
                GalleryBusiness eB = new GalleryBusiness(this.DbContext);
                eB.Edit(vm.Gallery);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }

            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            GalleryTypeBusiness gTB = new GalleryTypeBusiness(this.DbContext);
            ViewBag.GalleryTypes = new SelectList(gTB.Load(null, null), "GalleryTypeId", "GalleryTypeName");
            return View("~/Areas/CoreHandler/Views/Gallery/Edit.cshtml", vm);

        }

        // GET: Gallery/Delete/5
        public ActionResult Delete(int? MGalleryID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (MGalleryID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GalleryBusiness eB = new GalleryBusiness(this.DbContext);
            GalleryVM vm = new GalleryVM();
            vm.Gallery = eB.Load(MGalleryID);
            if (vm.Gallery == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Gallery/Delete.cshtml", vm);
        }

        // POST: Gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int MGalleryID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            GalleryBusiness eB = new GalleryBusiness(this.DbContext);
            GalleryVM vm = new GalleryVM();
            vm.Gallery = eB.Load(MGalleryID);
            eB.Delete(vm.Gallery);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public ActionResult ManageGalleryPhotos(int? GalleryId)
        {
            GalleryVM vm = new GalleryVM();

            if (GalleryId != null)
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                GalleryBusiness gb = new GalleryBusiness(this.DbContext);
                vm.Gallery = gb.Load(GalleryId);
                ViewBag.GTitle = vm.Gallery.Title;
                vm.GalleryPicture = new GalleryPictures();
                vm.GalleryPicture.GalleryID = vm.Gallery.GalleryID;
                vm.GalleryPicture.NewsPicture = new NewsPictures();

                GalleryPictureBusiness gPB = new GalleryPictureBusiness(this.DbContext);
                Expression<Func<GalleryPictures, bool>> _filter = e => e.GalleryID == vm.Gallery.GalleryID;
                vm.galleryPictureLst = gPB.Load(_filter, null);
            }
            ViewBag.RSmallImgPath = this.RelativeSmallImgPath;
            ViewBag.RGalleryImgPath = this.RelativeGalleryImgPath;
            return View("~/Areas/CoreHandler/Views/Gallery/galleryphoto.cshtml", vm);
        }

        public ActionResult testFileUpload()
        {
            return View();
        }

        public ActionResult CropFile()
        {
            if (Request.Files.Count > 0)
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                for (int i = 0; i < Request.Files.Count; i++)
                {
                }
            }
            return Json("");
        }

        public bool SaveUploadedFile(HttpPostedFileBase file, string pathString, ref string picName, bool waterMarkFlag)
        {

            bool isSavedSuccessfully = true;
            string fName = "";

            try
            {

                fName = file.FileName;
                if (file != null && file.ContentLength > 0)
                {

                    picName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string extension = Path.GetExtension(fName);
                    picName = picName + extension;//"." + fName.Split('.')[1];

                    bool isExists = System.IO.Directory.Exists(pathString);

                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);

                    var path = string.Format("{0}\\{1}", pathString, picName);

                    // file.SaveAs(path);

                    Stream strm = file.InputStream;
                    Bitmap bitmap = new Bitmap(Image.FromStream(strm));
                    Size imgSize = new Size(bitmap.Width, bitmap.Height);

                    if (waterMarkFlag)
                    {
                        AddWaterMark(ref bitmap, picName);
                    }

                    SaveImage(ref bitmap, imgSize, path, false);

                }
                return isSavedSuccessfully;
            }

            catch 
            {
                isSavedSuccessfully = false;
                return isSavedSuccessfully;
            }
        }


        public ActionResult AddToGalleryPhotos(GalleryVM vm)
        {
            HttpFileCollectionBase imgFiles = Request.Files;
            SaveGalleryImages(vm, imgFiles);
            return ManageGalleryPhotos(vm.Gallery.GalleryID);
        }

        private void SaveGalleryImages(GalleryVM vm, HttpFileCollectionBase imgFiles)
        {
            if (imgFiles.Count > 0)
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                GalleryPictureBusiness gpB = new GalleryPictureBusiness(this.DbContext);
                GalleryBusiness gB = new GalleryBusiness(this.DbContext);

                //string originalDirectory = new System.IO.DirectoryInfo(string.Format("{0}Images\\GalleryImages", Server.MapPath(@"\"))).ToString();
                string filepath = (Convert.ToDateTime(vm.Gallery.GDate)).ToString("yyyyMMdd");
                string pathString = System.IO.Path.Combine(this.GalleryImgPath, filepath);//(originalDirectory.ToString(), filepath);

                string desc = "";
                string keyword = "";

                if (vm.GalleryPicture != null)
                {
                    vm.GalleryPicture.GalleryID = vm.Gallery.GalleryID;
                    desc = vm.GalleryPicture.NewsPicture.PicCaption;
                    keyword = vm.GalleryPicture.NewsPicture.KeyWords;
                }

                for (int i = 0; i < imgFiles.Count; i++)
                {
                    string picName = "";

                    bool saveImgFlag = SaveUploadedFile(imgFiles[i], pathString, ref picName,vm.waterMarkFlag);
                    if (saveImgFlag)
                    {
                        if (i > 0)
                        {
                            vm.GalleryPicture = new GalleryPictures();
                            vm.GalleryPicture.GalleryID = vm.Gallery.GalleryID;
                            vm.GalleryPicture.NewsPicture = new NewsPictures();
                            vm.GalleryPicture.NewsPicture.PicCaption = desc;
                            vm.GalleryPicture.NewsPicture.KeyWords = keyword;
                           
                        }
                        vm.GalleryPicture.NewsPicture.added_date = DateTime.Now;
                        vm.GalleryPicture.NewsPicture.JournalID = 1;
                        vm.GalleryPicture.NewsPicture.Source = 0;
                        vm.GalleryPicture.NewsPicture.PictureName = picName;

                        gpB.Add(vm.GalleryPicture);
                        this.SaveChanges();
                        this.DbContext.Entry(vm.GalleryPicture).GetDatabaseValues();
                        this.ModelState.Clear();
                        ///set default mainimg if not identified before
                        if (i == 0 && vm.Gallery.MainPictureID == null)
                        {
                            vm.Gallery.MainPictureID = vm.GalleryPicture.PictureID;
                            gB.Edit(vm.Gallery);
                            this.SaveChanges();
                        }

                       
                    }
                }
            }

        }

        public ActionResult DeleteFromGalleryPhotos(int GalleryId,int PictureId)
        {
            GalleryVM vm = new GalleryVM();
            vm.GalleryPicture = new GalleryPictures();
            vm.GalleryPicture.GalleryID = GalleryId;
            vm.GalleryPicture.PictureID = PictureId;
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            GalleryPictureBusiness gpB = new GalleryPictureBusiness(this.DbContext);
            gpB.Delete(vm.GalleryPicture);
            this.SaveChanges();
            return ManageGalleryPhotos(GalleryId);
        }

        public ActionResult SetAsMainGalleryPhoto(int GalleryId, int PictureId)
        {
            GalleryVM vm = new GalleryVM();
            vm.Gallery = new Gallery();
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                TryUpdateModel<Gallery>(vm.Gallery, new string[] { "GalleryID", "MainPictureID" });
                GalleryBusiness gB = new GalleryBusiness(this.DbContext);
                vm.Gallery = gB.Load(GalleryId);
                vm.Gallery.MainPictureID = PictureId;
                gB.Edit(vm.Gallery);
                this.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                //            ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                //throw;
            }
            return ManageGalleryPhotos(GalleryId);
        }
        
    }
}
