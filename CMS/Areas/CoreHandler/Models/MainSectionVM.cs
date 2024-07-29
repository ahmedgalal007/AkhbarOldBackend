using Domain.Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class MainSectionVM
    {
        public MainSectionVM()
        {
            page = 1;
            SHide = false;
            SWeeklySection = false;
            RelatedCaller = "MainSections";
            SearchString = "";
            orderedlst = new List<MainSection>();
        }
        public IPagedList<MainSection> lst { get; set; }
        public List<MainSection> orderedlst { get; set; }
        public MainSection section { get; set; }
        public int page { get; set; }
        public bool SubSecFlag { get; set; }
        public bool SHide { get; set; }
        public bool SWeeklySection { get; set; }
        public string RelatedTarget = "RelatedmainSections";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }
}