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
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private DbManager db = new DbManager();
        // GET: Movies
        public ActionResult Index()
        {
            var movies = db.GetMovies();
            return View(movies);
        }

        public ActionResult Create()
        {
            //Name has to be the same as the view
            var directors = db.GetDirectors().AsEnumerable(); //Converts type from Icollection to Ienumerable
            ViewBag.DirectorId = new SelectList(directors, "Id", "Name");

            var categories = db.GetCategories();
            ViewBag.Genre = new SelectList(categories, "Name", "Name");
            MovieVM vm = new MovieVM()
            {
                Movie = new Movie(),
                Actors = db.GetActors().Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                })
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieVM vm)
        {
            if (!ModelState.IsValid)
            {
                var directors = db.GetDirectors().AsEnumerable();
                ViewBag.DirectorId = new SelectList(directors, "Id", "Name", vm.Movie.DirectorId);

                var categories = db.GetCategories();
                ViewBag.Genre = new SelectList(categories, "Name", "Name", vm.Movie.Genre);
                return View(vm);
            }

            db.AddMovie(vm.Movie, vm.SelectedActors);
            Session["Actions"] = (int)Session["Actions"] + 1;
            return RedirectToAction("Index");

        }
        public ActionResult Edit(int id)
        {
            Movie movie = db.GetMovieFull(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            MovieVM vm = new MovieVM()
            {
                Movie = movie,
                Actors = db.GetActors().Select(i => new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                })
            };


            ViewBag.DirectorId = new SelectList(db.GetDirectors(), "Id", "Name", movie.DirectorId);
            ViewBag.Genre = new SelectList(db.GetCategories(), "Name", "Name", movie.Genre);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DirectorId = new SelectList(db.GetDirectors(), "Id", "Name", vm.Movie.DirectorId);
                ViewBag.Genre = new SelectList(db.GetCategories(), "Name", "Name", vm.Movie.Genre);
                return View(vm);
            }

            db.UpdateMovie(vm.Movie, vm.SelectedActors);
            Session["Actions"] = (int)Session["Actions"] + 1;
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Movie movie = db.GetMovieFull(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeleteMovie(id);
            Session["Actions"] = (int)Session["Actions"] + 1;
            return RedirectToAction("Index");
        }
    }
}