namespace WatchAuthApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WatchAuthApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WatchAuthApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            Category cat_horror = new Category { Name = "Horror" };
            Category cat_action = new Category { Name = "Action" };
            Category cat_scifi = new Category { Name = "SciFi" };
            Category cat_comedy = new Category { Name = "Comedy" };
            Category cat_western = new Category { Name = "Western" };

            context.Categories.AddOrUpdate(x => x.Name, cat_horror);
            context.Categories.AddOrUpdate(x => x.Name, cat_action);
            context.Categories.AddOrUpdate(x => x.Name, cat_scifi);
            context.Categories.AddOrUpdate(x => x.Name, cat_comedy);
            context.Categories.AddOrUpdate(x => x.Name, cat_western);

            //Add Directors
            Director dir_tarantino = new Director() { Name = "Quentin Tarantin", Age = 55 };
            Director dir_aronofsky = new Director() { Name = "Darren Aronofksy", Age = 60 };
            Director dir_kubrick = new Director() { Name = "Stanley Kubrick", Age = 59 };
            context.Directors.AddOrUpdate(x => x.Name, dir_tarantino);
            context.Directors.AddOrUpdate(x => x.Name, dir_aronofsky);
            context.Directors.AddOrUpdate(x => x.Name, dir_kubrick);

            //Add Actors
            Actor actor_travolta = new Actor() { Name = "John Travolta", Age = 52 };
            Actor actor_ford = new Actor() { Name = "Harrison Ford", Age = 68 };
            Actor actor_tom = new Actor() { Name = "Tom Cruz", Age = 51 };

            context.Actors.AddOrUpdate(x => x.Name, actor_travolta);
            context.Actors.AddOrUpdate(x => x.Name, actor_ford);
            context.Actors.AddOrUpdate(x => x.Name, actor_tom);

            context.SaveChanges(); //we need this cause movies need actors and categories and directors, and they need Ids

            //Add Movies

            Movie movie_killbill = new Movie()
            {
                Title = "Kill Bill",
                Year = 2003,
                Director = dir_tarantino,
                Category = cat_action
            };
            movie_killbill.Actors.Add(actor_travolta);
            movie_killbill.Actors.Add(actor_ford);

            Movie movie_fiction = new Movie()
            {
                Title = "Pulp Fiction",
                Year = 1995,
                Director = dir_tarantino,
                Category = cat_action
            };
            movie_fiction.Actors.Add(actor_travolta);
            movie_fiction.Actors.Add(actor_tom);

            context.Movies.AddOrUpdate(x => x.Title, movie_fiction);
            context.Movies.AddOrUpdate(x => x.Title, movie_killbill);
            context.SaveChanges();

            //Assign Roles
            var userStore = new UserStore<ApplicationUser>(context);
            ApplicationUserManager userManager = new ApplicationUserManager(userStore);
            var roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            string email = "admin@gmail.com";
            string username = "admin";
            string password = "iamtheadmin";
            string roleName = "Admin";

            IdentityRole role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                roleManager.Create(role);
            }

            ApplicationUser user = userManager.FindByName(username);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = email,
                    Email = email
                };
                userManager.Create(user, password);
            }

            if (!userManager.IsInRole(user.Id, role.Name))
            {
                userManager.AddToRole(user.Id, role.Name);
            }

        }
    }
}
    