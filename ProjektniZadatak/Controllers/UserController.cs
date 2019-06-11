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
            foreach (var item in RegisteredUsers)
            {
                if (item.Username == username)
                {
                    if (item.Password == password)
                    {
                        item.IsLoggedIn = true;
                        break;
                    }
                    else
                    {
                        ModelState.AddModelError("TEST", "ERROR");
                    }
                }
            }
            return View();
        }
    }
}