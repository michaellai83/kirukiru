namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFinalMission : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinalMissions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        Title = c.String(),
                        Main = c.String(),
                        InitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FinalMissions", "ArticleId", "dbo.Articles");
            DropIndex("dbo.FinalMissions", new[] { "ArticleId" });
            DropTable("dbo.FinalMissions");
        }
    }
}
