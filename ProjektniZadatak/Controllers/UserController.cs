using ProjektniZadatak.Models;
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
                foreach (var item in ModelState)
                {
                    if (item.Value.Errors.Count != 0)
                    {
                        var k = item.Key;
                        TempData[k] = item.Value.Errors.FirstOrDefault().ErrorMessage;
                    }
                }

                return View("Index");
            }
            RegisteredUsers.Add(user);
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            bool usernameFound = false;
            bool passwordMatch = false;

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
                        item.IsLoggedIn = true;
                        passwordMatch = true;
                        break;
                    }
                }
            }

            if (!usernameFound)
            {
                TempData["Username"] = "Username does not exist";
                return View();
            }

            if(!passwordMatch)
            {
                TempData["Password"] = "Invalid password";
                return View();
            }

            return View("LoogedIn");
        }
    }
}