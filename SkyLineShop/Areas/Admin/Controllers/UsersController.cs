using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SkyLineShop.Models;

namespace SkyLineShop.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private skyshopEntities db = new skyshopEntities();

        // GET: Admin/Users
        public ActionResult Index()
        {
            var user = db.User.Where(x => x.id_role == 2).Include(u => u.Role);
            return View(user.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
