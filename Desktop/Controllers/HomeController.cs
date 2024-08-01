using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AkhbarElyoum.Models;
using System.Xml.Linq;
using System.IO;

namespace AkhbarElyoum.Controllers
{
	public class HomeController : Controller
	{

		public static string GetPureURL(string URL)
		{
			string s;
			//s = (URL.Trim().Replace("_", "-").Replace("+", "-").Replace("«", " ").Replace(":", " ").Replace("»", " "));
			s = (URL.Trim().Replace("_", "-").Replace("+", "-").Replace("«", "-").Replace(":", "-").Replace("»", "-").Replace("/", "-").Replace("\\", "-").Replace(" ", "-").Replace("%", "-").Replace("|", "-"));
			s = s.Replace("--", "-").Replace("..", ".").Replace(".", "").Replace("\"", "").Trim();

			try
			{
				if (s.Length > 40)
				{
					s = s.Substring(0, 40);
				}
			}
			catch { }
			return s;
		}

		public ActionResult Index()
		{
			return View();
		}
		public ActionResult Nogoom()
		{
			return View();
		}
		public ActionResult Hawadeth()
		{
			return View();
		}
		public ActionResult Riadah()
		{
			return View();
		}
		public ActionResult Error()
		{
			return View();
		}

		public static string GetSubDomain(int JournalID)
		{
			string SubDomain = "akhbarelyom.com";

			return SubDomain;

		}
		public static string GetJournalID(string Domain, int? JournalID)
		{
			string journalID = "1";
			return journalID;
		}



		#region sitemapview
		//public ActionResult SectionsXMl()
		//{
		//    AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
		//    List<NewsDetails> lst;
		//    lst = (from p in db.MainSections.Where(a => a.Hide == 0 && a.WeeklySection == 0)
		//           select new NewsDetails
		//           {

		//               SectionID = p.SectionID,
		//               SecTitle = p.SecTitle

		//           }).ToList();
		//    // secSiteMapGen();
		//    return View(lst);

		//}
		//public ActionResult NewsXMl()
		//{
		//   AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
		//    List<NewsDetails> lst;
		//    lst = (from p in db.news_Sitemap()
		//           select new NewsDetails
		//           {

		//               NewsID = p.newid,
		//               Title = (p.Title != null) ? p.Title : "",

		//           }).ToList();

		//    //  NewsSiteMapGen();
		//    return View(lst);

		//}


		public string secSiteMapGen()
		{
			AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
			List<NewsDetails> lst;
			lst = (from p in db.sectionsSitemap()
						 select new NewsDetails
						 {

							 SectionID = p.sectionid,
							 SecTitle = p.sectitle

						 }).ToList();


			XDocument doc;
			XNamespace ns = "https://www.sitemaps.org/schemas/sitemap/0.9";
			using (FileStream fsBasic = new FileStream(Server.MapPath("~/App_Data/MapBasic.xml"), FileMode.Open, FileAccess.Read))
			{
				try
				{
					doc = XDocument.Load(fsBasic, LoadOptions.PreserveWhitespace);
				}
				catch (Exception ex)
				{

					throw ex;
				}
				doc.Root.Add(new XElement(ns + "url",
						 new XElement(ns + "loc", "https://akhbarelyom.com"),
						//new XElement(ns+ "loc", "https://www.akhbarelyom.com"),
						// new XElement("LastMod", DateTime.UtcNow),
						// new XElement(ns+ "LastMod", DateTime.Now.ToShortDateString()),
						new XElement(ns + "changefreq", "Daily"),
						 new XElement(ns + "priority", "1.0")
						));
				foreach (var item in lst)
				{
					doc.Root.Add(new XElement(ns + "url",
					 new XElement(ns + "loc", "https://akhbarelyom.com/news/newssection/" + @item.SectionID + "/1/" + GetPureURL(item.SecTitle) + ""),
					//new XElement(ns+"LastMod", DateTime.Now.ToShortDateString()),
					//new XElement("LastMod", DateTime.UtcNow),
					new XElement(ns + "changefreq", "Daily"),
					 new XElement(ns + "priority", "1.0")
					));
				}
				doc.Save(Server.MapPath("~/sitemapsections.xml"), SaveOptions.OmitDuplicateNamespaces);
			}
			Response.Clear();
			Response.ContentType = "text/xml";
			//Response.Write(doc.ToString());
			//Response.End();
			return doc.ToString();
		}

