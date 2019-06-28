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
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register(Guest user)
        {
            if (!ModelState.IsValid)
            {
                WriteErrors();

                return View("Index");
            }

            using (var model = new Model())
            {
                if (!model.UsernameExists(user))
                {
                    model.AddUser(user);
                }
                else
                {
                    TempData["Username"] = "Username already exists";
                    return View("Index");
                }
            }

            return View("Login");
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

            using (var model = new Model())
            {
                foreach (var item in model.GetUsers())
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

                return RedirectToAction("ViewApartments", "Apartment");
            }
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
                var to = model.GetUser(user.Username);

                to.Gender = user.Gender;
                to.Lastname = user.Lastname;
                to.Name = user.Name;
                to.Password = user.Password;

                model.EditUser(from: Session["User"] as User, to: to);

                Session["User"] = model.GetUser(user.Username);
                return View("Dashboard", Session["User"] as User);
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Remove("User");
            return View("Index");
        }

        public ActionResult AllUsers()
        {
            using (var model = new Model())
            {
                var users = new List<User>(model.GetUsers());
                return View(users);
            }
        }

        public ActionResult DeleteUser(string id)
        {
            using (var model = new Model())
            {
                model.RemoveUser(id);
                return RedirectToAction("AllUsers");
            }
        }

        public ActionResult AdminCreateHost()
        {
            return View();
        }

        public ActionResult CreateHost(Host host)
        {
            if (!ModelState.IsValid)
            {
                WriteErrors();
                return View("AdminCreateHost");
            }

            using (var model = new Model())
            {
                if (model.UsernameExists(host))
                {
                    TempData["Username"] = "Username already taken";
                    return View("AdminCreateHost");
                }
                model.AddUser(host);
                var users = new List<User>(model.GetUsers());
                return View("AllUsers", users);
            }
        }

        public ActionResult Search(string Username, string Role, string Gender)
        {
            using (var model = new Model())
            {
                var users = new List<User>(model.GetUsers());

                if (!string.IsNullOrEmpty(Username))
                {
                    users = users.Where(u => u.Username == Username).ToList();
                    ViewBag.username = Username;
                }

                if (!string.IsNullOrEmpty(Role))
                {
                    switch (Role)
                    {
                        case "Guest":
                            users = users.Where(u => u is Guest).ToList();
                            break;
                        case "Host":
                            users = users.Where(u => u is Host).ToList();
                            break;
                        case "Admin":
                            users = users.Where(u => u is Administrator).ToList();
                            break;
                        default:
                            break;
                    }
                    ViewBag.role = Role;
                }

                if (Gender != "Both")
                {
                    var gen = (Genders)Enum.Parse(typeof(Genders), Gender, true);
                    users = users.Where(u => u.Gender == gen).ToList();
                }

                ViewBag.gender = Gender;

                return View("AllUsers", users);
            }
        }
    }
}