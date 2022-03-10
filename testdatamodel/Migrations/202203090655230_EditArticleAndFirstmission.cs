namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditArticleAndFirstmission : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ArticleMains", "ArticleMainStringId", "dbo.ArticleMainStrings");
            DropForeignKey("dbo.Firstmissions", "FirstmissionStringId", "dbo.FirstmissionStrings");
            DropIndex("dbo.ArticleMains", new[] { "ArticleMainStringId" });
            DropIndex("dbo.Firstmissions", new[] { "FirstmissionStringId" });
            AddColumn("dbo.ArticleMains", "Main", c => c.String());
            AddColumn("dbo.Firstmissions", "FirstItem", c => c.String());
            AddColumn("dbo.Firstmissions", "ItemNumber", c => c.String());
            DropColumn("dbo.ArticleMains", "ArticleMainStringId");
            DropColumn("dbo.Firstmissions", "FirstmissionStringId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Firstmissions", "FirstmissionStringId", c => c.Int(nullable: false));
            AddColumn("dbo.ArticleMains", "ArticleMainStringId", c => c.Int(nullable: false));
            DropColumn("dbo.Firstmissions", "ItemNumber");
            DropColumn("dbo.Firstmissions", "FirstItem");
            DropColumn("dbo.ArticleMains", "Main");
            CreateIndex("dbo.Firstmissions", "FirstmissionStringId");
            CreateIndex("dbo.ArticleMains", "ArticleMainStringId");
            AddForeignKey("dbo.Firstmissions", "FirstmissionStringId", "dbo.FirstmissionStrings", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ArticleMains", "ArticleMainStringId", "dbo.ArticleMainStrings", "Id", cascadeDelete: true);
        }
    }
}
