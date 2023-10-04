using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SkyLineShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "Category",
            url: "Shop/Category/{categoryName}",
            defaults: new { controller = "Shop", action = "Category" }
            );
            //routes.MapRoute(
            //name: "ProductDetail",
            //url: "Shop/ProductDetails/{productName}",
            //defaults: new { controller = "Shop", action = "ProductDetail" }
            //);

        }
    }
}
