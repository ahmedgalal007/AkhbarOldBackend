using Domain.Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class PollsVM
    {

        public PollsVM()
        {
            page = 1;
            RelatedCaller = "Pollss";
            SearchString = "";

            Active = true;
            pollsOptionLst = new List<PollsOption>();
        }
        public IPagedList<Polls> lst { get; set; }
        public Polls Poll { get; set; }
        public bool Active { get; set; }
        public int page { get; set; }

        public string RelatedTarget = "Relatedpollss";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

        public List<PollsOption> pollsOptionLst { get; set; }
        public PollsOption PollsOption { get; set; }

    }

}