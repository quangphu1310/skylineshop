using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SkyLineShop.Models;

namespace SkyLineShop.Controllers
{

    public class AccountController : Controller
    {
        skyshopEntities db = new skyshopEntities();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginAction(string username, string password)
        {

            int user = db.User.Count(u => u.username.ToLower() == username.ToLower() && u.password == password && u.id_role == 2);
            int admin = db.User.Count(u => u.username.ToLower() == username.ToLower() && u.password == password && u.id_role == 1);
            if (user == 1 ) 
            {
                User u = db.User.Where(us => us.username.ToLower() == username.ToLower() && us.password == password && us.id_role == 2).FirstOrDefault();
                Session["user"] = username;
                TempData["iduser"] = u;
                return Redirect("/Shop");
            }
            if(admin == 1)
            {
                Session["admin"] = username;
                return Redirect("/Admin/Home");
            }
            TempData["Error"] = "Tài khoản không chính xác!";
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            //remove session
            Session.Remove("user");
            Session["CartSession"] = null;
            //xoa session trong au
            FormsAuthentication.SignOut();
            return RedirectToAction("/Login");

        }
    }
}