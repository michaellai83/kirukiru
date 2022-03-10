namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Editmember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "Opencollectarticles", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "Opencollectarticles");
        }
    }
}
