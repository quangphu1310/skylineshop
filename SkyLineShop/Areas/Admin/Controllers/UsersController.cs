using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SkyLineShop.Models;

namespace SkyLineShop.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private skyshop2Entities db = new skyshop2Entities();

        // GET: Admin/Users
        public ActionResult Index()
        {
            var user = db.Users.Where(x => x.id_role == 2).Include(u => u.Role);
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
        public ActionResult convertXML()
        {
            var users = db.Users.Where(x=>x.id_role == 2).ToList();

            // Tạo XmlDocument mới với khai báo XML version và encoding
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            // Tạo phần tử gốc <users>
            XmlElement root = xmlDoc.CreateElement("users");
            xmlDoc.AppendChild(root);

            foreach (var user in users)
            {
                // Tạo phần tử <User> cho mỗi sản phẩm
                XmlElement userElement = xmlDoc.CreateElement("User");

                // Tạo các phần tử con của <User> và thiết lập giá trị từ model User
                userElement.SetAttribute("id_user", user.id_user.ToString());
                userElement.SetAttribute("username", user.username);
                userElement.SetAttribute("email", user.email);
                userElement.SetAttribute("phone", user.phone.ToString());

                root.AppendChild(userElement);
            }

            // Lưu XmlDocument vào tệp XML
            string filePath = Server.MapPath("~/App_Data/user.xml"); // Đường dẫn tới tệp user.xml

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
