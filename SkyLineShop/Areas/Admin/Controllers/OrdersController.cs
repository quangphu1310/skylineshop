using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkyLineShop.Models;
using System.Web.Mvc;
using System.IO;
using System.Xml;

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
                list = db.Order.OrderByDescending(x => x.id_order).ToList();
            }
            else
            {
                list = db.Order.Where(x => x.payment_status.Equals(SelectedCategory)).OrderByDescending(x => x.id_order).ToList();
            }
            var categories = new List<SelectListItem>
            {
                new SelectListItem { Value = "Chờ xác nhận", Text = "Chờ xác nhận" },
                new SelectListItem { Value = "Đã xác nhận", Text = "Đã xác nhận" },
                new SelectListItem { Value = "Vận chuyển", Text = "Vận chuyển" },
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
        public ActionResult ship(int id)
        {
            var order = db.Order.Where(x => x.id_order == id).FirstOrDefault();
            order.payment_status = "Vận chuyển";
            db.SaveChanges();
            return RedirectToAction("Index");
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
        public ActionResult convertXML()
        {
            var orders = db.Order.ToList();

            // Tạo XmlDocument mới với khai báo XML version và encoding
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            // Tạo phần tử gốc <orders>
            XmlElement root = xmlDoc.CreateElement("orders");
            xmlDoc.AppendChild(root);

            foreach (var order in orders)
            {
                // Tạo phần tử <Order> cho mỗi sản phẩm
                XmlElement orderElement = xmlDoc.CreateElement("Order");

                // Tạo các phần tử con của <Order> và thiết lập giá trị từ model Order
                orderElement.SetAttribute("id_order", order.id_order.ToString());
                orderElement.SetAttribute("address", order.address);
                orderElement.SetAttribute("username", order.User.username);
                orderElement.SetAttribute("customer", order.name);
                orderElement.SetAttribute("status_payment", order.payment_status);

                XmlElement noteElement = xmlDoc.CreateElement("note");
                noteElement.InnerText = order.note;
                orderElement.AppendChild(noteElement);

                root.AppendChild(orderElement);
            }

            // Lưu XmlDocument vào tệp XML
            string filePath = Server.MapPath("~/App_Data/order.xml"); // Đường dẫn tới tệp order.xml

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                xmlDoc.Save(streamWriter);
            }

            // Hiển thị nội dung file XML
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/xml";
            string xmlContent = System.IO.File.ReadAllText(filePath);
            ViewBag.XMLContent = xmlContent;
            return Content(xmlContent);
        }

    }
}