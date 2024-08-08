namespace CineExplorer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingUpdateModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        ImageURL = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ReleaseYear = c.Int(),
                        Description = c.String(),
                        ImageURL = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MovieId);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        TripId = c.Int(nullable: false, identity: true),
                        TripName = c.String(),
                        TripDescription = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TripId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        ReviewText = c.String(nullable: false, maxLength: 100),
                        DateReviewed = c.DateTime(nullable: false),
                        LocationId = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        DateJoined = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.MovieLocations",
                c => new
                    {
                        Movie_MovieId = c.Int(nullable: false),
                        Location_LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Movie_MovieId, t.Location_LocationId })
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId, cascadeDelete: true)
                .Index(t => t.Movie_MovieId)
                .Index(t => t.Location_LocationId);
            
            CreateTable(
                "dbo.TripLocations",
                c => new
                    {
                        Trip_TripId = c.Int(nullable: false),
                        Location_LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trip_TripId, t.Location_LocationId })
                .ForeignKey("dbo.Trips", t => t.Trip_TripId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId, cascadeDelete: true)
                .Index(t => t.Trip_TripId)
                .Index(t => t.Location_LocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserID", "dbo.Users");
            DropForeignKey("dbo.Reviews", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.TripLocations", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.TripLocations", "Trip_TripId", "dbo.Trips");
            DropForeignKey("dbo.MovieLocations", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.MovieLocations", "Movie_MovieId", "dbo.Movies");
            DropIndex("dbo.TripLocations", new[] { "Location_LocationId" });
            DropIndex("dbo.TripLocations", new[] { "Trip_TripId" });
            DropIndex("dbo.MovieLocations", new[] { "Location_LocationId" });
            DropIndex("dbo.MovieLocations", new[] { "Movie_MovieId" });
            DropIndex("dbo.Reviews", new[] { "UserID" });
            DropIndex("dbo.Reviews", new[] { "LocationId" });
            DropTable("dbo.TripLocations");
            DropTable("dbo.MovieLocations");
            DropTable("dbo.Users");
            DropTable("dbo.Reviews");
            DropTable("dbo.Trips");
            DropTable("dbo.Movies");
            DropTable("dbo.Locations");
        }
    }
}
