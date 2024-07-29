using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class UsersOpinionVM
    {

        public UsersOpinionVM()
        {
            page = 1;
            RelatedCaller = "UsersOpinions";
            SearchString = "";

            orderedlst = new List<UsersOpinion>();
        }
        public IPagedList<UsersOpinion> lst { get; set; }
        public List<UsersOpinion> orderedlst { get; set; }
        public UsersOpinion UsersOpinion { get; set; }
        public int page { get; set; }

        public string RelatedTarget = "RelatedUsersOpinions";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }


    }

}