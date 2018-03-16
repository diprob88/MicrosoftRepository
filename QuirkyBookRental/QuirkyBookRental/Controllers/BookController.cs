using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuirkyBookRental.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ReleaseMonth(int year,int month)
        {
            return Content("Release Date "+month+"/"+year);
        }

        public ActionResult ReleaseYearAndAuthor(int year, string author)
        {
            return Content(author+" " + year);
        }


    }
}