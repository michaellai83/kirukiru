namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBackMemberPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Backmembers", "Photo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Backmembers", "Photo");
        }
    }
}
