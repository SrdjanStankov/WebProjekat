namespace ProjektniZadatak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedIssueDates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IssueDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IssueDate = c.DateTime(nullable: false),
                        Apartment_Id = c.Int(),
                        Apartment_Id1 = c.Int(),
                        Apartment_Id2 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apartments", t => t.Apartment_Id)
                .ForeignKey("dbo.Apartments", t => t.Apartment_Id1)
                .ForeignKey("dbo.Apartments", t => t.Apartment_Id2)
                .Index(t => t.Apartment_Id)
                .Index(t => t.Apartment_Id1)
                .Index(t => t.Apartment_Id2);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IssueDates", "Apartment_Id2", "dbo.Apartments");
            DropForeignKey("dbo.IssueDates", "Apartment_Id1", "dbo.Apartments");
            DropForeignKey("dbo.IssueDates", "Apartment_Id", "dbo.Apartments");
            DropIndex("dbo.IssueDates", new[] { "Apartment_Id2" });
            DropIndex("dbo.IssueDates", new[] { "Apartment_Id1" });
            DropIndex("dbo.IssueDates", new[] { "Apartment_Id" });
            DropTable("dbo.IssueDates");
        }
    }
}
