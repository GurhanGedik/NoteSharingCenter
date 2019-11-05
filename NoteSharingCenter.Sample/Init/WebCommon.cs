using NoteSharing.Common;
using NoteSharingCenter.Entity;
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
            if (HttpContext.Current.Session["User"] != null)
            {
                Users user = HttpContext.Current.Session["User"] as Users;
                return user.Username;
            }
            return "System";
        }
    }
}