namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditMemberAddInfotable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Introduction", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "Introduction");
        }
    }
}
