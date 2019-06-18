using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System;
using System.Collections.Generic;
using System.Globalization;
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

                return View("LoogedIn");
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
                if (model.Exists(host))
                {
                    TempData["Username"] = "Username already taken";
                    return View("AdminCreateHost");
                }
                model.AddUser(host);
                var users = new List<User>(model.GetUsers());
                return View("AllUsers", users);
            }
        }

        public ActionResult AllApartments()
        {
            using (var model = new Model())
            {
                var host = model.GetUser((Session["User"] as Host).Username) as Host;
                host.ApartmentsForRent = model.GetApartments(host.Username).ToList();
                return View(host);
            }
        }

        //public ActionResult DetailsApartment(int id)
        //{
        //    Apartment apartment = null;

        //    using (var model = new Model())
        //    {
        //        apartment = model.GetApartment(id);
        //    }

        //    return View(apartment);
        //}

        public ActionResult CreateNewApartment()
        {
            return View(new Apartment());
        }

        public ActionResult CreateApartment(Apartment apartment)
        {
            var datesForIssueString = Request["DatesForIssues"];
            var datesStringArray = datesForIssueString.Split(',');
            var dateTimes = new List<DateTime>();
            foreach (var item in datesStringArray)
            {
                try
                {
                    dateTimes.Add(DateTime.ParseExact(item, "dd-mm-yyyy", CultureInfo.InvariantCulture));
                }
                catch (Exception) { }
            }

            apartment.DatesForIssues = dateTimes;
            apartment.AvailableDates = dateTimes;

            using (var model = new Model())
            {
                apartment.Host = model.GetUser((Session["User"] as Host).Username) as Host;
                apartment.Host.ApartmentsForRent.Add(apartment);
                model.AddApartment(apartment);
            }

            return RedirectToAction("AllApartments");
        }

        public ActionResult AddLocation()
        {
            // TODO: add location
            return View();
        }

        public ActionResult ReservationApartment(int id)
        {
            using (var model = new Model())
            {
                return View(model.GetApartment(id).Reservations);
            }
        }

        public ActionResult CommentApartment(int id)
        {
            using (var model = new Model())
            {
                return View(model.GetApartment(id).Comments);
            }
        }

        public ActionResult EditApartment(int id)
        {
            using (var model = new Model())
            {
                return View(model.GetApartment(id));
            }
        }

        public ActionResult ViewApartments()
        {
            using (var model = new Model())
            {
                List<Apartment> apartments = new List<Apartment>();
                if (Session["User"] != null)
                {
                    if (Session["User"] is Guest)
                    {
                        apartments = new List<Apartment>(model.GetApartments(ApartmentStatus.Active));
                        return View(apartments);
                    }
                    else if (Session["User"] is Host)
                    {
                        apartments = new List<Apartment>(model.GetApartments((Session["User"] as Host).Username));
                        return View(apartments);
                    }
                    else if (Session["User"] is Administrator)
                    {
                        return View(model.GetApartments());
                    }
                }
                else
                {
                    apartments = new List<Apartment>(model.GetApartments(ApartmentStatus.Active)); 
                }

                return View(apartments);
            }
        }

        public ActionResult Sort(string id)
        {
            var apartments = TempData["model"] as List<Apartment>;

            switch (id)
            {
                case "TypeAsc":
                    return View("ViewApartments", apartments.OrderBy(a => a.ApartmentType).ToList());
                case "TypeDsc":
                    ViewBag.TypeAsc = new object();
                    return View("ViewApartments", apartments.OrderByDescending(a => a.ApartmentType).ToList());
                case "RoomAsc":
                    return View("ViewApartments", apartments.OrderBy(a => a.NumberOfRooms).ToList());
                case "RoomDsc":
                    ViewBag.RoomAsc = new object();
                    return View("ViewApartments", apartments.OrderByDescending(a => a.NumberOfRooms).ToList());
                case "GuestAsc":
                    return View("ViewApartments", apartments.OrderBy(a => a.NumberOfGuests).ToList());
                case "GuestDsc":
                    ViewBag.GuestAsc = new object();
                    return View("ViewApartments", apartments.OrderByDescending(a => a.NumberOfGuests).ToList());
                case "PriceAsc":
                    return View("ViewApartments", apartments.OrderBy(a => a.PricePerNight).ToList());
                case "PriceDsc":
                    ViewBag.PriceAsc = new object();
                    return View("ViewApartments", apartments.OrderByDescending(a => a.PricePerNight).ToList());
                case "RegistrationAsc":
                    return View("ViewApartments", apartments.OrderBy(a => a.TimeOfRegistration).ToList());
                case "RegistrationDsc":
                    ViewBag.RegistrationAsc = new object();
                    return View("ViewApartments", apartments.OrderByDescending(a => a.TimeOfRegistration).ToList());
                case "CheckoutAsc":
                    return View("ViewApartments", apartments.OrderBy(a => a.TimeOfCheckOut).ToList());
                case "CheckoutDsc":
                    ViewBag.CheckoutAsc = new object();
                    return View("ViewApartments", apartments.OrderByDescending(a => a.TimeOfCheckOut).ToList());
                default:
                    break;
            }

            return RedirectToAction("ViewApartments");
        }

        public ActionResult Search(string apartmentType, string numberOfRooms, string numberOfGuests, string pricePerNight, string registrationTime, string checkoutTime)
        {
            IEnumerable<Apartment> apartments = new List<Apartment>();
            bool isChanged = false;

            using (var model = new Model())
            {
                if (Session["User"] != null)
                {
                    if (Session["User"] is Guest)
                    {
                        apartments = new List<Apartment>(model.GetApartments(ApartmentStatus.Active));
                    }
                    else if (Session["User"] is Host)
                    {
                        apartments = new List<Apartment>(model.GetApartments((Session["User"] as Host).Username));
                    }
                    else if (Session["User"] is Administrator)
                    {
                        apartments = new List<Apartment>(model.GetApartments());
                    }
                }
                else
                {
                    apartments = new List<Apartment>(model.GetApartments(ApartmentStatus.Active));
                }
            }


            if (apartmentType != "None")
            {
                var type = (ApartmentType)Enum.Parse(typeof(ApartmentType), apartmentType, true);
                apartments = apartments.Where(i => i.ApartmentType == type);
                isChanged = true;
                ViewBag.apartmentType = type;
            }

            if (!string.IsNullOrEmpty(numberOfRooms))
            {
                var rooms = int.Parse(numberOfRooms);
                apartments = apartments.Where(i => i.NumberOfRooms == rooms);
                isChanged = true;
                ViewBag.numberOfRooms = rooms;
            }

            if (!string.IsNullOrEmpty(numberOfGuests))
            {
                var guests = int.Parse(numberOfGuests);
                apartments = apartments.Where(i => i.NumberOfGuests == guests);
                isChanged = true;
                ViewBag.numberOfGuests = guests;
            }

            if (!string.IsNullOrEmpty(pricePerNight))
            {
                var price = int.Parse(pricePerNight);
                apartments = apartments.Where(i => i.PricePerNight == price);
                isChanged = true;
                ViewBag.pricePerNight = price;
            }

            if (!string.IsNullOrEmpty(registrationTime))
            {
                var regTime = new DateTime(1, 1, 1, int.Parse(registrationTime.Split(':').First()), int.Parse(registrationTime.Split(':').Last()), 0);
                apartments = apartments.Where(i => i.TimeOfRegistration.ToShortTimeString() == regTime.ToShortTimeString());
                isChanged = true;
                ViewBag.registrationTime = registrationTime;
            }

            if (!string.IsNullOrEmpty(checkoutTime))
            {
                var outTime = new DateTime(1, 1, 1, int.Parse(checkoutTime.Split(':').First()), int.Parse(checkoutTime.Split(':').Last()), 0);
                apartments = apartments.Where(i => i.TimeOfCheckOut.ToShortTimeString() == outTime.ToShortTimeString());
                isChanged = true;
                ViewBag.checkoutTime = checkoutTime;
            }

            if (isChanged)
            {
                return View("ViewApartments", apartments);
            }
            else
            {
                using (var model = new Model())
                {
                    apartments = new List<Apartment>(model.GetApartments());
                    return View("ViewApartments", apartments);
                }
            }

        }
    }
}