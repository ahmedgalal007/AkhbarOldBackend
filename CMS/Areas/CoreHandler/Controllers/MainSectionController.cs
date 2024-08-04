using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Akhbar.Contexts;
using Domain.Akhbar.DBEntities;
using Domain.Akhbar.DBBusiness;
using PagedList;
using CMS.Areas.FrameWork.Controllers;
using CMS.Areas.FrameWork.ViewModels;
using System.Web.Routing;
using CMS.Areas.CoreHandler.Models;
using Newtonsoft.Json.Linq;

namespace CMS.Areas.CoreHandler.Controllers
{

    public class MainSectionController : BaseController
    {
        // GET: MainSection
        public ActionResult Index(string searchStr, MainSectionVM vm = null)
        {
            Session["CurrentModule"] = "الابواب الرئيسية";
            Session["CurrentService"] = "الابواب";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness EB = new MainSectionBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderBy(d => d.SectionID), e => e.SecTitle.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderBy(d => d.SectionID), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/MainSection/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }
       



        public ActionResult SectionSort(MainSectionVM vm = null)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness EB = new MainSectionBusiness(this.DbContext);

            var resList = EB.Load(e => e.Hide == 0 && (e.ParentSectionID == null || e.ParentSectionID == 0), null).OrderBy(e => e.DisplayOrder);

            vm.orderedlst = resList.ToList();

            return View(vm);

        }

        [HttpPost]
        public ActionResult SaveSectionSort(string Ids, MainSectionVM vm = null)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness msB = new MainSectionBusiness(this.DbContext);
            var secArray = JArray.Parse(Ids).Select(x => (int)x).ToArray();
            var resList = msB.Load(e => e.Hide == 0 && (e.ParentSectionID == null || e.ParentSectionID == 0), null).OrderBy(e => e.DisplayOrder);
            vm.orderedlst = resList.ToList();
           

            for (int i = 0; i < secArray.Count(); i++)
            {
                vm.section = vm.orderedlst.FirstOrDefault(e => e.SectionID == secArray[i]);
                vm.section.DisplayOrder = i + 1;
                if (vm.section.Description == null)
                    vm.section.Description = "Description";
                if (vm.section.Keywords == null)
                    vm.section.Keywords = "Keywords";
                msB.Edit(vm.section);
                this.SaveChanges();
            }

            vm.orderedlst = vm.orderedlst.OrderBy(e => e.DisplayOrder).ToList();

