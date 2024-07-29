using Akhbar.DBEntities;
using CMS.Areas.FrameWork.Controllers;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CMS.Areas.CoreHandler.Models
{
    public class NewsVM
    {

        public NewsVM()
        {
            page = 1;
            RelatedCaller = "Newss";
            SearchString = "";
            SecSearch = 0;

            NewsVersionLst = new List<NewsVersions>();
            tags = "";

            ActiveSectionLst = new List<MainSection>();

            //PublishPermissionIds = new List<int> { 0, 1, 2, 6 };
            //PassPermissionIds = new List<int> { 0, 1, 2, 6 };
            //EditPermissionIds = new List<int> { 6 };

            SliderNewsFlag = false;
            TickerNewsFlag = false;
            Mo7taratNewsFlag = false;
            SectionNewsFlag = false;
            waterMarkFlag = false;
            archImgFlag = false;

            oldSecId = 0;

            Tickerlst = new List<Ticker>();
            NewsForSort = new List<NewsForSort>();
            NewsForDeskLst = new List<News>();
            lastNewsLst = new List<News>();
        }
        public IPagedList<News> lst { get; set; }
        public News News { get; set; }

        public int page { get; set; }

        public string RelatedTarget = "Relatednewss";//represent div id
        public int RelatedID { get; set; }
        public string RelatedCaller { get; set; }
        public string SearchString { get; set; }
        public int SecSearch { get; set; }
        public string VideoUrl { get; set; }

        public NewsView NewsView { get; set; }
        public IssuesNew IssuesNew { get; set; }

        public NewsVersions NewsVersion { get; set; }
        public List<NewsVersions> NewsVersionLst { get; set; }
        public IPagedList<NewsVersions> VLst { get; set; }
        public IPagedList<NewsPictures> SearchedImages { get; set; }

        public string tags { get; set; }
        public NewsTag NewsTag { get; set; }
        public VersionTag VersionTag { get; set; }

        public int[] Albums { get; set; }
        public int[] Sections { get; set; }

        public NewsGallery NewsGallery { get; set; }

        public List<MainSection> ActiveSectionLst { get; set; }
        public List<TopNews> TopNewsOrderedlst { get; set; }
        public TopNews TopNews { get; set; }
        public TopNews TopNewsTemp { get; set; }
        public NewsTicker NewsTicker { get; set; }
        public IPagedList<NewsTicker> NewsTickerLst { get; set; }
        public List<Ticker> Tickerlst { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public News_Byline News_ByLine { get; set; }
        public int[] ByLines { get; set; }
        public int[] Infograhers { get; set; }
        public int[] Videograhers { get; set; }
        public int[] photograhers { get; set; }

        //public List<int> PublishPermissionIds { get; }
        //public List<int> PassPermissionIds { get; }
        //public List<int> EditPermissionIds { get; }

        public bool SliderNewsFlag { get; set; }
        public bool TickerNewsFlag { get; set; }
        public bool Mo7taratNewsFlag { get; set; }
        public bool SectionNewsFlag { get; set; }
        public bool waterMarkFlag { get; set; }
        public bool archImgFlag { get; set; }
   
        public List<NewsForSort> NewsForSort { get; set; }

        public NewsPictures SocialPicture { get; set; }

        public List<News> NewsForDeskLst { get; set; }

        public int oldSecId { get; set; }

        public List<News> lastNewsLst { get; set; }

    }

}