namespace DDAC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editonregisterpart2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "CompanyName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CompanyName");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
