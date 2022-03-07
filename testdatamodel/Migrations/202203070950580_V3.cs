namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Members", "ArticlecategoryId", "dbo.Articlecategories");
            DropIndex("dbo.Members", new[] { "ArticlecategoryId" });
            CreateTable(
                "dbo.MemberArticles",
                c => new
                    {
                        Member_ID = c.Int(nullable: false),
                        Article_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_ID, t.Article_ID })
                .ForeignKey("dbo.Members", t => t.Member_ID, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.Article_ID, cascadeDelete: true)
                .Index(t => t.Member_ID)
                .Index(t => t.Article_ID);
            
            DropColumn("dbo.Collects", "ArticleNorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Collects", "ArticleNorId", c => c.Int(nullable: false));
            DropForeignKey("dbo.MemberArticles", "Article_ID", "dbo.Articles");
            DropForeignKey("dbo.MemberArticles", "Member_ID", "dbo.Members");
            DropIndex("dbo.MemberArticles", new[] { "Article_ID" });
            DropIndex("dbo.MemberArticles", new[] { "Member_ID" });
            DropTable("dbo.MemberArticles");
            CreateIndex("dbo.Members", "ArticlecategoryId");
            AddForeignKey("dbo.Members", "ArticlecategoryId", "dbo.Articlecategories", "Id", cascadeDelete: true);
        }
    }
}
