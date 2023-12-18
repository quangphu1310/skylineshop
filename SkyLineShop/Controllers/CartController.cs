using SkyLineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkyLineShop.Controllers
{

    public class CartController : Controller
    {
        // GET: Cart
        public string CartSession = "CartSession";
        skyshop2Entities db = new skyshop2Entities();
        public int count = 0;
        public ActionResult Index()
        {

            var cart = Session[CartSession];
            var list = (List<CartItem>)cart;
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(int productid, int quantity, string size, string image)
        {
            var product = db.Products.Find(productid);
            var img = db.Product_Image.Where(m => m.id_product == productid);
            var cart = Session[CartSession];
            if (cart != null)
            {
                //cộng thêm số lượng
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.id_product == productid && x.Size == size))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.id_product == productid)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //Tạo đối tượng mới
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    item.Size = size;
                    item.Image = image;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
                count = list.Count;
            }
            else
            {
                //thêm mới
                //Tạo đối tượng mới
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                item.Size = size;
                item.Image = image;

                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
                count = list.Count;
            }


            var result = new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng.", countCart = count };
            return Json(result);
        }

        public ActionResult UpdateCart(FormCollection form)
        {
            var cart = Session[CartSession] as List<CartItem>;

            if (cart != null)
            {
                string[] idProducts = form.GetValues("id_product");
                string[] quantities = form.GetValues("quantity_pro");
                string[] sizes = null;
                if (form.GetValue("sizeSelect") != null)
                {
                    sizes = form.GetValues("sizeSelect");
                }


                if (idProducts != null && quantities != null && idProducts.Length == quantities.Length)
                {
                    for (int i = 0; i < idProducts.Length; i++)
                    {
                        int id = int.Parse(idProducts[i]);
                        int quantity = int.Parse(quantities[i]);
                        string size = "";
                        if (sizes != null && sizes.Length > i && sizes[i] != null)
                        {
                            size = sizes[i];
                        }
                        else
                        {
                            size = "";
                        }
                        var item = cart.Find(e => e.Product.id_product == id);
                        if (item != null)
                        {
                            item.Size = size;
                            if (quantity > 0)
                            {
                                item.Quantity = quantity;
                            }
                            else
                            {
                                TempData["error"] = "Không thể chuyển giá trị về 0";
                            }
                        }
                    }
                }
                count = cart.Count;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateItem(int id_product, int newValue, string sizeValue)
        {
            var cart = Session[CartSession];
            var list = (List<CartItem>)cart;
            CartItem item = null;
            if (sizeValue == null)
                item = list.FirstOrDefault(e => e.Product.id_product == id_product);
            else
                item = list.FirstOrDefault(e => (e.Product.id_product == id_product) && (e.Size == sizeValue));
            item.Quantity = newValue;

            decimal? sum = 0;
            foreach(var items in list)
            {
                sum += items.Quantity * items.Product.price;
            }
            var total = newValue * item.Product.price;
            return Json(new { success = true, total = total, countCart = count, sum = sum });
        }

        [HttpPost]
        public ActionResult UpdateSize(int id_product, string size)
        {
            var cart = Session[CartSession];
            var list = (List<CartItem>)cart;
            var item = list.Find(e => e.Product.id_product == id_product);
            item.Size = size;

            return Json(new { success = true });
        }


       
        public ActionResult RemoveItem(int id_product)
        {
            var cart = Session[CartSession];
            var list = (List<CartItem>)cart;

            var item = list.Find(e => e.Product.id_product == id_product);
            if (item != null)
            {
                list.Remove(item);
            }
            count = list.Count;

            return Json(new { success = true, countCart = count });
           
        }
        public ActionResult RemoveAll()
        {
            Session[CartSession] = null;
            return RedirectToAction("Index");
        }
        
    }
}