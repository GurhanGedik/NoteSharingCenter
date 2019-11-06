using NoteSharingCenter.Entity;
using NoteSharingCenter.Entity.Messages;
using NoteSharingCenter.Entity.ValueObjects;
using NoteSharingCenter.Repository;
using NoteSharingCenter.Sample.Models;
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

                OkViewModel okModel = new OkViewModel()
                {
                    Title = "Registration Successful",
                    RedirectinUrl = "/Home/Login",
                };
                okModel.Items.Add("Please activate your account by clicking on the activation link we sent to your email address. If you don't activate your account, you won't be able to add notes and make ratings.");

                return View("Ok", okModel);
            }

            return View(model);
        }

        public ActionResult ActiveteUser()
        {
            return View();
        }

        public ActionResult UserActivate(Guid id)
        {
            UserRepository er = new UserRepository();
            RepositoryLayerResult<Users> user = er.ActivateUser(id);

            if (user.Errors.Count > 0)
            {
                ErrorViewModel erModel = new ErrorViewModel()
                {
                    Title = "Account not valid.",
                    Items = user.Errors
                };
                return View("Error", erModel);
            }

            OkViewModel okModel = new OkViewModel()
            {
                Title = "Account Activation Successful",
                RedirectinUrl = "/Home/Login"
            };

            return View("Ok", okModel);
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

            if (re.Errors.Count > 0)
            {
                ErrorViewModel erModel = new ErrorViewModel()
                {
                    Title = "An error occurred",
                    Items = re.Errors,
                    RedirectingTimeout=15
                };
                return View("Error", erModel);
            }
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