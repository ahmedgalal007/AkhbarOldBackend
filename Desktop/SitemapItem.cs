using System;

namespace AkhbarElyoum
{
    public class SitemapItem :ISitemapItem
    {
        public SitemapItem(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        public DateTime? LastModified { get; set; }

        public ChangeFrequency? ChangeFrequency { get; set; }

        public float? Priority { get; set; }

        public string image { get; set; }

        public string imageCaption { get; set; }

        public string imageUrl { get; set; }

        public string imageTitle { get; set; }



    }
}