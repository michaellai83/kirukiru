namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBack : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BackArticles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BackmemberID = c.Int(nullable: false),
                        Title = c.String(),
                        Titlepicname = c.String(),
                        Picfilename = c.String(),
                        Main = c.String(),
                        IniDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Backmembers", t => t.BackmemberID, cascadeDelete: true)
                .Index(t => t.BackmemberID);
            
            CreateTable(
                "dbo.Backmembers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Salt = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        IniDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BackArticles", "BackmemberID", "dbo.Backmembers");
            DropIndex("dbo.BackArticles", new[] { "BackmemberID" });
            DropTable("dbo.Backmembers");
            DropTable("dbo.BackArticles");
        }
    }
}
