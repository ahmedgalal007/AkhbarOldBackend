namespace Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMainVideoFeedToNews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "HasVideo", c => c.Boolean(nullable: false));
            AddColumn("dbo.News", "MainVideoFeedId", c => c.Int());
            CreateIndex("dbo.News", "MainVideoFeedId");
            AddForeignKey("dbo.News", "MainVideoFeedId", "dbo.VideoFeeds", "EntryID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "MainVideoFeedId", "dbo.VideoFeeds");
            DropIndex("dbo.News", new[] { "MainVideoFeedId" });
            DropColumn("dbo.News", "MainVideoFeedId");
            DropColumn("dbo.News", "HasVideo");
        }
    }
}
