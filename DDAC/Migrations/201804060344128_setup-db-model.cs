namespace DDAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setupdbmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookScheduleModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDelivered = c.Boolean(nullable: false),
                        ScheduleDetailsId = c.Int(nullable: false),
                        totalBayUsed = c.Int(nullable: false),
                        CustomerModelsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerModels", t => t.CustomerModelsId, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleDetails", t => t.ScheduleDetailsId, cascadeDelete: true)
                .Index(t => t.ScheduleDetailsId)
                .Index(t => t.CustomerModelsId);
            
            CreateTable(
                "dbo.ContainerModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContainerType = c.String(nullable: false),
                        NumberOfBayUsed = c.Int(nullable: false),
                        BookScheduleModel_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookScheduleModels", t => t.BookScheduleModel_Id)
                .Index(t => t.BookScheduleModel_Id);
            
            CreateTable(
                "dbo.CustomerModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        IcNo = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        AspNetUsersId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScheduleDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Origin = c.String(nullable: false),
                        Destination = c.String(nullable: false),
                        ShipDetailsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShipDetails", t => t.ShipDetailsId, cascadeDelete: true)
                .Index(t => t.ShipDetailsId);
            
            CreateTable(
                "dbo.ShipDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShipName = c.String(nullable: false),
                        BaySize = c.Int(nullable: false),
                        Origin = c.String(nullable: false),
                        Destination = c.String(nullable: false),
                        Availability = c.Boolean(nullable: false),
                        RemainingBaySize = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Origin = c.String(),
                        Destination = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookScheduleModels", "ScheduleDetailsId", "dbo.ScheduleDetails");
            DropForeignKey("dbo.ScheduleDetails", "ShipDetailsId", "dbo.ShipDetails");
            DropForeignKey("dbo.BookScheduleModels", "CustomerModelsId", "dbo.CustomerModels");
            DropForeignKey("dbo.ContainerModels", "BookScheduleModel_Id", "dbo.BookScheduleModels");
            DropIndex("dbo.ScheduleDetails", new[] { "ShipDetailsId" });
            DropIndex("dbo.ContainerModels", new[] { "BookScheduleModel_Id" });
            DropIndex("dbo.BookScheduleModels", new[] { "CustomerModelsId" });
            DropIndex("dbo.BookScheduleModels", new[] { "ScheduleDetailsId" });
            DropTable("dbo.Locations");
            DropTable("dbo.ShipDetails");
            DropTable("dbo.ScheduleDetails");
            DropTable("dbo.CustomerModels");
            DropTable("dbo.ContainerModels");
            DropTable("dbo.BookScheduleModels");
        }
    }
}
