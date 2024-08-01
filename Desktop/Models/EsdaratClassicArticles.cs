using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
    public class EsdaratClassicArticles
    {
        public int NewID { get; set; }
        public int EditorID { get; set; }
        public string Title { get; set; }
        public string EditorName { get; set; }
        public int JournalID { get; set; }
    }
}