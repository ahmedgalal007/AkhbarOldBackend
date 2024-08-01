using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace AkhbarElyoum
{
    public class XmlSitemapResult2 : ActionResult
    {
        private IEnumerable<ISitemapItem> _items;
        bool _stroySiteMap; 
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        XNamespace nsImage = "http://www.google.com/schemas/sitemap-image/1.1";
        private XNamespace nsXhtml = "http://www.w3.org/1999/xhtml";

        public XmlSitemapResult2(IEnumerable<ISitemapItem> items,bool stroySiteMap)
        {
            _items = items;
            _stroySiteMap = stroySiteMap;
        }
        
        public override void ExecuteResult(ControllerContext context)
        {
            
            string encoding = context.HttpContext.Response.ContentEncoding.WebName;
            XDocument sitemap;
            if (!_stroySiteMap)
            {
             sitemap = new XDocument(new XDeclaration("1.0", encoding, "yes"), Environment.NewLine,
             new XElement("sitemapindex", new XAttribute("xmlns", ns),
                  from item in _items
                  select CreateMainItemElement(item)
                  )
             );
            }
            else
            {
                sitemap = new XDocument(new XDeclaration("1.0", encoding, "yes"), Environment.NewLine,
                new XElement(ns+ "urlset", new XAttribute(XNamespace.Xmlns+ "image", nsImage),
                  from item in _items
                  select CreateStoryItemElement(item)
                  ));
            }

            context.HttpContext.Response.ContentType = "text/xml";
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Write(sitemap.Declaration + sitemap.ToString());
        }

        private XElement CreateMainItemElement(ISitemapItem item)
        {
             XElement itemElement = new XElement("sitemap", new XElement("loc", item.Url.ToLower()));

            if (item.LastModified.HasValue)
                itemElement.Add(new XElement("lastmod", TimeZone.CurrentTimeZone.ToUniversalTime(item.LastModified.Value)));

            return itemElement;
        }

        private XElement CreateStoryItemElement(ISitemapItem item)
        {
            XElement itemElement = new XElement(ns + "url",
                   new XElement(ns+"loc", item.Url),
                   new XElement(ns+"lastmod", TimeZone.CurrentTimeZone.ToUniversalTime(item.LastModified.Value)),
                   //new XElement(ns + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()),
                   new XElement(ns + "changefreq", "always"),
                   new XElement(ns+"priority", item.Priority.Value.ToString(CultureInfo.InvariantCulture)),
                   
                   new XAttribute(XNamespace.Xmlns + "image", nsImage.NamespaceName),
                   new XElement(nsImage + "image",
                     new XElement(nsImage + "loc", item.imageUrl),
                     new XElement(nsImage + "title", item.imageTitle),
                     new XElement(nsImage + "caption", item.imageCaption))
                   );
           
            return itemElement;
        }
    }
}