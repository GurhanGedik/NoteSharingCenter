using NoteSharingCenter.Entity;
using NoteSharingCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NoteSharingCenter.Sample.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Select(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryRepository cr = new CategoryRepository();
            Category cat = cr.GetCategoryById(id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }
            TempData["modelNote"] = cat.Notes;
            return RedirectToAction("Index", "Home");
        }
    }
}