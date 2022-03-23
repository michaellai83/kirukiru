namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditArticleNormalAddCheckArticle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleNormals", "CheckArticle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleNormals", "CheckArticle");
        }
    }
}
