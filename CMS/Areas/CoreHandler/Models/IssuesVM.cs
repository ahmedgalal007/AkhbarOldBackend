using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class IssuesVM
    {
        public IssuesVM()
        {
            page = 1;
            Status = 2;
            RelatedCaller = "Issues";
            SearchString = "";
            orderedlst = new List<Issue>();
        }
        public IPagedList<Issue> lst { get; set; }
        public List<Issue> orderedlst { get; set; }
        public Issue issue { get; set; }
        public int page { get; set; }
        public int Status { get; set; }
       
        public string RelatedTarget = "RelatedIssues";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }
}