            return View("~/Areas/CoreHandler/Views/MainSection/SectionSort.cshtml", vm);
        }

      
        // GET: MainSection/Details/5
        public ActionResult Details(int? MSectionID)
        {
            ViewBag.Title = "تفاصيل القسم";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (MSectionID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainSectionBusiness EB = new MainSectionBusiness(this.DbContext);
            MainSectionVM vm = new MainSectionVM();
            vm.section = EB.Load(MSectionID);

            if (vm.section.ParentSectionID != null && vm.section.ParentSectionID != 0)
                vm.SubSecFlag = true;
            else
                vm.SubSecFlag = false;

            if (vm.section.Hide != null)
                vm.SHide = Convert.ToBoolean(vm.section.Hide);
            if (vm.section.WeeklySection != null)
                vm.SWeeklySection = Convert.ToBoolean(vm.section.WeeklySection);
            if (vm.section == null)
            {
                return HttpNotFound();
            }

            vm.orderedlst = EB.Load(e => e.ParentSectionID == null || e.ParentSectionID == 0, null).OrderBy(e => e.SectionID).ToList();
            ViewBag.Sec_ID = new SelectList(vm.orderedlst, "SectionID", "SecTitle");
            return PartialView("~/Areas/CoreHandler/Views/MainSection/Details.cshtml", vm);
        }

        // GET: MainSection/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionVM vm = new MainSectionVM();
            MainSectionBusiness EB = new MainSectionBusiness(this.DbContext);

            vm.orderedlst = EB.Load(e => e.ParentSectionID == null || e.ParentSectionID == 0, null).OrderBy(e => e.SectionID).ToList();
            
            ViewBag.Sec_ID = new SelectList(vm.orderedlst, "SectionID", "SecTitle");

            return View("~/Areas/CoreHandler/Views/MainSection/Create.cshtml",vm);
        }

        // POST: MainSection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MainSectionVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<MainSection>(vm.section, new string[] { "SecTitle", "Hide", "WeeklySection", "Keywords", "Description" , "ParentSectionID" });

            if (ModelState.IsValid)
            {
                if (!vm.SubSecFlag)
                    vm.section.ParentSectionID = 0;

                vm.section.Hide = Convert.ToByte(vm.SHide);
                vm.section.WeeklySection = Convert.ToByte(vm.SWeeklySection);
                vm.section.JournalID = 1;

                MainSectionBusiness eB = new MainSectionBusiness(this.DbContext);
                eB.Add(vm.section);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }
            return View("~/Areas/CoreHandler/Views/MainSection/Create.cshtml", vm);
        }

        // GET: MainSection/Edit/5
        public ActionResult Edit(int? MSectionID, MainSectionVM vm = null)
        {
            if (MSectionID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness eB = new MainSectionBusiness(this.DbContext);
            vm.section = eB.Load(MSectionID);

            if (vm.section.ParentSectionID != null && vm.section.ParentSectionID != 0)
                vm.SubSecFlag = true;
            else
                vm.SubSecFlag = false;

            if (vm.section.Hide != null)
                vm.SHide = Convert.ToBoolean(vm.section.Hide);
            if (vm.section.WeeklySection != null)
                vm.SWeeklySection = Convert.ToBoolean(vm.section.WeeklySection);
            if (vm.section == null)
            {
                return HttpNotFound();
            }

            vm.orderedlst = eB.Load(e => e.ParentSectionID == null || e.ParentSectionID == 0, null).OrderBy(e => e.SectionID).ToList();
            ViewBag.Sec_ID = new SelectList(vm.orderedlst, "SectionID", "SecTitle");
            return PartialView("~/Areas/CoreHandler/Views/MainSection/Edit.cshtml", vm);
        }

        // POST: MainSection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MainSectionVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<MainSection>(vm.section, new string[] { "SectionID", "SecTitle", "Hide", "WeeklySection", "Keywords", "Description", "ParentSectionID" });
            MainSectionBusiness eB = new MainSectionBusiness(this.DbContext);

            if (ModelState.IsValid)
            {
                if (!vm.SubSecFlag)
                    vm.section.ParentSectionID = 0;

                vm.section.Hide = Convert.ToByte(vm.SHide);
                vm.section.WeeklySection = Convert.ToByte(vm.SWeeklySection);
                vm.section.JournalID = 1;
                eB.Edit(vm.section);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }

            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            vm.orderedlst = eB.Load(e => e.ParentSectionID == null || e.ParentSectionID == 0, null).OrderBy(e => e.SectionID).ToList();
            ViewBag.Sec_ID = new SelectList(vm.orderedlst, "SectionID", "SecTitle");

            return View("~/Areas/CoreHandler/Views/MainSection/Edit.cshtml", vm);

        }

        // GET: MainSection/Delete/5
        public ActionResult Delete(int? MSectionID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (MSectionID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MainSectionBusiness eB = new MainSectionBusiness(this.DbContext);
            MainSectionVM vm = new MainSectionVM();
            vm.section = eB.Load(MSectionID);
            if (vm.section == null)
            {
                return HttpNotFound();
            }
            vm.orderedlst = eB.Load(e => e.ParentSectionID == null || e.ParentSectionID == 0, null).OrderBy(e => e.SectionID).ToList();
            ViewBag.Sec_ID = new SelectList(vm.orderedlst, "SectionID", "SecTitle");
            return PartialView("~/Areas/CoreHandler/Views/MainSection/Delete.cshtml", vm);
        }

        // POST: MainSection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int MSectionID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness eB = new MainSectionBusiness(this.DbContext);
            MainSectionVM vm = new MainSectionVM();
            vm.section = eB.Load(MSectionID);
            eB.Delete(vm.section);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("Index");
        }


        public ActionResult SubSecSort(int? ParentSecId, string message)
        {
            Session["CurrentModule"] = "الابواب";
            Session["CurrentService"] = "ترتيب الابواب الفرعية";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            NewsVM vm = new NewsVM();
            vm = FillSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (message != null)
                ViewBag.alert = GetMessage(message);

            return View(vm);
        }

        public ActionResult LoadSubSecListToOrder(int? secId)
        {
            NewsVM vm = new NewsVM();
            MainSection selectLstItem = new MainSection();

            if (secId == null)
                secId = 0;

            if (secId != 0)
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
                List<MainSection> sections = secB.Load(e => e.Hide == 0 && e.ParentSectionID == secId, null).OrderBy(e => e.SectionID).ToList(); 

                foreach (MainSection item in sections)
                {
                    selectLstItem = new MainSection();
                    selectLstItem.SectionID = item.SectionID;
                    selectLstItem.SecTitle = item.SecTitle;
                    vm.ActiveSectionLst.Add(selectLstItem);
                }
            }


            return PartialView("~/Areas/CoreHandler/Views/MainSection/LoadSubSecListToOrder.cshtml", vm);
        }

        private NewsVM FillSectionDDL(NewsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);

            List<MainSection> sections = secB.Load(e => e.Hide == 0 && e.ParentSectionID == 0 || e.ParentSectionID == null, null).OrderBy(e => e.SectionID).ToList(); ;

            MainSection selectLstItem;

            foreach (MainSection item in sections)
            {
                selectLstItem = new MainSection();
                selectLstItem.SectionID = item.SectionID;
                selectLstItem.SecTitle = item.SecTitle;
                vm.ActiveSectionLst.Add(selectLstItem);
            }

            return vm;
        }


        [HttpPost]
        public ActionResult SaveSubSecSort(string Ids, int secId, MainSectionVM vm = null)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                MainSectionBusiness msB = new MainSectionBusiness(this.DbContext);

                var secArray = JArray.Parse(Ids).Select(x => (int)x).ToArray();
                var resList = msB.Load(e => e.Hide == 0 && e.ParentSectionID == secId, null).OrderBy(e => e.DisplayOrder);
                vm.orderedlst = resList.ToList();

                for (int i = 0; i < secArray.Count(); i++)
                {
                    vm.section = vm.orderedlst.FirstOrDefault(e => e.SectionID == secArray[i]);
                    vm.section.DisplayOrder = i + 1;
                    if (vm.section.Description == null)
                        vm.section.Description = "Description";
                    if (vm.section.Keywords == null)
                        vm.section.Keywords = "Keywords";
                    msB.Edit(vm.section);
                    this.SaveChanges();
                }


                NewsVM nvm = new NewsVM();
                nvm = FillSectionDDL(nvm);
                ViewBag.Sec_ID = new SelectList(nvm.ActiveSectionLst, "SectionID", "SecTitle");

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
            return View("~/Areas/CoreHandler/Views/MainSection/SubSecSort.cshtml", vm);

        }




        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
