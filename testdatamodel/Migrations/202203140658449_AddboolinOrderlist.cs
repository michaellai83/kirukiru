namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddboolinOrderlist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orderlists", "Issuccess", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orderlists", "Issuccess");
        }
    }
}
