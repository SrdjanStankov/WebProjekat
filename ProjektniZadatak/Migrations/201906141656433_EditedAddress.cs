namespace ProjektniZadatak.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class EditedAddress : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Street = c.String(),
                        Number = c.Int(nullable: false),
                        Town = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Locations", "Address_Id", c => c.Int());
            CreateIndex("dbo.Locations", "Address_Id");
            AddForeignKey("dbo.Locations", "Address_Id", "dbo.Addresses", "Id");
            DropColumn("dbo.Locations", "Address_Street");
            DropColumn("dbo.Locations", "Address_Number");
            DropColumn("dbo.Locations", "Address_Town");
            DropColumn("dbo.Locations", "Address_ZipCode");
        }

        public override void Down()
        {
            AddColumn("dbo.Locations", "Address_ZipCode", c => c.String());
            AddColumn("dbo.Locations", "Address_Town", c => c.String());
            AddColumn("dbo.Locations", "Address_Number", c => c.Int(nullable: false));
            AddColumn("dbo.Locations", "Address_Street", c => c.String());
            DropForeignKey("dbo.Locations", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.Locations", new[] { "Address_Id" });
            DropColumn("dbo.Locations", "Address_Id");
            DropTable("dbo.Addresses");
        }
    }
}
