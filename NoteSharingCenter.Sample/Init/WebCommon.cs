using NoteSharing.Common;
using NoteSharingCenter.Entity;
using NoteSharingCenter.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteSharingCenter.Sample.Init
{
    public class WebCommon : ICommon
    {
        public string GetUsername()
        {
            Users user = MySession.CurrentUser;
            if (user != null)
            {
                return user.Username;
            }
            else
                return "System";
        }
    }
}