using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vnnews.Models
{
    public class ViewSignUp
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPass { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ViewSignIn
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}