using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkyLineShop.Models
{
    public class ProductPro
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDesc { get; set; }
        public int IDcate { get; set; }
        public int IDbrand { get; set; }
    }
}