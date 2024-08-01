using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AkhbarElyoum.Models;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AkhbarElyoum.Controllers
{
   
    public class EsdaratController : Controller
    {
  
        public ActionResult Classic()
        {
            List<EsdaratClassicData> classicLst = new List<EsdaratClassicData>();

            try
            {
                using (var client = new WebClient())
                {
                    var query = HttpUtility.ParseQueryString(string.Empty);
                    //    var url = new UriBuilder("https://195.122.185.21:8080/home/classic");
                    var url = new UriBuilder("https://api.akhbarelyom.com/home/classic");
                    url.Query = query.ToString();
                    client.Encoding = Encoding.UTF8;
                    string json = client.DownloadString(url.ToString());
                    dynamic obj = JsonConvert.DeserializeObject(json);

                    foreach (var item in obj)
                    {
                        EsdaratClassicData classicItem = new EsdaratClassicData();

                        classicItem.NewID = int.Parse(item.SelectToken("NewID").ToString());
                        classicItem.Title = item.SelectToken("Title").ToString();
                        classicItem.PictureName = item.SelectToken("PictureName").ToString();
                        classicItem.JournalID = int.Parse(item.SelectToken("JournalID").ToString());
                        classicLst.Add(classicItem);
                    }

                }
            }
            catch
            { }

            return PartialView(classicLst);
        }

    
        public ActionResult Articles()
        {
            List<EsdaratClassicArticles> articleLst = new List<EsdaratClassicArticles>();
            try
            {
                using (var client = new WebClient())
                {
                    var query = HttpUtility.ParseQueryString(string.Empty);
                    // var url = new UriBuilder("https://195.122.185.21:8030/Home/Articles");
                    var url = new UriBuilder("https://api.akhbarelyom.com/home/Articles");
                    url.Query = query.ToString();
                    client.Encoding = Encoding.UTF8;
                    string json = client.DownloadString(url.ToString());
                    dynamic obj = JsonConvert.DeserializeObject(json);

                    foreach (var item in obj)
                    {
                        EsdaratClassicArticles articleItem = new EsdaratClassicArticles();
                        articleItem.EditorID = int.Parse(item.SelectToken("EditorID").ToString());
                        articleItem.Title = item.SelectToken("Title").ToString();
                        articleItem.NewID = int.Parse(item.SelectToken("NewID").ToString());
                        articleItem.EditorName = item.SelectToken("EditorName").ToString();
                        articleItem.JournalID = int.Parse(item.SelectToken("JournalID").ToString());
                        articleLst.Add(articleItem);
                    }

                }
            }
            catch 
            {
            }

            return PartialView(articleLst);

        }
    }
}
