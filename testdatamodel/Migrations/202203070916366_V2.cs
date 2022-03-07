namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Member_ID", c => c.Int());
            AddColumn("dbo.Members", "Article_ID", c => c.Int());
            AddColumn("dbo.Members", "Article_ID1", c => c.Int());
            CreateIndex("dbo.Articles", "Member_ID");
            CreateIndex("dbo.Members", "Article_ID");
            CreateIndex("dbo.Members", "Article_ID1");
            AddForeignKey("dbo.Articles", "Member_ID", "dbo.Members", "ID");
            AddForeignKey("dbo.Members", "Article_ID", "dbo.Articles", "ID");
            AddForeignKey("dbo.Members", "Article_ID1", "dbo.Articles", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Members", "Article_ID1", "dbo.Articles");
            DropForeignKey("dbo.Members", "Article_ID", "dbo.Articles");
            DropForeignKey("dbo.Articles", "Member_ID", "dbo.Members");
            DropIndex("dbo.Members", new[] { "Article_ID1" });
            DropIndex("dbo.Members", new[] { "Article_ID" });
            DropIndex("dbo.Articles", new[] { "Member_ID" });
            DropColumn("dbo.Members", "Article_ID1");
            DropColumn("dbo.Members", "Article_ID");
            DropColumn("dbo.Articles", "Member_ID");
        }
    }
}
