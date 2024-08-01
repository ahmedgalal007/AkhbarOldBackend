using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
    public class Ticker
    {
        public int? NewsID { set; get; }
        public string Title { set; get; }
        public string SecTitle { set; get; }
        public DateTime Added_Date { get; set; }
    }
}