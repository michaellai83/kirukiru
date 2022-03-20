namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrdernumberInOrderlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orderlists", "Ordernumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orderlists", "Ordernumber");
        }
    }
}
