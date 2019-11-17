using NoteSharing.Common;
using NoteSharingCenter.Entity;
using NoteSharingCenter.Entity.Messages;
using NoteSharingCenter.Entity.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class UserRepository : ManagerBase<Users>
    {
        public RepositoryLayerResult<Users> RegisterUser(RegisterViewModel data)
        {
            Users user = Find(x => x.Username == data.Username || x.Email == data.EMail);
            RepositoryLayerResult<Users> layerResult = new RepositoryLayerResult<Users>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "This username already exists!");
                }
                if (user.Email == data.EMail)
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "Email already exists!");
                }
            }
            else
            {
                int Result = base.Insert(new Users()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    ProfileImageFilename = "avatar.png",
                    Password = data.Password,
                    ActiveteGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });

                if (Result > 0)
                {
                    layerResult.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{layerResult.Result.ActiveteGuid}";
                    string body = $"<a href='{activateUri}' target='_blank'>Click here</a> to activate your account.";
                    MailHelper.SendMail(body, layerResult.Result.Email, "Account activation");
                }
            }
            return layerResult;
        }

        public RepositoryLayerResult<Users> LoginUser(LoginViewModel data)
        {
            RepositoryLayerResult<Users> layerResult = new RepositoryLayerResult<Users>();
            layerResult.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserIsNotActive, "User is not activated.");
                    layerResult.AddError(ErrorMessageCode.CheckYourEmail, "Please check your email address.");
                }
            }
            else
            {
                layerResult.AddError(ErrorMessageCode.UsernameOrPasswordWrong, "Username or password incorrect.");
            }

            return layerResult;
        }

        public RepositoryLayerResult<Users> ActivateUser(Guid id)
        {
            RepositoryLayerResult<Users> layerResult = new RepositoryLayerResult<Users>();
            layerResult.Result = Find(x => x.ActiveteGuid == id);

            if (layerResult.Result != null)
            {
                if (layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserAlreadyActive, "The user has already been activated.");
                    return layerResult;
                }

                layerResult.Result.IsActive = true;
                Update(layerResult.Result);
            }
            else
            {
                layerResult.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "No users to activate");
            }
            return layerResult;
        }

        public RepositoryLayerResult<Users> GetUserById(int id)
        {
            RepositoryLayerResult<Users> layerResult = new RepositoryLayerResult<Users>();
            layerResult.Result = Find(x => x.Id == id);
            if (layerResult.Result == null)
            {
                layerResult.AddError(ErrorMessageCode.UserNotFound, "User not found.");
            }

            return layerResult;
        }

        public RepositoryLayerResult<Users> UpdateProfile(Users data)
        {

            Users user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            RepositoryLayerResult<Users> layerResult = new RepositoryLayerResult<Users>();

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "Username registered.");
                }

                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "E-mail address already registered.");
                }

                return layerResult;
            }

            layerResult.Result = Find(x => x.Id == data.Id);
            layerResult.Result.Email = data.Email;
            layerResult.Result.Name = data.Name;
            layerResult.Result.Surname = data.Surname;
            layerResult.Result.AboutMe = data.AboutMe;
            layerResult.Result.Password = data.Password;
            layerResult.Result.Username = data.Username;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                layerResult.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(layerResult.Result) == 0)
            {
                layerResult.AddError(ErrorMessageCode.UserCouldNotUpdate, "Failed to update profile.");
            }

            return layerResult;
        }
        
        public new RepositoryLayerResult<Users> Insert(Users data)
        {
            Users user = Find(x => x.Username == data.Username || x.Email == data.Email);
            RepositoryLayerResult<Users> layerResult = new RepositoryLayerResult<Users>();
            layerResult.Result = data;
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "This username already exists!");
                }
                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "Email already exists!");
                }
            }
            else
            {
                layerResult.Result.ProfileImageFilename = "avatar.png";
                layerResult.Result.ActiveteGuid = Guid.NewGuid();


                if (base.Insert(layerResult.Result) == 0)
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "Failed to Add User.");
                }

            }
            return layerResult;
        }

        public new RepositoryLayerResult<Users> Update(Users data)
        {
            Users user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            RepositoryLayerResult<Users> layerResult = new RepositoryLayerResult<Users>();

            layerResult.Result = data;

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "Username registered.");
                }

                if (user.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "E-mail address already registered.");
                }

                return layerResult;
            }

            layerResult.Result = Find(x => x.Id == data.Id);
            layerResult.Result.Email = data.Email;
            layerResult.Result.Name = data.Name;
            layerResult.Result.Surname = data.Surname;
            layerResult.Result.AboutMe = data.AboutMe;
            layerResult.Result.Password = data.Password;
            layerResult.Result.Username = data.Username;
            layerResult.Result.IsActive = data.IsActive;
            layerResult.Result.IsAdmin = data.IsAdmin;


            if (base.Update(layerResult.Result) == 0)
            {
                layerResult.AddError(ErrorMessageCode.UserCouldNotUpdate, "Failed to update user.");
            }

            return layerResult;
        }

        public override int Delete(Users user)
        {
            NoteRepository nr = new NoteRepository();
            LikedRepository lr = new LikedRepository();
            CommentRepository cr = new CommentRepository();

            foreach (var note in user.Notes.ToList())
            {
                foreach (Liked like in note.Likes.ToList())
                {
                    lr.Delete(like);
                }

                foreach (Comment comment in note.Comments.ToList())
                {
                    cr.Delete(comment);
                }

                nr.Delete(note);
            }
            List<Liked> Likedd = lr.List(x => x.LikedUser.Id == user.Id);
            List<Comment> Comment = cr.List(x => x.Owner.Id == user.Id);
            foreach (var item in Likedd)
            {
                lr.Delete(item);
            }
            foreach (var item in Comment)
            {
                cr.Delete(item);
            }
            return base.Delete(user);
        }
    }
}
