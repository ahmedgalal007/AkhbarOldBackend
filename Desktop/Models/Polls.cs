using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{

    public class Polls
    {
        public int PollID { get; set; }
        public string PollName { get; set; }
        public int OptionID { get; set; }
        public string OptionName { get; set; }
        public int? Votes { get; set; }
        public int? TotalVotes { get; set; }
        public double? Total { get { return Convert.ToInt32(((double) Votes / TotalVotes) * 100); } }
        public double? TotalPixel { get { return Convert.ToInt32(((double)Votes / TotalVotes) * 195); } }
    }
}