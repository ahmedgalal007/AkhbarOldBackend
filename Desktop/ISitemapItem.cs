using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum
{
    public interface ISitemapItem
    {
        string Url { get; }

        DateTime? LastModified { get; }

        ChangeFrequency? ChangeFrequency { get; }

        float? Priority { get; }

        string image { get; }

        string imageCaption { get; }

        string imageUrl { get; }

        string imageTitle { get; }


    }
}