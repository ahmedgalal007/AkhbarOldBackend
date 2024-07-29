using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class BlocksVM
    {

        public BlocksVM()
        {
            page = 1;
            RelatedCaller = "BlocksZones";
            SearchString = "";

            orderedlst = new List<BlocksZone>();
        }
        public IPagedList<BlocksZone> lst { get; set; }
        public List<BlocksZone> orderedlst { get; set; }
        public BlocksZone blocksZone { get; set; }
        public NewsBlocks NewsBlocks { get; set; }

        public int page { get; set; }

        public string RelatedTarget = "RelatedBlocksZones";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }

}