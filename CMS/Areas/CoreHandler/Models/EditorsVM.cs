using Domain.Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class EditorsVM
    {

        public EditorsVM()
        {
            page = 1;
            RelatedCaller = "Editorss";
            SearchString = "";
            orderedlst = new List<Editor>();
        }
        public IPagedList<Editor> lst { get; set; }
        public List<Editor> orderedlst { get; set; }
        public Editor editor { get; set; }
        public int page { get; set; }

        public string RelatedTarget = "Relatededitorss";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }

}