namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageNorAndMemberFK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MessageNormals", "MemberID", c => c.Int(nullable: false));
            CreateIndex("dbo.MessageNormals", "MemberID");
            AddForeignKey("dbo.MessageNormals", "MemberID", "dbo.Members", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MessageNormals", "MemberID", "dbo.Members");
            DropIndex("dbo.MessageNormals", new[] { "MemberID" });
            DropColumn("dbo.MessageNormals", "MemberID");
        }
    }
}
