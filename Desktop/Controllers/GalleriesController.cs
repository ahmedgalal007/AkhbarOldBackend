using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using AkhbarElyoum.Models;
using System.Text;

namespace AkhbarElyoum.Controllers
{
    public class GalleriesController : Controller
    {
        //
        // GET: /Galleries/
        AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
        public ActionResult GetNewsGalleryList(int NID)
        {
            return PartialView(GetNewsGalleries(NID));
        }

        public List<AkhbarElyoum.Models.Gallery> GetNewsGalleries(int NID)
        {
            var lst_Gallery = new List<AkhbarElyoum.Models.Gallery>();
            lst_Gallery = (from p in db.sp_GetNewsGalleries(NID)
                           select new AkhbarElyoum.Models.Gallery
                           {
                               GalleryID = p.GalleryID,
                               Title = p.Title,
                               Description = p.Description,
                               GDate = p.GDate,
                               Keywords = p.Keywords,
                               MainPicture = (p.PictureName1 != null) ? p.PictureName1 : " "
                           }).ToList();
            
            return (lst_Gallery.ToList());
        }

        public ActionResult GetHomeGalleryList(int JournalID)
        {
            return PartialView(GetHomeGalleries(JournalID));
        }

        public List<AkhbarElyoum.Models.Gallery> GetHomeGalleries(int JournalID)
        {
            var lst_Gallery = new List<AkhbarElyoum.Models.Gallery>();
            lst_Gallery = (from p in db.GetHomeGalleries(JournalID)
                           select new AkhbarElyoum.Models.Gallery
                           {
                               GalleryID = p.GalleryID,
                               Title = p.title,
                               Description = p.description,
                               GDate = p.GDate,
                               Keywords = p.Keywords,
                               MainPicture = (p.PictureName1 != null) ? p.PictureName1 : " "
                           }).ToList();
            return (lst_Gallery.ToList());
        }

        [OutputCache(Duration = 120)]
        public ActionResult Album(int ID, int JournalID)
        {
            ViewBag.RGalleryImgPath = "https://Images.akhbarelyom.com/images/images/GalleryImages/";
            return PartialView(AlbumPictures(ID, JournalID));
        }

        [OutputCache(Duration = 120)]
        public ActionResult GalleryPictures(int GalleryID, int JournalID)
        {
            var lst_Pictures = new List<Pictures>();
            lst_Pictures = (from p in db.GetHomeGalleriesPictures(GalleryID, JournalID)
                            select new AkhbarElyoum.Models.Pictures
                            {
                                PictureID = p.GalleryID,
                                PictureName = (p.PictureName != null) ? p.PictureName : " ",
                                PictureCaption = (p.PicCaption != null) ? p.PicCaption : " ",
                                AlbumTitle = (p.Title != null) ? p.Title : "",
                                AlbumDescription = (p.Description != null) ? p.Description : "",
                                GDate = p.GDate
                            }).ToList();

            ViewBag.GalleryID = GalleryID;
            ViewBag.RGalleryImgPath = "https://Images.akhbarelyom.com/images/images/GalleryImages/";

            return PartialView(lst_Pictures.ToList());
        }

        [OutputCache(Duration = 120)]
        public List<Pictures> AlbumPictures(int GalleryID, int JournalID)
        {
            var lst_Pictures = new List<Pictures>();
            lst_Pictures = (from p in db.GetHomeGalleriesPictures(GalleryID, JournalID)
                            select new AkhbarElyoum.Models.Pictures
                            {
                                PictureID = p.GalleryID,
                                PictureName = (p.PictureName != null) ? p.PictureName : " ",
                                PictureCaption = (p.PicCaption != null) ? p.PicCaption : " ",
                                AlbumTitle = (p.Title != null) ? p.Title : "",
                                AlbumDescription = (p.Description != null) ? p.Description : "",
                                GDate = p.GDate
                            }).ToList();

            ViewBag.GalleryID = GalleryID;
            ViewBag.RGalleryImgPath = "https://Images.akhbarelyom.com/images/images/GalleryImages/";

            return lst_Pictures.ToList();
        }

        public ActionResult GetPictureofDayList(int JournalID)
        {
            return PartialView(GetPictureofDay(JournalID));
        }

