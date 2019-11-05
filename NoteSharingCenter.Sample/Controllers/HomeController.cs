using NoteSharingCenter.Entity;
using NoteSharingCenter.Entity.Messages;
using NoteSharingCenter.Entity.ValueObjects;
using NoteSharingCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NoteSharingCenter.Sample.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            NoteRepository nr = new NoteRepository();
            return View(nr.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        #region Filter

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

            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteRepository nr = new NoteRepository();
            return View("Index", nr.GetAllNote().OrderByDescending(x => x.LikeCount).ToList());
        }

        #endregion

        #region Login / Add User / User Active / Logout

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserRepository ur = new UserRepository();
                RepositoryLayerResult<Users> re = ur.LoginUser(model);

                if (re.Errors.Count > 0)
                {
                    if (re.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "http://Home/Activate/123-123-123";
                    }

                    re.Errors.ForEach(x => ModelState.AddModelError("", x.Message));



                    return View(model);
                }

                Session["User"] = re.Result;
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                UserRepository ur = new UserRepository();
                RepositoryLayerResult<Users> re = ur.RegisterUser(model);

                if (re.Errors.Count > 0)
                {
                    re.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                return RedirectToAction("RegisterOk");
            }

            return View(model);
        }

        public ActionResult ActiveteUser()
        {
            return View();
        }

        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            UserRepository er = new UserRepository();
            RepositoryLayerResult<Users> user = er.ActivateUser(id);

            if (user.Errors.Count > 0)
            {
                TempData["errors"] = user.Errors;
                return RedirectToAction("UserActivateCancel");
            }

            return RedirectToAction("UserActivateOk");
        }

        public ActionResult UserActivateOk()
        {

            return View();
        }

        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;

            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }

            return View(errors);
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }
        #endregion

        #region User Profile

        public ActionResult ShowProfile()
        {
            Users currentUser = Session["User"] as Users;
            if (currentUser == null)
            {
                return RedirectToAction("Login");
            }
            UserRepository ur = new UserRepository();
            RepositoryLayerResult<Users> re = ur.GetUserById(currentUser.Id);
            
            return View(re.Result);
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditProfile(Users model)
        {
            return View();
        }

        public ActionResult DeleteProfile()
        {
            return View();
        }

        #endregion
    }
}