using Domain.Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class GalleryTypeVM
    {

        public GalleryTypeVM()
        {
            page = 1;
            RelatedCaller = "GalleryTypes";
            SearchString = "";
        }
        public IPagedList<GalleryType> lst { get; set; }
        public GalleryType GalleryType { get; set; }
        public int page { get; set; }

        public string RelatedTarget = "RelatedgalleryTypes";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }

}