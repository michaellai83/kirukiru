namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteauthorandauthorpicInartilceAndnormalarticle : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ArticleNormals", "AuthorName");
            DropColumn("dbo.ArticleNormals", "AuthorPic");
            DropColumn("dbo.Articles", "AuthorName");
            DropColumn("dbo.Articles", "AuthorPic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Articles", "AuthorPic", c => c.String());
            AddColumn("dbo.Articles", "AuthorName", c => c.String());
            AddColumn("dbo.ArticleNormals", "AuthorPic", c => c.String());
            AddColumn("dbo.ArticleNormals", "AuthorName", c => c.String());
        }
    }
}
