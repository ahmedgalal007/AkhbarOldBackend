using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using AkhbarElyoum.Models;
using System.Text;
using System.Data.SqlClient;

namespace AkhbarElyoum.Controllers
{
    public class NewsController : Controller
    {
        AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();

        public ActionResult tiker(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetTicker(JournalID));
        }
        public ActionResult TickerFrame(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IList<Ticker> lst_Highlight;
            lst_Highlight = (from p in db.sp_GetTicker(JournalID)
                             select new Ticker
                             {
                                 NewsID = p.NewsID,
                                 Title = p.Title
                             }).ToList();
            return PartialView(lst_Highlight);
        }
        public IList<Ticker> GetTicker(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IList<Ticker> lst_Highlight;
            lst_Highlight = (from p in db.sp_GetTicker(JournalID)
                             select new Ticker
                             {
                                 NewsID = p.NewsID,
                                 Title = p.Title,
                                 SecTitle=p.SecTitle
                             }).ToList();
            return lst_Highlight;
        }


        

        [OutputCache(Duration = 120)]
        public ActionResult NewDetails(int ID, int? JournalID, string name)
        {
            List<NewsDetails> lst = (GetNewsDetails(ID));
            if (lst.Count > 0)
            {
                    return PartialView(lst);   
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public void UpdateViews(int ID)
        {
            db.UpdateViews(ID);
        }
        public ActionResult GetTodayStar(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            List<NewsDetails> lst_TodayStar = (from p in db.TodayStar(JournalID)
                                               select new NewsDetails
                                               {
                                                   NewsID = p.NewID,
                                                   Title = p.Title,
                                                   PictureName1 = (p.picturename != null) ? p.picturename : "",
                                                   PictureCaption1 = (p.PictureCaption1 != null) ? 
                                                   p.PictureCaption1 : "",
                                                   EditorName=(p.editorname!=null)?p.editorname:"",
                                                   Breif = (p.Brief != null) ? p.Brief : ""
                                               }).ToList();
            return View(lst_TodayStar);
        }
        public List<NewsDetails> GetNewsDetails(int NewID = 0)
        {
            var lst_Ticker = new List<NewsDetails>();
            lst_Ticker = (from p in db.sp_GetNewsDetails(NewID)
                          select new NewsDetails
                          {
                              NewsID = p.NewID,
                              Title = (p.Title != null) ? p.Title : "",
                              PictureName1 = (p.PictureName1 != null) ? p.PictureName1 : "",
                              PictureCaption1 = (p.PictureCaption1 != null) ? p.PictureCaption1 : "",

                              SocialPictureName = (p.SocialPictureName != null) ? p.SocialPictureName : "",
                              SocialPictureCaption = (p.SocialPictureCaption != null) ? p.SocialPictureCaption : "",
                              SocialTitle = (p.SocialTitle != null) ? p.SocialTitle : "",

                              PublishDate = (p.AddedDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy - hh:mm tt}", p.AddedDate) : "",
                              PubDate = p.AddedDate,
                              Story = (p.Story != null) ? p.Story : "",
                              SectionID = p.SectionID,
                              SectionTitle=p.SecTitle,
                              ByLine = (p.ByLine != null) ? p.ByLine : "",
                              SubTitle = (p.SubTitle != null) ? p.SubTitle : "",
                              EditorName = p.EditorName,
                              EditorID = p.EditorID,
                              Keywords = p.Keywords,
                              Breif = (p.Brief != null) ? p.Brief : "",
                              CategoryID = (p.CategoryID != null) ? p.CategoryID : 1
                          }).ToList();
            if (lst_Ticker.Count > 0)
            {
                ViewBag.Title = lst_Ticker.FirstOrDefault().Title;
            }

             db.sp_NewsView(NewID);

            return lst_Ticker;
        }
       
        public ActionResult IndexSearchNews()
        {
            return (RedirectToAction("/Search?query="));
        }
        public ActionResult Search(string query, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            ViewBag.SearchQuery = query;
            if (query != string.Empty && query != null)
                return PartialView(SearchNews(JournalID, query, 0));
            else
                return (RedirectToAction("Index", "Home"));
        }
        public ActionResult GetAttachments(int NID)
        {
            return PartialView(GetAttachmentsList(NID));
        }
        public List<AkhbarElyoum.Models.Attachment> GetAttachmentsList(int NID)
        {
            var lst_News = new List<AkhbarElyoum.Models.Attachment>();
            lst_News = (from p in db.GetNewsAttachment(NID)
                        select new AkhbarElyoum.Models.Attachment
                        {
                            NewID = p.NewID,
                            FileName = p.FileName,
                            FieldID = p.FileId,
                            Discription = p.Discription
                        }).ToList();
            return (lst_News.ToList());
        }
        public ActionResult SearchPaging(int JournalID, int? LastID, string query)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(SearchNews(JournalID, query, LastID));
        }
        public List<NewsDetails> SearchNews(int JournalID, string Title, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_News = new List<NewsDetails>();
            ViewBag.Title = Title;
            lst_News = (from p in db.News_SearchTop(Title, LastID, JournalID)
                        select new NewsDetails
                        {
                            NewsID = p.NewID,
                            Title = (p.Title != null) ? p.Title : " ",
                            Breif = (p.Brief != null) ? p.Brief : " ",
                            PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                            PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                        }).ToList();
            return (lst_News.ToList());
        }
      
      

        public List<News> GetSectionList(int JournalID, int SectionID, int? page)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_News = new List<News>();
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            lst_News = (from p in db.sp_GetSectionLatestNews(SectionID, JournalID)
                        select new News
                        {
                            NewsID = p.NewID,
                            Title = (p.Title != null) ? p.Title : " ",

                        }).ToList();
            return (lst_News.ToList());
        }
        public ActionResult GetMostCommentedList(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1; 
            var lst_MostCommentedNews = (from p in db.sp_GetHomeMostCommentedNews(JournalID)
                                         select new NewsDetails
                                         {
                                             NewsID = p.NewID,
                                             Title = p.Title

                                         });
            return PartialView(lst_MostCommentedNews.ToList());
        }

