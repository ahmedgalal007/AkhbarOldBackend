using Domain.Akhbar.DBEntities;
using CMS.ReportingWebService;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS.Areas.Reports.Models
{
    public class ReportsVM
    {

        public ReportsVM()
        {
            page = 1;
            RelatedCaller = "ReportsZones";
            SearchString = "";
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
            UserId = 0;
            SectionId = 0;
        }

        public string ReportName { get; set; }
        public string ReportPath { get; set; }

        [Display(Name = "من التاريخ")]
        public DateTime StartDate { get; set; }
        [Display(Name = "الي التاريخ")]
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public List<AdminUser> userLst { get; set; }
        public int SectionId { get; set; }
        public List<MainSection> secLst { get; set; }

        public int page { get; set; }

        public string RelatedTarget = "RelatedReportsZones";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }
    }
}