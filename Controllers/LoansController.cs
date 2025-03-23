using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class LoansController : Controller
    {
        private OnlineLibraryContext db = new OnlineLibraryContext();

        // GET: Loans
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.Book).Include(l => l.User);
            return View(loans.ToList());
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create()
        {
            var users = db.Users.ToList();
            var books = db.Books.ToList();
            ViewBag.Users = new SelectList(users, "Id", "Username");
            ViewBag.Books = new SelectList(books, "Id", "Title");

            return View();
        }

        // POST: Loans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,BookId,BorrowDate,ReturnDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                if (loan.BorrowDate < DateTime.Now.Date)
                {
                    ModelState.AddModelError("BorrowDate", "Borrow date cannot be in the past.");
                }
                if (loan.ReturnDate < DateTime.Now.Date)
                {
                    ModelState.AddModelError("ReturnDate", "Return date cannot be in the past.");
                }

                if (loan.ReturnDate <= loan.BorrowDate)
                {
                    ModelState.AddModelError("ReturnDate", "Return date must be after borrow date.");
                }

                if (ModelState.IsValid)
                {
                    db.Loans.Add(loan);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            var users = db.Users.ToList();
            var books = db.Books.ToList();
            ViewBag.Users = new SelectList(users, "Id", "Username", loan.UserId);
            ViewBag.Books = new SelectList(books, "Id", "Title", loan.BookId);

            return View(loan);
        }

        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }

            var users = db.Users.ToList();
            var books = db.Books.ToList();
            ViewBag.Users = new SelectList(users, "Id", "Username", loan.UserId);
            ViewBag.Books = new SelectList(books, "Id", "Title", loan.BookId);

            return View(loan);
        }

        // POST: Loans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,BookId,BorrowDate,ReturnDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                if (loan.BorrowDate < DateTime.Now.Date)
                {
                    ModelState.AddModelError("BorrowDate", "Borrow date cannot be in the past.");
                }
                if (loan.ReturnDate < DateTime.Now.Date)
                {
                    ModelState.AddModelError("ReturnDate", "Return date cannot be in the past.");
                }

                if (loan.ReturnDate <= loan.BorrowDate)
                {
                    ModelState.AddModelError("ReturnDate", "Return date must be after borrow date.");
                }

                if (ModelState.IsValid)
                {
                    db.Entry(loan).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            var users = db.Users.ToList();
            var books = db.Books.ToList();
            ViewBag.Users = new SelectList(users, "Id", "Username", loan.UserId);
            ViewBag.Books = new SelectList(books, "Id", "Title", loan.BookId);

            return View(loan);
        }

        // GET: Loans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
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
