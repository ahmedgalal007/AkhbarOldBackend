using Domain.Akhbar.DBEntities;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Domain.Akhbar.Contexts
{
    public class AkhbarDBContext : DbContext
    {
        public AkhbarDBContext() 
            : base(ConfigurationManager.ConnectionStrings["AkhbarDBConnection"].ConnectionString)
        {
            Database.SetInitializer<AkhbarDBContext>((IDatabaseInitializer<AkhbarDBContext>)null);
        }

        public AkhbarDBContext(string strConnectionStringName)
          : base("name=" + strConnectionStringName)
        {
            Database.SetInitializer<AkhbarDBContext>((IDatabaseInitializer<AkhbarDBContext>)null);
        }

        public virtual DbSet<AdminEditor> AdminEditors { get; set; }

        public virtual DbSet<AdminLog> AdminLogs { get; set; }

        public virtual DbSet<AdminUser> AdminUsers { get; set; }

        public virtual DbSet<Nationality> Nationalities { get; set; }

        public virtual DbSet<UMModules> UMModules { get; set; }

        public virtual DbSet<UMServices> UMServices { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<RoleServices> RoleServices { get; set; }

        public virtual DbSet<Attachment> Attachments { get; set; }

        public virtual DbSet<BlocksZone> BlocksZones { get; set; }

        public virtual DbSet<Editor> Editors { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Gallery> Galleries { get; set; }

        public virtual DbSet<GalleryType> GalleryTypes { get; set; }

        public virtual DbSet<GalleryPictures> GalleryPictures { get; set; }

        public virtual DbSet<Issue> Issues { get; set; }

        public virtual DbSet<IssuesNew> IssuesNews { get; set; }

        public virtual DbSet<IssuesTopNew> IssuesTopNews { get; set; }

        public virtual DbSet<Journal> Journals { get; set; }

        public virtual DbSet<LinkedNew> LinkedNews { get; set; }

        public virtual DbSet<MainSection> MainSections { get; set; }

        public virtual DbSet<Profile> Profiles { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<News_Videos> News_Videos { get; set; }

        public virtual DbSet<NewsBlocks> NewsBlocks { get; set; }

        public virtual DbSet<NewsCategory> NewsCategories { get; set; }

        public virtual DbSet<NewsGallery> NewsGallery { get; set; }

        public virtual DbSet<NewsPictures> NewsPictures { get; set; }

        public virtual DbSet<NewsPicturesCat> NewsPicturesCats { get; set; }

        public virtual DbSet<NewsPoll> NewsPolls { get; set; }

        public virtual DbSet<NewsSearch> NewsSearches { get; set; }

        public virtual DbSet<NewsSection> NewsSections { get; set; }

        public virtual DbSet<NewsTag> NewsTags { get; set; }

        public virtual DbSet<NewsTicker> NewsTickers { get; set; }

        public virtual DbSet<NewsView> NewsViews { get; set; }

        public virtual DbSet<PictureofDay> PictureofDays { get; set; }

        public virtual DbSet<Polls> Polls { get; set; }

        public virtual DbSet<PollsOption> PollsOptions { get; set; }

        public virtual DbSet<RelatedNew> RelatedNews { get; set; }

        public virtual DbSet<ReviewerNew> ReviewerNews { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<SesstionText> SesstionTexts { get; set; }

        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<TopNews> TopNews { get; set; }

        public virtual DbSet<TraceTable> TraceTables { get; set; }

        public virtual DbSet<UsersOpinion> UsersOpinions { get; set; }

        public virtual DbSet<VersionTag> VersionTags { get; set; }

        public virtual DbSet<ComicsofDay> ComicsofDays { get; set; }

        public virtual DbSet<HomeMostCommented> HomeMostCommenteds { get; set; }

        public virtual DbSet<NewsVersions> NewsVersions { get; set; }

        public virtual DbSet<SectionLatestNew> SectionLatestNews { get; set; }

        public virtual DbSet<VideoFeed> VideoFeeds { get; set; }

        public virtual DbSet<vTopNewsToday> vTopNewsTodays { get; set; }

        public virtual DbSet<VUserPermissions> VUserPermissions { get; set; }

        public virtual DbSet<TempDeskShift> TempDeskShifts { get; set; }

        public virtual DbSet<ByLine> ByLines { get; set; }

        public virtual DbSet<News_Byline> News_Byline { get; set; }

        public virtual DbSet<Trend> Trends { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminEditor>().Property((Expression<Func<AdminEditor, string>>)(e => e.EditorName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<AdminEditor>().Property((Expression<Func<AdminEditor, string>>)(e => e.Position)).IsUnicode(new bool?(false));
            modelBuilder.Entity<AdminEditor>().Property((Expression<Func<AdminEditor, string>>)(e => e.Email)).IsUnicode(new bool?(false));
            modelBuilder.Entity<AdminUser>().Property((Expression<Func<AdminUser, string>>)(e => e.FullName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<AdminUser>().Property((Expression<Func<AdminUser, string>>)(e => e.UserName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<AdminUser>().Property((Expression<Func<AdminUser, string>>)(e => e.Password)).IsUnicode(new bool?(false));
            modelBuilder.Entity<AdminUser>().Property((Expression<Func<AdminUser, string>>)(e => e.Telephone)).IsUnicode(new bool?(false));
            modelBuilder.Entity<AdminUser>().HasMany<UserRole>((Expression<Func<AdminUser, ICollection<UserRole>>>)(e => e.UserRoleLst)).WithRequired((Expression<Func<UserRole, AdminUser>>)(e => e.AdminUser)).HasForeignKey<int?>((Expression<Func<UserRole, int?>>)(e => e.AdminUserID)).WillCascadeOnDelete(false);
            modelBuilder.Entity<Role>().HasMany<UserRole>((Expression<Func<Role, ICollection<UserRole>>>)(e => e.UserRolesLst)).WithOptional((Expression<Func<UserRole, Role>>)(e => e.Role)).HasForeignKey<int?>((Expression<Func<UserRole, int?>>)(e => e.RoleId));
            modelBuilder.Entity<Role>().HasMany<RoleServices>((Expression<Func<Role, ICollection<RoleServices>>>)(e => e.RoleServiceLst)).WithRequired((Expression<Func<RoleServices, Role>>)(e => e.Role)).HasForeignKey<int>((Expression<Func<RoleServices, int>>)(e => e.RoleId)).WillCascadeOnDelete(false);
            modelBuilder.Entity<Editor>().Property((Expression<Func<Editor, string>>)(e => e.EditorName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Editor>().Property((Expression<Func<Editor, string>>)(e => e.ArticleName)).IsFixedLength();
            modelBuilder.Entity<Gallery>().Property((Expression<Func<Gallery, string>>)(e => e.Title)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Gallery>().Property((Expression<Func<Gallery, string>>)(e => e.Description)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Gallery>().Property((Expression<Func<Gallery, string>>)(e => e.Keywords)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Gallery>().Property((Expression<Func<Gallery, string>>)(e => e.GalleryType)).IsUnicode(new bool?(false));
            modelBuilder.Entity<GalleryType>().Property((Expression<Func<GalleryType, string>>)(e => e.GalleryTypeName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Gallery>().HasMany<GalleryPictures>((Expression<Func<Gallery, ICollection<GalleryPictures>>>)(e => e.GalleryPictureLst)).WithRequired((Expression<Func<GalleryPictures, Gallery>>)(e => e.Gallery)).HasForeignKey<int>((Expression<Func<GalleryPictures, int>>)(e => e.GalleryID)).WillCascadeOnDelete(false);
            modelBuilder.Entity<Issue>().Property((Expression<Func<Issue, string>>)(e => e.IssueTitle)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.ImageLargeSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.ImageSmallSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.ImageSliderSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.ComicLargeSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.ComicSmallSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.EditorLargeSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.EditorSmallSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.PainterLargeSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Journal>().Property((Expression<Func<Journal, string>>)(e => e.PainterSmallSize)).IsUnicode(new bool?(false));
            modelBuilder.Entity<MainSection>().Property((Expression<Func<MainSection, string>>)(e => e.SecTitle)).IsUnicode(new bool?(false));
            modelBuilder.Entity<MainSection>().Property((Expression<Func<MainSection, string>>)(e => e.Keywords)).IsUnicode(new bool?(false));
            modelBuilder.Entity<MainSection>().Property((Expression<Func<MainSection, string>>)(e => e.Description)).IsUnicode(new bool?(false));
            modelBuilder.Entity<MainSection>().HasMany<NewsSection>((Expression<Func<MainSection, ICollection<NewsSection>>>)(e => e.NewsSectionLst)).WithRequired((Expression<Func<NewsSection, MainSection>>)(e => e.MainSection)).WillCascadeOnDelete(false);
            modelBuilder.Entity<MainSection>().HasMany<Editor>((Expression<Func<MainSection, ICollection<Editor>>>)(e => e.EditorsLst)).WithRequired((Expression<Func<Editor, MainSection>>)(e => e.Section)).HasForeignKey<int?>((Expression<Func<Editor, int?>>)(e => e.SectionID)).WillCascadeOnDelete(false);
            modelBuilder.Entity<NewsBlocks>().Property((Expression<Func<NewsBlocks, string>>)(e => e.BlockText)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsCategory>().Property((Expression<Func<NewsCategory, string>>)(e => e.Name)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsPictures>().Property((Expression<Func<NewsPictures, string>>)(e => e.PictureName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsPictures>().Property((Expression<Func<NewsPictures, string>>)(e => e.KeyWords)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsPictures>().Property((Expression<Func<NewsPictures, string>>)(e => e.PicCaption)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsPictures>().HasMany<News>((Expression<Func<NewsPictures, ICollection<News>>>)(e => e.NewsLst)).WithRequired((Expression<Func<News, NewsPictures>>)(e => e.NewsPicture)).HasForeignKey<int?>((Expression<Func<News, int?>>)(e => e.PictureID1)).WillCascadeOnDelete(false);
            modelBuilder.Entity<NewsPictures>().HasMany<NewsVersions>((Expression<Func<NewsPictures, ICollection<NewsVersions>>>)(e => e.NewsVersionsLst)).WithRequired((Expression<Func<NewsVersions, NewsPictures>>)(e => e.NewsPicture)).HasForeignKey<int>((Expression<Func<NewsVersions, int>>)(e => e.PictureID1)).WillCascadeOnDelete(false);
            modelBuilder.Entity<News>().HasMany<NewsTag>((Expression<Func<News, ICollection<NewsTag>>>)(e => e.NewsTagsLst)).WithRequired((Expression<Func<NewsTag, News>>)(e => e.News)).HasForeignKey<int>((Expression<Func<NewsTag, int>>)(e => e.NewID)).WillCascadeOnDelete(false);
            modelBuilder.Entity<NewsVersions>().HasMany<VersionTag>((Expression<Func<NewsVersions, ICollection<VersionTag>>>)(e => e.VersionTagsLst)).WithRequired((Expression<Func<VersionTag, NewsVersions>>)(e => e.NewsVersions)).HasForeignKey<long>((Expression<Func<VersionTag, long>>)(e => e.VersionID)).WillCascadeOnDelete(false);
            modelBuilder.Entity<News>().HasMany<NewsGallery>((Expression<Func<News, ICollection<NewsGallery>>>)(e => e.NewsGalleryLst)).WithRequired((Expression<Func<NewsGallery, News>>)(e => e.News)).HasForeignKey<int>((Expression<Func<NewsGallery, int>>)(e => e.NewID)).WillCascadeOnDelete(false);
            modelBuilder.Entity<News>().HasMany<News_Byline>((Expression<Func<News, ICollection<News_Byline>>>)(e => e.NewsByLineLst)).WithRequired((Expression<Func<News_Byline, News>>)(e => e.News)).HasForeignKey<int>((Expression<Func<News_Byline, int>>)(e => e.NewId)).WillCascadeOnDelete(false);
            modelBuilder.Entity<NewsPicturesCat>().Property((Expression<Func<NewsPicturesCat, string>>)(e => e.CatName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsSearch>().Property((Expression<Func<NewsSearch, string>>)(e => e.title)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsSearch>().Property((Expression<Func<NewsSearch, string>>)(e => e.byline)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsSearch>().Property((Expression<Func<NewsSearch, string>>)(e => e.brief)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsSearch>().Property((Expression<Func<NewsSearch, string>>)(e => e.title2)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsSearch>().Property((Expression<Func<NewsSearch, string>>)(e => e.brief2)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsSearch>().Property((Expression<Func<NewsSearch, string>>)(e => e.byline2)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsTicker>().Property((Expression<Func<NewsTicker, string>>)(e => e.Title)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Polls>().Property((Expression<Func<Polls, string>>)(e => e.PollBody)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Polls>().HasMany<NewsPoll>((Expression<Func<Polls, ICollection<NewsPoll>>>)(e => e.NewsPollLst)).WithRequired((Expression<Func<NewsPoll, Polls>>)(e => e.Poll)).WillCascadeOnDelete(false);
            modelBuilder.Entity<PollsOption>().Property((Expression<Func<PollsOption, string>>)(e => e.OptionBody)).IsUnicode(new bool?(false));
            modelBuilder.Entity<SesstionText>().Property((Expression<Func<SesstionText, string>>)(e => e.Story)).IsUnicode(new bool?(false));
            modelBuilder.Entity<UsersOpinion>().Property((Expression<Func<UsersOpinion, string>>)(e => e.UserName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<UsersOpinion>().Property((Expression<Func<UsersOpinion, string>>)(e => e.Email)).IsUnicode(new bool?(false));
            modelBuilder.Entity<UsersOpinion>().Property((Expression<Func<UsersOpinion, string>>)(e => e.Subject)).IsUnicode(new bool?(false));
            modelBuilder.Entity<UsersOpinion>().Property((Expression<Func<UsersOpinion, string>>)(e => e.messagebody)).IsUnicode(new bool?(false));
            modelBuilder.Entity<UsersOpinion>().Property((Expression<Func<UsersOpinion, string>>)(e => e.OperationalUser)).IsUnicode(new bool?(false));
            modelBuilder.Entity<UsersOpinion>().Property((Expression<Func<UsersOpinion, string>>)(e => e.UserIP)).IsUnicode(new bool?(false));
            modelBuilder.Entity<UsersOpinion>().Property((Expression<Func<UsersOpinion, string>>)(e => e.CloudIP)).IsUnicode(new bool?(false));
            modelBuilder.Entity<HomeMostCommented>().Property((Expression<Func<HomeMostCommented, string>>)(e => e.Title)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.Title)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.SubTitle)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.Story)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.Brief)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.quote)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.Keywords)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.PictureCaption1)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.PictureCaption2)).IsUnicode(new bool?(false));
            modelBuilder.Entity<NewsVersions>().Property((Expression<Func<NewsVersions, string>>)(e => e.ByLine)).IsUnicode(new bool?(false));
            modelBuilder.Entity<SectionLatestNew>().Property((Expression<Func<SectionLatestNew, string>>)(e => e.Title)).IsUnicode(new bool?(false));
            modelBuilder.Entity<SectionLatestNew>().Property((Expression<Func<SectionLatestNew, string>>)(e => e.Brief)).IsUnicode(new bool?(false));
            modelBuilder.Entity<SectionLatestNew>().Property((Expression<Func<SectionLatestNew, string>>)(e => e.PictureName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<SectionLatestNew>().Property((Expression<Func<SectionLatestNew, string>>)(e => e.PictureCaption1)).IsUnicode(new bool?(false));
            modelBuilder.Entity<SectionLatestNew>().Property((Expression<Func<SectionLatestNew, string>>)(e => e.SecTitle)).IsUnicode(new bool?(false));
            modelBuilder.Entity<VideoFeed>().Property((Expression<Func<VideoFeed, string>>)(e => e.Link)).IsUnicode(new bool?(false));
            modelBuilder.Entity<VideoFeed>().Property((Expression<Func<VideoFeed, string>>)(e => e.Type)).IsUnicode(new bool?(false));
            modelBuilder.Entity<VideoFeed>().Property((Expression<Func<VideoFeed, string>>)(e => e.Thumb)).IsUnicode(new bool?(false));
            modelBuilder.Entity<VideoFeed>().Property((Expression<Func<VideoFeed, string>>)(e => e.TypeAR)).IsUnicode(new bool?(false));
            modelBuilder.Entity<vTopNewsToday>().Property((Expression<Func<vTopNewsToday, string>>)(e => e.PublishDate)).IsUnicode(new bool?(false));
            modelBuilder.Entity<vTopNewsToday>().Property((Expression<Func<vTopNewsToday, string>>)(e => e.Brief)).IsUnicode(new bool?(false));
            modelBuilder.Entity<vTopNewsToday>().Property((Expression<Func<vTopNewsToday, string>>)(e => e.Title)).IsUnicode(new bool?(false));
            modelBuilder.Entity<vTopNewsToday>().Property((Expression<Func<vTopNewsToday, string>>)(e => e.PictureName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<vTopNewsToday>().Property((Expression<Func<vTopNewsToday, string>>)(e => e.SubTitle)).IsUnicode(new bool?(false));
            modelBuilder.Entity<VUserPermissions>().Property((Expression<Func<VUserPermissions, string>>)(e => e.UserName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<VUserPermissions>().Property((Expression<Func<VUserPermissions, string>>)(e => e.Password)).IsUnicode(new bool?(false));
            modelBuilder.Entity<VUserPermissions>().Property((Expression<Func<VUserPermissions, string>>)(e => e.FullName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Nationality>().Property((Expression<Func<Nationality, string>>)(e => e.Name)).IsFixedLength().IsUnicode(new bool?(false));
            modelBuilder.Entity<Profile>().Property((Expression<Func<Profile, string>>)(e => e.ProfName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Profile>().Property((Expression<Func<Profile, string>>)(e => e.Keyword)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Profile>().Property((Expression<Func<Profile, string>>)(e => e.Description)).IsUnicode(new bool?(false));
            modelBuilder.Entity<Profile>().Property((Expression<Func<Profile, string>>)(e => e.MainPictureName)).IsUnicode(new bool?(false));
            modelBuilder.Entity<News>().HasOptional(e => e.VideoFeed).WithMany((e => e.News)).HasForeignKey(e => e.MainVideoFeedId).WillCascadeOnDelete(false);
            // modelBuilder.Entity<News_Videos>().HasIndex(e => new { e.NewsID, e.VideoFeedID }).IsUnique(true);
            // modelBuilder.Entity<News_Videos>().HasOptional(e => e.News).WithMany(e => e.NewsVideoLst).HasForeignKey(e => e.NewsID).WillCascadeOnDelete(false);
            // modelBuilder.Entity<News_Videos>().HasOptional(e => e.VideoFeed).WithMany(e => e.NewsVideos).HasForeignKey(e => e.VideoFeedID).WillCascadeOnDelete(false);
        }
    }
}