		public string NewsSiteMapGen()
		{
			AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
			List<NewsDetails> lst;
			lst = (from p in db.news_Sitemap()
						 select new NewsDetails
						 {

							 NewsID = p.newid,
							 Title = (p.Title != null) ? p.Title : "",

						 }).ToList();


			XDocument doc;
			XNamespace ns = "https://www.sitemaps.org/schemas/sitemap/0.9";
			using (FileStream fsBasic = new FileStream(Server.MapPath("~/App_Data/MapBasic.xml"), FileMode.Open, FileAccess.Read))
			{
				try
				{
					doc = XDocument.Load(fsBasic, LoadOptions.PreserveWhitespace);
				}
				catch (Exception ex)
				{
					throw ex;
				}
				doc.Root.Add(new XElement(ns + "url",
						new XElement(ns + "loc", "https://akhbarelyom.com"),
						//new XElement(ns + "LastMod", DateTime.Now.ToShortDateString()),
						// new XElement("LastMod", DateTime.UtcNow),
						new XElement(ns + "changefreq", "Daily"),
						 new XElement(ns + "priority", "1.0")
						));
				foreach (var item in lst)
				{
					doc.Root.Add(new XElement(ns + "url",
					 new XElement(ns + "loc", "https://akhbarelyom.com/news/newdetails/" + item.NewsID + "/1/" + GetPureURL(item.Title) + ""),
					//new XElement(ns+ "LastMod", DateTime.Now.ToShortDateString()),
					//new XElement("LastMod", DateTime.UtcNow),
					new XElement(ns + "changefreq", "Daily"),
					 new XElement(ns + "priority", "1.0")
					));
				}
				doc.Save(Server.MapPath("~/NewsSitemap.xml"), SaveOptions.OmitDuplicateNamespaces);
			}
			Response.Clear();
			Response.ContentType = "text/xml";
			//Response.Write(doc.ToString());
			//Response.End();
			return doc.ToString();
		}

		public string SiteMap()
		{

			XDocument doc;
			XNamespace ns = "https://www.sitemaps.org/schemas/sitemap/0.9";
			using (FileStream fsBasic = new FileStream(Server.MapPath("~/App_Data/Mainsitemap.xml"), FileMode.Open, FileAccess.Read))
			{
				try
				{
					doc = XDocument.Load(fsBasic, LoadOptions.PreserveWhitespace);
				}
				catch (Exception ex)
				{

					throw ex;
				}
				doc.Root.Add(new XElement(ns + "sitemap",
						new XElement(ns + "loc", "https://akhbarelyom.com/sitemapsections.xml")
						//new XElement("LastMod", DateTime.UtcNow)

						));
				doc.Root.Add(new XElement(ns + "sitemap",
					new XElement(ns + "loc", "https://akhbarelyom.com/NewsSitemap.xml")
					// new XElement("LastMod", DateTime.UtcNow)

					));

				// doc.Root.Add(new XElement("sitemap",
				//new XElement("loc", "  https://akhbarelyom.com/Mainsitemap.xml")
				//     // new XElement("LastMod", DateTime.UtcNow)

				//));
				doc.Save(Server.MapPath("~/sitemap.xml"), SaveOptions.OmitDuplicateNamespaces);
			}
			Response.Clear();
			Response.ContentType = "text/xml";

			//Response.Write(doc.ToString());
			//Response.End();
			return doc.ToString();

		}

		#endregion

		public ActionResult GetSiteLeftSideServices(int JournalID)
		{
			return PartialView("GetSiteLeftSideServices");
		}

		public ActionResult GetSiteLeftSideServicesInner(int JournalID)
		{
			return PartialView("GetSiteLeftSideServicesInner");
		}
		public ActionResult GetNewsLeftSideServices(int SecID)
		{
			ViewBag.secId = SecID;
			return PartialView("GetNewsLeftSideServices");
		}
		public ActionResult GetShareLinks(string URL, string Position)
		{
			ViewBag.Position = Position;
			ViewBag.URL = "https://akhbarelyom.com" + URL;
			return PartialView("GetShareLinks");
		}

		public ActionResult GetFooterBlock(int JournalID)
		{
			return PartialView("FooterIsdaratBlock");
		}

		[OutputCache(Duration = 120)]
		public ActionResult WhoWeAre()
		{
			return View();
		}

		[OutputCache(Duration = 120)]
		public ActionResult Privacy()
		{
			//return View(GetNewsPictures());
			return View();
		}

		[OutputCache(Duration = 120)]
		public ActionResult Responsibility()
		{
			return View();
		}

		[OutputCache(Duration = 120)]
		public ActionResult Advertising()
		{
			//return View(GetGalleryPictures());
			return View();
		}


		public List<Pictures> GetNewsPictures()
		{
			AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
			var lst_pic = new List<Pictures>();
			lst_pic = (from p in db.GetMissedNewsImages()
								 select new Pictures
								 {
									 PictureID = p.PictureID,
									 PictureName = (p.PictureName != null) ? p.PictureName : "",

								 }).ToList();

			return lst_pic;
		}

		public List<Pictures> GetGalleryPictures()
		{
			AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
			var lst_pic = new List<Pictures>();
			lst_pic = (from p in db.GetMissedGalleryImages()
								 select new Pictures
								 {
									 PictureID = p.PictureID,
									 PictureName = (p.PictureName != null) ? p.PictureName : "",
									 GDate = p.GDate
								 }).ToList();

			return lst_pic;

		}

	}
}
