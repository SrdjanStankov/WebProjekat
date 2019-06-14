using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProjektniZadatak.Controllers
{
    public class UserController : Controller
    {
        public static List<User> RegisteredUsers { get; } = new List<User>();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Guest user)
        {
            if (!ModelState.IsValid)
            {
                WriteErrors();

                return View("Index");
            }

            using (var model = new Model())
            {
                if (!model.Exists(user))
                {
                    model.AddUser(user);
                    RegisteredUsers.Add(user);
                }
                else
                {
                    TempData["Username"] = "Username already exists";
                    return View("Index");
                }
            }

            return View();

        }

        private void WriteErrors()
        {
            foreach (var item in ModelState)
            {
                if (item.Value.Errors.Count != 0)
                {
                    string k = item.Key;
                    TempData[k] = item.Value.Errors.FirstOrDefault().ErrorMessage;
                }
            }
        }

        public ActionResult Login(string username, string password)
        {
            bool usernameFound = false;
            bool passwordMatch = false;
            User sesion = null;

            if (username is null)
            {
                return View();
            }

            foreach (var item in RegisteredUsers)
            {
                if (item.Username == username)
                {
                    usernameFound = true;
                    if (item.Password == password)
                    {
                        passwordMatch = true;
                        sesion = item;
                        break;
                    }
                }
            }

            if (!usernameFound)
            {
                TempData["Username"] = "Username does not exist";
                return View();
            }

            if (!passwordMatch)
            {
                TempData["Password"] = "Invalid password";
                return View();
            }

            Session["User"] = sesion;

            return View("LoogedIn");
        }

        public ActionResult Dashboard()
        {

            return View(Session["User"] as User);
        }

        public ActionResult ChangeUserData(User user)
        {
            if (!ModelState.IsValid)
            {
                WriteErrors();
                return View("Dashboard", Session["User"] as User);
            }

            using (var model = new Model())
            {
                var to = model.GetAllUsers().Select(s => s).Where(u => u.Username == user.Username).FirstOrDefault();
                to.Gender = user.Gender;
                to.Lastname = user.Lastname;
                to.Name = user.Name;
                to.Password = user.Password;

                if (!model.ReplaceUser(from: Session["User"] as User, to: to))
                {
                    return View("Dashboard", Session["User"] as User);
                }

                Session["User"] = user;
                return View("Dashboard", Session["User"] as User);

            }
        }

        public ActionResult Logout()
        {
            Session.Remove("User");
            return View("Index");
        }

        public ActionResult AllUsers()
        {
            ViewBag.allUsers = RegisteredUsers;

            return View(RegisteredUsers);
        }

        public ActionResult EditUser(string id)
        {
            // TODO: Edit User
            var user = RegisteredUsers.Select(s => s).Where(u => u.Username == id).FirstOrDefault();
            return View(user);
        }

        public ActionResult AllApartments()
        {
            return View(Session["User"] as Host);
        }

        public ActionResult DetailsApartment(int id)
        {
            Apartment apartment = null;

            using (var model = new Model())
            {
                apartment = model.GetApartment(id);
            }

            return View(apartment);
        }

        public ActionResult CreateNewApartment()
        {
            return View();
        }

        public ActionResult AddLocation()
        {
            // TODO: add location
            return View();
        }
    }
}