namespace WatchAuthApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Movies : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUserMovies",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Movie_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Movie_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.Movie_Id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Movie_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUserMovies", "Movie_Id", "dbo.Movies");
            DropForeignKey("dbo.ApplicationUserMovies", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserMovies", new[] { "Movie_Id" });
            DropIndex("dbo.ApplicationUserMovies", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserMovies");
        }
    }
}
