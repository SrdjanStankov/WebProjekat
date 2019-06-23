namespace ProjektniZadatak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomDates", "AvailableDateOfApartment_Id", "dbo.Apartments");
            DropForeignKey("dbo.CustomDates", "IssueDateOfApartment_Id", "dbo.Apartments");
            DropForeignKey("dbo.CustomDates", "Apartment_Id", "dbo.Apartments");
            DropForeignKey("dbo.CustomDates", "Apartment_Id1", "dbo.Apartments");
            DropIndex("dbo.CustomDates", new[] { "AvailableDateOfApartment_Id" });
            DropIndex("dbo.CustomDates", new[] { "IssueDateOfApartment_Id" });
            DropIndex("dbo.CustomDates", new[] { "Apartment_Id" });
            DropIndex("dbo.CustomDates", new[] { "Apartment_Id1" });
            DropTable("dbo.CustomDates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.DateTime(nullable: false),
                        AvailableDateOfApartment_Id = c.Int(),
                        IssueDateOfApartment_Id = c.Int(),
                        Apartment_Id = c.Int(),
                        Apartment_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.CustomDates", "Apartment_Id1");
            CreateIndex("dbo.CustomDates", "Apartment_Id");
            CreateIndex("dbo.CustomDates", "IssueDateOfApartment_Id");
            CreateIndex("dbo.CustomDates", "AvailableDateOfApartment_Id");
            AddForeignKey("dbo.CustomDates", "Apartment_Id1", "dbo.Apartments", "Id");
            AddForeignKey("dbo.CustomDates", "Apartment_Id", "dbo.Apartments", "Id");
            AddForeignKey("dbo.CustomDates", "IssueDateOfApartment_Id", "dbo.Apartments", "Id");
            AddForeignKey("dbo.CustomDates", "AvailableDateOfApartment_Id", "dbo.Apartments", "Id");
        }
    }
}
