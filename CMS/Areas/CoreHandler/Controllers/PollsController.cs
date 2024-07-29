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

namespace CMS.Areas.CoreHandler.Controllers
{
    [SystemAuthorizationAttribute]
    public class PollsController : BaseController
    {
        // GET: Polls
        public ActionResult Index(string searchStr, PollsVM vm = null)
        {
            Session["CurrentModule"] = "التصويتات";
            Session["CurrentService"] = "التصويتات";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            PollsBusiness EB = new PollsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.lst = EB.Load(q => q.Take(50).OrderByDescending(d => d.PollID), e => e.PollBody.Contains(vm.SearchString), vm.page, PageSize, null);
            else
                vm.lst = EB.Load(q => q.Take(50).OrderBy(d => d.PollID), null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/Polls/Index.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }
        

        // GET: Polls/Details/5
        public ActionResult Details(int? PollId)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (PollId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PollsBusiness EB = new PollsBusiness(this.DbContext);
            PollsVM vm = new PollsVM();
            vm.Poll = EB.Load(PollId);
            if (vm.Poll.Activated != null)
                vm.Active = Convert.ToBoolean(vm.Poll.Activated);
            if (vm.Poll == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Polls/Details.cshtml", vm);
        }

        // GET: Polls/Create
        public ActionResult Create()
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            PollsVM vm = new PollsVM();
            return View("~/Areas/CoreHandler/Views/Polls/Create.cshtml", vm);
        }

        // POST: Polls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PollsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Polls>(vm.Poll, new string[] { "PollBody", "SectionID", "Added_Date", "Activated", "DateActivated" });
            if (ModelState.IsValid)
            {
                vm.Poll.Activated = Convert.ToByte(vm.Active);
                vm.Poll.Added_Date = DateTime.Now;
                if (vm.Active == true)
                    vm.Poll.DateActivated = DateTime.Now;
                vm.Poll.TotalVotes = 0;
                PollsBusiness eB = new PollsBusiness(this.DbContext);
                eB.Add(vm.Poll);
                this.SaveChanges();

                TempData["Action"] = "Create";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["Action"] = "Error";
            }

            return View("~/Areas/CoreHandler/Views/Polls/Create.cshtml", vm);
        }

        // GET: Polls/Edit/5
        public ActionResult Edit(int? PollId, PollsVM vm = null)
        {
            if (PollId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            PollsBusiness eB = new PollsBusiness(this.DbContext);
            vm.Poll = eB.Load(PollId);
            if (vm.Poll.Activated != null)
                vm.Active = Convert.ToBoolean(vm.Poll.Activated);
            if (vm.Poll == null)
            {
                return HttpNotFound();
            }

            return PartialView("~/Areas/CoreHandler/Views/Polls/Edit.cshtml", vm);
        }

        // POST: Polls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PollsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            TryUpdateModel<Polls>(vm.Poll, new string[] { "PollID", "PollBody", "SectionID", "Added_Date", "Activated", "DateActivated" });
            if (ModelState.IsValid)
            {
                vm.Poll.Activated = Convert.ToByte(vm.Active);
                if (vm.Active == true)
                    vm.Poll.DateActivated = DateTime.Now;
                PollsBusiness eB = new PollsBusiness(this.DbContext);
                eB.Edit(vm.Poll);
                this.SaveChanges();

                TempData["Action"] = "Edit";
            }
            else
            {
                TempData["Action"] = "Error";
            }

            ViewBag.alert = GetMessage(TempData["Action"].ToString());

            return View("~/Areas/CoreHandler/Views/Polls/Edit.cshtml", vm);

        }

        // GET: Polls/Delete/5
        public ActionResult Delete(int? PollId)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            if (PollId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PollsBusiness eB = new PollsBusiness(this.DbContext);
            PollsVM vm = new PollsVM();
            vm.Poll = eB.Load(PollId);
            if (vm.Poll == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/Polls/Delete.cshtml", vm);
        }

        // POST: Polls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int PollId)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            PollsBusiness eB = new PollsBusiness(this.DbContext);
            PollsVM vm = new PollsVM();
            vm.Poll = eB.Load(PollId);
            eB.Delete(vm.Poll);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }



        public ActionResult ManagePollOptions(PollsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (vm.Poll != null && vm.Poll.PollID != 0)
            {
                PollsBusiness pB = new PollsBusiness(this.DbContext);
                vm.Poll = pB.Load(vm.Poll.PollID);
                ViewBag.PollBody = vm.Poll.PollBody;
                PollsOptionBusiness pOB = new PollsOptionBusiness(this.DbContext);
                Expression<Func<PollsOption, bool>> _filter = e => e.PollID == vm.Poll.PollID;
                vm.pollsOptionLst = pOB.Load(_filter, null);
            }

            return PartialView("~/Areas/CoreHandler/Views/Polls/ManagePollOptions.cshtml", vm);
        }

        public ActionResult AddToPollOptions(PollsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            vm.PollsOption.PollID = vm.Poll.PollID;
            PollsOptionBusiness pOB = new PollsOptionBusiness(this.DbContext);
            if (vm.PollsOption != null)
            {
                pOB.AddIfNotExists(vm.PollsOption,e=>e.OptionBody==vm.PollsOption.OptionBody);
                this.SaveChanges();
                this.ModelState.Clear();
                vm.PollsOption = null;
            }
            return ManagePollOptions(vm);
        }

        public ActionResult DeleteFromPollOptions(PollsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            vm.PollsOption.PollID = vm.Poll.PollID;
            PollsOptionBusiness pOB = new PollsOptionBusiness(this.DbContext);
            pOB.Delete(vm.PollsOption);
            this.SaveChanges();
            return ManagePollOptions(vm);
        }
    }
}
