using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Controllers
{
    public class UserController : Controller
    {
        private OnlineLibraryContext db = new OnlineLibraryContext();

        // GET: User/Index 
        public ActionResult Index()
        {
            var users = db.Users
                          .Select(u => new
                          {
                              User = u,
                              LoanCount = db.Loans.Count(l => l.UserId == u.Id) 
                          })
                          .ToList();

            if (!users.Any())
            {
                ViewBag.Message = "No users found in the database.";
            }

            
            System.Diagnostics.Debug.WriteLine("Users count: " + users.Count());

            var userList = users.Select(u => new UserWithLoanCount
            {
                Id = u.User.Id,
                Username = u.User.Username,
                Email = u.User.Email,
                Role = u.User.Role,
                LoanCount = u.LoanCount
            }).ToList();

            return View(userList);
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }
                           
        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please enter both username and password.";
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                ViewBag.Error = "Username does not exist.";
                return View();
            }

            if (user.Password != password)
            {
                ViewBag.Error = "Incorrect password.";
                return View();
            }

            
            Session["UserId"] = user.Id;
            Session["Username"] = user.Username;
            Session["Role"] = user.Role;

            return RedirectToAction("Index", "Home");
        }

        // GET: User/Logout
        public ActionResult Logout()
        {
            System.Diagnostics.Debug.WriteLine("User logged out: " + Session["Username"]);
            Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: User/Create 
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Username, Password, Role, Email")] User user)
        {
            if (ModelState.IsValid)
            {
                
                if (db.Users.Any(u => u.Username == user.Username))
                {
                    ViewBag.ErrorMessage = "Username already exists.";
                    return View(user); 
                }

                if (db.Users.Any(u => u.Email == user.Email))
                {
                    ViewBag.ErrorMessage = "Email already exists.";
                    return View(user); 
                }

                
                db.Users.Add(user);
                db.SaveChanges();

                
                return RedirectToAction("Index");
            }

            
            ViewBag.ErrorMessage = "There was an error creating the user.";
            return View(user);
        }



        // GET: User/Edit/5 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            
            System.Diagnostics.Debug.WriteLine("Editing user: " + user.Username);

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Username, Password, Role, Email")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var existingUser = db.Users.Find(user.Id);

                    if (existingUser != null)
                    {
                        
                        existingUser.Username = user.Username;
                        existingUser.Password = user.Password;  
                        existingUser.Role = user.Role;
                        existingUser.Email = user.Email;


                        db.SaveChanges();

                        
                        Debug.WriteLine($"User {user.Username} updated successfully.");
                    }
                    else
                    {
                        Debug.WriteLine("User not found.");
                    }

                    return RedirectToAction("Index"); 
                }

                
                return View(user);
            }
            catch (Exception ex)
            {
                
                Debug.WriteLine($"Error updating user: {ex.Message}");
                
                ViewBag.ErrorMessage = "An error occurred while updating the user.";
                return View(user);
            }
        }

        // GET: User/Delete/5 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);

            
            System.Diagnostics.Debug.WriteLine("Deleting user: " + user.Username);

            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
    }
}
