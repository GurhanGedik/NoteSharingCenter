using NoteSharingCenter.Entity;
using NoteSharingCenter.Entity.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteSharingCenter.Repository
{
    public class EvernoteRepository
    {
        private Repository<EvernoteUser> er = new Repository<EvernoteUser>();

        public RepositoryLayerResult<EvernoteUser> RegisterUser(RegisterViewModel data)
        {
            EvernoteUser user = er.Find(x => x.Username == data.Username || x.Email == data.EMail);
            RepositoryLayerResult<EvernoteUser> layerResult = new RepositoryLayerResult<EvernoteUser>();
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.Errors.Add("This username already exists!");
                }
                if (user.Email == data.EMail)
                {
                    layerResult.Errors.Add("Email already exists!");
                }
            }
            else
            {
                int Result = er.Insert(new EvernoteUser()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    ActiveteGuid=Guid.NewGuid(),
                    IsActive =false,
                    IsAdmin=false
                });

                if (Result > 0)
                {
                    layerResult.Result = er.Find(x => x.Email == data.EMail && x.Username == data.Username);
                    //layerResult.Result.ActiveteGuid
                }
            }
            return layerResult;
        }

        public RepositoryLayerResult<EvernoteUser> LoginUser(LoginViewModel data)
        {
            RepositoryLayerResult<EvernoteUser> layerResult = new RepositoryLayerResult<EvernoteUser>();
            layerResult.Result = er.Find(x => x.Username == data.Username && x.Password == data.Password);
            
            if (layerResult.Result != null)
            {
                if (!layerResult.Result.IsActive)
                {
                    layerResult.Errors.Add("User is not activated. Please check your email address.");
                }
            }
            else
            {
                layerResult.Errors.Add("Username or password incorrect.");
            }

            return layerResult;
        }
    }
}
