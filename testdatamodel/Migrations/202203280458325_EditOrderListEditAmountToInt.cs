namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditOrderListEditAmountToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orderlists", "Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orderlists", "Amount", c => c.String(maxLength: 200));
        }
    }
}
