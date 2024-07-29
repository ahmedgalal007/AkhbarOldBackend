using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class ProfileVM
    {
        public ProfileVM()
        {
            page = 1;
            HomePageFlag = false;
            waterMarkFlag = false;

            RelatedCaller = "Profiles";
            SearchString = "";
        }
        public IPagedList<Profile> lst { get; set; }
        public List<Profile> orderedlst { get; set; }
        public Profile Profile { get; set; }
        public int page { get; set; }

        public bool HomePageFlag { get; set; }
        public bool waterMarkFlag { get; set; }
        public bool SHide { get; set; }

        public string RelatedTarget = "Relatedprofiles";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }
}