using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Akhbar.DBEntities
{
    public class VideoFeed
    {
        [Key]
        public int EntryID { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Brief { get; set; }

        [StringLength(100)]
        public string Link { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(100)]
        public string Thumb { get; set; }

        [StringLength(150)]
        public string TitleAR { get; set; }

        [StringLength(1000)]
        public string BriefAR { get; set; }

        [StringLength(100)]
        public string TypeAR { get; set; }

        public virtual ICollection<News> News { get; set; }
        // public virtual ICollection<News_Videos> NewsVideos { get; set; }
    }
}
