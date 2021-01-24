using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vnnews.Models;

namespace vnnews.Controllers
{
    public class ViewController : Controller
    {
        vnnewsEntities db = new vnnewsEntities();
        // GET: View
        public PartialViewResult Menu()
        {
            return PartialView();
        }
        public PartialViewResult SignUp()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult SignUp(ViewSignUp signUp)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(new User
                {
                    user_name = signUp.DisplayName,
                    user_email = signUp.Email,
                    user_pass = signUp.Password,
                    user_bin = false,
                    user_active = true,
                    role_id = 1,
                    user_datecreate = DateTime.Now,
                    user_datelogin = DateTime.Now
                });
                db.SaveChanges();

                User user = db.Users.SingleOrDefault(t => t.user_email == signUp.Email && t.user_pass == signUp.Password && t.user_bin == false && t.user_active == true);
                HttpCookie cookie = new HttpCookie("user_cookie", user.user_id.ToString());
                cookie.Expires.AddDays(10);
                Response.SetCookie(cookie);
            }
            return Redirect(signUp.ReturnUrl);
        }

        public PartialViewResult SignIn()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult SignIn(ViewSignIn signIn)
        {
            User user = db.Users.SingleOrDefault(t => t.user_email == signIn.Email && t.user_pass == signIn.Password && t.user_bin == false && t.user_active == true);
            if(user != null)
            {
                HttpCookie cookie = new HttpCookie("user_cookie", user.user_id.ToString());
                cookie.Expires.AddDays(10);
                Response.SetCookie(cookie);
            }
            return Redirect(signIn.ReturnUrl);
        }
    }
}