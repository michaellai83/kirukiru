namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditSubscriptionplanAddInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptionplans", "Introduction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptionplans", "Introduction");
        }
    }
}
