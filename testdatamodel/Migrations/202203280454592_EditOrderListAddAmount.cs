namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOrderListAddAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orderlists", "Amount", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orderlists", "Amount");
        }
    }
}
