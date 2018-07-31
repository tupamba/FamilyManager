namespace FamilyManager.DataProvider.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FamilyCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FamilyCategories", t => t.FamilyCategory_Id)
                .Index(t => t.FamilyCategory_Id);
            
            CreateTable(
                "dbo.FamilyCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupFamilies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.MemberFamilies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        GroupFamily_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupFamilies", t => t.GroupFamily_Id)
                .Index(t => t.GroupFamily_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        CategoryProduct_Id = c.Int(),
                        GroupFamily_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryProducts", t => t.CategoryProduct_Id)
                .ForeignKey("dbo.GroupFamilies", t => t.GroupFamily_Id)
                .Index(t => t.CategoryProduct_Id)
                .Index(t => t.GroupFamily_Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Create = c.DateTime(nullable: false),
                        LastModify = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        GroupFamily_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupFamilies", t => t.GroupFamily_Id)
                .Index(t => t.GroupFamily_Id);
            
            CreateTable(
                "dbo.TicketProducts",
                c => new
                    {
                        Ticket_Id = c.Long(nullable: false),
                        Product_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ticket_Id, t.Product_Id })
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .Index(t => t.Ticket_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketProducts", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.TicketProducts", "Ticket_Id", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "GroupFamily_Id", "dbo.GroupFamilies");
            DropForeignKey("dbo.Products", "GroupFamily_Id", "dbo.GroupFamilies");
            DropForeignKey("dbo.Products", "CategoryProduct_Id", "dbo.CategoryProducts");
            DropForeignKey("dbo.MemberFamilies", "GroupFamily_Id", "dbo.GroupFamilies");
            DropForeignKey("dbo.CategoryProducts", "FamilyCategory_Id", "dbo.FamilyCategories");
            DropIndex("dbo.TicketProducts", new[] { "Product_Id" });
            DropIndex("dbo.TicketProducts", new[] { "Ticket_Id" });
            DropIndex("dbo.Tickets", new[] { "GroupFamily_Id" });
            DropIndex("dbo.Products", new[] { "GroupFamily_Id" });
            DropIndex("dbo.Products", new[] { "CategoryProduct_Id" });
            DropIndex("dbo.MemberFamilies", new[] { "GroupFamily_Id" });
            DropIndex("dbo.GroupFamilies", new[] { "Name" });
            DropIndex("dbo.CategoryProducts", new[] { "FamilyCategory_Id" });
            DropTable("dbo.TicketProducts");
            DropTable("dbo.Tickets");
            DropTable("dbo.Products");
            DropTable("dbo.MemberFamilies");
            DropTable("dbo.GroupFamilies");
            DropTable("dbo.FamilyCategories");
            DropTable("dbo.CategoryProducts");
        }
    }
}
