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
    public class ProductsController : Controller
    {
        private skyshopEntities db = new skyshopEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Product_Image);
            return View(product.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.id_brand = new SelectList(db.Brand, "id_brand", "brand_name");
            ViewBag.id_cate = new SelectList(db.Category, "id_cate", "cate_name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_product,product_name,price,desc,id_cate,id_brand")] Product product, string name1, string name2, string name3)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();

                if (name1 != null && name2 != null && name3 != null)
                {
                    var p1 = new Product_Image();
                    p1.id_product = product.id_product;
                    p1.image = name1;
                    db.Product_Image.Add(p1);
                    var p2 = new Product_Image();
                    p2.id_product = product.id_product;
                    p2.image = name2;
                    db.Product_Image.Add(p2);
                    var p3 = new Product_Image();
                    p3.id_product = product.id_product;
                    p3.image = name3;
                    db.Product_Image.Add(p3);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }


            ViewBag.id_brand = new SelectList(db.Brand, "id_brand", "brand_name", product.id_brand);
            ViewBag.id_cate = new SelectList(db.Category, "id_cate", "cate_name", product.id_cate);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_brand = new SelectList(db.Brand, "id_brand", "brand_name", product.id_brand);
            ViewBag.id_cate = new SelectList(db.Category, "id_cate", "cate_name", product.id_cate);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_product,product_name,price,desc,id_cate,id_brand")] Product product, string img1, string img2, string img3)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                if (img1 != null && img2 != null && img3 != null)
                {
                    var pro = db.Product_Image.Where(p => p.id_product == product.id_product).ToList();
                    
                    var p1 = pro[0];
                    var p2 = pro[1];
                    var p3 = pro[2];
                    
                    p1.id_product = product.id_product;
                    p1.image = img1;

                    p2.id_product = product.id_product;
                    p2.image = img2;

                    p3.id_product = product.id_product;
                    p3.image = img3;
                    
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }
            ViewBag.id_brand = new SelectList(db.Brand, "id_brand", "brand_name", product.id_brand);
            ViewBag.id_cate = new SelectList(db.Category, "id_cate", "cate_name", product.id_cate);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
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
    }
}
