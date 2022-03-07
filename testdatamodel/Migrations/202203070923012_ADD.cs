namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADD : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Articles", "Member_ID", "dbo.Members");
            DropForeignKey("dbo.Members", "Article_ID", "dbo.Articles");
            DropForeignKey("dbo.Members", "Article_ID1", "dbo.Articles");
            DropIndex("dbo.Articles", new[] { "Member_ID" });
            DropIndex("dbo.Members", new[] { "Article_ID" });
            DropIndex("dbo.Members", new[] { "Article_ID1" });
            DropColumn("dbo.Articles", "Member_ID");
            DropColumn("dbo.Members", "Article_ID");
            DropColumn("dbo.Members", "Article_ID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "Article_ID1", c => c.Int());
            AddColumn("dbo.Members", "Article_ID", c => c.Int());
            AddColumn("dbo.Articles", "Member_ID", c => c.Int());
            CreateIndex("dbo.Members", "Article_ID1");
            CreateIndex("dbo.Members", "Article_ID");
            CreateIndex("dbo.Articles", "Member_ID");
            AddForeignKey("dbo.Members", "Article_ID1", "dbo.Articles", "ID");
            AddForeignKey("dbo.Members", "Article_ID", "dbo.Articles", "ID");
            AddForeignKey("dbo.Articles", "Member_ID", "dbo.Members", "ID");
        }
    }
}
