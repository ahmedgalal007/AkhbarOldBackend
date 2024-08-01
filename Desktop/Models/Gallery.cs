using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
    public class Gallery
    {
        public int GalleryID { get; set; }
        public int pictureid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        //.ToString("yyyyMMdd")
        public DateTime? GDate { get; set; }
        public string MainPicture { get; set; }
        public string PicCaption { get; set; }
        public string Picturename { get; set; }

        

    }
}