using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.UserManagment.Models
{
    public class TempDeskShiftVM
    {

        public TempDeskShiftVM()
        {
            page = 1;
            RelatedCaller = "TempDeskShift";
            SearchString = "";
            lst = new List<TempDeskShift>();
            adminUserLst = new List<AdminUser>();
        }

        public int page { get; set; }
        public string RelatedTarget = "RelatedtempDeskShifts";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

        public List<TempDeskShift> lst { get; set; }
        public TempDeskShift tempDeskShift { get; set; }
        public TempDeskShift waitingTempDeskShift { get; set; }
        public TempDeskShift currentAssignTempDeskShift { get; set; }


        public List<AdminUser> adminUserLst { get; set; }
        public AdminUser adminUser { get; set; }

    }
}