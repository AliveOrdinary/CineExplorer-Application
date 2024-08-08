namespace CineExplorer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedTripModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Trips", "UserID");
            AddForeignKey("dbo.Trips", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trips", "UserID", "dbo.Users");
            DropIndex("dbo.Trips", new[] { "UserID" });
            DropColumn("dbo.Trips", "UserID");
        }
    }
}
