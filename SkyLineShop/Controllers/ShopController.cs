using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyLineShop.Models;
using System.Text.RegularExpressions;

namespace SkyLineShop.Controllers
{
    public class ShopController : Controller
    {
        skyshopEntities db = new skyshopEntities();
        // GET: Shop
        public ActionResult Index(int? categoryID,int? brandID, string productName)
        {
            ViewBag.Category = db.Category.ToList();
            ViewBag.Brand = db.Brand.ToList();
            var query = from p in db.Product
                        join m in db.Product_Image on p.id_product equals m.id_product
                        where (categoryID == null || p.id_cate == categoryID)
                        && (brandID == null || p.id_brand == brandID)
                        && (p.product_name.ToLower().Contains(productName.ToLower()) || productName == null)
                        group m by p into g
                        select new ProductPro
                        {
                            ProductID = g.Key.id_product,
                            ProductName = g.Key.product_name,
                            ProductPrice = (decimal)g.Key.price,
                            ProductDesc = g.Key.desc,
                            IDbrand = (int)g.Key.id_brand,
                            IDcate = (int)g.Key.id_cate,
                            ProductImage = g.FirstOrDefault().image
                        };

            return View(query.ToList());
        }
       
        public ActionResult ProductDetail(int idProduct)
        {
            ViewBag.Product = db.Product.FirstOrDefault(p => p.id_product == idProduct);
            ViewBag.Img = db.Product_Image.Where(m => m.id_product == idProduct).ToList();
            //var product = db.Product.FirstOrDefault(p => p.product_name.ToLower() == productName.ToLower());
            return View();
        }
      

    }
}