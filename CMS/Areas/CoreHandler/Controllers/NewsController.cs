using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Akhbar.DBContext;
using Domain.Akhbar.DBEntities;
using Domain.Akhbar.DBBusiness;
using PagedList;
using CMS.Areas.FrameWork.Controllers;
using CMS.Areas.FrameWork.ViewModels;
using System.Web.Routing;
using CMS.Areas.CoreHandler.Models;
using System.Data.Entity.Validation;
using Newtonsoft.Json.Linq;
using System.Web.Helpers;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using CMS.Areas.UserManagment.Models;

namespace CMS.Areas.CoreHandler.Controllers
{
  
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult SocialPublish(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = " الاخبار المنشورة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && d.SectionID == vm.SecSearch && (d.NewStatus == 7 || d.NewStatus == 6 || d.NewStatus == 0), vm.page, PageSize, null);
                else
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && (d.NewStatus == 7 || d.NewStatus == 6 || d.NewStatus == 0), vm.page, PageSize, null);
            }
            else
            {
                if (vm.SecSearch != 0)
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, e => e.SectionID == vm.SecSearch && (e.NewStatus == 7 || e.NewStatus == 6 || e.NewStatus == 0), vm.page, PageSize, null);
                else
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, e => e.NewStatus == 7 || e.NewStatus == 6 || e.NewStatus == 0, vm.page, PageSize, null);
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            return View("~/Areas/CoreHandler/Views/News/SocialPublish.cshtml", vm);
        }

        // GET: News/Edit/5
        public ActionResult EditNewsForSocialMedia(int? MNewsID, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "تعديل خبر";
            if (MNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness eB = new NewsBusiness(this.DbContext);

            if (vm == null)
                vm = new NewsVM();

            vm.News = eB.Load(MNewsID);

            if (vm.News.SocialPictureId != null && vm.News.SocialPictureId.ToString() != "")
            {
                NewsPictureBusiness nbb = new NewsPictureBusiness(this.DbContext);
                vm.SocialPicture = nbb.Load(vm.News.SocialPictureId);
            }

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            ViewBag.RSocialPath = this.RelativeSocialImgPath;
            ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;

            return View("~/Areas/CoreHandler/Views/News/EditNewsForSocialMedia.cshtml", vm);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditNewsForSocialMedia(NewsVM vm)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

                var img = this.HttpContext.Request.Files["PictureName1"];

                //update can happen only during 24 hours from news publising
                if (vm.News.PublishDate != null)
                {
                    if (vm.News.PublishDate.Value.AddHours(24) <= DateTime.Now)
                    {
                        TempData["Action"] = "ForbiddenEdit";
                        ViewBag.alert = GetMessage(TempData["Action"].ToString());
                        return View("~/Areas/CoreHandler/Views/News/EditNewsForSocialMedia.cshtml", vm);
                    }
                }

                if (vm.News.SocialTitle == "" && !IsValidMainImg(img))
                {
                    TempData["Action"] = "Error";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());

                    return View("~/Areas/CoreHandler/Views/News/EditNewsForSocialMedia.cshtml", vm);
                }
                else
                {
                    if (vm.News.SocialTitle == "")
                        vm.News.SocialTitle = vm.News.Title;

                    string picName = "";

                    bool imgFlag = SaveSocialImage(ref picName, img, vm.waterMarkFlag);
                    if (imgFlag)
                    {
                        vm.SocialPicture = new NewsPictures();
                        vm.SocialPicture.PictureName = picName;
                        if (vm.News.SocialTitle == null || vm.News.SocialTitle == "")
                        {
                            vm.SocialPicture.KeyWords = vm.News.Title;
                            vm.SocialPicture.PicCaption = vm.News.Title;
                        }
                        else
                        {
                            vm.SocialPicture.KeyWords = vm.News.SocialTitle;
                            vm.SocialPicture.PicCaption = vm.News.SocialTitle;
                        }
                        vm.SocialPicture.CatID = 0;
                        vm.SocialPicture.JournalID = 1;
                        vm.SocialPicture.Source = 1;
                        vm.SocialPicture.added_date = DateTime.Now;

                        NewsPictureBusiness nbB = new NewsPictureBusiness(this.DbContext);
                        nbB.Add(vm.SocialPicture);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.SocialPicture).GetDatabaseValues();

                        vm.News.SocialPictureId = vm.SocialPicture.PictureID;

                    }

                    NewsBusiness nB = new NewsBusiness(this.DbContext);

                    //string[] probs = new string[] { "SocialTitle", "SocialPictureId" };
                    // nB.Edit(vm.News, probs);

                    vm.News.SocialEditorID = LoginUserID;
                    nB.Edit(vm.News);
                    this.SaveChanges();

                    TempData["Action"] = "Edit";

                    return RedirectToAction("SocialPublish");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        private bool SaveSocialImage(ref string picName, HttpPostedFileBase img, bool waterMarkFlag)
        {
            if (img == null || img.ContentLength <= 0)
                return false;
            string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            picName = "" + DateTimePattern + ".jpg";
            string socialFilename = this.SocialImgPath + picName;
            try
            {
                Stream strm = img.InputStream;
                Size socialImgSize = new Size(this.NewsImgSocialWidth, this.NewsImgSocialHeight);
                Bitmap bitmap = new Bitmap(Image.FromStream(strm));

                if (waterMarkFlag)
                {
                    AddWaterMark(ref bitmap, picName);
                }

                SaveImage(ref bitmap, socialImgSize, socialFilename, true);

                return true;
            }
            catch (Exception ex)
            {
                string e = ex.ToString();
                return false;
            }
        }



        // GET: News
        public ActionResult PublishedIndex(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = " الاخبار المنشورة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value,this.NewsLimit, d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && d.SectionID == vm.SecSearch && (d.NewStatus == 7 || d.NewStatus == 6 || d.NewStatus == 0), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && (d.NewStatus == 7 || d.NewStatus == 6 || d.NewStatus == 0), vm.page, PageSize, null);
                }
            }
            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, e => e.SectionID == vm.SecSearch && (e.NewStatus == 7 || e.NewStatus == 6 || e.NewStatus == 0), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, e => e.NewStatus == 7 || e.NewStatus == 6 || e.NewStatus == 0, vm.page, PageSize, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/News/PublishedIndex.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }


        // GET: News
        public ActionResult UnPublished(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = " الاخبار المنشورة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && d.SectionID == vm.SecSearch && (d.NewStatus == 7 || d.NewStatus == 6 || d.NewStatus == 0), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && (d.NewStatus == 7 || d.NewStatus == 6 || d.NewStatus == 0), vm.page, PageSize, null);
                }
            }
            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, e => e.SectionID == vm.SecSearch && (e.NewStatus == 7 || e.NewStatus == 6 || e.NewStatus == 0), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, e => e.NewStatus == 7 || e.NewStatus == 6 || e.NewStatus == 0, vm.page, PageSize, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/News/UnPublished.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }


        // GET: News/Details/5
        public ActionResult Details(int? MNewsID)
        {
            Session["CurrentService"] = "تفاصيل الخبر";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            if (MNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsBusiness EB = new NewsBusiness(this.DbContext);
            NewsVM vm = new NewsVM();
            vm.News = EB.Load(MNewsID);
            //if (vm.News.IsHome != null)
            //    vm.HomePageFlag = Convert.ToBoolean(vm.News.IsHome);

            if (vm.News == null)
            {
                return HttpNotFound();
            }
            return PartialView("~/Areas/CoreHandler/Views/News/Details.cshtml", vm);
        }


        // GET: News/Create
        public ActionResult Create()
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "إضافة خبر";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
            // ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

            NewsVM vm = new NewsVM();

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            return View("~/Areas/CoreHandler/Views/News/Create.cshtml", vm);
        }



        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NewsVM vm)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "SectionID", "CategoryID", "journalid","ParentID", "NewsType", "NewsSource" , "Title", "SubTitle",
                     "Story","Brief","quote","Keywords","PictureID1","PictureID2","PictureCaption1","PictureCaption2","PublishDate","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Creator","Notes","SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9", "SEOTitle"});

                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title, null);
                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                    ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                    ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                    ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                    ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                    ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                    ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                    FillAllowedSectionDDL(vm);
                    ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                    return View("~/Areas/CoreHandler/Views/News/Create.cshtml", vm);
                }
                else
                {
                    vm.News.Keywords = vm.tags;
                    var img = this.HttpContext.Request.Files["PictureName1"];

                    if (vm.archImgFlag && TempData["PicID"] != null)
                    {
                        int picid = Convert.ToInt32(TempData["PicID"].ToString());
                        vm.News.PictureID1 = picid;
                        NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                        vm.News.NewsPicture = nb.Load(picid);
                        TempData["PicID"] = null;
                    }
                    if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                    {
                        if (vm.News.Keywords == "" || vm.News.Keywords == null)
                            TempData["Action"] = "KeyWords";
                        else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                            TempData["Action"] = "Images";
                        else
                            TempData["Action"] = "Error";

                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        return View("~/Areas/CoreHandler/Views/News/Create.cshtml", vm);
                    }
                    else
                    {
                        if (vm.News.NewsPicture == null && vm.News.PictureID1 == null)
                        {
                            vm.News.NewsPicture = new NewsPictures();

                            string picName = "";
                            bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                            if (imgFlag)
                                vm.News.NewsPicture.PictureName = picName;
                        }

                        vm = FillOtherSections(vm);

                        nB.Add(vm.News);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.News).GetDatabaseValues();

                        vm = FillNewByLines(vm);

                        vm = FillVersionNews(vm, true);

                        vm = FillIssueNews(vm);

                        vm = FillNewsViews(vm);

                        vm = FillNewsAndNewsVersionTags(vm);

                        vm = FillNewsAlbums(vm);

                        //TODO: Check Related News
                        var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                        var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);


                        TempData["Action"] = "Create";
                        return RedirectToAction("Create");

                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }


        // GET: News/Edit/5
        public ActionResult NewsHandlerNewsPublish(int? MNewsID, NewsVM vm = null)
        {
            Session["CurrentModule"] = "معالجة الاخبار";
            Session["CurrentService"] = "نشر خبر";
            if (MNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness eB = new NewsBusiness(this.DbContext);

            if (vm == null)
                vm = new NewsVM();

            vm.News = eB.Load(MNewsID);
            foreach (NewsTag tg in vm.News.NewsTagsLst)
            {
                vm.tags += tg.Tag.Name + ",";
            }

            List<int> arr = new List<int>();
            foreach (NewsGallery ng in vm.News.NewsGalleryLst)
                arr.Add(ng.GalleryID);
            vm.Albums = arr.ToArray();
            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
            //ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50,null, null), "GalleryID", "Title");

            vm = FillSubSectionArr(vm);

            List<int> arr1 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 1))
                arr1.Add(nb.ByLineId);
            vm.ByLines = arr1.ToArray();

            List<int> arr2 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 2))
                arr2.Add(nb.ByLineId);
            vm.photograhers = arr2.ToArray();

            List<int> arr3 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 3))
                arr3.Add(nb.ByLineId);
            vm.Infograhers = arr3.ToArray();

            List<int> arr4 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 4))
                arr4.Add(nb.ByLineId);
            vm.Videograhers = arr4.ToArray();

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

            if (vm.News == null)
            {
                return HttpNotFound();
            }
           
            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            return View("~/Areas/CoreHandler/Views/News/NewsHandlerNewsPublish.cshtml", vm);
        }


        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult NewsHandlerNewsPublish(NewsVM vm)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "newID","SectionID", "NewsSource" , "Title", "SubTitle",
                      "Story","Brief","quote","Keywords","PictureID1","PictureCaption1","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Notes", "SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9", "SEOTitle"});

                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title && n.NewID != vm.News.NewID, null);


                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    fillData(vm);

                    return RedirectToAction("NewsHandlerNewsPublish", new { MNewsID = vm.News.NewID });
                }
                else
                {
                    //update can happen only during 24 hours from news publising
                    if (vm.News.PublishDate != null)
                    {
                        if (vm.News.PublishDate.Value.AddHours(24) <= DateTime.Now)
                        {
                            TempData["Action"] = "ForbiddenEdit";
                            ViewBag.alert = GetMessage(TempData["Action"].ToString());
                            fillData(vm);

                            return RedirectToAction("NewsHandlerNewsPublish", new { MNewsID = vm.News.NewID });
                        }
                    }

                    var img = this.HttpContext.Request.Files["PictureName1"];
                    vm.News.Keywords = vm.tags;

                    if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                    {
                        if (vm.News.Keywords == "" || vm.News.Keywords == null)
                            TempData["Action"] = "KeyWords";
                        else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                            TempData["Action"] = "Images";
                        else
                            TempData["Action"] = "Error";

                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        return RedirectToAction("NewsHandlerNewsPublish", new { MNewsID = vm.News.NewID });
                    }

                    else
                    {
                        if (vm.archImgFlag && TempData["PicID"] != null)
                        {
                            int picid = Convert.ToInt32(TempData["PicID"].ToString());
                            vm.News.PictureID1 = picid;
                            NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                            vm.News.NewsPicture = nb.Load(picid);
                            TempData["PicID"] = null;
                        }
                        else
                        {
                            string picName = "";
                            if (vm.News.NewsPicture == null)
                                vm.News.NewsPicture = new NewsPictures();

                            bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                            if (imgFlag)
                                vm.News.NewsPicture.PictureName = picName;
                        }

                        vm = SetDefaultValues(vm, false, false, true, false, false);

                        vm = FillOtherSections(vm);

                        // add Edit after rejection Flag
                        if (vm.News.NewStatus == 8 || vm.News.NewStatus == 9)
                            vm.News.EditAfterRejectFlag = 1;

                        nB.Edit(vm.News);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.News).GetDatabaseValues();

                        vm = FillNewByLines(vm);

                        vm = FillVersionNews(vm, false);

                        vm = FillNewsAndNewsVersionTags(vm);

                        vm = FillNewsAlbums(vm);

                        var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                        var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);

                        Publish(vm);

                        TempData["Action"] = "Publish";


                        ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
                        return RedirectToAction("NewsHandlerNewsPublish", new { MNewsID = vm.News.NewID });

                        // return RedirectToAction("NewsHandler");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        private void fillData(NewsVM vm)
        {
            GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
            ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

        }

        // GET: News/Edit/5
        public ActionResult Edit(int? MNewsID, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "تعديل خبر";
            if (MNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness eB = new NewsBusiness(this.DbContext);

            if (vm == null)
                vm = new NewsVM();

            vm.News = eB.Load(MNewsID);
            foreach (NewsTag tg in vm.News.NewsTagsLst)
            {
                vm.tags += tg.Tag.Name + ",";
            }

            List<int> arr = new List<int>();
            foreach (NewsGallery ng in vm.News.NewsGalleryLst)
                arr.Add(ng.GalleryID);
            vm.Albums = arr.ToArray();
            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
            // ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50,null, null), "GalleryID", "Title");

            vm = FillSubSectionArr(vm);

            List<int> arr1 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 1))
                arr1.Add(nb.ByLineId);
            vm.ByLines = arr1.ToArray();

            List<int> arr2 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 2))
                arr2.Add(nb.ByLineId);
            vm.photograhers = arr2.ToArray();

            List<int> arr3 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 3))
                arr3.Add(nb.ByLineId);
            vm.Infograhers = arr3.ToArray();

            List<int> arr4 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 4))
                arr4.Add(nb.ByLineId);
            vm.Videograhers = arr4.ToArray();

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

            if (vm.News == null)
            {
                return HttpNotFound();
            }

            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
            return View("~/Areas/CoreHandler/Views/News/Edit.cshtml", vm);
        }


        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(NewsVM vm)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "newID","SectionID", "NewsSource" , "Title", "SubTitle",
                     "Story","Brief","quote","Keywords","PictureID1","PictureCaption1","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Notes","SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9", "SEOTitle"});

                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title && n.NewID != vm.News.NewID, null);
                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    fillData(vm);
                    return View("~/Areas/CoreHandler/Views/News/Edit.cshtml", vm);
                }
                else
                {
                    //update can happen only during 24 hours from news publising
                    if (vm.News.PublishDate != null)
                    {
                        if (vm.News.PublishDate.Value.AddHours(24) <= DateTime.Now)
                        {
                            TempData["Action"] = "ForbiddenEdit";
                            ViewBag.alert = GetMessage(TempData["Action"].ToString());
                            fillData(vm);
                            return View("~/Areas/CoreHandler/Views/News/Edit.cshtml", vm);
                        }
                    }

                    var img = this.HttpContext.Request.Files["PictureName1"];
                    vm.News.Keywords = vm.tags;

                    if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                    {
                        if (vm.News.Keywords == "" || vm.News.Keywords == null)
                            TempData["Action"] = "KeyWords";
                        else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                            TempData["Action"] = "Images";
                        else
                            TempData["Action"] = "Error";

                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        return View("~/Areas/CoreHandler/Views/News/Edit.cshtml", vm);
                    }

                    else
                    {
                        if (vm.archImgFlag && TempData["PicID"] != null)
                        {
                            int picid = Convert.ToInt32(TempData["PicID"].ToString());
                            vm.News.PictureID1 = picid;
                            NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                            vm.News.NewsPicture = nb.Load(picid);
                            TempData["PicID"] = null;
                        }
                        else
                        {
                            string picName = "";
                            if (vm.News.NewsPicture == null)
                                vm.News.NewsPicture = new NewsPictures();

                            bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                            if (imgFlag)
                                vm.News.NewsPicture.PictureName = picName;
                        }

                        vm = SetDefaultValues(vm, false, false, false, false, false);

                        NewsPictureBusiness nbB = new NewsPictureBusiness(this.DbContext);
                        nbB.Edit(vm.News.NewsPicture);
                        this.SaveChanges();

                        vm = FillOtherSections(vm);

                        // add Edit after rejection Flag
                        if (vm.News.NewStatus == 8 || vm.News.NewStatus == 9)
                            vm.News.EditAfterRejectFlag = 1;

                        nB.Edit(vm.News);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.News).GetDatabaseValues();

                        vm = FillNewByLines(vm);

                        vm = FillVersionNews(vm, false);

                        vm = FillNewsAndNewsVersionTags(vm);

                        vm = FillNewsAlbums(vm);

                        var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                        var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);

                        TempData["Action"] = "Edit";
                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        GalleryBusiness gB = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
                        return View("~/Areas/CoreHandler/Views/News/Edit.cshtml", vm);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }


        // GET: News/Edit/5
        public ActionResult EditUnPublished(int? MNewsID, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "تعديل خبر";
            if (MNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness eB = new NewsBusiness(this.DbContext);

            if (vm == null)
                vm = new NewsVM();

            vm.News = eB.Load(MNewsID);
            if (vm.News.NewStatus != 7 && vm.News.Approved != 1)
            {
                foreach (NewsTag tg in vm.News.NewsTagsLst)
                {
                    vm.tags += tg.Tag.Name + ",";
                }

                List<int> arr = new List<int>();
                foreach (NewsGallery ng in vm.News.NewsGalleryLst)
                    arr.Add(ng.GalleryID);
                vm.Albums = arr.ToArray();
                GalleryBusiness gB = new GalleryBusiness(this.DbContext);
                // ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
                ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                vm = FillSubSectionArr(vm);

                List<int> arr1 = new List<int>();
                foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 1))
                    arr1.Add(nb.ByLineId);
                vm.ByLines = arr1.ToArray();

                List<int> arr2 = new List<int>();
                foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 2))
                    arr2.Add(nb.ByLineId);
                vm.photograhers = arr2.ToArray();

                List<int> arr3 = new List<int>();
                foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 3))
                    arr3.Add(nb.ByLineId);
                vm.Infograhers = arr3.ToArray();

                List<int> arr4 = new List<int>();
                foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 4))
                    arr4.Add(nb.ByLineId);
                vm.Videograhers = arr4.ToArray();

                if (vm.News == null)
                {
                    return HttpNotFound();
                }

                ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                FillAllowedSectionDDL(vm);
                ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                if (TempData["Action"] != null)
                {
                    if (TempData["Action"].ToString() != "")
                        ViewBag.alert = GetMessage(TempData["Action"].ToString());
                }

                ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
                return View("~/Areas/CoreHandler/Views/News/EditUnPublished.cshtml", vm);
            }
            else
            {
                TempData["Action"] = "NotPermitted";
                ViewBag.alert = GetMessage(TempData["Action"].ToString());
                return RedirectToAction("MyNews");
            }
        }


        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditUnPublished(NewsVM vm)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "newID","SectionID", "NewsSource" , "Title", "SubTitle",
                     "Story","Brief","quote","Keywords","PictureID1","PictureCaption1","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Notes", "SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9", "SEOTitle"});

                var img = this.HttpContext.Request.Files["PictureName1"];
                vm.News.Keywords = vm.tags;
                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title && n.NewID != vm.News.NewID, null);
                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    fillData(vm);

                    ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                    return View("~/Areas/CoreHandler/Views/News/NewsPass.cshtml", vm);
                }
                else
                {
                    if (vm.News.NewStatus != 7 && vm.News.Approved != 1)
                    {
                        if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                        {
                            if (vm.News.Keywords == "" || vm.News.Keywords == null)
                                TempData["Action"] = "KeyWords";
                            else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                                TempData["Action"] = "Images";
                            else
                                TempData["Action"] = "Error";

                            ViewBag.alert = GetMessage(TempData["Action"].ToString());
                            fillData(vm);

                            return View("~/Areas/CoreHandler/Views/News/EditUnPublished.cshtml", vm);
                        }

                        else
                        {
                            if (vm.archImgFlag && TempData["PicID"] != null)
                            {
                                int picid = Convert.ToInt32(TempData["PicID"].ToString());
                                vm.News.PictureID1 = picid;
                                NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                                vm.News.NewsPicture = nb.Load(picid);
                                TempData["PicID"] = null;
                            }
                            else
                            {
                                string picName = "";
                                if (vm.News.NewsPicture == null)
                                    vm.News.NewsPicture = new NewsPictures();

                                bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                                if (imgFlag)
                                    vm.News.NewsPicture.PictureName = picName;
                            }

                            vm = SetDefaultValues(vm, false, false, false, false, false);
                            NewsPictureBusiness nbB = new NewsPictureBusiness(this.DbContext);
                            nbB.Edit(vm.News.NewsPicture);
                            this.SaveChanges();

                            vm = FillOtherSections(vm);

                            // add Edit after rejection Flag
                            if (vm.News.NewStatus == 8 || vm.News.NewStatus == 9)
                                vm.News.EditAfterRejectFlag = 1;

                            // add Edit after rejection Flag
                            if (vm.News.NewStatus == 8 || vm.News.NewStatus == 9)
                                vm.News.EditAfterRejectFlag = 1;

                            nB.Edit(vm.News);
                            this.SaveChanges();

                            this.DbContext.Entry(vm.News).GetDatabaseValues();

                            vm = FillNewByLines(vm);

                            vm = FillVersionNews(vm, false);

                            vm = FillNewsAndNewsVersionTags(vm);

                            vm = FillNewsAlbums(vm);

                            var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                            var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);

                            TempData["Action"] = "Edit";
                            ViewBag.alert = GetMessage(TempData["Action"].ToString());

                            FillAllowedSectionDDL(vm);
                            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
                            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                            ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
                            return View("~/Areas/CoreHandler/Views/News/EditUnPublished.cshtml", vm);
                        }
                    }
                    else
                    {
                        TempData["Action"] = "NotPermitted";
                        ViewBag.alert = GetMessage(TempData["Action"].ToString());
                        return View("~/Areas/CoreHandler/Views/News/EditUnPublished.cshtml", vm);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }


        // GET: News/Create
        public ActionResult NewsPass()
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "تمرير خبر";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
            // ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50,null, null), "GalleryID", "Title");

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            NewsVM vm = new NewsVM();
            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            return View("~/Areas/CoreHandler/Views/News/NewsPass.cshtml", vm);
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsPass(NewsVM vm)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "SectionID", "CategoryID", "journalid","ParentID", "NewsType", "NewsSource" , "Title", "SubTitle",
                     "Story","Brief","quote","Keywords","PictureID1","PictureID2","PictureCaption1","PictureCaption2","PublishDate","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Creator","Notes", "SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9", "SEOTitle"});

                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title, null);
                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                    ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                    ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                    ViewBag.ByLines = new SelectList(blBuss.Load(e=>e.Active==true, null), "ByLineId", "ByLineName");
                    ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                    ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                    ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                    FillAllowedSectionDDL(vm);
                    ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                    return View("~/Areas/CoreHandler/Views/News/NewsPass.cshtml", vm);
                }
                else
                {
                    vm.News.Keywords = vm.tags;
                    var img = this.HttpContext.Request.Files["PictureName1"];

                    if (vm.archImgFlag && TempData["PicID"] != null)
                    {
                        int picid = Convert.ToInt32(TempData["PicID"].ToString());
                        vm.News.PictureID1 = picid;
                        NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                        vm.News.NewsPicture = nb.Load(picid);
                        TempData["PicID"] = null;
                    }

                    if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                    {
                        if (vm.News.Keywords == "" || vm.News.Keywords == null)
                            TempData["Action"] = "KeyWords";
                        else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                            TempData["Action"] = "Images";
                        else
                            TempData["Action"] = "Error";

                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        return View("~/Areas/CoreHandler/Views/News/NewsPass.cshtml", vm);
                    }

                    else
                    {
                        if (vm.News.NewsPicture == null && vm.News.PictureID1 == null)
                        {
                            vm.News.NewsPicture = new NewsPictures();

                            string picName = "";
                            bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                            if (imgFlag)
                                vm.News.NewsPicture.PictureName = picName;
                        }

                        vm = SetDefaultValues(vm, true, true, false, false, false);

                        Pass(vm);

                        vm = FillOtherSections(vm);

                        nB.Add(vm.News);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.News).GetDatabaseValues();

                        vm = FillNewByLines(vm);

                        vm = FillVersionNews(vm, true);

                        vm = FillIssueNews(vm);

                        vm = FillNewsViews(vm);

                        vm = FillNewsAndNewsVersionTags(vm);

                        vm = FillNewsAlbums(vm);

                        var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                        var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);

                        TempData["Action"] = "NewsPass";

                        AssignDesk(vm);

                        return RedirectToAction("NewsPass");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

      
        // GET: News/Create
        public ActionResult NewsPublish()
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "نشر خبر";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
            // ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50,null, null), "GalleryID", "Title");

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");


            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            NewsVM vm = new NewsVM();
            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            return View("~/Areas/CoreHandler/Views/News/NewsPublish.cshtml", vm);
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsPublish(NewsVM vm)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "SectionID", "CategoryID", "journalid","ParentID", "NewsType", "NewsSource" , "Title", "SubTitle",
                     "Story","Brief","quote","Keywords","PictureID1","PictureID2","PictureCaption1","PictureCaption2","PublishDate","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Creator","Notes" ,"SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9", "SEOTitle"});

                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title, null);
                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                    ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                    ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                    ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                    ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                    ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                    ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                    FillAllowedSectionDDL(vm);
                    ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                    return View("~/Areas/CoreHandler/Views/News/NewsPublish.cshtml", vm);
                }
                else
                {
                    vm.News.Keywords = vm.tags;
                    var img = this.HttpContext.Request.Files["PictureName1"];

                    if (vm.archImgFlag && TempData["PicID"] != null)
                    {
                        int picid = Convert.ToInt32(TempData["PicID"].ToString());
                        vm.News.PictureID1 = picid;
                        NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                        vm.News.NewsPicture = nb.Load(picid);
                        TempData["PicID"] = null;
                    }

                    if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                    {
                        if (vm.News.Keywords == "" || vm.News.Keywords == null)
                            TempData["Action"] = "KeyWords";
                        else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                            TempData["Action"] = "Images";
                        else
                            TempData["Action"] = "Error";

                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        return View("~/Areas/CoreHandler/Views/News/NewsPublish.cshtml", vm);
                    }

                    else
                    {
                        if (vm.News.NewsPicture == null && vm.News.PictureID1 == null)
                        {
                            vm.News.NewsPicture = new NewsPictures();

                            string picName = "";
                            bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                            if (imgFlag)
                                vm.News.NewsPicture.PictureName = picName;
                        }

                        vm = SetDefaultValues(vm, true, false, true, false, false);

                        vm = FillOtherSections(vm);

                        nB.Add(vm.News);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.News).GetDatabaseValues();

                        vm = FillNewByLines(vm);

                        vm = FillVersionNews(vm, true);

                        vm = FillIssueNews(vm);

                        vm = FillNewsViews(vm);

                        vm = FillNewsAndNewsVersionTags(vm);

                        vm = FillNewsAlbums(vm);

                        var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                        var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);

                        Publish(vm);

                        TempData["Action"] = "Publish";
                        return RedirectToAction("EditPublishedNews",new { MNewsID=vm.News.NewID, vm });
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        // GET: News/Edit/5
        public ActionResult EditPublishedNews(int? MNewsID, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "تعديل خبر";
            if (MNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness eB = new NewsBusiness(this.DbContext);

            if (vm == null)
                vm = new NewsVM();

            vm.News = eB.Load(MNewsID);
            foreach (NewsTag tg in vm.News.NewsTagsLst)
            {
                vm.tags += tg.Tag.Name + ",";
            }

            vm.oldSecId = vm.News.SectionID;

            List<int> arr = new List<int>();
            foreach (NewsGallery ng in vm.News.NewsGalleryLst)
                arr.Add(ng.GalleryID);
            vm.Albums = arr.ToArray();
            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
            //ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50,null, null), "GalleryID", "Title");

            vm = FillSubSectionArr(vm);

            List<int> arr1 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 1))
                arr1.Add(nb.ByLineId);
            vm.ByLines = arr1.ToArray();

            List<int> arr2 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 2))
                arr2.Add(nb.ByLineId);
            vm.photograhers = arr2.ToArray();

            List<int> arr3 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 3))
                arr3.Add(nb.ByLineId);
            vm.Infograhers = arr3.ToArray();

            List<int> arr4 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 4))
                arr4.Add(nb.ByLineId);
            vm.Videograhers = arr4.ToArray();

            if (vm.News == null)
            {
                return HttpNotFound();
            }

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
            return View("~/Areas/CoreHandler/Views/News/EditPublishedNews.cshtml", vm);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditPublishedNews(NewsVM vm)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "newID","SectionID", "NewsSource" , "Title", "SubTitle",
                     "Story","Brief","quote","Keywords","PictureID1","PictureCaption1","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Notes", "SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9","SEOTitle"});

                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title && n.NewID != vm.News.NewID, null);
                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    fillData(vm);

                    return View("~/Areas/CoreHandler/Views/News/EditPublishedNews.cshtml", vm);
                }
                else
                {

                    //update can happen only during 24 hours from news publising
                    if (vm.News.PublishDate != null)
                    {
                        if (vm.News.PublishDate.Value.AddHours(24) <= DateTime.Now)
                        {
                            TempData["Action"] = "ForbiddenEdit";
                            ViewBag.alert = GetMessage(TempData["Action"].ToString());
                            fillData(vm);

                            return View("~/Areas/CoreHandler/Views/News/EditPublishedNews.cshtml", vm);
                        }
                    }

                    vm.News.Keywords = vm.tags;
                    var img = this.HttpContext.Request.Files["PictureName1"];

                    if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                    {
                        if (vm.News.Keywords == "" || vm.News.Keywords == null)
                            TempData["Action"] = "KeyWords";
                        else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                            TempData["Action"] = "Images";
                        else
                            TempData["Action"] = "Error";

                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
                        return View("~/Areas/CoreHandler/Views/News/EditPublishedNews.cshtml", vm);
                    }
                    else
                    {

                        if (vm.archImgFlag && TempData["PicID"] != null)
                        {
                            int picid = Convert.ToInt32(TempData["PicID"].ToString());
                            vm.News.PictureID1 = picid;
                            NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                            vm.News.NewsPicture = nb.Load(picid);
                            TempData["PicID"] = null;
                        }
                        else
                        {
                            string picName = "";
                            if (vm.News.NewsPicture == null)
                                vm.News.NewsPicture = new NewsPictures();

                            bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                            if (imgFlag)
                                vm.News.NewsPicture.PictureName = picName;
                        }

                        vm = SetDefaultValues(vm, false, false, false, false, false);
                        
                        NewsPictureBusiness nbB = new NewsPictureBusiness(this.DbContext);
                        nbB.Edit(vm.News.NewsPicture);
                        this.SaveChanges();

                        vm = FillOtherSections(vm);

                        // add Edit after rejection Flag
                        if (vm.News.NewStatus == 8 || vm.News.NewStatus == 9)
                            vm.News.EditAfterRejectFlag = 1;

                        nB.Edit(vm.News);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.News).GetDatabaseValues();

                        vm = FillNewByLines(vm);

                        vm = FillVersionNews(vm, false);

                        vm = FillNewsAndNewsVersionTags(vm);

                        vm = FillNewsAlbums(vm);

                        var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                        var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);

                        EditPublish(vm);

                        TempData["Action"] = "Edit";
                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        GalleryBusiness gB = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");
                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
                        return View("~/Areas/CoreHandler/Views/News/EditPublishedNews.cshtml", vm);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }

        // GET: News/Delete/5
        public ActionResult DeletePublishedNews(int? MNewsID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsSectionBusiness nsB = new NewsSectionBusiness(this.DbContext);
            IssuesTopNewBusiness iTNB = new IssuesTopNewBusiness(this.DbContext);
            IssuesNewBusiness iNB = new IssuesNewBusiness(this.DbContext);
            NewsGalleryBusiness nGB = new NewsGalleryBusiness(this.DbContext);
            NewsPollBusiness nPB = new NewsPollBusiness(this.DbContext);
            TopNewsBusiness tNB = new TopNewsBusiness(this.DbContext);
            UsersOpinionBusiness uOB = new UsersOpinionBusiness(this.DbContext);
            NewsBusiness nB = new NewsBusiness(this.DbContext);

            List<NewsSection> ns = nsB.Load(e => e.NewsID == MNewsID);
            foreach (var item in ns)
            {
                nsB.Delete(item);
                this.SaveChanges();
            }
            List<IssuesTopNew> itn = iTNB.Load(e => e.NewsID == MNewsID);
            foreach (var item in itn)
            {
                iTNB.Delete(item);
                this.SaveChanges();
            }
            List<NewsPoll> np = nPB.Load(e => e.NewsID == MNewsID);
            foreach (var item in np)
            {
                nPB.Delete(item);
                this.SaveChanges();
            }
            List<TopNews> tn = tNB.Load(e => e.NewsID == MNewsID);
            foreach (var item in tn)
            {
                tNB.Delete(item);
                this.SaveChanges();
            }
            List<UsersOpinion> uo = uOB.Load(e => e.NewID == MNewsID);
            foreach (var item in uo)
            {
                uOB.Delete(item);
                this.SaveChanges();
            }
            News n = nB.Load(e => e.NewID == MNewsID).FirstOrDefault();
            // To Resort Top news after Unpublished 
            int count = 0;
            tn = tNB.Load(e => e.SecID == n.SectionID);
            foreach (var item in tn.OrderBy(e => e.DisplayOrder))
            {
                if (item.DisplayOrder != count + 1)
                {
                    item.DisplayOrder = count + 1;
                    tNB.Edit(item);
                    this.SaveChanges();
                }
                count++;
            }
            nB.Delete(n);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("UnPublished");
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? MNewsID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsSectionBusiness nsB = new NewsSectionBusiness(this.DbContext);
            IssuesTopNewBusiness iTNB = new IssuesTopNewBusiness(this.DbContext);
            IssuesNewBusiness iNB = new IssuesNewBusiness(this.DbContext);
            NewsGalleryBusiness nGB = new NewsGalleryBusiness(this.DbContext);
            NewsPollBusiness nPB = new NewsPollBusiness(this.DbContext);
            TopNewsBusiness tNB = new TopNewsBusiness(this.DbContext);
            UsersOpinionBusiness uOB = new UsersOpinionBusiness(this.DbContext);
            NewsBusiness nB = new NewsBusiness(this.DbContext);

            List<NewsSection> ns = nsB.Load(e => e.NewsID == MNewsID);
            foreach (var item in ns)
            {
                nsB.Delete(item);
                this.SaveChanges();
            }
            List<IssuesTopNew> itn = iTNB.Load(e => e.NewsID == MNewsID);
            foreach (var item in itn)
            {
                iTNB.Delete(item);
                this.SaveChanges();
            }
            List<NewsPoll> np = nPB.Load(e => e.NewsID == MNewsID);
            foreach (var item in np)
            {
                nPB.Delete(item);
                this.SaveChanges();
            }
            List<TopNews> tn = tNB.Load(e => e.NewsID == MNewsID);
            foreach (var item in tn)
            {
                tNB.Delete(item);
                this.SaveChanges();
            }
            List<UsersOpinion> uo = uOB.Load(e => e.NewID == MNewsID);
            foreach (var item in uo)
            {
                uOB.Delete(item);
                this.SaveChanges();
            }

            News n = nB.Load(e => e.NewID == MNewsID).FirstOrDefault();
            // To Resort Top news after Unpublished 
            int count = 0;
            tn = tNB.Load(e => e.SecID == n.SectionID);
            foreach (var item in tn.OrderBy(e => e.DisplayOrder))
            {
                if (item.DisplayOrder != count + 1)
                {
                    item.DisplayOrder = count + 1;
                    tNB.Edit(item);
                    this.SaveChanges();
                }
                count++;
            }

            nB.Delete(n);
            this.SaveChanges();

            TempData["Action"] = "Delete";

            return RedirectToAction("NewsHandler");
        }

        
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadFiles(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {

            //http://stackoverflow.com/a/4088194/167670
            //http://arturito.net/2010/11/03/file-and-image-upload-with-asp-net-mvc2-with-ckeditor-wysiwyg-rich-text-editor/
            //http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx

            if (upload.ContentLength <= 0)
                return null;

            string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var url = "";
            string message;
            var output = "";

            string exten = Path.GetExtension(upload.FileName);
            //embed is doc or pdf or gif file
            if (IsValidDoc(upload))
            {
                try
                {
                    string picName = "" + DateTimePattern + exten;
                    string embedFilename = this.EmbededImagePath + picName;

                    url = string.Format("{0}{1}", this.ReltativeEmbededImagesPath, picName);

                    upload.SaveAs(embedFilename);

                    // passing message File/failure
                    message = "تم إضافة الملف بنجاح";

                    // since it is an ajax request it requires this string
                    output = string.Format(
                        "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                        CKEditorFuncNum, url, message);
                }
                catch (Exception ex)
                {
                }
            }
            //embed is image file  -- for case join image on href
            else if (IsValidMainImg(upload))
            {
                string picName = "" + DateTimePattern + ".jpg";
                string embedFilename = this.EmbededImagePath + picName;

                try
                {
                    Stream strm = upload.InputStream;
                    Bitmap bitmap = new Bitmap(Image.FromStream(strm));
                    Size imgSize = new Size(bitmap.Width, bitmap.Height);

                    SaveImage(ref bitmap, imgSize, embedFilename, false);

                    url = string.Format("{0}{1}", this.ReltativeEmbededImagesPath, picName);

                    // passing message success/failure
                    message = "تم إضافة الصورة بنجاح";

                    // since it is an ajax request it requires this string
                    output = string.Format(
                        "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                        CKEditorFuncNum, url, message);

                }
                catch (Exception ex)
                { }
            }
            else
            {
                // passing message not Accepted Format
                message = "نوع الملف غير مسموح";
                url = "";
                // since it is an ajax request it requires this string
                output = string.Format(
                    "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                    CKEditorFuncNum, url, message);
            }

            return Content(output);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {

            //http://stackoverflow.com/a/4088194/167670
            //http://arturito.net/2010/11/03/file-and-image-upload-with-asp-net-mvc2-with-ckeditor-wysiwyg-rich-text-editor/
            //http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx

            if (upload.ContentLength <= 0)
                return null;

            string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var url = "";
            string message;
            var output = "";
            //embed is image file
            if (IsValidMainImg(upload))
            {
                string exten = Path.GetExtension(upload.FileName);
                string picName = "";
                if (exten.ToLower() == ".gif")
                {
                    picName = "" + DateTimePattern + exten;
                }
                else
                {
                    picName = "" + DateTimePattern + ".jpg";

                }
                string embedFilename = this.EmbededImagePath + picName;

                try
                {
                    if (exten.ToLower() == ".gif")
                    {
                        upload.SaveAs(embedFilename);
                    }
                    else
                    {
                        Stream strm = upload.InputStream;
                        Bitmap bitmap = new Bitmap(Image.FromStream(strm));
                        Size imgSize = new Size(bitmap.Width, bitmap.Height);

                        SaveImage(ref bitmap, imgSize, embedFilename, false);
                    }

                    url = string.Format("{0}{1}", this.ReltativeEmbededImagesPath, picName);

                    // passing message success/failure
                    message = "تم إضافة الصورة بنجاح";

                    // since it is an ajax request it requires this string
                    output = string.Format(
                        "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                        CKEditorFuncNum, url, message);

                }
                catch (Exception ex)
                { }
                
            }          
            else
            {
                // passing message not Accepted Format
                message = "نوع الملف غير مسموح";
                url = "";
                // since it is an ajax request it requires this string
                output = string.Format(
                    "<html><body><script>window.parent.CKEDITOR.tools.callFunction({0}, \"{1}\", \"{2}\");</script></body></html>",
                    CKEditorFuncNum, url, message);
            }

            return Content(output);
        }


        // GET: News/Pass/5
        public ActionResult Pass(int? MNewsID, string CallerScreen)
        {
            NewsVM nvm = new NewsVM();
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness nB = new NewsBusiness(this.DbContext);
            if (MNewsID != 0)
            {
                nvm.News = nB.Load(MNewsID);
                if (nvm.News != null)
                {
                    nvm = Pass(nvm);
                    try
                    {
                        nB.Edit(nvm.News);
                        this.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    TempData["Action"] = "NewsPass";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());

                }
            }
            if (CallerScreen == "MyNews")
                return RedirectToAction("MyNews");
            else
                return RedirectToAction("NewsHandler");
        }

        public ActionResult NewsVersions(int? MNewsID)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "اصدارات الخبر";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsVersionBusiness nvB = new NewsVersionBusiness(this.DbContext);
            NewsVM vm = new NewsVM();
            vm.VLst = nvB.Load(q => q.Take(50).OrderByDescending(d => d.VersionId), d => d.NewID == MNewsID, vm.page, PageSize, null);
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }
            return View("~/Areas/CoreHandler/Views/News/NewsVersions.cshtml", vm);
        }

        public ActionResult RestoreNewsVersion(int? MNewsID, int? VNewsID)
        {
            if (MNewsID == null || VNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            // NewsVersionBusiness nvB = new NewsVersionBusiness(this.DbContext);
            NewsBusiness eB = new NewsBusiness(this.DbContext);
            NewsVM vm = new NewsVM();
            vm.News = eB.Load(MNewsID);

            //update can happen only during 24 hours from news publising
            if (vm.News.PublishDate != null)
            {
                if (vm.News.PublishDate.Value.AddHours(24) <= DateTime.Now)
                {
                    TempData["Action"] = "ForbiddenEdit";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());

                    return RedirectToAction("NewsVersions", new { MNewsID = MNewsID });
                }
            }

            vm.NewsVersion = vm.News.NewsVersionsLst.Where(e => e.VersionId == VNewsID).FirstOrDefault();

            vm.News.SectionID = vm.NewsVersion.SectionID;
            vm.News.JournalID = vm.NewsVersion.JournalID;
            vm.News.CategoryID = vm.NewsVersion.CategoryID;
            vm.News.ParentID = vm.NewsVersion.ParentID;
            vm.News.NewsType = vm.NewsVersion.NewsType;
            vm.News.NewsSource = vm.NewsVersion.NewsSource;
            vm.News.Title = vm.NewsVersion.Title;
            vm.News.SubTitle = vm.NewsVersion.SubTitle;
            vm.News.Story = vm.NewsVersion.Story;
            vm.News.Brief = vm.NewsVersion.Brief;
            vm.News.quote = vm.NewsVersion.quote;
            vm.News.Keywords = vm.NewsVersion.Keywords;
            vm.News.PictureID1 = vm.NewsVersion.PictureID1;
            vm.News.PictureID2 = vm.NewsVersion.PictureID2;
            vm.News.PictureCaption1 = vm.NewsVersion.PictureCaption1;
            vm.News.PictureCaption2 = vm.NewsVersion.PictureCaption2;
            vm.News.EditorID = vm.NewsVersion.EditorID;
            vm.News.ByLine = vm.NewsVersion.ByLine;

            eB.Edit(vm.News);
            this.SaveChanges();
            FillVersionNews(vm, false);

            TempData["Action"] = "Restore";

            return RedirectToAction("NewsVersions", new { MNewsID = MNewsID });
        }

        public ActionResult HomePageNewsSort(int? secId, string message)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "ترتيب الاخبار";

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            NewsVM vm = new NewsVM();
            vm = FillSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (message != null)
                ViewBag.alert = GetMessage(message);
            
            return View("~/Areas/CoreHandler/Views/news/HomePageNewsSort.cshtml", vm);
        }

        public ActionResult LoadListToOrder(int? secId)
        {
            if (secId == null)
                secId = 0;
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsVM vm = new NewsVM();
            var sp = new m_SP_SelectNewsToArrange() { CatID = 1, JournalID = 1, SectionID = secId.Value };
            vm.NewsForSort = this.DbContext.Database.ExecuteStoredProcedure(sp).ToList();

            return PartialView("~/Areas/CoreHandler/Views/news/LoadListToOrder.cshtml", vm);
        }

        
        public ActionResult SaveHomePageNewsSort(string Ids, int secId, NewsVM vm = null)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                TopNewsBusiness tNB = new TopNewsBusiness(this.DbContext);
                var newArray = JArray.Parse(Ids).Select(x => (int)x).ToArray();

                var resList = tNB.Load(null, null).OrderBy(e => e.DisplayOrder);

                vm.TopNewsOrderedlst = resList.ToList();

                for (int i = 0; i < newArray.Count(); i++)
                {
                    if (i < 8)
                    {
                        vm.TopNews = vm.TopNewsOrderedlst.FirstOrDefault(e => e.NewsID == newArray[i] && e.SecID == secId && e.CatID == 1);
                        if (vm.TopNews == null)
                        {
                            vm.TopNewsTemp = tNB.Load(e => e.SecID == secId && e.CatID == 1 && e.DisplayOrder == i + 1).FirstOrDefault();
                            if (vm.TopNewsTemp != null)
                            {
                                tNB.Delete(vm.TopNewsTemp);
                                this.SaveChanges();
                            }

                            vm.TopNews = new TopNews();
                            vm.TopNews.SecID = secId;
                            vm.TopNews.NewsID = newArray[i];
                            vm.TopNews.CatID = 1;
                            vm.TopNews.DisplayOrder = i + 1;
                            vm.TopNews.JournalID = 1;
                            Func<TopNews, bool> expr1 = (x => x.NewsID == newArray[i]);
                            Func<TopNews, bool> expr2 = (x => x.SecID == secId);
                            Func<TopNews, bool> exprs = (x => expr1(x) && expr2(x));
                            tNB.AddIfNotExists(vm.TopNews, exprs);
                            this.SaveChanges();
                           
                        }
                        else
                        {
                            vm.TopNews.DisplayOrder = i + 1;
                            if (vm.TopNews.JournalID.ToString() == "")
                                vm.TopNews.JournalID = 1;
                            tNB.Edit(vm.TopNews);
                            this.SaveChanges();
                        }
                       
                    }
                }

                vm.TopNewsOrderedlst = tNB.Load(e => e.SecID == secId && e.CatID == 1 && e.JournalID == 1 && e.DisplayOrder > 8).ToList();
                foreach (var item in vm.TopNewsOrderedlst)
                {
                    tNB.Delete(item);
                    this.SaveChanges();
                }

                TempData["Action"] = "Sorted";
                //ViewBag.alert = GetMessage(TempData["Action"].ToString());

                return RedirectToAction("HomePageNewsSort", new { secId = secId, message = TempData["Action"] });
                //View("~/Areas/CoreHandler/Views/MainSection/HomePageNewsSort.cshtml", vm);
            }
            catch
            {
                TempData["Action"] = "Error";
                return RedirectToAction("HomePageNewsSort", new { message = TempData["Action"] });
            }


        }

        public ActionResult MyNews(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "أخباري";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;


            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID,this.NewsLimit, (d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && d.SectionID == vm.SecSearch && (d.sDepartDirectorID == LoginUserID || d.sDeskID == LoginUserID ||
                 d.sEditorID == LoginUserID || d.sManagerEditorID == LoginUserID || d.sReviewerID == LoginUserID ||
                 d.sUploaderID == LoginUserID || d.EditorID == LoginUserID || d.Creator == LoginUserID)), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID,this.NewsLimit, (d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && (d.sDepartDirectorID == LoginUserID || d.sDeskID == LoginUserID ||
                d.sEditorID == LoginUserID || d.sManagerEditorID == LoginUserID || d.sReviewerID == LoginUserID ||
                d.sUploaderID == LoginUserID || d.EditorID == LoginUserID || d.Creator == LoginUserID)), vm.page, PageSize, null);
                }
            }
            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit, (d => d.SectionID == vm.SecSearch && (d.sDepartDirectorID == LoginUserID || d.sDeskID == LoginUserID ||
                         d.sEditorID == LoginUserID || d.sManagerEditorID == LoginUserID || d.sReviewerID == LoginUserID ||
                         d.sUploaderID == LoginUserID || d.EditorID == LoginUserID || d.Creator == LoginUserID)), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit, (d => d.sDepartDirectorID == LoginUserID || d.sDeskID == LoginUserID ||
                        d.sEditorID == LoginUserID || d.sManagerEditorID == LoginUserID || d.sReviewerID == LoginUserID ||
                        d.sUploaderID == LoginUserID || d.EditorID == LoginUserID || d.Creator == LoginUserID), vm.page, PageSize, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/News/MyNews.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

        public ActionResult NewsHandler(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "معالجة الاخبار";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0 && (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && d.SectionID == vm.SecSearch), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0 && (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString))), vm.page, PageSize, null);
                }
            }

            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0 && d.SectionID == vm.SecSearch), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                  (d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0), vm.page, PageSize, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/News/NewsHandler.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

        public ActionResult NewsTicker(string searchStr, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "شريط الاخبار";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsTickerBusiness tB = new NewsTickerBusiness(this.DbContext);

            if (vm == null)
                vm = new NewsVM();

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;

            if (!String.IsNullOrEmpty(vm.SearchString))
                vm.NewsTickerLst = tB.LoadDesc(d => d.NewID, this.NewsLimit, d => (d.Title.Contains(vm.SearchString)), vm.page, PageSize, null);
            else
                vm.NewsTickerLst = tB.LoadDesc(d => d.NewID, this.NewsLimit, null, vm.page, PageSize, null);

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/News/NewsTicker.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

        public ActionResult DeleteFromTicker(int MNewsID)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsTickerBusiness tB = new NewsTickerBusiness(this.DbContext);
            NewsVM vm = new NewsVM();
            vm.NewsTicker = tB.Load(MNewsID);
            if (vm.NewsTicker != null)
            {
                tB.Delete(vm.NewsTicker);
                this.SaveChanges();
                TempData["Action"] = "Delete";
            }

            return RedirectToAction("NewsTicker");
        }

        public ActionResult ManageTicker()
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "ضم لشريط الاخبار";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");

            NewsVM vm = new NewsVM();
            NewsBusiness nB = new NewsBusiness(this.DbContext);

            NewsTickerBusiness ntB = new NewsTickerBusiness(this.DbContext);
            List<NewsTicker> NewsTickerTemplst = ntB.LoadDesc(e => e.NewID, this.NewsLimit, d => d.NewsID != null, null).ToList();
            List<int> listIDs = new List<int>();
            foreach (var item in NewsTickerTemplst)
            {
                listIDs.Add(item.NewsID.Value);
            }
            DateTime? date = DateTime.Now.Subtract(TimeSpan.FromDays(2));
            List<News> NewsTickerlst = nB.LoadDesc(e => e.NewID, this.NewsLimit, e => e.Approved == 1 && e.JournalID == 1 && e.NewStatus == 7 && e.PublishDate >= date && !listIDs.Contains(e.NewID), null).ToList();

            Ticker tick;
            foreach (var item in NewsTickerlst)
            {
                tick = new Ticker();
                tick.NewID = item.NewID;
                tick.JournalID = item.JournalID;
                tick.Title = item.Title;
                tick.SectionId = item.SectionID;
                tick.Added_Date = item.PublishDate;
                tick.SecTitle = item.MainSection.SecTitle;
                vm.Tickerlst.Add(tick);
            }

            return View("~/Areas/CoreHandler/Views/News/ManageTicker.cshtml", vm);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddToTicker(NewsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsTickerBusiness ntB = new NewsTickerBusiness(this.DbContext);
            bool flag = false;
            foreach (var item in vm.Tickerlst)
            {
                if (item.IsTicker)
                {
                    vm.NewsTicker = new NewsTicker();
                    vm.NewsTicker.NewsID = item.NewID;
                    vm.NewsTicker.JournalID = item.JournalID;
                    vm.NewsTicker.Title = item.Title;
                    vm.NewsTicker.SectionId = item.SectionId;
                    vm.NewsTicker.Added_Date = item.Added_Date;

                    Func<NewsTicker, bool> expr1 = (x => x.NewsID == item.NewID);
                    Func<NewsTicker, bool> expr2 = (x => x.Title == item.Title);
                    Func<NewsTicker, bool> expr3 = (x => x.JournalID == item.JournalID);
                    Func<NewsTicker, bool> exprs = (x => expr1(x) && expr2(x) && expr3(x));

                    ntB.AddIfNotExists(vm.NewsTicker, exprs);
                    this.SaveChanges();
                    flag = true;

                }
            }
            if (flag)
                TempData["Action"] = "Create";

            return ManageTicker();

        }

        private void AssignDesk(NewsVM vm)
        {
            NewsBusiness nB = new NewsBusiness(this.DbContext);
            TempDeskShiftBusiness tmpDSBuss = new TempDeskShiftBusiness(this.DbContext);
            TempDeskShiftVM tDSVM = new TempDeskShiftVM();
            tDSVM.lst = tmpDSBuss.Load(null, null).OrderBy(e => e.OrderID).ToList();

            for (int i = 0; i < tDSVM.lst.Count; i++)
            {
                if (tDSVM.lst.Count == 1)
                {
                    vm.News.sDeskID = tDSVM.lst[i].UserId;
                    vm.News.SDeskDate = DateTime.Now;
                    nB.Edit(vm.News);
                    this.SaveChanges();

                    tDSVM.lst[i].AssignFlag = true;
                    tDSVM.lst[i].WaitingFlag = true;
                    tmpDSBuss.Edit(tDSVM.lst[i]);
                    this.SaveChanges();
                    break;
                }
                else
                {
                    if (tDSVM.lst[i].WaitingFlag)
                    {
                        vm.News.sDeskID = tDSVM.lst[i].UserId;
                        vm.News.SDeskDate = DateTime.Now;
                        nB.Edit(vm.News);
                        this.SaveChanges();

                        tDSVM.lst[i].AssignFlag = true;
                        tDSVM.lst[i].WaitingFlag = false;
                        tmpDSBuss.Edit(tDSVM.lst[i]);
                        this.SaveChanges();

                        if (tDSVM.lst[i] == tDSVM.lst.Last())
                        {
                            tDSVM.lst[0].WaitingFlag = true;
                            tDSVM.lst[0].AssignFlag = false;
                            tmpDSBuss.Edit(tDSVM.lst[0]);
                            this.SaveChanges();
                        }
                        else
                        {
                            tDSVM.lst[i + 1].WaitingFlag = true;
                            tDSVM.lst[i + 1].AssignFlag = false;
                            tmpDSBuss.Edit(tDSVM.lst[i + 1]);
                            this.SaveChanges();
                        }
                        break;
                    }
                }
            }    


                 
        }


        public ActionResult NewsCustomizeForDesk(NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "تخصيص الاخبار للديسك";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness ndB = new NewsBusiness(this.DbContext);

            vm.NewsForDeskLst = ndB.LoadDesc(e => e.NewID,this.NewsLimit, d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0 && d.NewStatus != 8, null).ToList();

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            AdminUserBusiness adminUB = new AdminUserBusiness(this.DbContext);

            TempDeskShiftBusiness tempDeskBuss = new TempDeskShiftBusiness(this.DbContext);
            TempDeskShiftVM tvm = new TempDeskShiftVM();
            tvm.lst = tempDeskBuss.Load(null, null);

            List<int> userIDLs = new List<int>();
            foreach (var item in tvm.lst)
            {
                userIDLs.Add(item.UserId);
            }

            // Add User شفت الفجر
            userIDLs.Add(453);

            ViewBag.adminULst = new SelectList(adminUB.Load(d => userIDLs.Contains(d.UserID), null), "UserID", "FullName");

            return View("~/Areas/CoreHandler/Views/News/NewsCustomizeForDesk.cshtml", vm);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddToDesk(NewsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness nB = new NewsBusiness(this.DbContext);

            foreach (var item in vm.NewsForDeskLst)
            {
                if (item.sDeskID != 0 && item.sDeskID != null)
                {
                    News news = new News();
                    news = nB.Load(item.NewID);
                    //if (news.sDeskID == null || news.sDeskID == 0)
                    //{
                        news.sDeskID = item.sDeskID;
                        news.SDeskDate = DateTime.Now;
                        //nB.EditSingleCol(item, o => o.sDeskID);
                        nB.Edit(news);
                        this.SaveChanges();
                    //}
                }
            }

            TempData["Action"] = "DeskAdded";
            return NewsCustomizeForDesk(vm);
        }


        //TODO: Handle Events Reject -Delay -Publish
        public ActionResult NewsUploader(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "معالجة الاخبار المخصصة لي";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0
                    && d.NewStatus != 8 && (d.NewStatus== 9 && d.EditAfterRejectFlag != null || d.NewStatus !=9)
                    && d.sDeskID == this.LoginUserID && (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && d.SectionID == vm.SecSearch), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0
                    && (d.NewStatus == 8 && d.EditAfterRejectFlag != null || d.NewStatus != 8) && (d.NewStatus == 9 && d.EditAfterRejectFlag != null || d.NewStatus != 9)
                    && d.sDeskID == this.LoginUserID && (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString))), vm.page, PageSize, null);
                }
            }

            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0
                    && d.NewStatus != 8 && (d.NewStatus == 9 && d.EditAfterRejectFlag != null || d.NewStatus != 9)
                    && d.sDeskID == this.LoginUserID && d.SectionID == vm.SecSearch), vm.page, PageSize, null);
                }
                else
                {
                    //&& d.NewStatus != 8 && d.NewStatus != 9 
                  vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                  (d => d.Approved != 1 && d.NewStatus != 7 && d.NewStatus != 0
                  && d.NewStatus != 8 && (d.NewStatus == 9 && d.EditAfterRejectFlag != null || d.NewStatus != 9)
                  && d.sDeskID == this.LoginUserID), vm.page, PageSize, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/News/NewsUploader.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

        // GET: News/Edit/5
        public ActionResult NewsDesk(int? MNewsID, NewsVM vm = null)
        {
            if (MNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness eB = new NewsBusiness(this.DbContext);

            if (vm == null)
                vm = new NewsVM();

            vm.News = eB.Load(MNewsID);
            foreach (NewsTag tg in vm.News.NewsTagsLst)
            {
                vm.tags += tg.Tag.Name + ",";
            }

            List<int> arr = new List<int>();
            foreach (NewsGallery ng in vm.News.NewsGalleryLst)
                arr.Add(ng.GalleryID);
            vm.Albums = arr.ToArray();
            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
            //ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

            vm = FillSubSectionArr(vm);

            List<int> arr1 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 1))
                arr1.Add(nb.ByLineId);
            vm.ByLines = arr1.ToArray();

            List<int> arr2 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 2))
                arr2.Add(nb.ByLineId);
            vm.photograhers = arr2.ToArray();

            List<int> arr3 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 3))
                arr3.Add(nb.ByLineId);
            vm.Infograhers = arr3.ToArray();

            List<int> arr4 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 4))
                arr4.Add(nb.ByLineId);
            vm.Videograhers = arr4.ToArray();

            if (vm.News == null)
            {
                return HttpNotFound();
            }

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            return View("~/Areas/CoreHandler/Views/News/NewsDesk.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult NewsDesk(string submitButton, NewsVM vm)
        {
            switch (submitButton)
            {
                case "نشر":
                    // delegate sending to another controller action
                    return (DeskPublish(vm,true,false,false));
                case "رفض":
                    // call another action to perform the cancellation
                    return (DeskPublish(vm,false,true,false));
                default:
                    // If they've submitted the form without a submitButton, 
                    // just return the view again.
                    return (DeskPublish(vm,false,false,true));
            }
        }

        
        public ActionResult DeskPublish(NewsVM vm, bool PublishFlag, bool RejectFlag, bool WaitingFlag)
        {
            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "newID","SectionID", "NewsSource" , "Title", "SubTitle",
                     "Story","Brief","quote","Keywords","PictureID1","PictureCaption1","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Notes", "SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9"});

                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title && n.NewID != vm.News.NewID, null);

                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    fillData(vm);

                    ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                    return RedirectToAction("NewsUploader");
                }
                else
                {
                    //update can happen only during 24 hours from news publising
                    if (vm.News.PublishDate != null)
                    {
                        if (vm.News.PublishDate.Value.AddHours(24) <= DateTime.Now)
                        {
                            TempData["Action"] = "ForbiddenEdit";
                            ViewBag.alert = GetMessage(TempData["Action"].ToString());
                            fillData(vm);

                            return RedirectToAction("NewsUploader");
                        }
                    }

                    var img = this.HttpContext.Request.Files["PictureName1"];
                    vm.News.Keywords = vm.tags;

                    if (vm.archImgFlag && TempData["PicID"] != null)
                    {
                        int picid = Convert.ToInt32(TempData["PicID"].ToString());
                        vm.News.PictureID1 = picid;
                        NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                        vm.News.NewsPicture = nb.Load(picid);
                        TempData["PicID"] = null;
                    }
                    if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                    {
                        if (vm.News.Keywords == "" || vm.News.Keywords == null)
                            TempData["Action"] = "KeyWords";
                        else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                            TempData["Action"] = "Images";
                        else
                            TempData["Action"] = "Error";

                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        return RedirectToAction("NewsUploader");
                    }

                    else
                    {
                        if (vm.News.NewsPicture == null && vm.News.PictureID1 == null)
                        {
                            vm.News.NewsPicture = new NewsPictures();

                            string picName = "";
                            bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                            if (imgFlag)
                                vm.News.NewsPicture.PictureName = picName;
                        }

                        if (RejectFlag)
                            vm = SetDefaultValues(vm, false, false, false, true, false);
                        else if (WaitingFlag)
                            vm = SetDefaultValues(vm, false, false, false, false, true);
                        else
                            vm = SetDefaultValues(vm, false, false, true, false, false);

                        vm = FillOtherSections(vm);

                        nB.Edit(vm.News);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.News).GetDatabaseValues();

                        vm = FillNewByLines(vm);

                        vm = FillVersionNews(vm, false);

                        vm = FillNewsAndNewsVersionTags(vm);

                        vm = FillNewsAlbums(vm);

                        var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                        var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);

                        if (PublishFlag)
                        {
                            Publish(vm);
                            TempData["Action"] = "Publish";
                        }
                        else if (WaitingFlag)
                            TempData["Action"] = "Waiting";
                        else
                            TempData["Action"] = "Rejected";

                        return RedirectToAction("NewsUploader");
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            // return Content("Create clicked");
        }

        public ActionResult NewsSheet(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "الشيت";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && d.SectionID == vm.SecSearch), vm.page,30 /*PageSize*/, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString))), vm.page,30 /*PageSize*/, null);
                }
            }

            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.SectionID == vm.SecSearch), vm.page,30 /*PageSize*/, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit, null, vm.page, 30/*PageSize*/, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/News/NewsSheet.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }

        public ActionResult ImageArch(NewsVM vm = null)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsPictureBusiness nPB = new NewsPictureBusiness(this.DbContext);

            if (vm.SearchString!="")
            {
                vm.SearchedImages = nPB.LoadDesc(d => d.PictureID, this.NewsLimit,
                (d => (d.PicCaption.Contains(vm.SearchString))), vm.page, PageSize, null);
            }
            else
            {
                vm.SearchedImages = nPB.LoadDesc(d => d.PictureID, this.NewsLimit, null, vm.page, PageSize, null);
            }

            ViewBag.RSmallImgPath = this.RelativeSmallImgPath;
            return PartialView("~/Areas/CoreHandler/Views/News/Partials/_ImageArch.cshtml", vm);
        }

        public ActionResult SetAsNewsPhoto(int PicID, string PicName)
        {
            if (PicID.ToString() != null && !string.IsNullOrEmpty(PicName))
            {
                ViewBag.ImgPath = this.RelativeMediumImgPath + PicName;
                TempData["PicID"] = PicID;
            }

            return PartialView("~/Areas/CoreHandler/Views/News/Partials/_ViewArchImage.cshtml");
        }


        //TODO: Handle Events Delay News 
        public ActionResult DelayUploader(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "معالجة الاخبار المؤجلة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.NewStatus == 9 && (d.sDeskID == this.LoginUserID || d.Creator == this.LoginUserID) 
                    && (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) && d.SectionID == vm.SecSearch), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.NewStatus == 9 && (d.sDeskID == this.LoginUserID || d.Creator == this.LoginUserID)
                    && (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString))), vm.page, PageSize, null);
                }
            }

            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.NewStatus == 9 && (d.sDeskID == this.LoginUserID || d.Creator == this.LoginUserID) && 
                    d.SectionID == vm.SecSearch), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                  (d => d.NewStatus == 9 && (d.sDeskID == this.LoginUserID || d.Creator == this.LoginUserID)), vm.page, PageSize, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            return View(vm);
        }

        //TODO: Handle Events Rejected News 
        public ActionResult RejectedUploader(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "معالجة الاخبار الملغاة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.NewStatus == 8 && 
                    (d.sDeskID == this.LoginUserID ||d.Creator==this.LoginUserID) && 
                    (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString)) 
                    && d.SectionID == vm.SecSearch), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.NewStatus == 8 &&
                    (d.sDeskID == this.LoginUserID || d.Creator == this.LoginUserID) &&
                    (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString))), vm.page, PageSize, null);
                }
            }

            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.NewStatus == 8 &&
                    (d.sDeskID == this.LoginUserID || d.Creator == this.LoginUserID) &&
                    d.SectionID == vm.SecSearch), vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.NewID, this.NewsLimit,
                    (d => d.NewStatus == 8 && (d.sDeskID == this.LoginUserID || d.Creator == this.LoginUserID) ), vm.page, PageSize, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            return View(vm);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }



        #region published older than 24 hours 

        //todo: using transaction scope
        //     using (this.DbContext = new AkhbarDBContext("AkhbarDBConnection"))
        //        {
        //    using (this.DbContext.Database.BeginTransaction())
        //    {
        //    }
        //}

        // GET: News
        public ActionResult PublishedBeforeFullDayIndex(string searchStr, int? SecId, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "الاخبار المنشورة قبل ٢٤ ساعة";
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness EB = new NewsBusiness(this.DbContext);

            if (searchStr != null && searchStr != "")
                vm.SearchString = searchStr;
            if (SecId != null)
                vm.SecSearch = SecId.Value;

            DateTime dateBeforeOneDay = DateTime.Now.AddHours(-24);

            if (!String.IsNullOrEmpty(vm.SearchString))
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString))
                    && d.SectionID == vm.SecSearch && (d.NewStatus == 7 || d.NewStatus == 6 || d.NewStatus == 0) && d.PublishDate < dateBeforeOneDay,
                    vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, d => (d.Title.Contains(vm.SearchString) || d.ByLine.Contains(vm.SearchString))
                    && (d.NewStatus == 7 || d.NewStatus == 6 || d.NewStatus == 0) && d.PublishDate < dateBeforeOneDay, vm.page, PageSize, null);
                }
            }
            else
            {
                if (vm.SecSearch != 0)
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit, e => e.SectionID == vm.SecSearch
                    && (e.NewStatus == 7 || e.NewStatus == 6 || e.NewStatus == 0) && e.PublishDate < dateBeforeOneDay, vm.page, PageSize, null);
                }
                else
                {
                    vm.lst = EB.LoadDesc(d => d.PublishDate.Value, this.NewsLimit,
                        e => (e.NewStatus == 7 || e.NewStatus == 6 || e.NewStatus == 0) && e.PublishDate < dateBeforeOneDay
                        , vm.page, PageSize, null);
                }
            }
            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            FillSectionDDLForSearch(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/CoreHandler/Views/News/PublishedBeforeFullDayIndex.cshtml", vm);
            }
            else
            {
                return View(vm);
            }
        }
        // GET: News/Edit/5
        public ActionResult EditBeforeDayPublishedNews(int? MNewsID, NewsVM vm = null)
        {
            Session["CurrentModule"] = "الاخبار";
            Session["CurrentService"] = "تعديل خبر";
            if (MNewsID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            NewsBusiness eB = new NewsBusiness(this.DbContext);

            if (vm == null)
                vm = new NewsVM();

            vm.News = eB.Load(MNewsID);
            foreach (NewsTag tg in vm.News.NewsTagsLst)
            {
                vm.tags += tg.Tag.Name + ",";
            }

            vm.oldSecId = vm.News.SectionID;

            List<int> arr = new List<int>();
            foreach (NewsGallery ng in vm.News.NewsGalleryLst)
                arr.Add(ng.GalleryID);
            vm.Albums = arr.ToArray();
            GalleryBusiness gB = new GalleryBusiness(this.DbContext);
            //ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, e => e.GalleryType == "NewsAlbum", null).Take(50), "GalleryID", "Title");
            ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

            vm = FillSubSectionArr(vm);

            List<int> arr1 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 1))
                arr1.Add(nb.ByLineId);
            vm.ByLines = arr1.ToArray();

            List<int> arr2 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 2))
                arr2.Add(nb.ByLineId);
            vm.photograhers = arr2.ToArray();

            List<int> arr3 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 3))
                arr3.Add(nb.ByLineId);
            vm.Infograhers = arr3.ToArray();

            List<int> arr4 = new List<int>();
            foreach (News_Byline nb in vm.News.NewsByLineLst.Where(e => e.Flag == 4))
                arr4.Add(nb.ByLineId);
            vm.Videograhers = arr4.ToArray();

            if (vm.News == null)
            {
                return HttpNotFound();
            }

            ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
            ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
            ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

            FillAllowedSectionDDL(vm);
            ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

            if (TempData["Action"] != null)
            {
                if (TempData["Action"].ToString() != "")
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
            }

            ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
            return View("~/Areas/CoreHandler/Views/News/EditBeforeDayPublishedNews.cshtml", vm);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditBeforeDayPublishedNews(NewsVM vm)
        {

            try
            {
                this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
                NewsBusiness nB = new NewsBusiness(this.DbContext);

                TryUpdateModel<News>(vm.News, new string[] { "newID","SectionID", "NewsSource" , "Title", "SubTitle",
                     "Story","Brief","quote","Keywords","PictureID1","PictureCaption1","EditorID","ByLine","Views","NewStatus",
                      "BackEndNewId","Notes", "SectionID1", "SectionID2", "SectionID3", "SectionID4", "SectionID5", "SectionID6",
                      "SectionID7", "SectionID8", "SectionID9", "SEOTitle"});

                vm.lastNewsLst = nB.LoadDesc(e => e.NewID, 30, n => n.Title == vm.News.Title && n.NewID != vm.News.NewID, null);
                if (vm.lastNewsLst.Count > 0)
                {
                    TempData["Action"] = "Exists";
                    ViewBag.alert = GetMessage(TempData["Action"].ToString());
                    fillData(vm);

                    return View("~/Areas/CoreHandler/Views/News/EditBeforeDayPublishedNews.cshtml", vm);
                }
                else
                {

                    vm.News.Keywords = vm.tags;
                    var img = this.HttpContext.Request.Files["PictureName1"];

                    if (!ModelState.IsValid || vm.News.Keywords == "" || vm.News.Keywords == null || (!IsValidMainImg(img) && vm.News.NewsPicture == null))
                    {
                        if (vm.News.Keywords == "" || vm.News.Keywords == null)
                            TempData["Action"] = "KeyWords";
                        else if (!IsValidMainImg(img) && vm.News.NewsPicture == null)
                            TempData["Action"] = "Images";
                        else
                            TempData["Action"] = "Error";

                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        GalleryBusiness gBuss = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gBuss.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");

                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
                        return View("~/Areas/CoreHandler/Views/News/EditBeforeDayPublishedNews.cshtml", vm);
                    }
                    else
                    {

                        if (vm.archImgFlag && TempData["PicID"] != null)
                        {
                            int picid = Convert.ToInt32(TempData["PicID"].ToString());
                            vm.News.PictureID1 = picid;
                            NewsPictureBusiness nb = new NewsPictureBusiness(this.DbContext);
                            vm.News.NewsPicture = nb.Load(picid);
                            TempData["PicID"] = null;
                        }
                        else
                        {
                            string picName = "";
                            if (vm.News.NewsPicture == null)
                                vm.News.NewsPicture = new NewsPictures();

                            bool imgFlag = SaveNewsImages(ref picName, img, vm.waterMarkFlag);
                            if (imgFlag)
                                vm.News.NewsPicture.PictureName = picName;
                        }

                        vm = SetDefaultValues(vm, false, false, false, false, false);

                        NewsPictureBusiness nbB = new NewsPictureBusiness(this.DbContext);
                        nbB.Edit(vm.News.NewsPicture);
                        this.SaveChanges();

                        vm = FillOtherSections(vm);

                        // add Edit after rejection Flag
                        if (vm.News.NewStatus == 8 || vm.News.NewStatus == 9)
                            vm.News.EditAfterRejectFlag = 1;

                        nB.Edit(vm.News);
                        this.SaveChanges();

                        this.DbContext.Entry(vm.News).GetDatabaseValues();

                        vm = FillNewByLines(vm);

                        vm = FillVersionNews(vm, false);

                        vm = FillNewsAndNewsVersionTags(vm);

                        vm = FillNewsAlbums(vm);

                        var updateRelatedNewsStoredProcedure = new UpdateRelatedNews() { newsId = vm.News.NewID };
                        var result = DbContext.Database.ExecuteStoredProcedure(updateRelatedNewsStoredProcedure);

                        EditPublish(vm);

                        TempData["Action"] = "Edit";
                        ViewBag.alert = GetMessage(TempData["Action"].ToString());

                        GalleryBusiness gB = new GalleryBusiness(this.DbContext);
                        ViewBag.Albums = new SelectList(gB.LoadDesc(e => e.GalleryID, 50, null, null), "GalleryID", "Title");
                        ByLineBusiness blBuss = new ByLineBusiness(this.DbContext);
                        ViewBag.ByLines = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Photograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Infograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");
                        ViewBag.Vidograhers = new SelectList(blBuss.Load(e => e.Active == true, null), "ByLineId", "ByLineName");

                        FillAllowedSectionDDL(vm);
                        ViewBag.Sec_ID = new SelectList(vm.ActiveSectionLst, "SectionID", "SecTitle");

                        ViewBag.RMeduimImgPath = this.RelativeMediumImgPath;
                        return View("~/Areas/CoreHandler/Views/News/EditBeforeDayPublishedNews.cshtml", vm);
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }
        #endregion


        #region Helpers
        private bool SaveNewsImages(ref string picName, HttpPostedFileBase img, bool waterMarkFlag)
        {
            if (img == null || img.ContentLength <= 0)
                return false;
            string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            picName = "" + DateTimePattern + ".jpg";
            string orFilename = this.OrImgPath + picName;
            string largeFilename = this.LargeImgPath + picName;
            string mediumFilename = this.MediumImgPath + picName;
            string smallFilename = this.SmallImgPath + picName;

            try
            {
                Stream strm = img.InputStream;
                Size lImgSize = new Size(this.NewsImgLargeWidth, this.NewsImgLargeHeight);
                Size mImgSize = new Size(this.NewsImgMediumWidth, this.NewsImgMediumHeight);
                Size sImgSize = new Size(this.NewsImgSmallWidth, this.NewsImgSmallHeight);
                Bitmap bitmap = new Bitmap(Image.FromStream(strm));

                //Save Original image with large size
                SaveImage(ref bitmap, lImgSize, orFilename, false);

                if (waterMarkFlag)
                {
                    this.AddWaterMark(ref bitmap, picName);
                }

                SaveImage(ref bitmap, lImgSize, largeFilename, true);
                SaveImage(ref bitmap, mImgSize, mediumFilename, true);
                SaveImage(ref bitmap, sImgSize, smallFilename, true);

                return true;
            }
            catch (Exception ex)
            {
                string e = ex.ToString();
                return false;
            }
        }

        private NewsVM FillSubSectionArr(NewsVM vm)
        {
            List<int> subSec = new List<int>();
            if (vm.News.SectionID1 != null)
                subSec.Add(vm.News.SectionID1.Value);
            if (vm.News.SectionID2 != null)
                subSec.Add(vm.News.SectionID2.Value);
            if (vm.News.SectionID3 != null)
                subSec.Add(vm.News.SectionID3.Value);
            if (vm.News.SectionID4 != null)
                subSec.Add(vm.News.SectionID4.Value);
            if (vm.News.SectionID5 != null)
                subSec.Add(vm.News.SectionID5.Value);
            if (vm.News.SectionID6 != null)
                subSec.Add(vm.News.SectionID6.Value);
            if (vm.News.SectionID7 != null)
                subSec.Add(vm.News.SectionID7.Value);
            if (vm.News.SectionID8 != null)
                subSec.Add(vm.News.SectionID8.Value);
            if (vm.News.SectionID9 != null)
                subSec.Add(vm.News.SectionID9.Value);
            vm.Sections = subSec.ToArray();

            return vm;
        }

        private NewsVM FillOtherSections(NewsVM vm)
        {
            //Insert sections to news
            if (vm.Sections != null)
            {
                int i = 1;
                foreach (int secId in vm.Sections)
                {
                    if (i == 1)
                        vm.News.SectionID1 = secId;
                    if (i == 2)
                        vm.News.SectionID2 = secId;
                    if (i == 3)
                        vm.News.SectionID3 = secId;
                    if (i == 4)
                        vm.News.SectionID4 = secId;
                    if (i == 5)
                        vm.News.SectionID5 = secId;
                    if (i == 6)
                        vm.News.SectionID6 = secId;
                    if (i == 7)
                        vm.News.SectionID7 = secId;
                    if (i == 8)
                        vm.News.SectionID8 = secId;
                    if (i == 9)
                        vm.News.SectionID9 = secId;
                    i++;
                    if (i > 9)
                        return vm;
                }
            }

            return vm;
        }

        private NewsVM FillNewsAlbums(NewsVM vm)
        {
            NewsGalleryBusiness ngB = new NewsGalleryBusiness(this.DbContext);
            //Delete old news Galleries
            vm.News.NewsGalleryLst = ngB.Load(e => e.NewID == vm.News.NewID);
            foreach (NewsGallery album in vm.News.NewsGalleryLst.ToList())
            {
                ngB.Delete(album);
                this.SaveChanges();
            }

            //Insert new news Galleries
            if (vm.Albums != null)
            {
                foreach (int albumId in vm.Albums)
                {
                    vm.NewsGallery = new NewsGallery();
                    vm.NewsGallery.NewID = vm.News.NewID;
                    vm.NewsGallery.GalleryID = albumId;

                    Func<NewsGallery, bool> expr1 = (x => x.NewID == vm.News.NewID);
                    Func<NewsGallery, bool> expr2 = (x => x.GalleryID == albumId);
                    Func<NewsGallery, bool> exprs = (x => expr1(x) && expr2(x));
                    ngB.AddIfNotExists(vm.NewsGallery, exprs);
                    this.SaveChanges();
                }
            }

            return vm;
        }

        private NewsVM FillNewByLines(NewsVM vm)
        {
            News_BylineBusiness nbB = new News_BylineBusiness(this.DbContext);
            //Delete old news Editors-Photographer-Infographer
            vm.News.NewsByLineLst = nbB.Load(e => e.NewId == vm.News.NewID);
            foreach (News_Byline editor in vm.News.NewsByLineLst.ToList())
            {
                nbB.Delete(editor);
                this.SaveChanges();
            }

            ByLineBusiness blB = new ByLineBusiness(this.DbContext);
            vm.News.ByLine = "";
            ByLine BLname;

            //Insert new newsEditors
            if (vm.ByLines != null)
            {
                foreach (int editorId in vm.ByLines)
                {
                    vm.News_ByLine = new News_Byline();
                    vm.News_ByLine.NewId = vm.News.NewID;
                    vm.News_ByLine.ByLineId = editorId;
                    vm.News_ByLine.Flag = (int)EditorType.Editor;
                    nbB.Add(vm.News_ByLine);
                    this.SaveChanges();

                    BLname = blB.Load(editorId);
                    if (BLname != null)
                        vm.News.ByLine = vm.News.ByLine + "-" + BLname.ByLineName;
                }
            }

            //Insert new photograhers
            if (vm.photograhers != null)
            {
                foreach (int phId in vm.photograhers)
                {
                    vm.News_ByLine = new News_Byline();
                    vm.News_ByLine.NewId = vm.News.NewID;
                    vm.News_ByLine.ByLineId = phId;
                    vm.News_ByLine.Flag = (int)EditorType.Photographer;
                    nbB.Add(vm.News_ByLine);
                    this.SaveChanges();

                    BLname = blB.Load(phId);
                    if (BLname != null)
                        vm.News.ByLine = vm.News.ByLine + "-" + BLname.ByLineName;
                }
            }

            //Insert new Infograhers
            if (vm.Infograhers != null)
            {
                foreach (int infoId in vm.Infograhers)
                {
                    vm.News_ByLine = new News_Byline();
                    vm.News_ByLine.NewId = vm.News.NewID;
                    vm.News_ByLine.ByLineId = infoId;
                    vm.News_ByLine.Flag = (int)EditorType.Infographer;
                    nbB.Add(vm.News_ByLine);
                    this.SaveChanges();

                    BLname = blB.Load(infoId);
                    if (BLname != null)
                        vm.News.ByLine = vm.News.ByLine + "-" + BLname.ByLineName;
                }
            }

            //Insert new Videograhers
            if (vm.Videograhers != null)
            {
                foreach (int videoGId in vm.Videograhers)
                {
                    vm.News_ByLine = new News_Byline();
                    vm.News_ByLine.NewId = vm.News.NewID;
                    vm.News_ByLine.ByLineId = videoGId;
                    vm.News_ByLine.Flag = (int)EditorType.Videographer;
                    nbB.Add(vm.News_ByLine);
                    this.SaveChanges();

                    BLname = blB.Load(videoGId);
                    if (BLname != null)
                        vm.News.ByLine = vm.News.ByLine + "-" + BLname.ByLineName;
                }
            }

            //keep old Byline if not empty or add (بوابة أخبار اليوم)
            if (vm.News.ByLine == null || vm.News.ByLine == "")
            {
                if (vm.News.ByLine == "")
                {
                    vm.News_ByLine = new News_Byline();
                    vm.News_ByLine.NewId = vm.News.NewID;
                    vm.News_ByLine.ByLineId = 2; //بوابة أخبار اليوم
                    vm.News_ByLine.Flag = (int)EditorType.Editor;
                    nbB.Add(vm.News_ByLine);
                    this.SaveChanges();

                    BLname = blB.Load(2);
                    if (BLname != null)
                        vm.News.ByLine = vm.News.ByLine + "-" + BLname.ByLineName;
                }
            }
            //Remove lastest "-" separated by Bylines
            if (vm.News.ByLine != null && vm.News.ByLine != "")
                vm.News.ByLine = vm.News.ByLine.Substring(1, vm.News.ByLine.Length - 1);


            return vm;
        }

        private NewsVM FillNewsAndNewsVersionTags(NewsVM vm)
        {
            //Delete Old Tags
            NewsTagBusiness ntB = new NewsTagBusiness(this.DbContext);
            vm.News.NewsTagsLst = ntB.Load(e => e.NewID == vm.News.NewID);

            VersionTagBusiness vtB = new VersionTagBusiness(this.DbContext);
            vm.NewsVersion.VersionTagsLst = vtB.Load(e => e.VersionID == vm.NewsVersion.VersionId);

            foreach (NewsTag newstag in vm.News.NewsTagsLst.ToList())
            {
                ntB.Delete(newstag);
                this.SaveChanges();
            }

            foreach (VersionTag versiontag in vm.NewsVersion.VersionTagsLst.ToList())
            {
                vtB.Delete(versiontag);
                this.SaveChanges();
            }


            //Insert new tags
            TagBusiness tB = new TagBusiness(this.DbContext);
            string[] strTags = vm.tags.Split(',');
            foreach (string str in strTags)
            {
                string newTag = GetPureKeyWord(str);
                if (newTag.Trim() == "")
                    newTag = "بوابة اخبار اليوم";
                vm.NewsTag = new NewsTag();
                vm.NewsTag.NewID = vm.News.NewID;

                vm.VersionTag = new VersionTag();
                vm.VersionTag.VersionID = vm.NewsVersion.VersionId;

                /// check if tag exists on tag Table
                vm.NewsTag.Tag = tB.Load(e => e.Name == newTag).FirstOrDefault();
                if (vm.NewsTag.Tag == null || vm.NewsTag.Tag.ID == 0)
                {
                    vm.NewsTag.Tag = new Tag();
                    vm.NewsTag.Tag.Name = newTag;
                }
                else
                {
                    vm.NewsTag.TagID = vm.NewsTag.Tag.ID;
                }

                ntB.Add(vm.NewsTag);
                this.SaveChanges();
                this.DbContext.Entry(vm.NewsTag).GetDatabaseValues();

                vm.VersionTag.TagID = vm.NewsTag.TagID;
                vtB.Add(vm.VersionTag);
                this.SaveChanges();

            }

            return vm;
        }

        private NewsVM FillNewsViews(NewsVM vm)
        {
            vm.NewsView = new NewsView();
            vm.NewsView.Views = 0;
            vm.NewsView.NewID = vm.News.NewID;
            NewsViewBusiness nvB = new NewsViewBusiness(this.DbContext);
            nvB.Add(vm.NewsView);
            this.SaveChanges();

            return vm;
        }

        private NewsVM FillIssueNews(NewsVM vm)
        {
            vm.IssuesNew = new IssuesNew();
            vm.IssuesNew.IssueID = 1;
            vm.IssuesNew.NewsID = vm.News.NewID;
            IssuesNewBusiness inB = new IssuesNewBusiness(this.DbContext);
            inB.Add(vm.IssuesNew);
            this.SaveChanges();

            return vm;
        }

        private NewsVM FillVersionNews(NewsVM vm, bool addNewFalg)
        {
            vm.NewsVersion = new NewsVersions();
            vm.NewsVersion.NewID = vm.News.NewID;
            vm.NewsVersion.PictureCaption1 = vm.News.PictureCaption1;
            vm.NewsVersion.PictureID1 = (int)vm.News.PictureID1;
            vm.NewsVersion.Title = vm.News.Title;
            vm.NewsVersion.SectionID = vm.News.SectionID;
            vm.NewsVersion.ByLine = vm.News.ByLine;
            vm.NewsVersion.SubTitle = vm.News.SubTitle;
            vm.NewsVersion.Brief = vm.News.Brief;
            vm.NewsVersion.PictureCaption1 = vm.News.PictureCaption1;
            vm.NewsVersion.Story = vm.News.Story;
            vm.NewsVersion.Keywords = vm.News.Keywords;
            vm.NewsVersion.CategoryID = 1;
            vm.NewsVersion.AddedDate = DateTime.Now;
            vm.NewsVersion.PublishDate = vm.News.PublishDate;
            vm.NewsVersion.SDeskDate = vm.News.SDeskDate;
            vm.NewsVersion.JournalID = 1;
            vm.NewsVersion.ParentID = 0;
            vm.NewsVersion.NewsType = 1;
            vm.NewsVersion.NewsSource = 1;
            vm.NewsVersion.quote = "";
            vm.NewsVersion.PictureCaption2 = "";
            vm.NewsVersion.PictureID2 = 0;
            vm.NewsVersion.EditorID = 0;
            vm.NewsVersion.Notes = vm.News.Notes;
            vm.NewsVersion.SocialTitle = vm.News.SocialTitle;
            vm.NewsVersion.SocialPictureId = vm.News.SocialPictureId;
            vm.NewsVersion.SocialEditorID = vm.News.SocialEditorID;

            if (addNewFalg)
                vm.NewsVersion.Version = 1;
            else
            {
                if (vm.News.NewsVersionsLst.Count == 0)
                {
                    NewsVersionBusiness nvb = new NewsVersionBusiness(this.DbContext);
                    vm.News.NewsVersionsLst = nvb.Load(e => e.NewID == vm.News.NewID);
                }
                vm.NewsVersion.Version = vm.News.NewsVersionsLst.Count() + 1;
            }
            vm.NewsVersion.AddedBy = LoginUserID;
            NewsVersionBusiness nvB = new NewsVersionBusiness(this.DbContext);
            nvB.Add(vm.NewsVersion);
            this.SaveChanges();
            this.DbContext.Entry(vm.NewsVersion).GetDatabaseValues();
            return vm;
        }

        private NewsVM SetDefaultValues(NewsVM vm, bool addNewFalg, bool passFlag, bool publishFlag, bool rejectFlag, bool WaitingFlag)
        {
            vm.News.NewsPicture.PicCaption = vm.News.PictureCaption1;
            vm.News.NewsPicture.KeyWords = vm.News.PictureCaption1;
            vm.News.NewsPicture.CatID = 0;
            vm.News.NewsPicture.JournalID = 1;
            vm.News.NewsPicture.Source = 1;
            vm.News.NewsPicture.added_date = DateTime.Now;

            vm.News.quote = "";

            if (addNewFalg)
            {
                vm.News.CategoryID = 1;
                vm.News.AddedDate = DateTime.Now;
                //vm.News.PublishDate = DateTime.Now;
                vm.News.JournalID = 1;
                vm.News.ParentID = 0;
                vm.News.NewsType = 1;
                vm.News.NewsSource = 0;
                vm.News.NewsID = 1;
                vm.News.Creator = LoginUserID;
                vm.News.PictureCaption2 = "";
                vm.News.PictureID2 = 0;
                vm.News.NewStatus = 1;
                vm.News.EditorID = 0;
                vm.News.AssignedTo = 0;
                vm.News.sEditorID = LoginUserID;
                vm.News.sDepartDirectorID = 0;
                vm.News.sManagerEditorID = 0;
                vm.News.sReviewerID = 0;
                vm.News.sUploaderID = 0;
                vm.News.Approved = 0;
                vm.News.Views = 0;
            }
            else
            {
                vm.News.EditorID = LoginUserID;
                vm.News.Views = vm.News.Views + 1;
            }

            if (passFlag)
                vm.News.NewStatus = 1;

            if (publishFlag)
            {
                vm.News.sUploaderID = LoginUserID;
                vm.News.Approved = 1;
                vm.News.NewStatus = 7;
                if (vm.News.PublishDate == null)
                    vm.News.PublishDate = DateTime.Now;
            }
            if (rejectFlag)
            {
                vm.News.sUploaderID = LoginUserID;
                vm.News.NewStatus = 8;
                vm.News.SDeskDate = DateTime.Now;
                // vm.News.PublishDate = DateTime.Now;
            }
            if (WaitingFlag)
            {
                vm.News.sUploaderID = LoginUserID;
                vm.News.NewStatus = 9;
                vm.News.SDeskDate = DateTime.Now;
                //vm.News.PublishDate = DateTime.Now;
            }
            return vm;
        }

        private NewsVM EditPublish(NewsVM vm)
        {
            if (vm != null)
            {
                if (vm.News != null)
                {

                    TopNewsBusiness tnB = new TopNewsBusiness(this.DbContext);
                    NewsTickerBusiness ntB = new NewsTickerBusiness(this.DbContext);

                    Func<TopNews, bool> newsexpr = (x => x.NewsID == vm.News.NewID);
                    Func<TopNews, bool> catexpr = (x => x.CatID == vm.News.CategoryID.Value);
                    Func<TopNews, bool> journalexpr = (x => x.JournalID == vm.News.JournalID);

                    Func<TopNews, bool> slidexpr = (x => x.SecID == 0);
                    Func<TopNews, bool> secexpr = (x => x.SecID == vm.News.SectionID);
                    Func<TopNews, bool> mo7taratexpr = (x => x.SecID == -1);

                    Func<TopNews, bool> existOnSliderExpr = (x => newsexpr(x) && catexpr(x) && journalexpr(x) && slidexpr(x));
                    Func<TopNews, bool> existOnMo7taratExpr = (x => newsexpr(x) && catexpr(x) && journalexpr(x) && mo7taratexpr(x));

                    Func<TopNews, bool> existOnHomeSecExpr = (x => newsexpr(x) && catexpr(x) && journalexpr(x) && secexpr(x));

                    List<TopNews> nl = tnB.Load(e => e.NewsID == vm.News.NewID);
                    TopNews sliderNew = nl.Where(existOnSliderExpr).FirstOrDefault();
                    TopNews homeSecNew = nl.Where(existOnHomeSecExpr).FirstOrDefault();
                    TopNews mo7taratNew = nl.Where(existOnMo7taratExpr).FirstOrDefault();

                    if (homeSecNew == null)
                    {
                        if (vm.SectionNewsFlag)
                        {

                            if (vm.oldSecId != 0 && vm.oldSecId != vm.News.SectionID)
                            {
                                vm.TopNewsTemp = tnB.Load(e => e.NewsID == vm.News.NewID && e.SecID == vm.oldSecId && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID, null).FirstOrDefault();
                                tnB.Delete(vm.TopNewsTemp);
                                this.SaveChanges();
                            }

                            vm.TopNews = new TopNews();
                            vm.TopNews.NewsID = vm.News.NewID;
                            vm.TopNews.SecID = vm.News.SectionID;
                            vm.TopNews.CatID = vm.News.CategoryID.Value;
                            vm.TopNews.JournalID = vm.News.JournalID;
                            vm.TopNews.DisplayOrder = 0;
                            tnB.AddIfNotExists(vm.TopNews, existOnHomeSecExpr);
                            this.SaveChanges();

                            vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == vm.News.SectionID && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID, null);
                            foreach (var item in vm.TopNewsOrderedlst)
                            {
                                item.DisplayOrder = item.DisplayOrder + 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }

                            // To Remove news that changed section after published 
                            int count = 0;
                            foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                            {
                                if (item.DisplayOrder != count + 1)
                                {
                                    item.DisplayOrder = count + 1;
                                    tnB.Edit(item);
                                    this.SaveChanges();
                                }
                                count++;
                            }

                            vm.TopNewsTemp = tnB.Load(e => e.SecID == vm.News.SectionID && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > 8, null).FirstOrDefault();
                            if (vm.TopNewsTemp != null)
                            {
                                tnB.Delete(vm.TopNewsTemp);
                                this.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        if (!vm.SectionNewsFlag)
                        {
                            vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == vm.News.SectionID && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > homeSecNew.DisplayOrder, null);
                            foreach (var item in vm.TopNewsOrderedlst)
                            {
                                item.DisplayOrder = item.DisplayOrder - 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }
                            tnB.Delete(homeSecNew);
                            this.SaveChanges();

                            // To Remove news that changed section after published 
                            int count = 0;
                            foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                            {
                                if (item.DisplayOrder != count + 1)
                                {
                                    item.DisplayOrder = count + 1;
                                    tnB.Edit(item);
                                    this.SaveChanges();
                                }
                                count++;
                            }
                        }
                    }

                    if (sliderNew == null)
                    {
                        if (vm.SliderNewsFlag)
                        {
                            vm.TopNews = new TopNews();
                            vm.TopNews.NewsID = vm.News.NewID;
                            vm.TopNews.SecID = 0;
                            vm.TopNews.CatID = vm.News.CategoryID.Value;
                            vm.TopNews.JournalID = vm.News.JournalID;
                            vm.TopNews.DisplayOrder = 0;
                            tnB.AddIfNotExists(vm.TopNews, existOnSliderExpr);
                            this.SaveChanges();

                            vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == 0 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID, null);
                            foreach (var item in vm.TopNewsOrderedlst)
                            {
                                item.DisplayOrder = item.DisplayOrder + 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }

                            // To Remove news that changed section after published 
                            int count = 0;
                            foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                            {
                                if (item.DisplayOrder != count + 1)
                                {
                                    item.DisplayOrder = count + 1;
                                    tnB.Edit(item);
                                    this.SaveChanges();
                                }
                                count++;
                            }

                            vm.TopNewsTemp = tnB.Load(e => e.SecID == 0 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > 8, null).FirstOrDefault();
                            if (vm.TopNewsTemp != null)
                            {
                                tnB.Delete(vm.TopNewsTemp);
                                this.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        if (!vm.SliderNewsFlag)
                        {
                            vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == 0 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > sliderNew.DisplayOrder, null);
                            foreach (var item in vm.TopNewsOrderedlst)
                            {
                                item.DisplayOrder = item.DisplayOrder - 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }
                            tnB.Delete(sliderNew);
                            this.SaveChanges();

                            // To Remove news that changed section after published 
                            int count = 0;
                            foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                            {
                                if (item.DisplayOrder != count + 1)
                                {
                                    item.DisplayOrder = count + 1;
                                    tnB.Edit(item);
                                    this.SaveChanges();
                                }
                                count++;
                            }
                        }

                    }

                    if (mo7taratNew == null)
                    {
                        if (vm.Mo7taratNewsFlag)
                        {
                            vm.TopNews = new TopNews();
                            vm.TopNews.NewsID = vm.News.NewID;
                            vm.TopNews.SecID = -1;
                            vm.TopNews.CatID = vm.News.CategoryID.Value;
                            vm.TopNews.JournalID = vm.News.JournalID;
                            vm.TopNews.DisplayOrder = 0;
                            tnB.AddIfNotExists(vm.TopNews, existOnMo7taratExpr);
                            this.SaveChanges();

                            vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == -1 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID, null);
                            foreach (var item in vm.TopNewsOrderedlst)
                            {
                                item.DisplayOrder = item.DisplayOrder + 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }

                            // To Remove news that changed section after published 
                            int count = 0;
                            foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                            {
                                if (item.DisplayOrder != count + 1)
                                {
                                    item.DisplayOrder = count + 1;
                                    tnB.Edit(item);
                                    this.SaveChanges();
                                }
                                count++;
                            }

                            vm.TopNewsTemp = tnB.Load(e => e.SecID == -1 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > 8, null).FirstOrDefault();
                            if (vm.TopNewsTemp != null)
                            {
                                tnB.Delete(vm.TopNewsTemp);
                                this.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        if (!vm.Mo7taratNewsFlag)
                        {
                            vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == -1 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > mo7taratNew.DisplayOrder, null);
                            foreach (var item in vm.TopNewsOrderedlst)
                            {
                                item.DisplayOrder = item.DisplayOrder - 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }
                            tnB.Delete(mo7taratNew);
                            this.SaveChanges();

                            // To Remove news that changed section after published 
                            int count = 0;
                            foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                            {
                                if (item.DisplayOrder != count + 1)
                                {
                                    item.DisplayOrder = count + 1;
                                    tnB.Edit(item);
                                    this.SaveChanges();
                                }
                                count++;
                            }
                        }
                    }


                    // خبر عاجل متحرك  Add Record to NewsTicker Table
                    if (vm.TickerNewsFlag)
                    {

                        vm.NewsTicker = new NewsTicker();
                        vm.NewsTicker.NewsID = vm.News.NewID;
                        vm.NewsTicker.Title = vm.News.Title;
                        vm.NewsTicker.Added_Date = DateTime.Now;
                        vm.NewsTicker.JournalID = vm.News.JournalID;
                        vm.NewsTicker.SectionId = vm.News.SectionID;
                        Func<NewsTicker, bool> expr1 = (x => x.NewsID == vm.News.NewID);
                        ntB.AddIfNotExists(vm.NewsTicker, expr1);
                        this.SaveChanges();
                    }
                    else
                    {
                        vm.NewsTicker = ntB.Load(e => e.NewsID == vm.News.NewID, null).FirstOrDefault();
                        if (vm.NewsTicker != null)
                        {
                            ntB.Delete(vm.NewsTicker);
                            this.SaveChanges();
                        }
                    }


                }
            }
            return vm;
        }

        private NewsVM Publish(NewsVM vm)
        {
            if (vm != null)
            {
                if (vm.News != null)
                {

                    TopNewsBusiness tnB = new TopNewsBusiness(this.DbContext);
                    NewsTickerBusiness ntB = new NewsTickerBusiness(this.DbContext);

                    Func<TopNews, bool> newsexpr = (x => x.NewsID == vm.News.NewID);
                    Func<TopNews, bool> catexpr = (x => x.CatID == vm.News.CategoryID.Value);
                    Func<TopNews, bool> journalexpr = (x => x.JournalID == vm.News.JournalID);

                    Func<TopNews, bool> slidexpr = (x => x.SecID == 0);
                    Func<TopNews, bool> secexpr = (x => x.SecID == vm.News.SectionID);
                    Func<TopNews, bool> mo7taratexpr = (x => x.SecID == -1);

                    Func<TopNews, bool> existOnSliderExpr = (x => newsexpr(x) && catexpr(x) && journalexpr(x) && slidexpr(x));
                    Func<TopNews, bool> existOnMo7taratExpr = (x => newsexpr(x) && catexpr(x) && journalexpr(x) && mo7taratexpr(x));

                    Func<TopNews, bool> existOnHomeSecExpr = (x => newsexpr(x) && catexpr(x) && journalexpr(x) && secexpr(x));

                    List<TopNews> nl = tnB.Load(e => e.NewsID == vm.News.NewID);
                    TopNews sliderNew = nl.Where(existOnSliderExpr).FirstOrDefault();
                    TopNews homeSecNew = nl.Where(existOnHomeSecExpr).FirstOrDefault();
                    TopNews mo7taratNew = nl.Where(existOnMo7taratExpr).FirstOrDefault();

                    // خبر سليدر  Add Record to TopNews Table with  SecId=0
                    if (vm.SliderNewsFlag)
                    {
                        vm.TopNews = new TopNews();
                        vm.TopNews.NewsID = vm.News.NewID;
                        vm.TopNews.SecID = 0;
                        vm.TopNews.CatID = vm.News.CategoryID.Value;
                        vm.TopNews.JournalID = vm.News.JournalID;
                        vm.TopNews.DisplayOrder = 0;
                        tnB.AddIfNotExists(vm.TopNews, existOnSliderExpr);
                        this.SaveChanges();

                        vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == 0 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID, null);
                        foreach (var item in vm.TopNewsOrderedlst)
                        {
                            item.DisplayOrder = item.DisplayOrder + 1;
                            tnB.Edit(item);
                            this.SaveChanges();
                        }

                        // To Remove news that changed section after published 
                        int count = 0;
                        foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                        {
                            if (item.DisplayOrder != count + 1)
                            {
                                item.DisplayOrder = count + 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }
                            count++;
                        }

                        vm.TopNewsTemp = tnB.Load(e => e.SecID == 0 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > 8, null).FirstOrDefault();
                        if (vm.TopNewsTemp != null)
                        {
                            tnB.Delete(vm.TopNewsTemp);
                            this.SaveChanges();
                        }
                    }
                    // خبر رئيسى للقسم  Add Record to TopNews Table with Actual SecId
                    if (vm.SectionNewsFlag)
                    {
                        vm.TopNews = new TopNews();
                        vm.TopNews.NewsID = vm.News.NewID;
                        vm.TopNews.SecID = vm.News.SectionID;//-2;
                        vm.TopNews.CatID = vm.News.CategoryID.Value;
                        vm.TopNews.JournalID = vm.News.JournalID;
                        vm.TopNews.DisplayOrder = 0;
                        tnB.AddIfNotExists(vm.TopNews, existOnHomeSecExpr);
                        this.SaveChanges();

                        vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == vm.News.SectionID && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID, null);
                        foreach (var item in vm.TopNewsOrderedlst)
                        {
                            item.DisplayOrder = item.DisplayOrder + 1;
                            tnB.Edit(item);
                            this.SaveChanges();
                        }

                        // To Remove news that changed section after published 
                        int count = 0;
                        foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                        {
                            if (item.DisplayOrder != count + 1)
                            {
                                item.DisplayOrder = count + 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }
                            count++;
                        }

                        vm.TopNewsTemp = tnB.Load(e => e.SecID == vm.News.SectionID && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > 8, null).FirstOrDefault();
                        if (vm.TopNewsTemp != null)
                        {
                            tnB.Delete(vm.TopNewsTemp);
                            this.SaveChanges();
                        }
                    }
                    // خبر مختارات  Add Record to TopNews Table with SecId=-1
                    if (vm.Mo7taratNewsFlag)
                    {
                        vm.TopNews = new TopNews();
                        vm.TopNews.NewsID = vm.News.NewID;
                        vm.TopNews.SecID = -1;
                        vm.TopNews.CatID = vm.News.CategoryID.Value;
                        vm.TopNews.JournalID = vm.News.JournalID;
                        vm.TopNews.DisplayOrder = 0;
                        tnB.AddIfNotExists(vm.TopNews, existOnMo7taratExpr);
                        this.SaveChanges();

                        vm.TopNewsOrderedlst = tnB.Load(e => e.SecID == -1 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID, null);
                        foreach (var item in vm.TopNewsOrderedlst)
                        {
                            item.DisplayOrder = item.DisplayOrder + 1;
                            tnB.Edit(item);
                            this.SaveChanges();
                        }

                        // To Remove news that changed section after published 
                        int count = 0;
                        foreach (var item in vm.TopNewsOrderedlst.OrderBy(e => e.DisplayOrder))
                        {
                            if (item.DisplayOrder != count + 1)
                            {
                                item.DisplayOrder = count + 1;
                                tnB.Edit(item);
                                this.SaveChanges();
                            }
                            count++;
                        }

                        vm.TopNewsTemp = tnB.Load(e => e.SecID == -1 && e.CatID == vm.News.CategoryID.Value && e.JournalID == vm.News.JournalID && e.DisplayOrder > 8, null).FirstOrDefault();
                        if (vm.TopNewsTemp != null)
                        {
                            tnB.Delete(vm.TopNewsTemp);
                            this.SaveChanges();
                        }
                    }
                    // خبر عاجل متحرك  Add Record to NewsTicker Table 
                    if (vm.TickerNewsFlag)
                    {
                        vm.NewsTicker = new NewsTicker();
                        vm.NewsTicker.NewsID = vm.News.NewID;
                        vm.NewsTicker.Title = vm.News.Title;
                        vm.NewsTicker.Added_Date = DateTime.Now;
                        vm.NewsTicker.JournalID = vm.News.JournalID;
                        vm.NewsTicker.SectionId = vm.News.SectionID;

                        Func<NewsTicker, bool> expr1 = (x => x.NewsID == vm.News.NewID);
                        ntB.AddIfNotExists(vm.NewsTicker, expr1);
                        this.SaveChanges();

                    }
                }
            }
            return vm;
        }

        private NewsVM Pass(NewsVM vm)
        {
            if (vm != null)
            {
                if (vm.News != null)
                {

                    if (vm.News.NewStatus == 1) // محرر
                    {
                        vm.News.sEditorID = LoginUserID;
                        vm.News.NewStatus = 2; // مدير القسم
                        return vm;
                    }
                    if (vm.News.NewStatus == 2) // مدير القسم
                    {
                        vm.News.sDepartDirectorID = LoginUserID;
                        vm.News.NewStatus = 6; // للديسك
                        return vm;
                    }
                    //if (vm.News.NewStatus == 6) // الديسك
                    //{
                    //    vm.News.sUploaderID = LoginUserID;
                    //    vm.News.NewStatus = 7; // النشر
                    //    return vm;
                    //}
                }
            }

            return vm;
        }

        private NewsVM FillSectionDDL(NewsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            List<UserRole> lstUserPermission = new List<UserRole>();
            UserRoleBusiness uRB = new UserRoleBusiness(this.DbContext);
            lstUserPermission = uRB.Load(e => e.JournalID == 1 && e.AdminUserID == LoginUserID, null);

            List<int> secIDLs = new List<int>();
            foreach (var item in lstUserPermission)
            {
                secIDLs.Add(item.SectionID.Value);
            }

            List<MainSection> sections = secB.Load(e => e.Hide == 0, null);

            MainSection selectLstItem = new MainSection();
            selectLstItem.SectionID = 0;
            selectLstItem.SecTitle = "الصفحة الرئيسية";
            vm.ActiveSectionLst.Add(selectLstItem);

            selectLstItem = new MainSection();
            selectLstItem.SectionID = -1;
            selectLstItem.SecTitle = "اهم الاخبار";
            vm.ActiveSectionLst.Add(selectLstItem);

            foreach (MainSection item in sections)
            {
                selectLstItem = new MainSection();
                selectLstItem.SectionID = item.SectionID;
                selectLstItem.SecTitle = item.SecTitle;
                vm.ActiveSectionLst.Add(selectLstItem);
            }

            return vm;
        }

        private NewsVM FillAllowedSectionDDL(NewsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            UserRoleBusiness uRB = new UserRoleBusiness(this.DbContext);

            vm.UserRoles = uRB.Load(e => e.AdminUserID == LoginUserID, null).ToList();

            List<int> secIDLs = new List<int>();
            foreach (var item in vm.UserRoles)
            {
                secIDLs.Add(item.SectionID.Value);
            }
            if (secIDLs.Contains(-1) || secIDLs.Contains(0))
                vm.ActiveSectionLst = secB.Load(e => e.Hide == 0, null);
            else
                vm.ActiveSectionLst = secB.Load(e => e.Hide == 0 && (secIDLs.Contains(e.SectionID)), null);

            return vm;
        }

        private NewsVM FillSectionDDLForSearch(NewsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            List<MainSection> ms = new List<MainSection>();
            ms = secB.Load(e => e.Hide == 0, null);
            MainSection msTemp = new MainSection();
            msTemp.SectionID = 0;
            msTemp.SecTitle = "كل الابواب";
            vm.ActiveSectionLst.Add(msTemp);
            foreach (var item in ms)
            {
                msTemp = new MainSection();
                msTemp.SectionID = item.SectionID;
                msTemp.SecTitle = item.SecTitle;
                vm.ActiveSectionLst.Add(msTemp);
            }
            return vm;
        }

        #endregion

    }

}
