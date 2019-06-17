namespace ProjektniZadatak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedApartmentIdToIssueDate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IssueDates", "Apartment_Id", "dbo.Apartments");
            DropIndex("dbo.IssueDates", new[] { "Apartment_Id1" });
            DropIndex("dbo.IssueDates", new[] { "Apartment_Id" });
            DropColumn("dbo.IssueDates", "Apartment_Id");
            RenameColumn(table: "dbo.IssueDates", name: "Apartment_Id1", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.IssueDates", name: "Apartment_Id2", newName: "Apartment_Id1");
            RenameColumn(table: "dbo.IssueDates", name: "__mig_tmp__0", newName: "Apartment_Id");
            RenameIndex(table: "dbo.IssueDates", name: "IX_Apartment_Id2", newName: "IX_Apartment_Id1");
            AddColumn("dbo.IssueDates", "ApartmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.IssueDates", "ApartmentId");
            AddForeignKey("dbo.IssueDates", "ApartmentId", "dbo.Apartments", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IssueDates", "ApartmentId", "dbo.Apartments");
            DropIndex("dbo.IssueDates", new[] { "ApartmentId" });
            DropColumn("dbo.IssueDates", "ApartmentId");
            RenameIndex(table: "dbo.IssueDates", name: "IX_Apartment_Id1", newName: "IX_Apartment_Id2");
            RenameColumn(table: "dbo.IssueDates", name: "Apartment_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.IssueDates", name: "Apartment_Id1", newName: "Apartment_Id2");
            RenameColumn(table: "dbo.IssueDates", name: "__mig_tmp__0", newName: "Apartment_Id1");
            AddColumn("dbo.IssueDates", "Apartment_Id", c => c.Int());
            CreateIndex("dbo.IssueDates", "Apartment_Id1");
            AddForeignKey("dbo.IssueDates", "Apartment_Id", "dbo.Apartments", "Id");
        }
    }
}
