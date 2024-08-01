using AkhbarElyoum.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace AkhbarElyoum.Controllers
{
    public class SitemapController : Controller
    { 
        List<ISitemapItem> mainSiteMapItems;
        List<ISitemapItem> storiesSiteMapItems;
        
        public XmlSitemapResult Index(int? year)
        {
			mainSiteMapItems = new List<ISitemapItem>();

            AddEntries(year);

            return new XmlSitemapResult(mainSiteMapItems,false);
        }

        private void AddEntries(int? year)
        {
            // DateTime startdate = new DateTime(2011, 11, 30);

            DateTime enddate = DateTime.Now;
            DateTime startdate = enddate.AddDays(-(12 * 365));
            if(!(year == null) && year > 0)
            {
                bool Kabisaa = DateTime.DaysInMonth((int)year, 02) == 29;
                startdate = new DateTime((int)year, 1, 1).AddDays(-1);
                enddate = startdate.AddDays(Kabisaa ? 366 : 365);
            }
            var dates = new List<DateTime>();

            for (var dt = enddate; dt > startdate; dt = dt.AddDays(-1))
            {
                if(!(dt > DateTime.Now))
                    dates.Add(dt);
            }

            foreach (var item in dates)
            {
				        string url = "https://akhbarelyom.com/Sitemap/" + @item.Year + "/" + @item.Month + "/" + @item.Day + "";

				        this.mainSiteMapItems.Add(
						        new SitemapItem(url)
						        {
							        LastModified = item,
							        Priority = 1,
							        ChangeFrequency = ChangeFrequency.Daily
						        }
						    );

			      }

        }

        public XmlSitemapResult Stories(int year,int month,int day)
        {
            storiesSiteMapItems = new List<ISitemapItem>();

            AddStories(year, month, day);

            return new XmlSitemapResult(storiesSiteMapItems,true);
        }


        private void AddStories(int year, int month, int day)
        {       
            try
            {
                DateTime requiredDate = new DateTime(year, month, day); 
                DateTime startDate = requiredDate.AddDays(-1);
                DateTime endDate = requiredDate.AddDays(1);

                AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
                List<NewsDetails> lst;
                lst = (from p in db.newsSitemapOfSpecificDay(startDate, endDate)
                       select new NewsDetails
                       {
                           NewsID = p.newid,
                           Title = (p.Title != null) ? p.Title : "",
                           PictureName1 = p.PictureName1,
                           PictureCaption1 = p.PictureCaption1,
                           PubDate = p.PublishDate
                       }).ToList();

                foreach (var item in lst)
                {
					TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

										string url = "https://akhbarelyom.com/news/newdetails/" + @item.NewsID + "/1/" + HomeController.GetPureURL(@item.Title) + "";
					string murl = "https://m.akhbarelyom.com/news/newdetails/" + @item.NewsID + "/1/" + HomeController.GetPureURL(@item.Title) + "";
					string ampurl = "https://m.akhbarelyom.com/news/newdetails/" + @item.NewsID + "/1/" + HomeController.GetPureURL(@item.Title) + "/amp";

					storiesSiteMapItems.Add(
                        new SitemapItem(url)
                        {
							// LastModified = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.PubDate, tzi),
							LastModified = item.PubDate,
							Priority = (float)0.6,
                            ChangeFrequency = ChangeFrequency.Always,
                            image = item.PictureName1,
                            imageCaption = item.PictureCaption1,
                            imageUrl = "https://Images.akhbarelyom.com/images/images/Large/" + item.PictureName1,
                            imageTitle = item.Title
                        }
                        );
					storiesSiteMapItems.Add(
												new SitemapItem(murl)
												{
                                                    //LastModified = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.PubDate, tzi),
                                                    LastModified = item.PubDate,
                                                    Priority = (float)0.6,
													ChangeFrequency = ChangeFrequency.Always,
													image = item.PictureName1,
													imageCaption = item.PictureCaption1,
													imageUrl = "https://Images.akhbarelyom.com/images/images/Large/" + item.PictureName1,
													imageTitle = item.Title
												}
												);
					//storiesSiteMapItems.Add(
					//							new SitemapItem(ampurl)
					//							{
					//								LastModified = TimeZoneInfo.ConvertTimeFromUtc((DateTime)item.PubDate, tzi),
					//								Priority = (float)0.6,
					//								ChangeFrequency = ChangeFrequency.Always,
					//								image = item.PictureName1,
					//								imageCaption = item.PictureCaption1,
					//								imageUrl = "https://Images.akhbarelyom.com/images/images/Large/" + item.PictureName1,
					//								imageTitle = item.Title
					//							}
					//);

				}
            }
            catch
            {

            }

        }


    }

}
