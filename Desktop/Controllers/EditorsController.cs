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
    public class EditorsController : Controller
    {
        AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();

        public ActionResult GetEditorsList(int JournalID)
        {
            return PartialView(GetEditors(JournalID));
        }

        [OutputCache(Duration = 120)]
        public List<Editors> GetEditors(int JournalID)
        {
            List<Editors>  lst_Editors = new List<AkhbarElyoum.Models.Editors>();
            lst_Editors= (from p in db.sp_GetEditors(JournalID)
                          select new AkhbarElyoum.Models.Editors
                                   {
                                       EditorID = p.EditorID,
                                       EditorName = (p.EditorName != null) ? p.EditorName  : " ",
                                       picture = (p.picture != null) ? p.picture : " ",
                                       
                                       ArticleName = p.subtitle,
                                       LastNewID=p.newid??-1
                                   }).ToList();
            return lst_Editors;
        }

        public ActionResult GetEditorsPageList(int JournalID)
        {
            return PartialView(GetEditorsPage(JournalID));
        }

        public List<Editors> GetEditorsPage(int JournalID)
        {
            IEnumerable<Editors>
                lst_Editors = (from p in db.sp_GetEditorsList(JournalID)
                               select new AkhbarElyoum.Models.Editors
                               {
                                    EditorID=p.EditorID,
                                    EditorName=p.EditorName ,
                                    ArticleName=p.ArticleName,
                                    picture=p.picture,
                                    JournalID=p.JournalID
                               });
            return lst_Editors.ToList();
        }


        #region Editors Archive
        [OutputCache(Duration = 120)]
        public ActionResult EditorNews(int ID, int JournalID, string name)
        {
            ViewBag.Title = name;
            List<NewsDetails> lst = GetEditorNewsList(1, ID, 0);
            
            return View(lst);
        }
        public List<NewsDetails> GetEditorNewsList(int JournalID, int EditorID, int? LastID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_Newsdetails = new List<NewsDetails>();
            lst_Newsdetails = (from p in db.sp_GetByLineNews_paged(EditorID, JournalID, LastID)
                               select new AkhbarElyoum.Models.NewsDetails
                               {
                                   SecTitle = (p.Title != null) ? p.Title : " ",
                                   NewsID = p.NewID,
                                   Breif = (p.Brief != null) ? p.Brief : " ",
                                   PictureName1 = (p.PictureName != null) ? p.PictureName : " ",
                                   Title = p.Title,
                                   EditorID = p.ByLineId,
                                   EditorName = p.ByLineName ,
                                   
                                   PublishDate = (p.PublishDate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.PublishDate) : " "
                               }).ToList();

            
            return (lst_Newsdetails.ToList());
        }
        public ActionResult EditorNewsListPaging(int? LastID, int JournalID, int ID)
        {
            List<NewsDetails> lst = GetEditorNewsList(1,ID,LastID);
            return PartialView(lst);
        }

        [OutputCache(Duration = 120)]
        public ActionResult GateEditors(string name)
        {
            List<Editors> lst = GetAllByLinesList(0);
            return View(lst);
        }
        public ActionResult GateEditorsPaging(int? LastID)
        {
            List<Editors> lst = GetAllByLinesList(LastID);
            return PartialView(lst);
        }
        public List<Editors> GetAllByLinesList(int? page)
        {
            var lst_ByLine = new List<Editors>();
            lst_ByLine = (from p in db.sp_GetAllBylines_paged(page)
                          select new AkhbarElyoum.Models.Editors
                          {
                              EditorID = p.ByLineId,
                              EditorName = p.FullName,
                              picture = p.UserPhoto,
                              DisplayOrder = p.DisplayOrder
                          }).ToList();
            return (lst_ByLine.ToList());
        }

        [OutputCache(Duration = 120)]
        public ActionResult GetNewByLines(int NID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            return PartialView(GetNewsEditrs(NID, JournalID));
        }

        public List<Editors> GetNewsEditrs(int NID, int JournalID)
        {
            if (JournalID.ToString() == "")
                JournalID = 1;
            var lst_NewsEditors = new List<Editors>();
            lst_NewsEditors = (from p in db.sp_GetNewsByLines(NID, JournalID)
                               select new AkhbarElyoum.Models.Editors
                               {
                                   EditorID = p.ByLineId,
                                   EditorName = (p.ByLineName != null) ? p.ByLineName : " ",
                                   ArticleName = (!string.IsNullOrEmpty(p.editorType.ToString())) ? p.editorType.ToString() : "1"
                               }).ToList();
            return (lst_NewsEditors.ToList());
        }

        #endregion
    }
}
