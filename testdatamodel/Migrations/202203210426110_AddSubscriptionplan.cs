namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubscriptionplan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscriptionplans",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        Amount = c.String(),
                        InitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptionplans", "MemberID", "dbo.Members");
            DropIndex("dbo.Subscriptionplans", new[] { "MemberID" });
            DropTable("dbo.Subscriptionplans");
        }
    }
}
