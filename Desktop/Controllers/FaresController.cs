using AkhbarElyoum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace AkhbarElyoum.Controllers
{
	public class FaresController : Controller
	{
		private AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
		private int SectionID1;
		private List<int> SubSectionsIds = new List<int>();
		public FaresController()
		{
			
			if (int.TryParse(WebConfigurationManager.AppSettings["FaresSectionID"],out SectionID1))
			{
				SectionID1 = int.Parse(WebConfigurationManager.AppSettings["FaresSectionID"]);
			}
			string subSetions = WebConfigurationManager.AppSettings["FaresSubSections"];


			SubSectionsIds = subSetions?.Split(',')?.Select(Int32.Parse)?.ToList();

		}
		// GET: Nogoom
		public ActionResult Index()
		{
			// System.Data.Linq.Table<NewsSection> NewsSections = db.NewsSections;
			// List<News> SectionList = GetSectionList(SectionID,1,1);
			// return View(new {/*Section = NewsSections,*/ List = SectionList});
			// var list = GetNewsSectionTop(SectionID, 1);
			// var Slider = GetNewsSection(SectionID, 1, 0).OrderByDescending(e => e.NewsID).Take(3).ToList();
			var Slider = GetSectionID1(SectionID1).Take(3).ToList();
			var news = new List<NewsDetails>();
			foreach (var Id in SubSectionsIds) {
				news.AddRange(GetNewsSectionTop(Id, 1));
			}
			var filterList = Slider.Select(e => e.NewsID).ToList();
			news = news.Where(e => !filterList.Contains(e.NewsID)).ToList();
			return View(new EsdarIndexModel{ Slider= Slider ,News = news});
		}

		public List<News> GetSectionList(int JournalID, int SectionID, int? page)
		{
			if (JournalID.ToString() == "")
				JournalID = 1;
			var lst_News = new List<News>();
			int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
			// رئيسي باب
			db.sp_GetSectionTopNews(SectionID, 1);
			lst_News = (from p in db.sp_GetSectionLatestNews(SectionID, JournalID)
									select new News
									{
										NewsID = p.NewID,
										Title = (p.Title != null) ? p.Title : " ",

									}).ToList();
			return (lst_News.ToList());
		}

		public List<NewsDetails> GetSectionID1(int SecID)
		{

			var lst_Newsdetails = new List<NewsDetails>();
			lst_Newsdetails = (from p in db.sp_GetSectionID1(SecID)
												 // where SubSectionsIds.Contains(p.SectionID)
												 select new AkhbarElyoum.Models.NewsDetails
												 {
													 SectionID = SecID,
													 SecTitle = p.SecTitle,
													 NewsID = p.NewID,
													 Breif = (p.Brief != null) ? p.Brief : " ",
													 PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
													 Title = p.Title,
													 PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " "
												 }).ToList();
			if (lst_Newsdetails.Count > 0)
				ViewBag.Title = lst_Newsdetails.First().SecTitle;
			return (lst_Newsdetails.ToList());
		}

		public List<NewsDetails> GetNewsSectionTop(int SecID, int JournalID)
		{
			if (JournalID.ToString() == "")
				JournalID = 1;
			var lst_Newsdetails = new List<NewsDetails>();
			lst_Newsdetails = (from p in db.sp_GetSectionTopNews(SecID, JournalID)
												 select new AkhbarElyoum.Models.NewsDetails
												 {
													 SectionID = SecID,
													 SecTitle = p.SecTitle,
													 NewsID = p.NewsID,
													 Breif = (p.Brief != null) ? p.Brief : " ",
													 PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
													 Title = p.Title,
													 PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
													 ByLine = p.ByLine,
													 SubTitle = p.SubTitle,
												 }).ToList();
			if (lst_Newsdetails.Count > 0)
				ViewBag.Title = lst_Newsdetails.First().SecTitle;
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

	}


}