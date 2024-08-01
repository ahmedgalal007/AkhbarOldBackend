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
    public class CommentsController : Controller
    {
        AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
        public ActionResult Create(int JournalID)
        {
            return View(new AkhbarElyoum.Models.Comments());
        }
        [HttpPost]
        [ValidateInput(false)]
        public string Create(FormCollection Comment)
        {
            if (ModelState.IsValid)
            {
                db.sp_InsertUserOpinion(Comment["UserName"].Replace('<', ' ').Replace('>', ' '), Comment["Email"].Replace('<', ' ').Replace('>', ' '), Comment["Subject"].Replace('<', ' ').Replace('>', ' '), int.Parse(Comment["NewId"].ToString()), Comment["messagebody"].Replace('<', ' ').Replace('>', ' '));
                return "true";
            }
            return "false";
        }
        public ActionResult Comments(int NID)
        {
            return PartialView("Comments", GetComments(NID, 0));
        }
        public ActionResult CommentsPaged(string page,string NID)
        {
            int lastid = int.Parse(page);
            return PartialView(GetComments(int.Parse(NID),lastid));
        }
        public List<Comments> GetComments(int NID,int? lastid)
        {
            var lst_Comments = new List<Comments>();
            lst_Comments = (from p in db.SP_GetAllUsersOpinions(NID, lastid)
                            select new Comments
                            {
                                CommentID = p.CommentID,
                                UserID = p.UserID,
                                NewId = p.NewID,
                                UserName = p.UserName,
                                Email = p.Email,
                                Subject = p.Subject,
                                messagebody = (p.messagebody != null) ? p.messagebody : p.messagebody.Substring(0, 10),
                                messagedate = (p.messagedate != null) ? string.Format(System.Globalization.CultureInfo.GetCultureInfo("ar-eg"), "{0:dddd، dd MMMM yyyy hh:mm tt}", p.messagedate) : " ",
                                Views = p.Views,
                                Count = (int)p.Count
                            }).ToList();
            return (lst_Comments.ToList());
        }
    }
}
