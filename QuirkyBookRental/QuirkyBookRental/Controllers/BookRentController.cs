using Microsoft.AspNet.Identity;
using QuirkyBookRental.Models;
using QuirkyBookRental.Utility;
using QuirkyBookRental.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace QuirkyBookRental.Controllers
{
    public class BookRentController : Controller
    {

        private ApplicationDbContext db;

        public BookRentController()
        {
            db = ApplicationDbContext.Create();
        }

        // GET: BookRent
        public ActionResult Index(int? pageNumber, string option = null, string search = null)
        {
            string userid = User.Identity.GetUserId();
            var model = from br in db.BookRental
                        join b in db.Books on br.BookId equals b.Id
                        join u in db.Users on br.UserId equals u.Id
                        select new BookRentalViewModel
                        {
                            BookId = b.Id,
                            RentalPrice = br.RentalPrice,
                            Price = b.Price,
                            Pages = b.Pages,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            BirthDate = u.BirthDate,
                            ScheduledEndDate = br.ScheduledEndDate,
                            Author = b.Author,
                            Avaibility = b.Avaibility,
                            DateAdded = b.DateAdded,
                            Description = b.Description,
                            Email = u.Email,
                            GenreId = b.GenreId,
                            Genre = db.Genres.Where(g => g.Id.Equals(b.GenreId)).FirstOrDefault(),
                            ISBN = b.ISBN,
                            ImageUrl = b.ImageUrl,
                            ProductDimensions = b.ProductDimensions,
                            PubblicationDate = b.PubblicationDate,
                            Publisher = b.Publisher,
                            RentalDuration = br.RentalDuration,
                            Status = br.Status.ToString(),
                            Title = b.Title,
                            UserId = u.Id,
                            Id = br.Id,
                            StartDate = br.StartDate
                        };

            if (option == "email" && search.Length > 0)
            {
                model = model.Where(u => u.Email.Contains(search));
            }
            if (option == "name" && search.Length > 0)
            {
                model = model.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search));
            }
            if (option == "status" && search.Length > 0)
            {
                model = model.Where(u => u.Status.Contains(search));
            }

            if (!User.IsInRole(SD.AdminUserRole))
            {
                model = model.Where(u => u.UserId.Equals(userid));
            }

            return View(model.ToList().ToPagedList(pageNumber ?? 1, 5));
        }

        //Get
        public ActionResult Create(string title = null, string ISBN = null)
        {
            if (title != null && ISBN != null)
            {
                BookRentalViewModel model = new BookRentalViewModel
                {
                    Title = title,
                    ISBN = ISBN
                };
            }
            return View(new BookRentalViewModel());
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookRentalViewModel bookRent)
        {
            if (ModelState.IsValid)
            {
                var email = bookRent.Email;
                var userDetails = from u in db.Users
                                  where u.Email.Equals(email)
                                  select new { u.Id };
                var ISBN = bookRent.ISBN;

                Book bookSelected = db.Books.Where(b => b.ISBN == ISBN).FirstOrDefault();
                var rentalDuration = bookRent.RentalDuration;
                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
                                 where u.Email.Equals(email)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateSixMonth };

                var oneMonthRental = Convert.ToDouble(bookSelected.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;
                var sixMonthRental = Convert.ToDouble(bookSelected.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;

                double rentalPr = 0;

                if (bookRent.RentalDuration == SD.SixMonth)
                {
                    rentalPr = sixMonthRental;
                }
                else
                {
                    rentalPr = oneMonthRental;
                }

                BookRent modelToAssToDb = new BookRent
                {
                    BookId = bookSelected.Id,
                    RentalPrice = rentalPr,
                    ScheduledEndDate = bookRent.ScheduledEndDate,
                    RentalDuration = bookRent.RentalDuration,
                    Status = BookRent.StatusEmum.Approved,
                    UserId = userDetails.ToList()[0].Id
                };

                bookSelected.Avaibility -= 1;
                db.BookRental.Add(modelToAssToDb);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reserve(BookRentalViewModel book)
        {
            string userid = User.Identity.GetUserId();
            Book bookToRent = db.Books.Find(book.BookId);
            double rentalPr = 0;

            if (userid != null)
            {
                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
                                 where u.Id.Equals(userid)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateSixMonth };

                if (book.RentalDuration == SD.SixMonth)
                {
                    rentalPr = Convert.ToDouble(bookToRent.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateSixMonth) / 100;
                }
                else
                {
                    rentalPr = Convert.ToDouble(bookToRent.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;
                }

                BookRent bookRent = new BookRent
                {
                    BookId = bookToRent.Id,
                    UserId = userid,
                    RentalDuration = book.RentalDuration,
                    RentalPrice = rentalPr,
                    Status = BookRent.StatusEmum.Requested
                };
                db.BookRental.Add(bookRent);
                var bookInDb = db.Books.SingleOrDefault(c => c.Id == book.BookId);
                bookInDb.Avaibility -= 1;
                db.SaveChanges();
                return RedirectToAction("Index", "BookRent");
            }

            return View();
        }

        public ActionResult Details(int? id)
        {
            if( id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(id);
            var model = getVMFromBookRent(bookRent);
            if(model==null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        //Decline GET
        public ActionResult Decline(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //Decline POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decline(BookRentalViewModel model)
        {
            if (model.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(model.Id);
            bookRent.Status = BookRent.StatusEmum.Rejected;
            Book bookInDb = db.Books.Find(bookRent.BookId);

            bookInDb.Avaibility += 1;
            db.SaveChanges();


            return RedirectToAction("Index");
        }



        //Approve GET
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Approve",model);
        }

        //Approve POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(BookRentalViewModel model)
        {
            if (model.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(model.Id);
            bookRent.Status = BookRent.StatusEmum.Approved;
            
            db.SaveChanges();


            return RedirectToAction("Index");
        }


        //PickUpe GET
        public ActionResult PickUp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Approve", model);
        }

        //PickUp POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PickUp(BookRentalViewModel model)
        {
            if (model.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(model.Id);
            bookRent.Status = BookRent.StatusEmum.Rented;
            bookRent.StartDate = DateTime.Now;
            if(bookRent.RentalDuration==SD.SixMonthCount)
            {
                bookRent.ScheduledEndDate = DateTime.Now.AddMonths(Convert.ToInt32(SD.SixMonthCount));
            }
            else
            {
                bookRent.ScheduledEndDate = DateTime.Now.AddMonths(Convert.ToInt32(SD.OneMonthCount));
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        //Return GET
        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("Approve", model);
        }

        //PickUp POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(BookRentalViewModel model)
        {
            if (model.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(model.Id);
            bookRent.Status = BookRent.StatusEmum.Closed;
            bookRent.AdditionalCharge = model.AdditionalCharge;

            Book bookInDb = db.Books.Find(bookRent.BookId);
            bookInDb.Avaibility += 1;
            bookRent.ActualEndDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index");
        }





        //Delete GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(id);
            var model = getVMFromBookRent(bookRent);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        //Decline POST
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookRent bookRent = db.BookRental.Find(Id);


            var bookInDb = db.Books.Where(b=>b.Id.Equals(bookRent.BookId)).FirstOrDefault();
            if(!bookRent.Status.ToString().Equals("Rented"))
            {
                bookInDb.Avaibility += 1;
            }

            db.BookRental.Remove(bookRent);            
            db.SaveChanges();

            return RedirectToAction("Index");
        }













        private BookRentalViewModel getVMFromBookRent(BookRent bookRent)
        {
            Book bookSelect = db.Books.Where(b=>b.Id == bookRent.BookId).FirstOrDefault();
            var userDetails = from u in db.Users
                              where u.Id.Equals(bookRent.UserId)
                              select new { u.Id,u.FirstName,u.LastName,u.BirthDate,u.Email};

            BookRentalViewModel model = new BookRentalViewModel
            {
                Id=bookRent.Id,
                BookId=bookSelect.Id,
                RentalPrice=bookRent.RentalPrice,
                Price=bookSelect.Price,
                Pages=bookSelect.Pages,
                FirstName=userDetails.ToList()[0].FirstName,
                LastName = userDetails.ToList()[0].LastName,
                BirthDate = userDetails.ToList()[0].BirthDate,
                Email = userDetails.ToList()[0].Email,
                UserId = userDetails.ToList()[0].Id,
                ScheduledEndDate=bookRent.ScheduledEndDate,
                Author=bookSelect.Author,
                StartDate=bookRent.StartDate,
                Avaibility=bookSelect.Avaibility,
                DateAdded=bookSelect.DateAdded,
                Description=bookSelect.Description,
                GenreId=bookSelect.GenreId,
                Genre=db.Genres.FirstOrDefault(g=>g.Id.Equals(bookSelect.GenreId)),
                ISBN=bookSelect.ISBN,
                ImageUrl=bookSelect.ImageUrl,
                ProductDimensions=bookSelect.ProductDimensions,
                PubblicationDate=bookSelect.PubblicationDate,
                Publisher=bookSelect.Publisher,
                RentalDuration=bookRent.RentalDuration,
                Status=bookRent.Status.ToString(),
                Title=bookSelect.Title,
                AdditionalCharge=bookRent.AdditionalCharge
            };

            return model;

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}