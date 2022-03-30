namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditBackArticleAddBackMemberPic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BackArticles", "BackMemberPic", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BackArticles", "BackMemberPic");
        }
    }
}
