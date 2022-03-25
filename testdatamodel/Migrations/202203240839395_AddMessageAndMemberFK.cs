namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageAndMemberFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "MemberID", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "MemberID");
            AddForeignKey("dbo.Messages", "MemberID", "dbo.Members", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "MemberID", "dbo.Members");
            DropIndex("dbo.Messages", new[] { "MemberID" });
            DropColumn("dbo.Messages", "MemberID");
        }
    }
}
