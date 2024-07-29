using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Areas.FrameWork.ViewModels
{
    
    public class alertMessageVM
    {
        public enum AlertType { Success, Info, Warning, Error, Delete, Edit, Create, Sorted, NewsPass, Publish, Restore,
                                  KeyWords, Images, Rejected, Waiting, DeskAdded, NotPermitted, JoinDesk, LeftDesk, Exists }
        public alertMessageVM()
        {
           
        }
        public string Message { get; set; }
        public AlertType alertType { get; set; }

        //public bool isInfo { get; set; }

        //public bool isWarning { get; set; }

        //public bool isError { get; set; }

        public int? AutoCloseTime { get; set; }

    }
}