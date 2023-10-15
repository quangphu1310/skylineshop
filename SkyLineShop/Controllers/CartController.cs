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
        skyshopEntities db = new skyshopEntities();
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
            var product = db.Product.Find(productid);
            var img = db.Product_Image.Where(m => m.id_product == productid);
            var cart = Session[CartSession];
            if (cart != null)
            {
                //cộng thêm số lượng
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.id_product == productid))
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
            }
            return RedirectToAction("Index", "Shop");
        }

        public ActionResult UpdateCart(FormCollection form)
        {
            var cart = Session[CartSession] as List<CartItem>;

            if (cart != null)
            {
                string[] idProducts = form.GetValues("id_product");
                string[] quantities = form.GetValues("quantity_pro");

                if (idProducts != null && quantities != null && idProducts.Length == quantities.Length)
                {
                    for (int i = 0; i < idProducts.Length; i++)
                    {
                        int id = int.Parse(idProducts[i]);
                        int quantity = int.Parse(quantities[i]);

                        var item = cart.Find(e => e.Product.id_product == id);

                        if (item != null)
                        {
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

            }

            return RedirectToAction("Index");
        }

        public ActionResult RemoveItem(int id_product)
        {
            var cart = Session[CartSession];
            var list = (List<CartItem>)cart;
            //string[] idProducts = form.GetValues("id_product");

            if (cart != null)
            {
                var item = list.Find(e => e.Product.id_product == id_product);
                if (item != null)
                {
                    list.Remove(item);
                }

            }
            return RedirectToAction("Index");
        }
        public ActionResult RemoveAll()
        {
            Session[CartSession] = null;
            return RedirectToAction("Index");
        }

    }
}