using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WatchAuthApp.Managers;
using WatchAuthApp.Models;
using WatchAuthApp.ViewModels;

namespace WatchAuthApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private AppManager db = new AppManager();
        public ActionResult Index(string search, string category, int sortBy = 0)
        {
            ViewBag.Categories = new SelectList(db.GetCategories(), "Name", "Name");
            AppVM vm = new AppVM()
            {  
                Movies = db.GetMovies(search, category, sortBy),
                Search = search,
                Category = category,
                SortBy = (SortOptions)sortBy,
                FavoriteMovies = db.GetFavoriteMovies(User.Identity.GetUserId())   
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize]
        public ActionResult ToggleFavorite(int movieId)
        {
            bool result = db.ToggleFavoriteMovie(movieId, User.Identity.GetUserId());

            return Json(result);
        }
    }
}