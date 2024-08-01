using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
    public class Pictures
    {
        public int PictureID { get; set; }
        public string PictureName { get; set; }
        public string PictureCaption { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumDescription { get; set; }
        public DateTime? GDate { get; set; }
    }
}