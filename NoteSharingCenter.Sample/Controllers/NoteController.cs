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

        #region List
        public ActionResult Index()
        {

            var notes = nr.ListQueryable().Include("Category").Include("Owner").Where(
                x => x.Owner.Id == MySession.CurrentUser.Id).OrderByDescending(
                x => x.ModifiedOn);
            return View(notes.ToList());
        }
        #endregion

        #region Detail
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
        #endregion

        #region Create
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
                note.NoteImageFilename = "blog-11.jpg";
                nr.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(cr.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }
        #endregion

        #region Edit
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
        public ActionResult Edit(Note note, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{note.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Content/img/{filename}"));
                    note.NoteImageFilename = filename;
                }

                RepositoryLayerResult<Note> re = nr.Updatee(note);

                if (re.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = re.Errors,
                        Title = "Failed to update note.",
                        RedirectinUrl = "/Note/Index"
                    };

                    return View("Error", errorNotifyObj);
                }

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(cr.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int id)
        {
            Note note = nr.Find(x => x.Id == id);
            nr.Delete(note);
            return RedirectToAction("Index");
        }
        #endregion

        #region Liked List
        public ActionResult UserLikedNotes()
        {
            var notes = lr.ListQueryable().Include("LikedUser").Include("Note").Where(
                x => x.LikedUser.Id == MySession.CurrentUser.Id).Select(
                x => x.Note).Include("Category").Include("Owner").OrderByDescending(
                x => x.ModifiedOn);

            return View(notes.ToList());
        }
        #endregion

        #region Home Page Note Likeds
        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (ids != null)
            {
                if (MySession.CurrentUser != null)
                {
                    List<int> likedNoteIds = lr.List(
                        x => x.LikedUser.Id == MySession.CurrentUser.Id
                        && ids.Contains(x.Note.Id)).Select(x => x.Note.Id).ToList();

                    return Json(new { result = likedNoteIds });
                }
                else
                {
                    return Json(new { result = new List<int>() });
                }
            }
            return Json(new { result = new List<int>() });
        }

        [HttpPost]
        public ActionResult SetLikeState(int noteid, bool liked)
        {
            int res = 0;

            if (MySession.CurrentUser == null)
                return Json(new { hasError = true, errorMessage = "You must be logged in to like.", result = 0 });

            Liked like =
                lr.Find(x => x.Note.Id == noteid && x.LikedUser.Id == MySession.CurrentUser.Id);

            Note note = nr.Find(x => x.Id == noteid);

            if (like != null && liked == false)
            {
                res = lr.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = lr.Insert(new Liked()
                {
                    LikedUser = MySession.CurrentUser,
                    Note = note
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }

                res = nr.Update(note);

                return Json(new { hasError = false, errorMessage = string.Empty, result = note.LikeCount });
            }

            return Json(new { hasError = true, errorMessage = "Failed to perform liking.", result = note.LikeCount });
        }
        #endregion
    }
}
