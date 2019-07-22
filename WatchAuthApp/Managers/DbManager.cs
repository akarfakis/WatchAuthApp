using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WatchAuthApp.Models;
namespace WatchAuthApp.Managers
{
    public class DbManager
    {
        public ICollection<Actor> GetActors()
        {
            ICollection<Actor> actors;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                actors = db.Actors.ToList();
            }

            return actors;
        }
        public Actor GetActor(int id)
        {
            Actor actor;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                actor = db.Actors.Find(id);
            }
            return actor;
        }
        public void AddActor(Actor actor)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Actors.Add(actor);
                db.SaveChanges();
            }
        }
        public void UpdateActor(Actor actor)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ////1st way
                //Actor db_actor = db.Actors.Find(actor.Id);
                ////State of db_actor is unchanged
                //db_actor.Name = actor.Name;
                ////now the State is "Modified"
                //db_actor.Age = actor.Age;

                //2nd Way

                db.Actors.Attach(actor);
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void DeleteActor(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Actor actor = db.Actors.Find(id);
                db.Actors.Remove(actor);
                db.SaveChanges();
            }
        }

        public bool DeleteActorBool(int id)
        {
            bool result;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Actor actor = db.Actors.Find(id);
                if (actor != null)
                {
                    db.Actors.Remove(actor);
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }


        public ICollection<Director> GetDirectors()
        {
            ICollection<Director> directors;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                directors = db.Directors.ToList();
            }
            return directors;
        }
        public Director GetDirector(int id)
        {
            Director director;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                director = db.Directors.Find(id);
            }
            return director;
        }
        public void AddDirector(Director director)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Directors.Add(director);
                db.SaveChanges();
            }
        }
        public void UpdateDirector(Director director)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Directors.Attach(director);
                db.Entry(director).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void DeleteDirector(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Director director = db.Directors.Find(id);
                db.Directors.Remove(director);
                db.SaveChanges();
            }
        }


        public ICollection<Category> GetCategories()
        {
            ICollection<Category> categories;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                categories = db.Categories.ToList();
            }
            return categories;
        }
        public Category GetCategory(string name)
        {
            Category category;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                category = db.Categories.Find(name);
            }
            return category;
        }
        public void AddCategory(Category category)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Categories.Add(category);
                db.SaveChanges();
            }
        }
        public void DeleteCategory(Category category)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Categories.Attach(category);
                db.Entry(category).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public void DeleteCategory(string name)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Category category = db.Categories.Find(name);
                db.Categories.Remove(category);
                db.SaveChanges();
            }
        }


        public ICollection<Movie> GetMovies()
        {
            ICollection<Movie> movies;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                movies = db.Movies.Include("Director")
                                  .Include("Category")
                                  .Include("Actors")
                                  .ToList();
            }
            return movies;
        }
        public Movie GetMovie(int id)
        {
            Movie movie;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                movie = db.Movies.Find(id);
            }
            return movie;
        }

        public Movie GetMovieFull(int id)
        {
            Movie movie;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                movie = db.Movies.Include("Director")
                                 .Include("Category")
                                 .Include("Actors")
                                 .FirstOrDefault(i => i.Id == id);
            }
            return movie;
        }
        public void AddMovie(Movie movie, List<int> actorIds)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                db.Movies.Add(movie);
                db.SaveChanges();

                foreach (int id in actorIds)
                {
                    Actor actor = db.Actors.Find(id);
                    if (actor != null)
                    {
                        movie.Actors.Add(actor);
                    }
                }
                db.SaveChanges();
            }
        }
        public void UpdateMovie(Movie movie, List<int> actorIds)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Movies.Attach(movie);
                db.Entry(movie).Collection("Actors").Load();
                movie.Actors.Clear();
                db.SaveChanges();


                foreach (int id in actorIds)
                {
                    Actor actor = db.Actors.Find(id);

                    if (actor != null)
                    {
                        movie.Actors.Add(actor);
                    }

                }

                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteMovie(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Movie movie = db.Movies.Find(id);
                db.Movies.Remove(movie);
                db.SaveChanges();
            }
        }
    }
}