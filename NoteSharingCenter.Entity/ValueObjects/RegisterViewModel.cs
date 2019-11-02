using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteSharingCenter.Entity.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Username"), Required(ErrorMessage = "{0} field cannot be empty."), StringLength(30,ErrorMessage = "{0} can be up to {1} characters.")]
        public string Username { get; set; }

        [DisplayName("Email"), Required(ErrorMessage = "{0} field cannot be empty."), StringLength(50, ErrorMessage = "{0} can be up to {1} characters."),EmailAddress(ErrorMessage = "  Enter a valid {0} address for the email field.")]
        public string EMail { get; set; }

        [DisplayName("Password"), Required(ErrorMessage = "{0} field cannot be empty."), DataType(DataType.Password), StringLength(30, ErrorMessage = "{0} can be up to {1} characters.")]
        public string Password { get; set; }

        [DisplayName("Password Repeat"), Required(ErrorMessage = "{0} field cannot be empty."), DataType(DataType.Password), StringLength(30, ErrorMessage = "{0} can be up to {1} characters."),Compare("Password", ErrorMessage = "{0} doesn't match {1}")]
        public string RePassword { get; set; }
    }
}