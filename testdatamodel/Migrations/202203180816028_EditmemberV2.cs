namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditmemberV2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "UserName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Members", "Name", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Members", "Email", c => c.String(nullable: false, maxLength: 200));
            DropColumn("dbo.Members", "Gender");
            DropColumn("dbo.Members", "Birthday");
            DropColumn("dbo.Members", "Address");
            DropColumn("dbo.Members", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "PhoneNumber", c => c.String());
            AddColumn("dbo.Members", "Address", c => c.String());
            AddColumn("dbo.Members", "Birthday", c => c.String());
            AddColumn("dbo.Members", "Gender", c => c.String());
            AlterColumn("dbo.Members", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "UserName", c => c.String(nullable: false));
        }
    }
}
