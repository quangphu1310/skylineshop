using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult register()
        {
            return View();
        }
        public ActionResult LoginAction(string username, string password)
        {

            int user = db.User.Count(u => u.username.ToLower() == username.ToLower() && u.password == password && u.id_role == 2);
            int admin = db.User.Count(u => u.username.ToLower() == username.ToLower() && u.password == password && u.id_role == 1);
            if (user == 1)
            {
                User u = db.User.Where(us => us.username.ToLower() == username.ToLower() && us.password == password && us.id_role == 2).FirstOrDefault();
                Session["user"] = username;
                TempData["iduser"] = u;
                return Redirect("/Shop");
            }
            if (admin == 1)
            {
                Session["admin"] = username;
                return Redirect("/Admin/Home");
            }
            TempData["Error"] = "Tài khoản không chính xác!";
            return RedirectToAction("Login");
        }

        public ActionResult registerAction(string username, string email, string phone, string password, string cpassword)
        {
            if (ModelState.IsValid)
            {
                var check = db.User.FirstOrDefault(u => u.username == username || u.email == email || u.phone == phone);
                if (check == null)
                {
                    if (password == cpassword)
                    {
                        if(phone.Length == 10)
                        {
                            User u = new User();
                            u.id_role = 2;
                            u.username = username;
                            u.email = email;
                            u.phone = phone;
                            u.password = password;

                            db.User.Add(u);
                            db.SaveChanges();

                            Session["user"] = username;
                            return Redirect("/shop");

                        }
                        TempData["errorPass"] = "Vui lòng nhập đúng định dạng số điện thoại!";
                    }
                    else
                    {
                        TempData["errorPass"] = "Xác thực mật khẩu không chính xác!";
                    }
                }
                else
                {
                    TempData["errorCheck"] = "Thông tin người dùng đã tồn tại!";
                }

            }
            return RedirectToAction("register");
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

        public ActionResult profile(string type)
        {

            //status: 1 là tổng quan, 2 là đổi mật khẩu
            var status = TempData["status"] as string;
            ViewBag.status = status;
            
            if (status == null)
            {
                ViewBag.status = "1";
            }
            if (type != null)
            {
                ViewBag.status = "3";
                ViewBag.type = type;
            }

            //profile
            var username = Session["user"] as string;
            var user = db.User.FirstOrDefault(u => u.username == username);
            ViewBag.user = user;

            //order

            List<Order> o = new List<Order>();
            if (type == null || type == "all")
            {
                o = db.Order.Where(x => x.id_cust == user.id_user).OrderByDescending(x => x.id_order).ToList();
            }
            else 
            {
                o = db.Order.Where(x => x.id_cust == user.id_user && x.payment_status == type).OrderByDescending(x=> x.id_order).ToList();
            }
            if (o != null)
            {
                List<int> listID = new List<int>();
                for (int i = 0; i < o.Count; i++)
                {
                    listID.Add(o[i].id_order);
                }

                ViewBag.listID = listID;
                ViewBag.o = o;

            }
            return View(o);
        }


        public ActionResult changePass(string oldpass, string newpass, string cfnewpass, string id)
        {
            int iduser = int.Parse(id);
            var user = db.User.FirstOrDefault(x => x.id_user == iduser);
            if (oldpass == user.password)
            {
                if (newpass == cfnewpass)
                {
                    user.password = newpass;
                    TempData["success"] = "Thay đổi mật khẩu thành công";
                    db.SaveChanges();
                }
                else
                    TempData["error"] = "Mật khẩu không trùng khớp";
            }
            else
                TempData["error"] = "Mật khẩu cũ không chính xác";
            TempData["status"] = "2";
            return RedirectToAction("profile");
        }
        //public ActionResult updateProfile(string avatar, string email, string username, string phone, int id)
        //{
        //    var user = db.User.FirstOrDefault(u => u.id_user == id);
        //    if (ModelState.IsValid)
        //    {
        //        var check = db.User.FirstOrDefault(u => (u.username == username || u.email == email || u.phone == phone) && u.id_user != user.id_user);
        //        if (check == null)
        //        {
        //            user.email = email;
        //            user.phone = phone;
        //            user.username = username;
        //            if (avatar != "")
        //            {
        //                user.avatar_user = avatar;
        //            }


        //            db.SaveChanges();
        //            Session["user"] = username;
        //        }
        //        else
        //            TempData["errorUpdate"] = "Thông tin đã được sử dụng, vui lòng đổi dữ liệu!";
        //    }

        //    return RedirectToAction("profile");
        //}
        public ActionResult updateProfile(HttpPostedFileBase avatar, string email, string username, string phone, int id)
        {
            var user = db.User.FirstOrDefault(u => u.id_user == id);
            if (ModelState.IsValid)
            {
                var check = db.User.FirstOrDefault(u => (u.username == username || u.email == email || u.phone == phone) && u.id_user != user.id_user);
                if (check == null)
                {
                    user.email = email;
                    user.phone = phone;
                    user.username = username;

                    if (avatar != null && avatar.ContentLength > 0) // Kiểm tra avatar có dữ liệu
                    {
                        string fileName = Path.GetFileName(avatar.FileName);
                        string path = Path.Combine(Server.MapPath("~/Assets/img/account/"), fileName);
                        avatar.SaveAs(path);

                        user.avatar_user = fileName;
                    }

                    db.SaveChanges();
                    Session["user"] = username;
                }
                else
                {
                    TempData["errorUpdate"] = "Thông tin đã được sử dụng, vui lòng đổi dữ liệu!";
                }
            }

            return RedirectToAction("profile");
        }
        public ActionResult cancel(int id)
        {
            var order = db.Order.Where(x => x.id_order == id).FirstOrDefault();
            order.payment_status = "Đã hủy";
            db.SaveChanges();
            TempData["status"] = "3";
            return RedirectToAction("profile");
        }
        public ActionResult complete(int id)
        {
            var order = db.Order.Where(x => x.id_order == id).FirstOrDefault();
            order.payment_status = "Đã hoàn thành";
            db.SaveChanges();
            TempData["status"] = "3";
            return RedirectToAction("profile");
        }
    }
}