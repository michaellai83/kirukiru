namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteFilrstPic : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Articles", "FirstPicId", "dbo.FirstPics");
            DropIndex("dbo.Articles", new[] { "FirstPicId" });
            AddColumn("dbo.Articles", "FirstPicName", c => c.String());
            AddColumn("dbo.Articles", "FirstPicFileName", c => c.String());
            DropColumn("dbo.Articles", "FirstPicId");
            DropTable("dbo.FirstPics");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FirstPics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Articles", "FirstPicId", c => c.Int(nullable: false));
            DropColumn("dbo.Articles", "FirstPicFileName");
            DropColumn("dbo.Articles", "FirstPicName");
            CreateIndex("dbo.Articles", "FirstPicId");
            AddForeignKey("dbo.Articles", "FirstPicId", "dbo.FirstPics", "Id", cascadeDelete: true);
        }
    }
}
