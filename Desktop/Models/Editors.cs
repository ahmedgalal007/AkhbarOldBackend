using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
    public class Editors
    {
        public int EditorID { get; set; }
        public string EditorName{get;set;}
        public string picture{get;set;}
        public string ArticleName { get; set; }
        public int JournalID { get; set; }
        public int LastNewID { get; set; }
        public int? DisplayOrder { get; set; }
    }
}