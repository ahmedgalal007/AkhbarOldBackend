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
    public class BlocksController : Controller
    {
        //
        // GET: /Blocks/
        AkhbarElyoumDataContext db = new AkhbarElyoumDataContext();

        public ActionResult GetBlockTextList(int BlockID,int JournalID)
        {
            return PartialView(GetBlockText(BlockID,JournalID));
        }
        public List<Block> GetBlockText(int BlockID,int JournalID)
        {
            var blocks = new List<Block>();
            blocks = (from p in db.GetBlockText(BlockID,JournalID)
                      select new AkhbarElyoum.Models.Block
                      {
                          BlockID = p.BlockID,
                          BlockName = p.BlockName,
                          BlockText = p.BlockText
                      }).ToList();
            return (blocks);
        }

        public ActionResult Services()
        {
            return View();
        
        }
        public ActionResult service(int ID)
        {
            return View(GetBlockText(ID, 1));
        }

    }
}
