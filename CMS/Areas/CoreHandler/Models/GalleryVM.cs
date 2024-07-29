using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class GalleryVM
    {
        public GalleryVM()
        {
            page = 1;
            HomePageFlag = false;
            waterMarkFlag = false;

            RelatedCaller = "Gallerys";
            SearchString = "";
            galleryPictureLst = new List<GalleryPictures>();
        }
        public IPagedList<Gallery> lst { get; set; }
        public Gallery Gallery { get; set; }
        public int page { get; set; }

        public bool HomePageFlag { get; set; }
        public bool waterMarkFlag { get; set; }

        public string RelatedTarget = "Relatedgallerys";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

        public List<GalleryPictures> galleryPictureLst { get; set; }
        public GalleryPictures GalleryPicture { get; set; }

     

    }
}