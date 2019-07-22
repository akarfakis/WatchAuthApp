using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchAuthApp.Models
{
    public class Movie
    {
        //Need to initialize Actors so it won't be null
        public Movie()
        {
            Actors = new HashSet<Actor>();
            FavoredBy = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public int Year { get; set; }
        public bool Watched { get; set; }

        //this is Category Foreign Key property
        [ForeignKey("Category")]
        public string Genre { get; set; }
        public virtual Category Category { get; set; }

        //this is Director Foreign Key property
        [DisplayName("Director")]
        public int DirectorId { get; set; }
        public virtual Director Director { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<ApplicationUser> FavoredBy { get; set; }
    }
}