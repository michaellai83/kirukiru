namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArticleNormalInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleNormals", "Introduction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleNormals", "Introduction");
        }
    }
}
