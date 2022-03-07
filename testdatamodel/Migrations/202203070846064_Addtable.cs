namespace testdatamodel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articlecategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleNormals",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Title = c.String(),
                        Main = c.String(),
                        ArticlecategoryId = c.Int(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                        IsPush = c.Boolean(nullable: false),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articlecategories", t => t.ArticlecategoryId, cascadeDelete: true)
                .Index(t => t.ArticlecategoryId);
            
            CreateTable(
                "dbo.MessageNormals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ArticleNorId = c.Int(nullable: false),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleNormals", t => t.ArticleNorId, cascadeDelete: true)
                .Index(t => t.ArticleNorId);
            
            CreateTable(
                "dbo.R_MessageNormal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageNorId = c.Int(nullable: false),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MessageNormals", t => t.MessageNorId, cascadeDelete: true)
                .Index(t => t.MessageNorId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstPicId = c.Int(nullable: false),
                        Title = c.String(),
                        IsFree = c.Boolean(nullable: false),
                        Introduction = c.String(),
                        ArticlecategoryId = c.Int(nullable: false),
                        IsPush = c.Boolean(nullable: false),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articlecategories", t => t.ArticlecategoryId, cascadeDelete: true)
                .ForeignKey("dbo.FirstPics", t => t.FirstPicId, cascadeDelete: true)
                .Index(t => t.FirstPicId)
                .Index(t => t.ArticlecategoryId);
            
            CreateTable(
                "dbo.ArticleMains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PicName = c.String(),
                        PicFileName = c.String(),
                        ArticleId = c.Int(nullable: false),
                        InDateTime = c.DateTime(nullable: false),
                        ArticleMainStringId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleMainStrings", t => t.ArticleMainStringId, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.ArticleMainStringId);
            
            CreateTable(
                "dbo.ArticleMainStrings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Main = c.String(),
                        InDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Firstmissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PicName = c.String(),
                        PicFileName = c.String(),
                        FirstmissionStringId = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.FirstmissionStrings", t => t.FirstmissionStringId, cascadeDelete: true)
                .Index(t => t.FirstmissionStringId)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.FirstmissionStrings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FirstPics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ArticleId = c.Int(nullable: false),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.R_Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        Main = c.String(),
                        InitDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: true)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.Remarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Main = c.String(),
                        InitTime = c.DateTime(nullable: false),
                        ArticleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        PassWord = c.String(nullable: false),
                        PasswordSalt = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Gender = c.String(),
                        Birthday = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(nullable: false),
                        ArticlecategoryId = c.Int(nullable: false),
                        Isidentify = c.Boolean(nullable: false),
                        Emailidentify = c.String(),
                        initDate = c.DateTime(nullable: false),
                        PicName = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Articlecategories", t => t.ArticlecategoryId, cascadeDelete: true)
                .Index(t => t.ArticlecategoryId);
            
            CreateTable(
                "dbo.Collects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ArticleId = c.Int(nullable: false),
                        ArticleNorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        ArticleId = c.Int(nullable: false),
                        ArticleNorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        IsSub = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Members", "ArticlecategoryId", "dbo.Articlecategories");
            DropForeignKey("dbo.Remarks", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.R_Message", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Messages", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.Articles", "FirstPicId", "dbo.FirstPics");
            DropForeignKey("dbo.Firstmissions", "FirstmissionStringId", "dbo.FirstmissionStrings");
            DropForeignKey("dbo.Firstmissions", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleMains", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleMains", "ArticleMainStringId", "dbo.ArticleMainStrings");
            DropForeignKey("dbo.Articles", "ArticlecategoryId", "dbo.Articlecategories");
            DropForeignKey("dbo.R_MessageNormal", "MessageNorId", "dbo.MessageNormals");
            DropForeignKey("dbo.MessageNormals", "ArticleNorId", "dbo.ArticleNormals");
            DropForeignKey("dbo.ArticleNormals", "ArticlecategoryId", "dbo.Articlecategories");
            DropIndex("dbo.Members", new[] { "ArticlecategoryId" });
            DropIndex("dbo.Remarks", new[] { "ArticleId" });
            DropIndex("dbo.R_Message", new[] { "MessageId" });
            DropIndex("dbo.Messages", new[] { "ArticleId" });
            DropIndex("dbo.Firstmissions", new[] { "ArticleId" });
            DropIndex("dbo.Firstmissions", new[] { "FirstmissionStringId" });
            DropIndex("dbo.ArticleMains", new[] { "ArticleMainStringId" });
            DropIndex("dbo.ArticleMains", new[] { "ArticleId" });
            DropIndex("dbo.Articles", new[] { "ArticlecategoryId" });
            DropIndex("dbo.Articles", new[] { "FirstPicId" });
            DropIndex("dbo.R_MessageNormal", new[] { "MessageNorId" });
            DropIndex("dbo.MessageNormals", new[] { "ArticleNorId" });
            DropIndex("dbo.ArticleNormals", new[] { "ArticlecategoryId" });
            DropTable("dbo.Subscriptions");
            DropTable("dbo.Goods");
            DropTable("dbo.Collects");
            DropTable("dbo.Members");
            DropTable("dbo.Remarks");
            DropTable("dbo.R_Message");
            DropTable("dbo.Messages");
            DropTable("dbo.FirstPics");
            DropTable("dbo.FirstmissionStrings");
            DropTable("dbo.Firstmissions");
            DropTable("dbo.ArticleMainStrings");
            DropTable("dbo.ArticleMains");
            DropTable("dbo.Articles");
            DropTable("dbo.R_MessageNormal");
            DropTable("dbo.MessageNormals");
            DropTable("dbo.ArticleNormals");
            DropTable("dbo.Articlecategories");
        }
    }
}
