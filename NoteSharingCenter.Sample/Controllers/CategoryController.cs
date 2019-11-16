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
using NoteSharingCenter.Sample.Models;

namespace NoteSharingCenter.Sample.Controllers
{
    public class CategoryController : Controller
    {
        CategoryRepository cr = new CategoryRepository();

        public ActionResult Index()
        {
            return View(cr.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = cr.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                cr.Insert(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = cr.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Category cat = cr.Find(x => x.Id == category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;
                cr.Update(cat);
                return RedirectToAction("Index");
            }
            return View(category);
        }
           
        public ActionResult Delete(int id)
        {
            Category category = cr.Find(x => x.Id == id);
            cr.Delete(category);
            return RedirectToAction("Index");
        }


    }
}
