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
    public class TagsController : Controller
    {
        //
        // GET: /Tags/
        AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();

        public ActionResult GetNewTags(int NID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetNewsTags(NID,JournalID));
        }
        public List<Tags> GetNewsTags(int NID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<Tags>();
            lst_Newsdetails = (from p in db.GetNewsTags(NID,JournalID)
                               select new AkhbarElyoum.Models.Tags
                               {
                                   TagID = p.ID,
                                   TagName = (p.Name != null) ? p.Name : " "
                               }).ToList();
            return (lst_Newsdetails.ToList());
        }

        [OutputCache(Duration = 120)]
        public ActionResult NewsTagList(int ID,int JournalID,string name)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(NewsTags(JournalID,ID, 0));
        }
        public ActionResult NewsTagListPaging(int? LastID, int JournalID, int TagID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(NewsTags(JournalID,TagID, LastID));
        }
        public List<NewsDetails> NewsTags(int JournalID, int TagID, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.sp_GetTagsNews_paged(TagID, JournalID, LastID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = (p.Title != null) ? p.Title : " ",
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " ",
                                   EditorID = p.EditorID,
                                   TagId=TagID,
                                   TagName=p.name
                               }).ToList();
            if (lst_Newsdetails.Count > 0)
                ViewBag.Title = lst_Newsdetails[0].TagName;
            return (lst_Newsdetails.ToList());
            
        }

    }
}
