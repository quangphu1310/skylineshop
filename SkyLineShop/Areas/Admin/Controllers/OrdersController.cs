using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyLineShop.Models;
using System.Web.Mvc;

namespace SkyLineShop.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Admin/Orders
        private skyshopEntities db = new skyshopEntities();
        public ActionResult Index(String SelectedCategory)
        {
            List<Order> list = null;
            if (SelectedCategory == null || SelectedCategory == "")
            {
                list = db.Order.ToList();
            }
            else
            {
                list = db.Order.Where(x => x.payment_status.Equals(SelectedCategory)).ToList();
            }
            var categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "Đang chờ xác nhận", Text = "Đang chờ xác nhận" },
                new SelectListItem { Value = "Đã xác nhận", Text = "Đã xác nhận" },
                new SelectListItem { Value = "Đã hủy", Text = "Đã hủy" },
                new SelectListItem { Value = "Đã hoàn thành", Text = "Đã hoàn thành" }
            };


            ViewBag.CategoryList = new SelectList(categories, "Value", "Text");
            return View(list);
        }
        public ActionResult Details(int id)
        {
            var list = db.Order_Detail.Where(e => e.id_order == id).ToList();
            var od = db.Order.Where(e => e.id_order == id).FirstOrDefault();
            ViewBag.od = od;
            return View(list);
        }
        public ActionResult confirm(int id)
        {
            var order = db.Order.Where(x => x.id_order == id).FirstOrDefault();
            order.payment_status = "Đã xác nhận";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult cancel(int id)
        {
            var order = db.Order.Where(x => x.id_order == id).FirstOrDefault();
            order.payment_status = "Đã hủy";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}