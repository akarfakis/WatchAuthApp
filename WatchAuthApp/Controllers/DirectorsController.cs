using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchAuthApp.Managers;
using WatchAuthApp.Models;

namespace WatchAuthApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DirectorsController : Controller
    {
        private DbManager db = new DbManager();
        // GET: Directors
        public ActionResult Index()
        {
            var directors = db.GetDirectors();

            return View(directors);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Director director)
        {
            if (!ModelState.IsValid)
            {
                return View(director);
            }
            db.AddDirector(director);
            Session["Actions"] = (int)Session["Actions"] + 1;


            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            Director director = db.GetDirector(id);

            if (director == null)
            {
                return HttpNotFound();
            }

            return View(director);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Director director)
        {
            if (!ModelState.IsValid)
            {
                return View(director);
            }

            db.UpdateDirector(director);
            Session["Actions"] = (int)Session["Actions"] + 1;

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {

            Director director = db.GetDirector(id);
            if (director == null)
            {
                return HttpNotFound();
            }

            return View(director);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeleteDirector(id);
            Session["Actions"] = (int)Session["Actions"] + 1;

            return RedirectToAction("Index");
        }
    }
}