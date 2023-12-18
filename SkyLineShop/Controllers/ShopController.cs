using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkyLineShop.Models;
using System.Text.RegularExpressions;
using PagedList;

namespace SkyLineShop.Controllers
{
    public class ShopController : Controller
    {
        skyshop2Entities db = new skyshop2Entities();
        // GET: Shop
        public ActionResult Index(int? categoryID, int? brandID, string search, int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            pageSize = 9; 
            string filter = TempData["filter"] as string;
            ViewBag.brandID = brandID;
            ViewBag.categoryID = categoryID;
            ViewBag.Category = db.Categories.ToList();
            ViewBag.Brand = db.Brands.ToList();
            var query = from p in db.Products
                        join m in db.Product_Image on p.id_product equals m.id_product
                        join b in db.Brands on p.id_brand equals b.id_brand
                        where (categoryID == null || p.id_cate == categoryID)
                        && (brandID == null || p.id_brand == brandID)
                        && (p.product_name.ToLower().Contains(search.ToLower()) || search == null)
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

            ViewBag.Filter = filter;
            var product = query.ToList();
            if (filter == "1")
            {
                product = query.OrderByDescending(p => p.ProductPrice).ToList();
            }
            else if (filter == "0")
            {
                product = query.OrderBy(p => p.ProductPrice).ToList();
            }
            else
                product = query.ToList();

            // Sử dụng ToPagedList để tạo đối tượng PagedList
            return View(product.ToPagedList((int)page, (int)pageSize));

        }

        public ActionResult ProductDetail(int idProduct)
        {
            Product pro = db.Products.FirstOrDefault(p => p.id_product == idProduct);
            ViewBag.Product = pro;
            ViewBag.Img = db.Product_Image.Where(m => m.id_product == idProduct).ToList();
            ViewBag.relatedProduct = db.Products.Where(x => (x.id_cate == pro.id_cate && x.id_product != pro.id_product)).Take(4).ToList();
            return View();
        }
        public ActionResult Filter(string priceSortSelect)
        {
            TempData["filter"] = priceSortSelect;
            return RedirectToAction("Index");
        }
    }
}