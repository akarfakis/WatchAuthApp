using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WatchAuthApp.Models;

namespace WatchAuthApp.ViewModels
{
    public enum SortOptions
    {
        Title = 1,
        Year       
    }
    public class AppVM
    {
        public IEnumerable<int> FavoriteMovies { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public string Search { get; set; }
        public string Category { get; set; }
        public SortOptions SortBy { get; set; }

    }
}