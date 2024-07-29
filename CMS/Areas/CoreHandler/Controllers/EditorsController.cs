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
using System.Data.Entity.Validation;

namespace CMS.Areas.CoreHandler.Controllers
{
    [SystemAuthorizationAttribute]
    public class EditorsController : BaseController
    {

        // GET: Editors
        public ActionResult Index(string searchStr, EditorsVM vm = null)
        {
            Session["CurrentModule"] = "كتاب الموقع";
            Session["CurrentService"] = "الكتاب";
          

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            EditorBusiness EB = new EditorBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderBy(d => d.EditorID), e => e.EditorName.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderBy(d => d.EditorID), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/Editors/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }


        public ActionResult EditorSort(EditorsVM vm = null)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            EditorBusiness EB = new EditorBusiness(this.DbContext);

            var resList = EB.Load(null, null).OrderBy(e => e.DisplayOrder);

            vm.orderedlst = resList.ToList();

            return View(vm);

        }

        [HttpPost]
        public ActionResult SaveEditorSort(string Ids, EditorsVM vm = null)
        {
            try {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                EditorBusiness eB = new EditorBusiness(this.DbContext);
                var secArray = JArray.Parse(Ids).Select(x => (int)x).ToArray();
                var resList = eB.Load(null, null).OrderBy(e => e.DisplayOrder);
                vm.orderedlst = resList.ToList();


                for (int i = 0; i < secArray.Count(); i++)
                {
                    vm.editor = vm.orderedlst.FirstOrDefault(e => e.EditorID == secArray[i]);
                    vm.editor.DisplayOrder = i + 1;
                    if (vm.editor.ArticleName == null)
                        vm.editor.ArticleName = "ArticleName";
                    eB.Edit(vm.editor);
                    this.SaveChanges();
                }

                vm.orderedlst = vm.orderedlst.OrderBy(e => e.DisplayOrder).ToList();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
            return View("~/Areas/CoreHandler/Views/MainSection/SectionSort.cshtml", vm);
        }

        // GET: Editors/Details/5
        public ActionResult Details(int? editorId)
        {
            ViewBag.Title = "تفاصيل الكاتب";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (editorId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditorBusiness EB = new EditorBusiness(this.DbContext);
            EditorsVM vm = new EditorsVM();
            vm.editor= EB.Load(editorId);
            if (vm.editor.SectionID != null)
            {
                vm.editor.Section = new MainSection();
                MainSectionBusiness msB = new MainSectionBusiness(this.DbContext);
                vm.editor.Section = msB.Load(vm.editor.SectionID);
            //vm.editor.Section.SectionID = vm.editor.SectionID ?? 0;
            }

            if (vm.editor == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Editors/Details.cshtml", vm);
        }

        // GET: Editors/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            ViewBag.Sec_ID = new SelectList(secB.Load(null, null), "SectionID", "SecTitle");
            EditorsVM vm = new EditorsVM();
            return View("~/Areas/CoreHandler/Views/Editors/Create.cshtml",vm);
        }

        // POST: Editors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditorsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Editor>(vm.editor, new string[] { "EditorName", "picture", "SectionID", "ArticleName" });
           if (ModelState.IsValid)
            {
                var img = this.HttpContext.Request.Files["Editor_Image"];
                if (img != null && img.ContentLength > 0)
                {
                    string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string filename = "/Images/Editors/" + DateTimePattern + ".jpg";
                    img.SaveAs(Server.MapPath(filename));
                    vm.editor.picture = filename;
                }
                EditorBusiness eB = new EditorBusiness(this.DbContext);
                eB.Add(vm.editor);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }
            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            ViewBag.Sec_ID = new SelectList(secB.Load(null, null), "SectionID", "SecTitle");
            return View("~/Areas/CoreHandler/Views/Editors/Create.cshtml", vm);
        }

        // GET: Editors/Edit/5
        public ActionResult Edit(int? editorId, EditorsVM vm = null)
        {
            if (editorId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            EditorBusiness eB = new EditorBusiness(this.DbContext);
            vm.editor = eB.Load(editorId);
            vm.editor.ArticleName = vm.editor.ArticleName.Trim();
            if (vm.editor == null)
            {
                return HttpNotFound();
            }
          
            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            ViewBag.Sec_ID = new SelectList(secB.Load(null, null), "SectionID", "SecTitle");
            return PartialView("~/Areas/CoreHandler/Views/Editors/Edit.cshtml", vm);
        }

        // POST: Editors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditorsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Editor>(vm.editor, new string[] {"EditorID","EditorName", "picture", "SectionID", "ArticleName" });
            if (ModelState.IsValid)
            {
                var img = this.HttpContext.Request.Files["Editor_Image"];
                if (img != null && img.ContentLength > 0)
                {
                    string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string filename = "/Images/Editors/" + DateTimePattern + ".jpg";
                    img.SaveAs(Server.MapPath(filename));
                    vm.editor.picture = filename;
                }
                EditorBusiness eB = new EditorBusiness(this.DbContext);
                eB.Edit(vm.editor);
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
            return View("~/Areas/CoreHandler/Views/Editors/Edit.cshtml", vm);

        }

        // GET: Editors/Delete/5
        public ActionResult Delete(int? editorId)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (editorId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditorBusiness eB = new EditorBusiness(this.DbContext);
            EditorsVM vm = new EditorsVM();
            vm.editor = eB.Load(editorId);
            if (vm.editor == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Editors/Delete.cshtml", vm); 
        }

        // POST: Editors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int editorId)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            EditorBusiness eB = new EditorBusiness(this.DbContext);
            EditorsVM vm = new EditorsVM();
            vm.editor = eB.Load(editorId);
            eB.Delete(vm.editor);
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
