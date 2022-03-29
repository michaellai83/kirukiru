namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArticleAndArticleNormalAddAuthorNameAndAuthorPic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleNormals", "AuthorName", c => c.String());
            AddColumn("dbo.ArticleNormals", "AuthorPic", c => c.String());
            AddColumn("dbo.Articles", "AuthorName", c => c.String());
            AddColumn("dbo.Articles", "AuthorPic", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "AuthorPic");
            DropColumn("dbo.Articles", "AuthorName");
            DropColumn("dbo.ArticleNormals", "AuthorPic");
            DropColumn("dbo.ArticleNormals", "AuthorName");
        }
    }
}
