using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteSharingCenter.Entity.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("Username"), Required(ErrorMessage = "{0} field cannot be empty."), StringLength(30, ErrorMessage = "{0} can be up to {1} characters.")]
        public string Username { get; set; }
        [DisplayName("Password"), Required(ErrorMessage = "{0} field cannot be empty."), DataType(DataType.Password), StringLength(30, ErrorMessage = "{0} can be up to {1} characters.")]
        public string Password { get; set; }
    }
}