        // Sart test Page
        public ActionResult GetTopStoriesTest()
        {
            return PartialView(GetTopStories(1));
        }
        // End Test Page
     
        [OutputCache(Duration = 60)]
        public ActionResult GetTopStoriesList1(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetTopStories(JournalID));





        }
       
        public List<NewsDetails> GetTopStories(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeTopStories(JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       SectionTitle = p.SecTitle,
                                       PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
																		 MagazineLogo = p.MagazineLogo,
																		 MagazineName = p.MagazineName,
																		 MagazineNikeName = p.MagazineNikeName,
																	 });




            return lst_Newsdetails.ToList();

        }


        // start sport slider


        [OutputCache(Duration = 60)]
        public ActionResult GetTopStoriesList2(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetTopStories_sport(JournalID));
        }

        public List<NewsDetails> GetTopStories_sport(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_SportsHomeTopStories(JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       SectionTitle = p.SecTitle,
                                       PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                       MagazineLogo = p.MagazineLogo,
                                       MagazineName = p.MagazineName,
                                       MagazineNikeName = p.MagazineNikeName,
                                   });
            return lst_Newsdetails.ToList();
        }


        // end sport slider





        [OutputCache(Duration = 60)]
        public ActionResult GetHomeList1(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHome(JournalID));
        }
        [OutputCache(Duration = 60)]
        public ActionResult GetHomeList(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHome(JournalID));
        }
        public List<NewsDetails> GetHome(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeLatestNews(JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       SecTitle = p.SecTitle,
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                       DisplayOrder = p.DisplayOrder,
                                       SectionID = p.SectionID,
                                       ByLine = p.byline
                                   });
            return lst_Newsdetails.ToList();
        }
        public ActionResult NewsSectionTop(int JournalID, int SecID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetNewsSectionTop(SecID, JournalID));
        }
        public List<NewsDetails> GetNewsSectionTop(int SecID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.sp_GetSectionTopNews(SecID, JournalID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SectionID=SecID,
                                   SecTitle = p.SecTitle,
                                   NewsID = p.NewsID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " "
                               }).ToList();
            if (lst_Newsdetails.Count > 0)
                ViewBag.Title = lst_Newsdetails.First().SecTitle;
            return (lst_Newsdetails.ToList());
        }

        [OutputCache(Duration = 60)]
        public ActionResult NewsSectionLeftSide(int SecID)
        {
            return PartialView(GetNewsSectionLeftSide(SecID));
        }
        public List<NewsDetails> GetNewsSectionLeftSide(int SecID)
        {
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.sp_GetNewsLeftSideServices(SecID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SectionID = SecID,
                                   SecTitle = p.SecTitle,
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " "
                               }).ToList();
            if (lst_Newsdetails.Count > 0)
                ViewBag.Title = lst_Newsdetails.First().SecTitle;
            return (lst_Newsdetails.ToList());
        }


        //[OutputCache(Duration = 60)]
        public ActionResult NewsSection2(int ID, int JournalID, string name)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            //ViewBag.SectionId = ID;
            return PartialView("NewsSection2", GetNewsSection(ID, JournalID, 0));
        }

        [OutputCache(Duration = 60)]
        public ActionResult NewsSection(int ID, int JournalID, string name)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            ViewBag.SectionId = ID;
            return PartialView("NewsSection", GetNewsSection(ID, JournalID, 0));
        }

        [OutputCache(Duration = 60)]
        public ActionResult NewsSectionPaged( int SecID, int JournalID,int LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            ViewBag.SectionId = SecID;
            List<NewsDetails> lst = GetNewsSection(SecID, JournalID, LastID);
            if (Session["dateprev"] != null)
            {
                ViewData["Date"] = Session["dateprev"];
                Session["dateprev"] = Session["datenext"];
            }
            return PartialView(lst);
        }


       // [OutputCache(Duration = 60)]
        public ActionResult SpecificSection(int ID, int JournalID, string name)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView("SpecificSection", SpecificSectionNews(ID, JournalID, 0));
        }

        public List<NewsDetails> SpecificSectionNews(int SecID, int JournalID, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.sp_GetSectionNews_paged(SecID, JournalID, LastID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = p.SecTitle,
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                   IssueDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dd MMMM yyyy}", p.PublishDate) : "",
                                   SectionID = SecID,
                                   Keywords = (p.Keywords != null) ? p.Keywords : " ",
                                   Desription = (p.Description != null) ? p.Description : " "
                               }).ToList();
            if (LastID <= 0 && lst_Newsdetails.Count > 0)
            {
                Session["dateprev"] = "";
                Session["datenext"] = "";
                ViewData["Date"] = "";
                ViewBag.Title = lst_Newsdetails.FirstOrDefault().SecTitle;
                Session["dateprev"] = lst_Newsdetails.LastOrDefault().IssueDate.ToString();
                //ViewData["Date"] = Session["dateprev"];
            }
            if (lst_Newsdetails.Count > 0 && LastID != 0)
            {
                //ViewData["Date"] = Session["dateprev"];
                Session["datenext"] = lst_Newsdetails.LastOrDefault().IssueDate;

            }
            return (lst_Newsdetails.ToList());
        }


        public List<NewsDetails> GetNewsSection(int SecID, int JournalID, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.sp_GetSectionNews_paged(SecID, JournalID, LastID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = p.SecTitle,
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                   IssueDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dd MMMM yyyy}", p.PublishDate) : "",
                                   SectionID = SecID,
                                   Keywords = (p.Keywords != null) ? p.Keywords : " ",
                                   Desription = (p.Description != null) ? p.Description : " "
                               }).ToList();
            if (LastID <= 0 && lst_Newsdetails.Count > 0)
            {
                Session["dateprev"] = "";
                Session["datenext"] = "";
                ViewData["Date"] = "";
                ViewBag.Title = lst_Newsdetails.FirstOrDefault().SecTitle;
                Session["dateprev"] = lst_Newsdetails.LastOrDefault().IssueDate.ToString();
                //ViewData["Date"] = Session["dateprev"];
            }
            if (lst_Newsdetails.Count > 0 && LastID != 0)
            {
                //ViewData["Date"] = Session["dateprev"];
                Session["datenext"] = lst_Newsdetails.LastOrDefault().IssueDate;

            }
            return (lst_Newsdetails.ToList());
        }

        public ActionResult GetHomeMostCommentedSectionList(int SectionID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHomeMostCommentedSection(SectionID, JournalID));
        }

        public List<NewsDetails> GetHomeMostCommentedSection(int SectionID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeMostCommentedNewsSection(SectionID, JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       CategoryID = p.categoryid,
                                       EditorID = p.editorid
                                   });
            return lst_Newsdetails.ToList();
        }
        public ActionResult GetHomeMostCommentedList(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHomeMostCommented(JournalID));
        }
        public List<NewsDetails> GetHomeMostCommented(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeMostCommentedNews(JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       CategoryID = p.categoryid,
                                       EditorID = p.editorid
                                   });
            return lst_Newsdetails.ToList();
        }
        public ActionResult GetHomeMostViewedSectionList(int SectionID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHomeMostViewedSection(SectionID, JournalID));
        }
        public List<NewsDetails> GetHomeMostViewedSection(int SectionID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeMostViewedNewsSection(SectionID, JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       CategoryID = p.categoryid,
                                       EditorID = p.editorid
                                   });
            return lst_Newsdetails.ToList();
        }
        public ActionResult GetHomeMostViewedList(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHomeMostViewed(JournalID));
        }

        

        public List<NewsDetails> GetHomeMostViewed(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeMostViewedNews(JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       CategoryID = p.categoryid,
                                       EditorID = p.editorid
                                   });
            return lst_Newsdetails.ToList();
        }
        public ActionResult GetHomeLatestSectionList(int SectionID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHomeLatestSection(SectionID, JournalID));
        }

        public List<NewsDetails> GetHomeLatestSection(int SectionID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeMostLatestSection(SectionID, JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       EditorID = p.editorid,
                                       CategoryID = p.categoryid
                                   });
            return lst_Newsdetails.ToList();
        }
        public ActionResult GetHomeLatestList(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHomeLatest(JournalID));
        }
        public List<NewsDetails> GetHomeLatest(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeMostLatest(JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title
                                   });
            return lst_Newsdetails.ToList();
        }
      
        public ActionResult GetHomeSecondSlideList1(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetMisc1(JournalID));
        }

        //public List<NewsDetails> GetMisc(int JournalID)
        //{
        //    var lst_Newsdetails = new List<NewsDetails>();
        //    lst_Newsdetails = (from p in db.sp_Misc(JournalID)
        //                       select new AkhbarElyoum.Models.NewsDetails
        //                       {
        //                           SecTitle = p.SecTitle,
        //                           NewsID = p.NewID,
        //                           Breif = (p.Brief != null) ? p.Brief : " ",
        //                           PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
        //                           Title = p.Title,
        //                           PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
        //                           SectionID = 148
        //                       }).Take(5).ToList();
        //    if (lst_Newsdetails.Count > 0)
        //    {
        //        ViewBag.Title = lst_Newsdetails.FirstOrDefault().SecTitle;
        //    }
        //    return (lst_Newsdetails.ToList());
        //}

        public List<NewsDetails> GetMisc1(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.sp_Misc1(JournalID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = "",
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = p.PictureName,
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                   SectionID = 148
                               }).Take(8).ToList();
            if (lst_Newsdetails.Count > 0)
            {
                ViewBag.Title = "مختارات";// "اهم اخبار المؤتمر الاقتصادي";
            }
            return (lst_Newsdetails.ToList());
        }

        public ActionResult GetHomeSecondSlideList(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHomeSecondSlide(JournalID));
        }
        public List<NewsDetails> GetHomeSecondSlide(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeLatestUnderSlide(JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = (p.NewID.ToString() != "") ? p.NewID : 0,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = (p.Title != null) ? p.Title : " "
                                   });
            return lst_Newsdetails.ToList();
        }


        public ActionResult ArticleSectionList(int id, int JournalID, string name)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(ArticleSection(id, JournalID, 0));
        }
        public ActionResult ArticleSectionListPaging(int EditorID ,int JournalID, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(ArticleSection(EditorID,JournalID, LastID));
        }
        public List<NewsDetails> ArticleSection( int EditorID,int JournalID, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<NewsDetails>();
            
           
            lst_Newsdetails = (from p in db.sp_GetEitorArticles_paged(EditorID, JournalID, LastID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = (p.Title != null) ? p.Title : " ",
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                   EditorID = p.EditorID,
                                   EditorName = p.EditorName
                               }).ToList();
            if (lst_Newsdetails.Count > 0)
                ViewBag.Title = lst_Newsdetails[0].EditorName;
            return (lst_Newsdetails.ToList());
        }


        public ActionResult Articles(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(EditorsArticles(JournalID, 0));
        }
        public ActionResult ArticlesPaging( int JournalID, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(EditorsArticles(JournalID, LastID));
        }
        public List<NewsDetails> EditorsArticles(int JournalID, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.EitorsArticles_paged(JournalID, LastID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = (p.Title != null) ? p.Title : " ",
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                   EditorID = p.EditorID,
                                   EditorName = p.EditorName
                               }).ToList();
            if (lst_Newsdetails.Count > 0)
                ViewBag.Title = "المقالات";
            return (lst_Newsdetails.ToList());
        }



        public ActionResult GetMoreReadListInside(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetMoreReadInsideNews(JournalID));
        }
        public ActionResult GetRelatedNewsList(int NID, int secid)
        {
            return PartialView(GetRelatedNews(NID, secid));
        }
        public List<NewsDetails> GetRelatedNews(int NID, int secid)
        {
            var lst_NewsDetails = new List<NewsDetails>();
            lst_NewsDetails = (from p in db.Newsofsamesec(NID, secid)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = (p.Title != null) ? p.Title : " ",
                                   NewsID = p.newid ,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " "
                               }).ToList();
            return (lst_NewsDetails.ToList());
        }

        public List<NewsDetails> GetMoreReadInsideNews(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetMostViewedNewsInside(JournalID)
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                   });
            return lst_Newsdetails.ToList();
        }

        public ActionResult GetSideSectionsList(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetSections(JournalID,""));
        }

        public ActionResult GetSectionsList(int JournalID, string Location)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetSections(JournalID, Location));
        }
        public ActionResult GetSectionsListFooter(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;

            return PartialView(GetSections(JournalID,""));
        }
        public List<Sections> GetSections(int JournalID,string Location)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;

            var lst_Sections = new List<Sections>();
            if (JournalID==7)
            {
                lst_Sections = (from p in db.SP_GetMainSections(JournalID, -1, 0, -1)
                                select new AkhbarElyoum.Models.Sections
                                {
                                    SectionID = p.SectionID,
                                    SectionName = p.SecTitle

                                }).ToList();
            }
            else
            {
                lst_Sections = (from p in db.SP_GetMainSections(JournalID, -1, 0, -1)
                            select new AkhbarElyoum.Models.Sections
                            {
                                SectionID = p.SectionID,
                                SectionName = p.SecTitle

                            }).ToList();
            }
            ViewBag.Loc = Location;
            return (lst_Sections.ToList());
        }
        public List<Sections> GetReport()
        {
            var lst_Newsdetails = new List<Sections>();
            lst_Newsdetails = (from p in db.GetPrivateFilesSections()
                               select new AkhbarElyoum.Models.Sections
                               {
                                   SectionName = p.sectitle,
                                   SectionID = p.sectionid
                               }).ToList();
            return (lst_Newsdetails.ToList());
        }
        public ActionResult GetReportNews()
        {
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.GetPrivateFilesNews()
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = p.SecTitle,
                                   NewsID = p.NewsID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                   SectionID = p.sectionid
                               }).ToList();
            if (lst_Newsdetails.Count > 0)
            {
                ViewBag.Title = lst_Newsdetails.FirstOrDefault().SecTitle;
            }
            return (PartialView(lst_Newsdetails.ToList()));
        }


        /// <summary>
        /// nogoomtv,mglta5barelnogom
        /// </summary>
        public ActionResult nogoomtv()
        {

            return View(nogomtvdetails(222));
        }
        public ActionResult nogoom_magazine()
        {

            return View(nogomtvdetails(221));
        }
        public List<NewsDetails> nogomtvdetails(int SECID)
        {

            AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
            List<NewsDetails> lst;
            lst = (from p in db.Secdetails(SECID)

                   select new NewsDetails
                   {

                       NewsID = p.NewID,
                       Title = (p.Title != null) ? p.Title : "",
                       PictureName1 = (p.picturename != null) ? p.picturename : "",
                       Story = (p.Story != null) ? p.Story : "",

                   }).ToList();

            return lst;
        }

        #region Karekater
        public ActionResult commic(int id)
        {
            return View(Commiclist(id));

        }
        public List<NewsDetails> Commiclist(int newid)
        {AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
            List<NewsDetails> lst;


            lst = (from p in db.Karekater(newid)

                   select new NewsDetails
                   {

                       NewsID = p.NewID,
                       Title = (p.Title != null) ? p.Title : "",
                       PictureName1 = (p.karekater != null) ? p.karekater : "",
                       PictureName2 = (p.elrsampicture != null) ? p.elrsampicture : "",
                       PictureCaption1 = (p.PictureCaption1 != null) ? p.PictureCaption1 : "",
                       PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy - hh:mm tt}", p.PublishDate) : "",
                       EditorName = (p.elrsam != null) ? p.elrsam : "",
                       EditorID = p.EditorID,
                       

                   }).ToList();


            return lst;
        
        
        
        }
        public ActionResult karekaters(int id)
        {

            return View(karekaterlist(id,0));
        }
        public ActionResult karekaterspaged(int LastID, int id)
        {
            return PartialView(karekaterlist(id, LastID));


        }
        public List<NewsDetails> karekaterlist(int id, int lastid)
        {
            AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
            List<NewsDetails> lst;
            lst = (from p in db.karekaters(id,lastid)

                   select new NewsDetails
                   {

                       NewsID = p.NewID,
                       Title = (p.Title != null) ? p.Title : "",
                       PictureName1 = (p.karekater != null) ? p.karekater : "",
                       PictureName2 = (p.elrsampicture != null) ? p.elrsampicture : "",
                       PictureCaption1 = (p.PictureCaption1 != null) ? p.PictureCaption1 : "",
                       PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy - hh:mm tt}", p.PublishDate) : "",
                       EditorName = (p.elrsam != null) ? p.elrsam : "",
                       EditorID = p.EditorID,


                   }).ToList();
            
            return lst;
        
        }




        public ActionResult photos()
        {

            return View(photoslist(0));
        }
        public ActionResult photospaged(int LastID)
        {
            return PartialView(photoslist(LastID));
        }
        public List<NewsDetails> photoslist( int lastid)
        {
            AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
            List<NewsDetails> lst;
            lst = (from p in db.photoalbums( lastid)

                   select new NewsDetails
                   {

                       NewsID = p.NewID,
                       Title = (p.Title != null) ? p.Title : "",
                       PictureName1 = (p.mainpic != null) ? p.mainpic : "",
                    
                       PictureCaption1 = (p.PictureCaption1 != null) ? p.PictureCaption1 : "",
                       PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy - hh:mm tt}", p.PublishDate) : ""
                   }).ToList();

            return lst;

        }

        public ActionResult album( int id)
        {

            return View(albumlist(id));
        }
        public List<NewsDetails> albumlist(int newid)
        {


            AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
            List<NewsDetails> lst;
            lst = (from p in db.adlbum(newid)

                   select new NewsDetails
                   {

                       NewsID = p.nid,
                      
                       PictureName1 = (p.picturename != null) ? p.picturename : "",

                       PictureCaption1 = (p.piccaption != null) ? p.piccaption : "",
                       Title = (p.title != null) ? p.title : ""
                
                   }).ToList();

            return lst;

        
        }


        #endregion

        #region cares version  

        public ActionResult cartabs( int journalid)
        {
            return PartialView(cartabsList(7));
        
        }

        public List<NewsDetails> cartabsList(int journalid)
        {


            IEnumerable<NewsDetails> lst_Newsdetails;


            lst_Newsdetails = (from p in db.sp_GetHomeLatestNewsforcars(journalid)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.picturename != null) ? p.picturename : " ",
                                   Title = p.newTitle,
                                   SectionTitle = (p.sectitle != null) ? p.sectitle : " ",
                                   SectionID = p.sectionid

                               }).ToList();
            return lst_Newsdetails.ToList();
        }

        [OutputCache(Duration = 60)]
        public ActionResult Specificsec(int ID, int JournalID, string name)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView("NewsSection", GetNewsSection(ID, JournalID, 170));
        }

        //public ActionResult onesec(int journalid, int sectionid)
        //{

        //    return View(specificseclist(journalid, sectionid));
        //}


        #endregion



        public ActionResult Print(int ID)
        {

            return View(GetNewsDetails(ID));
        }

        public ActionResult painters()
        {
            return PartialView(Painterslist());
        }


        public List<AkhbarElyoum.Models.Gallery> Painterslist()
        {

            List<AkhbarElyoum.Models.Gallery> g;
            g = (from q in db.Painters()
                 select new AkhbarElyoum.Models.Gallery
                 {

                     GalleryID = q.GalleryID,

                     Title = q.Title


                 }).ToList();
            return g;
        }

        public ActionResult painter(int ID)
        {
            return View(PainterPics(ID ,0));
        }

        public ActionResult painterpaged(int LastID,int Galleryid)
        {
            return View(PainterPics(Galleryid, LastID));
        }

        public List<AkhbarElyoum.Models.Gallery> PainterPics( int galleryid,int lastid)
        {

            List<AkhbarElyoum.Models.Gallery> g;
            g = (from q in db.Allpics_ofpainters(galleryid,lastid)
                 select new AkhbarElyoum.Models.Gallery
                 {

                     GalleryID = q.Galleryid,

                     Title = q.painter,
                     Picturename=q.picturename,
                     PicCaption=q.piccaption,
                     pictureid=q.Pictureid

                 }).ToList();
            return g;
        }

        [OutputCache(Duration = 120)]
        public ActionResult VideoDisplay(int ID, int? JournalID)
        {
            return PartialView(GetNewsDetails(ID));
        }

        public ActionResult GetHomeVideoList(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetHomeLatestVideos( JournalID));
        }

        public List<NewsDetails> GetHomeLatestVideos(int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            IEnumerable<NewsDetails>
                lst_Newsdetails = (from p in db.sp_GetHomeVideos()
                                   select new AkhbarElyoum.Models.NewsDetails
                                   {
                                       NewsID = p.NewID,
                                       Breif = (p.Brief != null) ? p.Brief : " ",
                                       PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                       Title = p.Title,
                                       EditorID = p.EditorID,
                                       CategoryID = p.CategoryID,
                                       Story=p.Story
                                   });
            return lst_Newsdetails.ToList();
        }


        public ActionResult GetRelatedNewsVideoList(int NID, int secid)
        {
            return PartialView(GetRelatedNewsVideo(NID, secid));
        }
        public List<NewsDetails> GetRelatedNewsVideo(int NID, int secid)
        {
            var lst_NewsDetails = new List<NewsDetails>();
            lst_NewsDetails = (from p in db.Newsofsamesec(NID, secid)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = (p.Title != null) ? p.Title : " ",
                                   NewsID = p.newid,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " "
                               }).ToList();
            return (lst_NewsDetails.ToList());
        }
















    }
}
