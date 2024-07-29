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
using System.IO;
using System.Web.Helpers;
using System.Drawing;

namespace CMS.Areas.CoreHandler.Controllers
{
   
    public class BlocksController : BaseController
    {
        // GET: Blocks
        public ActionResult Index(string searchStr, BlocksVM vm = null)
        {
            Session["CurrentModule"] = "البلوكات";
            Session["CurrentService"] = "إدارة البلوكات";


            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            BlocksZoneBusiness zB = new BlocksZoneBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = zB.Load(q => q.OrderBy(d => d.ZoneID), e => e.ZoneName.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = zB.Load(q => q.OrderBy(d => d.ZoneID), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/Blocks/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

     
        // GET: Blocks/Details/5
        public ActionResult Details(int? ZoneID)
        {
            ViewBag.Title = "تفاصيل المساحة الاعلانية";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (ZoneID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlocksZoneBusiness EB = new BlocksZoneBusiness(this.DbContext);
            BlocksVM vm = new BlocksVM();
            vm.blocksZone = EB.Load(ZoneID);

            if (vm.blocksZone == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Blocks/Details.cshtml", vm);
        }

        // GET: Blocks/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            BlocksVM vm = new BlocksVM();
            return View("~/Areas/CoreHandler/Views/Blocks/Create.cshtml", vm);
        }

        // POST: Blocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlocksVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<BlocksZone>(vm.blocksZone, new string[] { "ZoneName", "ZoneDescription", "AddedDate", "AddedBy" });
            if (ModelState.IsValid)
            {
                vm.blocksZone.AddedDate = DateTime.Now;
                vm.blocksZone.AddedBy = LoginUserID.Value;

                BlocksZoneBusiness eB = new BlocksZoneBusiness(this.DbContext);
                eB.Add(vm.blocksZone);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }

            return View("~/Areas/CoreHandler/Views/Blocks/Create.cshtml", vm);
        }

        // GET: Blocks/Edit/5
        public ActionResult Edit(int? ZoneID, BlocksVM vm = null)
        {
            if (ZoneID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            BlocksZoneBusiness eB = new BlocksZoneBusiness(this.DbContext);
            vm.blocksZone = eB.Load(ZoneID);

            if (vm.blocksZone == null)
            {
                return HttpNotFound();
            }

            return PartialView("~/Areas/CoreHandler/Views/Blocks/Edit.cshtml", vm);
        }

        // POST: Blocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BlocksVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<BlocksZone>(vm.blocksZone, new string[] { "ZoneID", "ZoneName", "ZoneDescription" });
            if (ModelState.IsValid)
            {

                BlocksZoneBusiness eB = new BlocksZoneBusiness(this.DbContext);
                eB.Edit(vm.blocksZone);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }
            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            return View("~/Areas/CoreHandler/Views/Blocks/Edit.cshtml", vm);

        }

        // GET: Blocks/Delete/5
        public ActionResult Delete(int? ZoneID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (ZoneID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlocksZoneBusiness eB = new BlocksZoneBusiness(this.DbContext);
            BlocksVM vm = new BlocksVM();
            vm.blocksZone = eB.Load(ZoneID);
            if (vm.blocksZone == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Blocks/Delete.cshtml", vm);
        }

        // POST: Blocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ZoneID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            BlocksZoneBusiness eB = new BlocksZoneBusiness(this.DbContext);
            BlocksVM vm = new BlocksVM();
            vm.blocksZone = eB.Load(ZoneID);
            eB.Delete(vm.blocksZone);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public ActionResult MangeBlockAds(int? ZoneID, BlocksVM vm = null)
        {
            if (ZoneID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            NewsBlocksBusiness nbB = new NewsBlocksBusiness(this.DbContext);
            vm.NewsBlocks = nbB.Load(x => x.ZoneID == ZoneID.Value, null).Where(x => x.JournalID == 1).FirstOrDefault();

            if (vm.NewsBlocks == null)
            {
                vm.NewsBlocks = new NewsBlocks();
                vm.NewsBlocks.ZoneID = ZoneID;
                vm.NewsBlocks.JournalID = 1;
            }

            return PartialView("~/Areas/CoreHandler/Views/Blocks/MangeBlockAds.cshtml", vm);
        }

        [ValidateInput(false)]
        public ActionResult SaveBlockAds(BlocksVM vm)
        {
            if (ModelState.IsValid)
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBlocksBusiness nbB = new NewsBlocksBusiness(this.DbContext);
                if (vm.NewsBlocks.ZoneID.ToString() != "" )
                {
                    nbB.Edit(vm.NewsBlocks);
                    TempData["Action"] = "Edit";
                }
                else
                {
                    nbB.Add(vm.NewsBlocks);
                    TempData["Action"] = "Create";
                }
                this.SaveChanges();
                
            }
            else
            {
                TempData["Action"] = "Error";
            }

            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            return View("~/Areas/CoreHandler/Views/Blocks/MangeBlockAds.cshtml", vm);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFiles(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {

            //http://stackoverflow.com/a/4088194/167670
            //http://arturito.net/2010/11/03/file-and-image-upload-with-asp-net-mvc2-with-ckeditor-wysiwyg-rich-text-editor/
            //http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx

            if (upload.ContentLength <= 0)
                return null;

            string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var url = "";
            string message;
            var output = "";

            string exten = Path.GetExtension(upload.FileName);
            //embed is doc or pdf or gif file
            if (IsValidDoc(upload))
            {
                try
                {
                    string picName = "" + DateTimePattern + exten;
                    string embedFilename = this.EmbededImagePath + picName;

                    url = string.Format("{0}{1}", this.ReltativeEmbededImagesPath, picName);

                    upload.SaveAs(embedFilename);

                    // passing message File/failure
                    message = "تم إضافة الملف بنجاح";

                    // since it is an ajax request it requires this string
                    output = string.Format(
                        "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                        CKEditorFuncNum, url, message);
                }
                catch (Exception ex)
                {
                }
            }
            //embed is image file  -- for case join image on href
            else if (IsValidMainImg(upload))
            {
                string picName = "" + DateTimePattern + ".jpg";
                string embedFilename = this.EmbededImagePath + picName;

                try
                {
                    Stream strm = upload.InputStream;
                    Bitmap bitmap = new Bitmap(Image.FromStream(strm));
                    Size imgSize = new Size(bitmap.Width, bitmap.Height);

                    SaveImage(ref bitmap, imgSize, embedFilename, false);

                    url = string.Format("{0}{1}", this.ReltativeEmbededImagesPath, picName);

                    // passing message success/failure
                    message = "تم إضافة الصورة بنجاح";

                    // since it is an ajax request it requires this string
                    output = string.Format(
                        "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                        CKEditorFuncNum, url, message);

                }
                catch (Exception ex)
                { }
            }
            else
            {
                // passing message not Accepted Format
                message = "نوع الملف غير مسموح";
                url = "";
                // since it is an ajax request it requires this string
                output = string.Format(
                    "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                    CKEditorFuncNum, url, message);
            }

            return Content(output);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {

            //http://stackoverflow.com/a/4088194/167670
            //http://arturito.net/2010/11/03/file-and-image-upload-with-asp-net-mvc2-with-ckeditor-wysiwyg-rich-text-editor/
            //http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx

            if (upload.ContentLength <= 0)
                return null;

            string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var url = "";
            string message;
            var output = "";
            //embed is image file
            if (IsValidMainImg(upload))
            {
                string exten = Path.GetExtension(upload.FileName);
                string picName = "";
                if (exten.ToLower() == ".gif")
                {
                    picName = "" + DateTimePattern + exten;
                }
                else
                {
                    picName = "" + DateTimePattern + ".jpg";

                }
                string embedFilename = this.EmbededImagePath + picName;

                try
                {
                    if (exten.ToLower() == ".gif")
                    {
                        upload.SaveAs(embedFilename);
                    }
                    else
                    {
                        Stream strm = upload.InputStream;
                        Bitmap bitmap = new Bitmap(Image.FromStream(strm));
                        Size imgSize = new Size(bitmap.Width, bitmap.Height);

                        SaveImage(ref bitmap, imgSize, embedFilename, false);
                    }

                    url = string.Format("{0}{1}", this.ReltativeEmbededImagesPath, picName);

                    // passing message success/failure
                    message = "تم إضافة الصورة بنجاح";

                    // since it is an ajax request it requires this string
                    output = string.Format(
                        "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                        CKEditorFuncNum, url, message);

                }
                catch (Exception ex)
                { }
            }
            else
            {
                // passing message not Accepted Format
                message = "نوع الملف غير مسموح";
                url = "";
                // since it is an ajax request it requires this string
                output = string.Format(
                    "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                    CKEditorFuncNum, url, message);
            }

            return Content(output);
        }

    }
}