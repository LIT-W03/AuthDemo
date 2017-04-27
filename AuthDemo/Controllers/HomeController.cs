using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AuthDemo.Data;
using AuthDemo.Models;

namespace AuthDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user, string password)
        {
            UserAuthDb db = new UserAuthDb(Properties.Settings.Default.ConStr);
            db.AddUser(user, password);
            return Redirect("/");
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/home/secret");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            UserAuthDb db = new UserAuthDb(Properties.Settings.Default.ConStr);
            User user = db.Login(email, password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(email, true);
                return Redirect("/home/secret");
            }
            else
            {
                return Redirect("/home/login");
            }
        }

        [Authorize]
        public ActionResult Secret()
        {
            UserAuthDb db = new UserAuthDb(Properties.Settings.Default.ConStr);
            User user = db.GetByEmail(User.Identity.Name);
            return View(new SecretPageViewModel {User = user});
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}