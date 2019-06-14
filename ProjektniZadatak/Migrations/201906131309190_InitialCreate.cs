namespace ProjektniZadatak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Lastname = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Username);

            CreateTable(
                "dbo.Apartments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApartmentType = c.Int(nullable: false),
                        NumberOfRooms = c.Int(nullable: false),
                        NumberOfGuests = c.Int(nullable: false),
                        PricePerNight = c.Int(nullable: false),
                        TimeOfRegistration = c.DateTime(nullable: false),
                        TimeOfCheckOut = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Host_Username = c.String(maxLength: 128),
                        Location_Id = c.Int(),
                        Guest_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Host_Username)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.Users", t => t.Guest_Username)
                .Index(t => t.Host_Username)
                .Index(t => t.Location_Id)
                .Index(t => t.Guest_Username);

            CreateTable(
                "dbo.Amenities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Apartment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apartments", t => t.Apartment_Id)
                .Index(t => t.Apartment_Id);

            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Rating = c.Int(nullable: false),
                        Apartment_Id = c.Int(),
                        GuestThaWroteComment_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Apartments", t => t.Apartment_Id)
                .ForeignKey("dbo.Users", t => t.GuestThaWroteComment_Username)
                .Index(t => t.Apartment_Id)
                .Index(t => t.GuestThaWroteComment_Username);

            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Address_Street = c.String(),
                        Address_Number = c.Int(nullable: false),
                        Address_Town = c.String(),
                        Address_ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReservationStartDate = c.DateTime(nullable: false),
                        NumberOfNights = c.Int(nullable: false),
                        TotalPrice = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Guest_Username = c.String(maxLength: 128),
                        ReservedApartment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Guest_Username)
                .ForeignKey("dbo.Apartments", t => t.ReservedApartment_Id)
                .Index(t => t.Guest_Username)
                .Index(t => t.ReservedApartment_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Apartments", "Guest_Username", "dbo.Users");
            DropForeignKey("dbo.Reservations", "ReservedApartment_Id", "dbo.Apartments");
            DropForeignKey("dbo.Reservations", "Guest_Username", "dbo.Users");
            DropForeignKey("dbo.Apartments", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.Apartments", "Host_Username", "dbo.Users");
            DropForeignKey("dbo.Comments", "GuestThaWroteComment_Username", "dbo.Users");
            DropForeignKey("dbo.Comments", "Apartment_Id", "dbo.Apartments");
            DropForeignKey("dbo.Amenities", "Apartment_Id", "dbo.Apartments");
            DropIndex("dbo.Reservations", new[] { "ReservedApartment_Id" });
            DropIndex("dbo.Reservations", new[] { "Guest_Username" });
            DropIndex("dbo.Comments", new[] { "GuestThaWroteComment_Username" });
            DropIndex("dbo.Comments", new[] { "Apartment_Id" });
            DropIndex("dbo.Amenities", new[] { "Apartment_Id" });
            DropIndex("dbo.Apartments", new[] { "Guest_Username" });
            DropIndex("dbo.Apartments", new[] { "Location_Id" });
            DropIndex("dbo.Apartments", new[] { "Host_Username" });
            DropTable("dbo.Reservations");
            DropTable("dbo.Locations");
            DropTable("dbo.Comments");
            DropTable("dbo.Amenities");
            DropTable("dbo.Apartments");
            DropTable("dbo.Users");
        }
    }
}
