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
using System.Xml.Linq;
using SkyLineShop.Models;

namespace SkyLineShop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private skyshopEntities db = new skyshopEntities();

        // GET: Admin/Products
        public ActionResult Index()

        {
            
            XDocument doc = XDocument.Load(Server.MapPath("~/App_Data/product.xml"));
            ViewBag.ProductXML = doc;
            return View();
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string filePath = Server.MapPath("~/App_Data/product.xml");
            XDocument doc = XDocument.Load(filePath);

            var product = doc.Descendants("Product")
                     .FirstOrDefault(p => (int)p.Attribute("id_product") == id);

            if (product != null)
            {
                ViewBag.ProductInfo = product;
            }
            else
            {
                ViewBag.ProductInfo = null; // Không tìm thấy sản phẩm
            }

            return View();
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            // Đọc dữ liệu từ tệp XML brand.xml và category.xml
            IEnumerable<SelectListItem> brands = GetSelectListItemsFromXML("brand.xml", "brand");
            IEnumerable<SelectListItem> categories = GetSelectListItemsFromXML("category.xml", "cate");

            // Tạo SelectList từ danh sách các SelectListItem đã đọc từ tệp XML
            ViewBag.id_brand = new SelectList(brands, "Value", "Text");
            ViewBag.id_cate = new SelectList(categories, "Value", "Text");

            return View();
        }

        // Phương thức để đọc dữ liệu từ tệp XML và chuyển đổi thành danh sách SelectListItem
        private List<SelectListItem> GetSelectListItemsFromXML(string filePath, string elementName)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            // Đường dẫn tới tệp XML
            string xmlFilePath = Server.MapPath("~/App_Data/" + filePath);

            XDocument doc = XDocument.Load(xmlFilePath);

            // Sử dụng LINQ to XML để đọc dữ liệu từ tệp XML và tạo danh sách SelectListItem
            foreach (XElement element in doc.Descendants(elementName))
            {
                string value = element.Attribute("id_" + elementName).Value;
                string text = element.Attribute(elementName + "_name").Value;

                // Tạo một SelectListItem và thêm vào danh sách
                SelectListItem item = new SelectListItem
                {
                    Value = value,
                    Text = text
                };

                selectListItems.Add(item);
            }

            return selectListItems;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_product,product_name,price,desc,id_cate,id_brand")] Product product,
        HttpPostedFileBase name1, HttpPostedFileBase name2, HttpPostedFileBase name3)
        {

            // Phương thức để thêm sản phẩm mới vào tệp XML

            string xmlFilePath = Server.MapPath("~/App_Data/product.xml");

            XDocument doc;
            try
            {
                doc = XDocument.Load(xmlFilePath);
            }
            catch (Exception)
            {
                doc = new XDocument(new XElement("quangphu"));
            }

            if (name1 != null && name2 != null && name3 != null)
            {
                string filename1 = Path.GetFileName(name1.FileName);
                string path1 = Path.Combine(Server.MapPath("~/Assets/img/product/"), filename1);
                name1.SaveAs(path1);
                string filename2 = Path.GetFileName(name2.FileName);
                string path2 = Path.Combine(Server.MapPath("~/Assets/img/product/"), filename2);
                name2.SaveAs(path2);
                string filename3 = Path.GetFileName(name3.FileName);
                string path3 = Path.Combine(Server.MapPath("~/Assets/img/product/"), filename3);
                name3.SaveAs(path3);

                Product_Image p1 = new Product_Image();
                p1.id_product = product.id_product;
                p1.image = filename1;
                db.Product_Image.Add(p1);

                Product_Image p2 = new Product_Image();
                p2.id_product = product.id_product;
                p2.image = filename2;
                db.Product_Image.Add(p2);

                Product_Image p3 = new Product_Image();
                p3.id_product = product.id_product;
                p3.image = filename3;
                db.Product_Image.Add(p3);

                db.Product.Add(product);

                db.SaveChanges();

                var lastProduct = doc.Descendants("Product").LastOrDefault();
                var id = int.Parse(lastProduct.Attribute("id_product").Value);

                XElement newProduct = new XElement("Product",
                    new XAttribute("id_product", p1.id_product),
                    new XAttribute("product_name", product.product_name),
                    new XAttribute("price", product.price),
                    new XAttribute("description", product.desc),

                    new XElement("Brand",
                    new XAttribute("id_brand", product.id_brand),
                    new XAttribute("brand_name", db.Brand.Where(x => x.id_brand == product.id_brand).FirstOrDefault().brand_name)

                ),
                new XElement("Category",
                    new XAttribute("id_cate", product.id_cate),
                     new XAttribute("category_name", db.Category.Where(x => x.id_cate == product.id_cate).FirstOrDefault().cate_name)
                ),
                new XElement("Image",
                    new XAttribute("image1", filename1),
                    new XAttribute("image2", filename2),
                    new XAttribute("image3", filename3)
                )
                );
                doc.Element("quangphu").Add(newProduct);
                doc.Save(xmlFilePath);


                
                return RedirectToAction("Index");
            }
            else
                return View();
        }


        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string filePath = Server.MapPath("~/App_Data/product.xml");
            XDocument doc = XDocument.Load(filePath);

            var productXElement = doc.Descendants("Product")
             .FirstOrDefault(p => (int)p.Attribute("id_product") == id);

            if (productXElement == null)
            {
                return HttpNotFound();
            }

            // Tạo một đối tượng Product từ XElement
            var product = new Product
            {
                id_product = (int)productXElement.Attribute("id_product"),
                product_name = productXElement.Attribute("product_name").Value,
                price = decimal.Parse(productXElement.Attribute("price").Value),
                desc = productXElement.Attribute("description").Value,
                id_brand = (int)productXElement.Element("Brand").Attribute("id_brand"),
                id_cate = (int)productXElement.Element("Category").Attribute("id_cate")

            };

            ViewBag.image1 = productXElement.Element("Image").Attribute("image1").Value;
            ViewBag.image2 = productXElement.Element("Image").Attribute("image2").Value;
            ViewBag.image3 = productXElement.Element("Image").Attribute("image3").Value;
            // Đọc dữ liệu từ tệp XML brand.xml và category.xml
            IEnumerable<SelectListItem> brands = GetSelectListItemsFromXML("brand.xml", "brand");
            IEnumerable<SelectListItem> categories = GetSelectListItemsFromXML("category.xml", "cate");

            // Tạo SelectList từ danh sách các SelectListItem đã đọc từ tệp XML
            ViewBag.id_brand = new SelectList(brands, "Value", "Text");
            ViewBag.id_cate = new SelectList(categories, "Value", "Text");

            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "id_product,product_name,price,desc,id_cate,id_brand")] Product product,
        HttpPostedFileBase name1, HttpPostedFileBase name2, HttpPostedFileBase name3)
        {

            // Phương thức để thêm sản phẩm mới vào tệp XML

            string xmlFilePath = Server.MapPath("~/App_Data/product.xml");
            XDocument doc;

            try
            {
                doc = XDocument.Load(xmlFilePath);
            }
            catch (Exception)
            {
                doc = new XDocument(new XElement("quangphu"));
            }
            var productXElement = doc.Descendants("Product")
             .FirstOrDefault(p => (int)p.Attribute("id_product") == product.id_product);

            string img1 = productXElement.Element("Image").Attribute("image1").Value; ;
            string img2 = productXElement.Element("Image").Attribute("image2").Value;
            string img3 = productXElement.Element("Image").Attribute("image3").Value;

            if (name1 != null && name2 != null && name3 != null)
            {
                string filename1 = Path.GetFileName(name1.FileName);
                string path1 = Path.Combine(Server.MapPath("~/Assets/img/product/"), filename1);
                name1.SaveAs(path1);
                string filename2 = Path.GetFileName(name2.FileName);
                string path2 = Path.Combine(Server.MapPath("~/Assets/img/product/"), filename2);
                name2.SaveAs(path2);
                string filename3 = Path.GetFileName(name3.FileName);
                string path3 = Path.Combine(Server.MapPath("~/Assets/img/product/"), filename3);
                name3.SaveAs(path3);

                img1 = filename1;
                img2 = filename2;
                img3 = filename3;
            }

            productXElement.SetAttributeValue("product_name", product.product_name);
            productXElement.SetAttributeValue("price", product.price);
            productXElement.SetAttributeValue("description", product.desc);
            productXElement.Element("Brand").SetAttributeValue("id_brand", product.id_brand);
            productXElement.Element("Brand").SetAttributeValue("brand_name", db.Brand.Where(x => x.id_brand == product.id_brand).FirstOrDefault().brand_name);
            productXElement.Element("Category").SetAttributeValue("id_cate", product.id_cate);
            productXElement.Element("Category").SetAttributeValue("category_name", db.Category.Where(x => x.id_cate == product.id_cate).FirstOrDefault().cate_name);
            productXElement.Element("Image").SetAttributeValue("image1", img1);
            productXElement.Element("Image").SetAttributeValue("image2", img2);
            productXElement.Element("Image").SetAttributeValue("image3", img3);

            doc.Save(xmlFilePath);

            var productDB = db.Product.FirstOrDefault(x => x.id_product == product.id_product);
            productDB.product_name = product.product_name;
            productDB.price = product.price;
            productDB.desc = product.desc;
            productDB.id_cate = product.id_cate;
            productDB.id_brand = product.id_brand;
            db.SaveChanges();

            var p1 = db.Product_Image.Where(x => x.id_product == product.id_product).ToList();
            if (p1.Count >= 3)
            {
                p1[0].image = img1;
                p1[1].image = img2;
                p1[2].image = img3;

                db.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string filePath = Server.MapPath("~/App_Data/product.xml");
            XDocument doc = XDocument.Load(filePath);

            var productX = doc.Descendants("Product")
                     .FirstOrDefault(p => (int)p.Attribute("id_product") == id);

            if (productX != null)
            {
                ViewBag.ProductInfo = productX;
            }
            else
            {
                ViewBag.ProductInfo = null; // Không tìm thấy sản phẩm
            }
            return View();
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            string xmlFilePath = Server.MapPath("~/App_Data/product.xml");
            XDocument doc;

            try
            {
                doc = XDocument.Load(xmlFilePath);
            }
            catch (Exception)
            {
                doc = new XDocument(new XElement("quangphu"));
            }
            var productXElement = doc.Descendants("Product")
             .FirstOrDefault(p => (int)p.Attribute("id_product") == id);

            productXElement.Remove();
            doc.Save(xmlFilePath);


            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public void ConvertXMl()
        {
            var products = db.Product
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Product_Image)
                .Include(p => p.Evaluation)
                .Include(p => p.Order_Detail)
                .Include(p => p.DetailSizePd)
                .ToList();
            var brands = db.Brand.ToList();
            var cates = db.Category.ToList();

            // Tạo XmlDocument mới với khai báo XML version và encoding
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            // Tạo phần tử gốc <quangphu>
            XmlElement root = xmlDoc.CreateElement("quangphu");
            xmlDoc.AppendChild(root);

            foreach (var product in products)
            {
                // Tạo phần tử <Product> cho mỗi sản phẩm
                XmlElement productElement = xmlDoc.CreateElement("Product");

                // Tạo các phần tử con của <Product> và thiết lập giá trị từ model Product
                productElement.SetAttribute("id_product", product.id_product.ToString());
                productElement.SetAttribute("product_name", product.product_name);
                productElement.SetAttribute("description", product.desc);
                productElement.SetAttribute("price", product.price.ToString());

                // Thêm Brand thông qua một phần tử con
                XmlElement brandElement = xmlDoc.CreateElement("Brand");
                brandElement.SetAttribute("id_brand", product.Brand.id_brand.ToString());
                brandElement.SetAttribute("brand_name", product.Brand.brand_name);
                productElement.AppendChild(brandElement);

                // Thêm Category thông qua một phần tử con
                XmlElement cateElement = xmlDoc.CreateElement("Category");
                cateElement.SetAttribute("id_cate", product.Category.id_cate.ToString());
                cateElement.SetAttribute("category_name", product.Category.cate_name);
                productElement.AppendChild(cateElement);

                // Thêm Image thông qua một phần tử con
                XmlElement imageElement = xmlDoc.CreateElement("Image");
                imageElement.SetAttribute("image1", product.Product_Image[0].image);
                imageElement.SetAttribute("image2", product.Product_Image[1].image);
                imageElement.SetAttribute("image3", product.Product_Image[2].image);
                productElement.AppendChild(imageElement);

                root.AppendChild(productElement);
            }

            // Lưu XmlDocument vào tệp XML
            string filePath = Server.MapPath("~/App_Data/product.xml"); // Đường dẫn tới tệp product.xml

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                xmlDoc.Save(streamWriter);
            }

            // Brand
            // Tạo XmlDocument mới với khai báo XML version và encoding
            XmlDocument xmlDocBrand = new XmlDocument();
            XmlDeclaration xmlDeclarationBrand = xmlDocBrand.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocBrand.AppendChild(xmlDeclarationBrand);

            XmlElement rootBrand = xmlDocBrand.CreateElement("BrandRoot");
            xmlDocBrand.AppendChild(rootBrand);

            foreach (var brand in brands)
            {
                XmlElement brandElements = xmlDocBrand.CreateElement("brand");

                brandElements.SetAttribute("id_brand", brand.id_brand.ToString());
                brandElements.SetAttribute("brand_name", brand.brand_name.ToString());


                rootBrand.AppendChild(brandElements);
            }

            // Lưu XmlDocument vào tệp XML
            string filePathBrand = Server.MapPath("~/App_Data/brand.xml"); // Đường dẫn tới tệp product.xml

            using (StreamWriter streamWriter2 = new StreamWriter(filePathBrand))
            {
                xmlDocBrand.Save(streamWriter2);
            }



            //Category
            // Tạo XmlDocument mới với khai báo XML version và encoding
            XmlDocument xmlDocCate = new XmlDocument();
            XmlDeclaration xmlDeclarationCate = xmlDocCate.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocCate.AppendChild(xmlDeclarationCate);

            XmlElement rootCate = xmlDocCate.CreateElement("BrandRoot");
            xmlDocCate.AppendChild(rootCate);

            foreach (var cate in cates)
            {
                XmlElement cateElements = xmlDocCate.CreateElement("cate");

                cateElements.SetAttribute("id_cate", cate.id_cate.ToString());
                cateElements.SetAttribute("cate_name", cate.cate_name.ToString());


                rootCate.AppendChild(cateElements);
            }

            string filePathCate = Server.MapPath("~/App_Data/category.xml");

            using (StreamWriter streamWriter3 = new StreamWriter(filePathCate))
            {
                xmlDocCate.Save(streamWriter3);
            }
        }

        public ActionResult convert()
        {
            ConvertXMl();
            return RedirectToAction("Index");
        }
    }
}
