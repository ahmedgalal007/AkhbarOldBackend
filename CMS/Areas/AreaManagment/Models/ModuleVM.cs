﻿using Domain.Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.AreaManagment.Models
{
    public class ModuleVM
    {

        public ModuleVM()
        {
            page = 1;
            RelatedCaller = "Modules";
            SearchString = "";

        }
        public IPagedList<UMModules> lst { get; set; }
        public UMModules Module { get; set; }
        public int page { get; set; }
       
        public string RelatedTarget = "Relatedmodules";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

    }

}