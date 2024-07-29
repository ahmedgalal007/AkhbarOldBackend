using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.UserManagment.Models
{
    public class RoleVM
    {

        public RoleVM()
        {
            page = 1;
            RelatedCaller = "Roles";
            SearchString = "";

            roleServiceLst = new List<RoleServices>();
        }
        public IPagedList<Role> lst { get; set; }
        public Role Role { get; set; }
        public int page { get; set; }
       
        public string RelatedTarget = "Relatedroles";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }

        public List<RoleServices> roleServiceLst { get; set; }
        public RoleServices RoleService { get; set; }

    }

}