using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyLineShop.Models;

namespace SkyLineShop.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        skyshopEntities db = new skyshopEntities();
        public ActionResult Index()
        {
            var username = Session["user"] as string;
            var user = db.User.FirstOrDefault(u => u.username == username);
            if (username != null)
            {
                ViewBag.user = user;
            }
            var cart = Session["CartSession"];
            var list = (List<CartItem>)cart;
            return View(list);
        }
        public ActionResult Payment(string name,string address, string phone,string email,string note)
        {
            
            var username = Session["user"] as string;
            var user = db.User.FirstOrDefault(u=>u.username == username);
            var cart = Session["CartSession"];
            var list = (List<CartItem>)cart;
            var o = new Order();
            //var user = Session["user"];
            //o.id_cust = ;
            if (user != null) {
                if(phone.Length == 10)
                {
                    o.id_cust = user.id_user;
                    o.name = name;
                    o.address = address;
                    o.phone = phone;
                    o.email = email;
                    o.note = note;
                    o.date_create = DateTime.Now;
                    o.payment_status = "Chờ xác nhận";
                    db.Order.Add(o);
                    db.SaveChanges();
                    foreach (var item in list)
                    {
                        var od = new Order_Detail();
                        od.id_order = o.id_order;
                        od.id_product = item.Product.id_product;
                        od.quantity = item.Quantity;
                        od.size = item.Size;
                        od.total = (item.Product.price * item.Quantity);
                        db.Order_Detail.Add(od);
                    }
                    db.SaveChanges();
                    Session["CartSession"] = null;
                    TempData["PaymentSuccess"] = "check"; // Gửi biến flag để thông báo thanh toán thành công

                    return RedirectToAction("Index", "Home");
                }
                else
                    TempData["loginyet"] = "VUI LÒNG NHẬP ĐÚNG ĐỊNH DẠNG SỐ ĐIỆN THOẠI!";
                    return RedirectToAction("Index");
            }
            
                TempData["loginyet"] = "VUI LÒNG ĐĂNG NHẬP!";
               return RedirectToAction("Index");
            
            
        }
        public ActionResult PaymentSuccess()
        {
            return View();
        }
    }
}