using NoteSharingCenter.Entity;
using NoteSharingCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteSharingCenter.Sample.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (TempData["modelNote"] != null)
            {
                return View(TempData["modelNote"] as List<Note>);
            }

            NoteRepository nr = new NoteRepository();
            return View(nr.GetAllNote());
        }
    }
}