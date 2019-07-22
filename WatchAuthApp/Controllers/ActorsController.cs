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
    public class ActorsController : Controller
    {
        // GET: Actors
        private DbManager db = new DbManager();
        public ActionResult Index()
        {
            var actors = db.GetActors();

            //way to pass data in View from a specific action
            ViewData["Message"] = "We are on index page!";
            return View(actors);
        }
        public ActionResult Create()
        {
            //another way to pass data in View from a specific action
            ViewBag.Message = "We are on create page!";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //validates AntiForgeryToken (ensures our form will post only from our web app)
        public ActionResult Create(Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }
            db.AddActor(actor);

            //TempData is used to pass data from an action to the NEXT action ONCE
            TempData["notification-color"] = "success";
            TempData["notification-message"] = "Actor inserted successfully!";
            Session["Actions"] = (int)Session["Actions"] + 1;
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            Actor actor = db.GetActor(id);

            if (actor == null)
            {
                //Returns ERROR - 404 To User
                return HttpNotFound();
            }

            return View(actor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            db.UpdateActor(actor);
            TempData["notification-message"] = "Actor edited successfully!";
            TempData["notification-color"] = "warning";
            Session["Actions"] = (int)Session["Actions"] + 1;

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Actor actor = db.GetActor(id);

            if (actor == null)
            {
                return HttpNotFound();
            }

            return View(actor);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.DeleteActor(id);
            TempData["notification-message"] = "Actor deleted successfully!";
            TempData["notification-color"] = "danger";
            Session["Actions"] = (int)Session["Actions"] + 1;

            return RedirectToAction("Index");
        }
    }
}