using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuirkyBookRental
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "BookReleaseMonth",
                "Book/ReleaseMonth/{year}/{month}",
                new { controller = "book", action = "ReleaseMonth" }
                );

            routes.MapRoute(
                "BookReleaseYearAndAuthor",
                "Book/ReleaseYearAndAuthor/{year}/{author}",
                new { controller = "book", action = "ReleaseYearAndAuthor" },
                constraints: new {year = @"\d{4}"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
