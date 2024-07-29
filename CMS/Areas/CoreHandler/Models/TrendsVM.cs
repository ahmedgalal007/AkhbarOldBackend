using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class TrendsVM
    {

        public TrendsVM()
        {
            page = 1;
            RelatedCaller = "Trendss";
            SearchString = "";
            orderedlst = new List<Trend>();
        }
        public IPagedList<Trend> lst { get; set; }
        public List<Trend> orderedlst { get; set; }
        public Trend Trend { get; set; }
        public int page { get; set; }

        public string RelatedTarget = "Relatedtrendss";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }

}