        public List<Pictures> GetPictureofDay(int JournalID)
        {
            var pictures = new List<Pictures>();
            pictures = (from p in db.sp_getPictureofDay(JournalID)
                        select new AkhbarElyoum.Models.Pictures
                        {
                            PictureID = p.PictureID,
                            PictureName = (p.PictureName != null) ? p.PictureName : " ",
                            PictureCaption = (p.picturetitle != null) ? p.picturetitle : " "
                        }).ToList();
            return (pictures);
        }

        public ActionResult GetComicsofDayList(int JournalID)
        {
            return PartialView(GetComicsofDay(JournalID));
        }

        public List<Pictures> GetComicsofDay(int JournalID)
        {
            var pictures = new List<Pictures>();
            pictures = (from p in db.sp_getComicsofDay(JournalID)
                        select new AkhbarElyoum.Models.Pictures
                        {
                            PictureID = p.PictureID,
                            PictureName = (p.PictureName != null) ? p.PictureName : " ",
                            PictureCaption = (p.Title != null) ? p.Title : " "
                        }).ToList();
            return (pictures);
        }







        #region NewForAlbum  by Heba   
        [OutputCache(Duration = 120)]
        public ActionResult GallerySectionTop(int JournalID)
        {
            ViewBag.Title = "ألبومات-الصور";
            return PartialView(GetGallerySectionTop(JournalID));
        }
        public List<AkhbarElyoum.Models.Gallery> GetGallerySectionTop(int JournalID)
        {
            var lst_Gallerydetails = new List<AkhbarElyoum.Models.Gallery>();
            lst_Gallerydetails = (from p in db.sp_GetGalleryTopNews(JournalID)
                               select new AkhbarElyoum.Models.Gallery
                               {
                                   GalleryID = p.GalleryID,
                                   MainPicture = (p.PictureName1 != null) ? p.PictureName1 : " ",
                                   Title = p.title,
                                   GDate = p.GDate,
                                   //GDate = (p.GDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dd MMMM yyyy}", p.GDate) : " ",
                                   Keywords = (p.Keywords != null) ? p.Keywords : " ",
                                   Description = (p.description != null) ? p.description : " " }).ToList();
            return (lst_Gallerydetails.ToList());
        }
        [OutputCache(Duration = 60)]
        public ActionResult GallerySection( int JournalID)
        {
            ViewBag.Title = "ألبومات-الصور";
            return PartialView("GallerySection", GetGallerySection(0, JournalID));
        }
        [OutputCache(Duration = 60)]
        public ActionResult GallerySectionPaged(int LastID, int JournalID)
        {
            ViewBag.Title = "ألبومات-الصور";
            List<AkhbarElyoum.Models.Gallery> lst = GetGallerySection(LastID, JournalID);
            if (Session["Gdateprev"] != null)
            {
                ViewData["GDate"] = Session["Gdateprev"];
                Session["Gdateprev"] = Session["Gdatenext"];
            }
            return PartialView(lst);
        }
        public List<AkhbarElyoum.Models.Gallery> GetGallerySection(int? LastID, int JournalID)
        {
            var lst_GalleriesDetails = new List<AkhbarElyoum.Models.Gallery>();
            lst_GalleriesDetails = (from p in db.sp_GetGallerySectionNews_paged(LastID, JournalID)
                               select new AkhbarElyoum.Models.Gallery
                               {
                                   GalleryID = p.GalleryID,
                                   MainPicture = (p.PictureName1 != null) ? p.PictureName1 : " ",
                                   Title = p.title,
                                   GDate= p.GDate,
                                   //GDate = (p.GDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dd MMMM yyyy}", p.GDate) : " ",
                                   Keywords = (p.Keywords != null) ? p.Keywords : " ",
                                   Description = (p.description != null) ? p.description : " "
                               }).ToList();
            if (LastID <= 0 && lst_GalleriesDetails.Count > 0)
            {
                Session["Gdateprev"] = "";
                Session["Gdatenext"] = "";
                ViewData["GDate"] = "";
                ViewBag.Title = lst_GalleriesDetails.FirstOrDefault().Title;
                Session["Gdateprev"] = lst_GalleriesDetails.LastOrDefault().GDate.ToString();
                //ViewData["Date"] = Session["dateprev"];
            }
            if (lst_GalleriesDetails.Count > 0 && LastID != 0)
            {
                //ViewData["Date"] = Session["dateprev"];
                Session["Gdatenext"] = lst_GalleriesDetails.LastOrDefault().GDate;

            }

            ViewBag.Title = "ألبومات-الصور";
            return (lst_GalleriesDetails.ToList());
        }

        #endregion

       
    }
}
