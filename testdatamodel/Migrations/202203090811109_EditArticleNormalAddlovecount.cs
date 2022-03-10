namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditArticleNormalAddlovecount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleNormals", "Lovecount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleNormals", "Lovecount");
        }
    }
}
