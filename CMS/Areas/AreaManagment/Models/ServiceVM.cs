using Domain.Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.AreaManagment.Models
{
    public class ServiceVM
    {

        public ServiceVM()
        {
            page = 1;
            RelatedCaller = "Services";
            SearchString = "";
        }

        public IPagedList<UMServices> lst { get; set; }
        public UMServices Service { get; set; }
        public int page { get; set; }
       
        public string RelatedTarget = "Relatedservices";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }

}