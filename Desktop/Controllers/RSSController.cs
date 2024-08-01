using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AkhbarElyoum.Models;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AkhbarElyoum.Controllers
{
	public class RSSController : Controller
	{
		//
		// GET: /RSS/
		AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
		//public ActionResult GetSectionRSS(int JournalID)
		//{
		//    return PartialView(GetSectionRSSList(JournalID));
		//}
		//public List<Sections> GetSectionRSSList(int JournalID)
		//{
		//    var lst_Sections = new List<Sections>();
		//    lst_Sections = (from p in db.SP_GetMainSections(JournalID, -1, 0, -1)
		//                    select new AkhbarElyoum.Models.Sections
		//                    {
		//                        SectionID = p.SectionID,
		//                        SectionName = p.SecTitle

		//                    }).ToList();
		//    return (lst_Sections.ToList());
		//}
		//[OutputCache(Duration = 60)]
		public ActionResult NewsRSS(int SectionID)
		{
			Response.ContentType = "text/xml";
			return PartialView(GetSectionNewsRSSList(SectionID, 1));
		}
		public List<RSSNewsDetails> GetSectionNewsRSSList(int SecID, int JournalID)
		{
			Calendar myCal = new HijriCalendar();
			CultureInfo arEG = new CultureInfo("ar-EG");
			arEG.DateTimeFormat.Calendar = myCal;
			TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
			var lst_Newsdetails = new List<RSSNewsDetails>();
			lst_Newsdetails = (from p in db.sp_GetSectionNews_RSS(SecID, JournalID, 0)
												 select new AkhbarElyoum.Models.RSSNewsDetails
												 {
													 SecTitle = p.SecTitle,
													 NewsID = p.NewID,
													 //Story = Regex.Replace(p.Story, @"<[^>]+>|&nbsp;", "").Trim(),
													 Story = p.Story,
													 Breif = (p.Brief != null) ? p.Brief : " ",
													 PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
													 Title = p.Title,
													 PublishDate = String.Format("{0:r}", p.PublishDate ?? DateTime.Now),
													 PublishDateAR =  TimeZoneInfo.ConvertTimeFromUtc((DateTime)p.PublishDate, tzi).ToString("dddd MMMM dd-MM-yyyy",arEG),

													 SectionID = SecID
												 }).ToList();
			return (lst_Newsdetails.ToList());
		}

	}
}
