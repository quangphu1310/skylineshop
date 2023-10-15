using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyLineShop.Models;

namespace SkyLineShop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        skyshopEntities db = new skyshopEntities();
        public ActionResult Index()
        {
            var query = from p in db.Product
                        join m in db.Product_Image on p.id_product equals m.id_product
                        where p.price >= 1000000
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

            var query2 = from p in db.Product
                         join m in db.Product_Image on p.id_product equals m.id_product
                         where p.id_brand == 1
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
            var query3 = from p in db.Product
                         join m in db.Product_Image on p.id_product equals m.id_product
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
            ViewBag.Product = query.ToList();
            ViewBag.Product2 = query2.ToList();
            ViewBag.Product3 = query3.OrderBy(p => p.ProductPrice).FirstOrDefault();
            return View();
        }
    }
}