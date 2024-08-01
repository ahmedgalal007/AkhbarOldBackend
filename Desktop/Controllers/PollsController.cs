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
    public class PollsController : Controller
    {
        //
        // GET: /Polls/
        AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();
        public ActionResult SubmitPoll(string PollID, string OptionID)
        {
            //Update Votes
            db.sp_SubmitPoll(int.Parse(OptionID));
            return PartialView("GetPollResults", GetPollsDetails(int.Parse(PollID)));
        }
        public ActionResult GetPollResults(int PollID)
        {
            return PartialView(GetPollsDetails(PollID));
        }
        public List<Polls> GetPollsDetails(int PollID)
        {
            var lst_Polls = new List<Polls>();
            lst_Polls = (from p in db.sp_GetPollDetails(PollID)
                         select new AkhbarElyoum.Models.Polls
                         {
                             PollName = p.PollBody,
                             PollID = p.PollID,
                             OptionID = p.OptionId,
                             OptionName = p.OptionBody,
                             Votes = (p.Votes != null) ? p.Votes : 0,
                             TotalVotes = (p.TotalVotes != null) ? p.TotalVotes : 0
                         }).ToList();
            return (lst_Polls.ToList());
        }
        public ActionResult GetPollList(int JournalID)
        {
            return PartialView(GetPolls(JournalID));
        }
        public List<Polls> GetPolls(int JournalID)
        {
            var lst_Polls = new List<Polls>();
            lst_Polls = (from p in db.sp_GetActivePoll(0,JournalID)
                         select new AkhbarElyoum.Models.Polls
                         {
                             PollID = p.PollID,
                             PollName = p.PollBody,
                             OptionID = p.OptionId,
                             OptionName = p.OptionBody

                         }).ToList();
            return (lst_Polls.ToList());
        }


        #region All Polls
        public List<Polls> GEtPollsList(int lastID)
        {

            var lst_Newsdetails = new List<Polls>();
            lst_Newsdetails = (from p in db.Get_PollsList_Paged(lastID)
                               select new Polls
                               {

                                   PollName = p.PollBody,
                                   PollID = p.PollID,
                                   OptionID = (int)p.OptionId,
                                   OptionName = p.OptionBody,
                                   Votes = (p.Votes != null) ? p.Votes : 0,
                                   TotalVotes = (p.TotalVotes != null) ? p.TotalVotes : 0
                               }).ToList();
            return lst_Newsdetails;


        }
        public ActionResult OPinion_poll()
        {

            return View(GEtPollsList(0));
        }
        public ActionResult OPinion_pollpaged(int LastID)
        {

            return View(GEtPollsList(LastID));
        }
        public ActionResult GetPollResultss_opinionpoll(int PollID)
        {
            return PartialView(GetPollsDetails(PollID));
        }
        public ActionResult GetPollList_opinionpoll(int pollID)
        {
            return PartialView(GetPolls_opinionpoll(pollID));
        }
        public List<Polls> GetPolls_opinionpoll(int pollID)
        {
            var lst_Polls = new List<Polls>();
            lst_Polls = (from p in db.GEtSpecifiedPoll(pollID)
                         select new AkhbarElyoum.Models.Polls
                         {
                             PollID = p.PollID,
                             PollName = p.PollBody,
                             OptionID = p.OptionId,
                             OptionName = p.OptionBody

                         }).ToList();
            return (lst_Polls.ToList());
        }
        public ActionResult SubmitPoll1(string PollID, string OptionID)
        {
            //Update Votes
            db.sp_SubmitPoll(int.Parse(OptionID));
            return PartialView("GetPollResultsss", GetPollsDetails(int.Parse(PollID)));
        }
        #endregion



        public ActionResult GetCurrentPoll(int JournalID)
        {
            return View();
        }
    }
}
