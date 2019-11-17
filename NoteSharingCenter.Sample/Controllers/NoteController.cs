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
    public class NoteController : Controller
    {
        NoteRepository nr = new NoteRepository();
        CategoryRepository cr = new CategoryRepository();
        LikedRepository lr = new LikedRepository();

        public ActionResult Index()
        {

            var notes = nr.ListQueryable().Include("Category").Include("Owner").Where(
                x => x.Owner.Id == MySession.CurrentUser.Id).OrderByDescending(
                x => x.ModifiedOn);
            return View(notes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nr.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(cr.List(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                note.Owner = MySession.CurrentUser;
                nr.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(cr.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nr.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(cr.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Note notee = nr.Find(x => x.Id == note.Id);
                notee.IsDraft = note.IsDraft;
                notee.CategoryId = note.CategoryId;
                notee.Text = note.Text;
                notee.Title = note.Title;

                nr.Update(notee);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(cr.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }


        public ActionResult Delete(int id)
        {
            Note note = nr.Find(x => x.Id == id);
            nr.Delete(note);
            return RedirectToAction("Index");
        }

        public ActionResult UserLikedNotes()
        {
            var notes = lr.ListQueryable().Include("LikedUser").Include("Note").Where(
                x => x.LikedUser.Id == MySession.CurrentUser.Id).Select(
                x => x.Note).Include("Category").Include("Owner").OrderByDescending(
                x => x.ModifiedOn);

            return View(notes.ToList());
        }
    }
}
