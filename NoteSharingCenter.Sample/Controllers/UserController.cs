using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NoteSharingCenter.Entity;
using NoteSharingCenter.Repository;

namespace NoteSharingCenter.Sample.Controllers
{
    public class UserController : Controller
    {
        private UserRepository ur = new UserRepository();

        public ActionResult Index()
        {
            return View(ur.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = ur.Find(x => x.Id == id.Value);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                RepositoryLayerResult<Users> re = ur.Insert(users);
                if (re.Errors.Count > 0)
                {
                    re.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(users);
                }
                return RedirectToAction("Index");
            }

            return View(users);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = ur.Find(x => x.Id == id.Value);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Users users)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                RepositoryLayerResult<Users> re = ur.Update(users);
                if (re.Errors.Count > 0)
                {
                    re.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(users);
                }

                return RedirectToAction("Index");
            }
            return View(users);
        }

        public ActionResult Delete(int id)
        {
            if (id == 1)
            {
                ViewBag.Admin = "dont delete";
                return RedirectToAction("Index","User");
            }
            Users users = ur.Find(x => x.Id == id);
            ur.Delete(users);
            Users currentUser = Session["User"] as Users;
            if (currentUser.Id == id)
            {
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}
