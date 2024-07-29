using Akhbar.DBEntities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.UserManagment.Models
{
    public class AdminUserVM
    {

        public AdminUserVM()
        {
            page = 1;
            RelatedCaller = "AdminUsers";
            SearchString = "";
            ManagerFlag = false;
            ActiveFlag = false;
            userRolesLst = new List<UserRole>();
        }
        public IPagedList<AdminUser> lst { get; set; }
        public AdminUser adminUser { get; set; }

        public int page { get; set; }
        public bool ManagerFlag { get; set; }
        public bool ActiveFlag { get; set; }
        public string RelatedTarget = "RelatedadminUsers";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }


        public List<UserRole> userRolesLst { get; set; }
        public UserRole UserRole{ get; set; }
    }

}