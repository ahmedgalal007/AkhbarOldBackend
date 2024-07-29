using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Akhbar.DBContext;
using Akhbar.DBEntities;

namespace CMS.Areas.FrameWork.Controllers
{
    public class SharedController : BaseController
    {
        AkhbarDBContext db;

        public SharedController()
        {
            db = new AkhbarDBContext(new GeneralHellper().AkhbarDbConnection);
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
           
            List<VUserPermissions> _lstUserPermission = new GeneralHellper().UserPermissions(db);

            return PartialView("~/Areas/FrameWork/Views/Shared/_Menu.cshtml", _lstUserPermission);
        }
        //[ChildActionOnly]
        //public PartialViewResult RowFunctions(int id)
        //{
        //    ViewBag.ID = id;
        //    return PartialView("~/Areas/FrameWork/Views/Shared/_RowFunctions.cshtml");
        //}
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}