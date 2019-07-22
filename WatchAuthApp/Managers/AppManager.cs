using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using WatchAuthApp.Models;
using WatchAuthApp.ViewModels;

namespace WatchAuthApp.Managers
{
    public class AppManager
    {
        public ICollection<Movie> GetMovies(string search, string category, int sortBy)
        {
            ICollection<Movie> movies;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Create a basic query and build it according to parameters
                var query = db.Movies.AsQueryable();
                if (!String.IsNullOrEmpty(search))
                {
                    query = query.Where(x => x.Title.Contains(search));
                }
                if (!String.IsNullOrEmpty(category))
                {
                    query = query.Where(x => x.Genre == category);
                }
                switch ((SortOptions)sortBy)
                {
                    case SortOptions.Title:
                        query = query.OrderBy(x => x.Title);
                        break;
                    case SortOptions.Year:
                        query = query.OrderBy(x => x.Year);
                        break;
                }
                movies = query.ToList();
            }
            return movies;
        }
        public ICollection<int> GetFavoriteMovies(string userId)
        {
            ICollection<int> favMoviesIds;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                favMoviesIds = db.Users.Include(x => x.FavoriteMovies)
                                       .Where(x => x.Id == userId)
                                       .First()
                                       .FavoriteMovies
                                       .Select(x => x.Id)
                                       .ToList();

            }
            return favMoviesIds;
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
        public bool ToggleFavoriteMovie(int movieId, string userId)
        {
            bool result;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                ApplicationUser user = db.Users.Include(x => x.FavoriteMovies)
                                               .Where(x => x.Id == userId)
                                               .First();
                Movie movie = user.FavoriteMovies.Where(x => x.Id == movieId).FirstOrDefault();
                if (movie == null)
                {
                    movie = db.Movies.Find(movieId);
                    user.FavoriteMovies.Add(movie);
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    user.FavoriteMovies.Remove(movie);
                    db.SaveChanges();
                    result = false;
                }
                
            }
            return result;
        }  
        
    }
}