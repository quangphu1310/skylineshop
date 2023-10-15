using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SkyLineShop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["admin"] != null)
                return View();
            else
                return RedirectToAction("Login", "Account", new { area = "" });

        }
        public ActionResult Logout()
        {
            //remove session
            Session.Remove("admin");
            //xoa session trong au
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", new { area = "" });

        }
    }
}