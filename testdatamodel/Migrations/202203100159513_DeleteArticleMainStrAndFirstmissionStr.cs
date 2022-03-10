namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteArticleMainStrAndFirstmissionStr : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.ArticleMainStrings");
            DropTable("dbo.FirstmissionStrings");
            DropTable("dbo.Goods");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ArticleId = c.Int(nullable: false),
                        ArticleNorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FirstmissionStrings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleMainStrings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Main = c.String(),
                        InDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
    }
}
