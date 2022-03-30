namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditBackArticleTitlePic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackArticles", "Titlepic", c => c.String());
            DropColumn("dbo.BackArticles", "Titlepicname");
            DropColumn("dbo.BackArticles", "Picfilename");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BackArticles", "Picfilename", c => c.String());
            AddColumn("dbo.BackArticles", "Titlepicname", c => c.String());
            DropColumn("dbo.BackArticles", "Titlepic");
        }
    }
}
