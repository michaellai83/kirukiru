namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditArticleAddLovecount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Lovecount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Lovecount");
        }
    }
}
