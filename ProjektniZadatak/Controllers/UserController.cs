using ProjektniZadatak.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProjektniZadatak.Controllers
{
    public class UserController : Controller
    {
        public static List<User> RegisteredUsers { get; } = new List<User>();

        // GET: User
        public ActionResult Index() => View();

        public ActionResult Register(User user)
        {
            user.Role = Role.Guest;
            RegisteredUsers.Add(user);
            Console.WriteLine("acid");
            return View();
        }
    }
}