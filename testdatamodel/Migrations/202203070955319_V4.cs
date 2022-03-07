namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V4 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MemberArticles", newName: "ArticleMembers");
            DropPrimaryKey("dbo.ArticleMembers");
            CreateTable(
                "dbo.MemberArticleNormals",
                c => new
                    {
                        Member_ID = c.Int(nullable: false),
                        ArticleNormal_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_ID, t.ArticleNormal_ID })
                .ForeignKey("dbo.Members", t => t.Member_ID, cascadeDelete: true)
                .ForeignKey("dbo.ArticleNormals", t => t.ArticleNormal_ID, cascadeDelete: true)
                .Index(t => t.Member_ID)
                .Index(t => t.ArticleNormal_ID);
            
            AddPrimaryKey("dbo.ArticleMembers", new[] { "Article_ID", "Member_ID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MemberArticleNormals", "ArticleNormal_ID", "dbo.ArticleNormals");
            DropForeignKey("dbo.MemberArticleNormals", "Member_ID", "dbo.Members");
            DropIndex("dbo.MemberArticleNormals", new[] { "ArticleNormal_ID" });
            DropIndex("dbo.MemberArticleNormals", new[] { "Member_ID" });
            DropPrimaryKey("dbo.ArticleMembers");
            DropTable("dbo.MemberArticleNormals");
            AddPrimaryKey("dbo.ArticleMembers", new[] { "Member_ID", "Article_ID" });
            RenameTable(name: "dbo.ArticleMembers", newName: "MemberArticles");
        }
    }
}
