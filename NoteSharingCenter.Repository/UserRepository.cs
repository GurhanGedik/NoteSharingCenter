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
    public class UserRepository
    {
        private Repository<Users> ur = new Repository<Users>();        

        public RepositoryLayerResult<Users> RegisterUser(RegisterViewModel data)
        {
            Users user = ur.Find(x => x.Username == data.Username || x.Email == data.EMail);
            RepositoryLayerResult<Users> layerResult = new RepositoryLayerResult<Users>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.AddError(ErrorMessageCode.UsernameAlreadyExists, "This username already exists!");
                }
                if (user.Email == data.EMail)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Email already exists!");
                }
            }
            else
            {
                int Result = ur.Insert(new Users()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    ProfileImageFilename= "avatar.png",
                    Password = data.Password,
                    ActiveteGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false
                });

                if (Result > 0)
                {
                    layerResult.Result = ur.Find(x => x.Email == data.EMail && x.Username == data.Username);
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
            layerResult.Result = ur.Find(x => x.Username == data.Username && x.Password == data.Password);

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
            layerResult.Result = ur.Find(x => x.ActiveteGuid == id);

            if (layerResult.Result != null)
            {
                if (layerResult.Result.IsActive)
                {
                    layerResult.AddError(ErrorMessageCode.UserAlreadyActive, "The user has already been activated.");
                    return layerResult;
                }

                layerResult.Result.IsActive = true;
                ur.Update(layerResult.Result);
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
            layerResult.Result = ur.Find(x => x.Id == id);
            if (layerResult.Result==null)
            {
                layerResult.AddError(ErrorMessageCode.UserNotFound, "User not found.");
            }

            return layerResult;
        }
    }
}
