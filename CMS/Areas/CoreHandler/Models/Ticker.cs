using System;

namespace CMS.Areas.CoreHandler.Models
{
    public class Ticker
    {
        public int NewID { get; set; }

        public string Title { get; set; }

        public string SecTitle { get; set; }

        public DateTime? Added_Date { get; set; }

        public int? JournalID { get; set; }

        public int? SectionId { get; set; }

        public bool IsTicker { get; set; }
    }
}