using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyLineShop.Models
{
    [Serializable]
    public class CartItem
    {
        public Product Product { set; get; }
        public int Quantity { set; get; }
        public string Size { set; get; }
        public string Image { set; get; }

    }
}