using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchAuthApp.Models;

namespace WatchAuthApp.ViewModels
{
    public class MovieVM
    {
        public Movie Movie { get; set; }
        public IEnumerable<SelectListItem> Actors { get; set; } //list of all actors

        List<int> _selectedActors;
        public List<int> SelectedActors
        {
            get
            {
                if (_selectedActors == null)
                {
                    _selectedActors = Movie.Actors.Select(i => i.Id).ToList();
                }
                return _selectedActors;
            }
            set
            {
                _selectedActors = value;
            }
        }
    }
}