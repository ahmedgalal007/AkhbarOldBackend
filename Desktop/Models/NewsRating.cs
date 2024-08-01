using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
    public class NewsRating
    {
        public int MostCommentedID { get; set; }
        public int? Rating { get; set; }
        public int? TotalRaters { get; set; }
        public double? AverageRating { get; set; }
        public string Check(double lower, double upper, double toCheck)
        {
            return toCheck > lower && toCheck <= upper ? " checked=\"checked\"" : null;
        }
    }

}