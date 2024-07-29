using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Akhbar.DBContext;
using Akhbar.DBEntities;
using Akhbar.DBBusiness;
using PagedList;
using CMS.Areas.FrameWork.Controllers;
using CMS.Areas.FrameWork.ViewModels;
using CMS.Areas.CoreHandler.Models;
using System.Web.Mvc;
using System.Net;

namespace CMS.Areas.CoreHandler.Controllers
{
 
    public class IssuesController : BaseController
    {
        // GET: Issue
        public ActionResult Index(string searchStr, IssuesVM vm = null)
        {
            Session["CurrentModule"] = "الاعداد الاسبوعية";
            Session["CurrentService"] = "الاعداد";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            IssueBusiness EB = new IssueBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.OrderByDescending(d => d.IssueID), e => e.IssueTitle.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.OrderByDescending(d => d.IssueID), null, vm.page, PageSize, null);
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/Issues/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }


     


        // GET: Issue/Details/5
        public ActionResult Details(int? MIssue)
        {
            ViewBag.Title = "تفاصيل العدد";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (MIssue == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueBusiness EB = new IssueBusiness(this.DbContext);
            IssuesVM vm = new IssuesVM();
            vm.issue = EB.Load(MIssue);
            
            if (vm.issue == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Issues/Details.cshtml", vm);
        }

        // GET: Issue/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            IssuesVM vm = new IssuesVM();
            return View("~/Areas/CoreHandler/Views/Issues/Create.cshtml", vm);
        }

        // POST: Issue/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IssuesVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Issue>(vm.issue, new string[] { "IssueTitle", "IssueDate", "IssueStatus", "JournalID" });

            if (ModelState.IsValid)
            {
               
                vm.issue.JournalID = 1;
                vm.issue.IssueStatus = vm.Status;

                IssueBusiness eB = new IssueBusiness(this.DbContext);
                eB.Add(vm.issue);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }
            return View("~/Areas/CoreHandler/Views/Issues/Create.cshtml", vm);
        }

        // GET: Issue/Edit/5
        public ActionResult Edit(int? MIssue, IssuesVM vm = null)
        {
            if (MIssue == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            IssueBusiness eB = new IssueBusiness(this.DbContext);
            vm.issue = eB.Load(MIssue);
           
            if (vm.issue == null)
            {
                return HttpNotFound();
            }

            return PartialView("~/Areas/CoreHandler/Views/Issues/Edit.cshtml", vm);
        }

        // POST: Issue/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IssuesVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Issue>(vm.issue, new string[] { "IssueID", "IssueTitle", "IssueDate", "IssueStatus", "JournalID" });

            if (ModelState.IsValid)
            {
                vm.issue.JournalID = 1;
                vm.issue.IssueStatus = vm.Status;
                IssueBusiness eB = new IssueBusiness(this.DbContext);
                eB.Edit(vm.issue);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }

            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            return View("~/Areas/CoreHandler/Views/Issues/Edit.cshtml", vm);

        }

        // GET: Issue/Delete/5
        public ActionResult Delete(int? MIssue)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (MIssue == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueBusiness eB = new IssueBusiness(this.DbContext);
            IssuesVM vm = new IssuesVM();
            vm.issue = eB.Load(MIssue);
            if (vm.issue == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Issues/Delete.cshtml", vm);
        }

        // POST: Issue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int MIssue)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            IssueBusiness eB = new IssueBusiness(this.DbContext);
            IssuesVM vm = new IssuesVM();
            vm.issue = eB.Load(MIssue);
            eB.Delete(vm.issue);
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
