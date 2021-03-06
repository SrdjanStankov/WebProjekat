﻿using Newtonsoft.Json;
using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ProjektniZadatak.Controllers
{
    public class ApartmentController : Controller
    {
        public ActionResult ViewApartments()
        {
            using (var model = new Model())
            {
                var apartments = new List<Apartment>();
                if (Session["User"] != null)
                {
                    if (Session["User"] is Guest)
                    {
                        return View(new List<Apartment>(model.GetApartments(ApartmentStatus.Active)));
                    }
                    else if (Session["User"] is Host)
                    {
                        return View(new List<Apartment>(model.GetApartments((Session["User"] as Host).Username)));
                    }
                    else if (Session["User"] is Administrator)
                    {
                        return View(new List<Apartment>(model.GetApartments()));
                    }
                }
                else
                {
                    apartments = new List<Apartment>(model.GetApartments(ApartmentStatus.Active));
                }

                return View(apartments);
            }
        }

        public ActionResult CreateNewApartment()
        {
            return View(new Apartment());
        }

        public ActionResult CreateApartment(Apartment apartment)
        {
            var a = Request["address"] as string;

            dynamic res = JsonConvert.DeserializeObject(a);

            var adres = new Address();

            if (res == null)
            {
                TempData["Location"] = "Location is required";
                return View("CreateNewApartment", apartment);
            }

            if (res.address != null)
            {
                adres.Number = res.address.house_number ?? "";
                adres.Street = res.address.road ?? "";
                adres.Town = res.address.city ?? "";
                adres.ZipCode = res.address.postcode ?? "";
            }
            else
            {
                TempData["Location"] = "Valid location is required";
                return View("CreateNewApartment", apartment);
            }

            ModelState.Remove("DatesForIssues");
            if (!ModelState.IsValid)
            {
                WriteErrors();

                return View("CreateNewApartment", apartment);
            }

            

            string datesForIssueString = Request["DatesForIssues"];
            string[] datesStringArray = datesForIssueString.Split(',');
            var dateTimes = new List<CustomDate>();
            foreach (string item in datesStringArray)
            {
                try
                {
                    dateTimes.Add(new CustomDate(DateTime.ParseExact(item, "dd-mm-yyyy", CultureInfo.InvariantCulture)));
                }
                catch (Exception) { }
            }

            apartment.DatesForIssues = dateTimes;
            apartment.AvailableDates = dateTimes;

            double latitude = double.Parse(Request["latitude"].Replace(".", ","), NumberStyles.AllowDecimalPoint);
            double longitude = double.Parse(Request["longitude"].Replace(".", ","), NumberStyles.AllowDecimalPoint);
            apartment.Location = new Location(latitude, longitude, adres);

            using (var model = new Model())
            {
                foreach (var item in model.GetAmenities())
                {
                    if (Request[item.Name] == "on")
                    {
                        apartment.Amenities.Add(item);
                    }
                }
                apartment.PictureCount = Request.Files.Count;
                apartment.Host = model.GetUser((Session["User"] as Host).Username) as Host;
                apartment.Host.ApartmentsForRent.Add(apartment);

                apartment = model.AddApartment(apartment);

                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        if (file.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(file.FileName);
                            Directory.CreateDirectory(Server.MapPath($"~/Images/Apartment{apartment.Id}"));
                            string path = Path.Combine(Server.MapPath($"~/Images/Apartment{apartment.Id}"), $"{i}.{filename.Split('.').Last()}");
                            file.SaveAs(path);
                        }
                    }
                }
                string basePathApartment = $"/Images/Apartment{apartment.Id}";

                apartment.PicturesLocation = basePathApartment;
                model.EditApartment(apartment);
            }

            return RedirectToAction("ViewApartments");
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

        public ActionResult EditedApartment(Apartment apartment)
        {
            ModelState.Remove("DatesForIssues");
            if (!ModelState.IsValid)
            {
                WriteErrors();

                return View("EditApartment", apartment.Id);
            }

            string datesForIssueString = Request["DatesForIssues"];
            string[] datesStringArray = datesForIssueString.Split(',');
            var dateTimes = new List<CustomDate>();
            foreach (string item in datesStringArray)
            {
                try
                {
                    dateTimes.Add(new CustomDate(DateTime.ParseExact(item, "dd-mm-yyyy", CultureInfo.InvariantCulture)));
                }
                catch (Exception) { }
            }

            apartment.DatesForIssues = dateTimes;
            apartment.AvailableDates = dateTimes;

            using (var model = new Model())
            {
                foreach (var item in model.GetAmenities())
                {
                    if (Request[item.Name] == "on")
                    {
                        apartment.Amenities.Add(item);
                    }
                }

                model.EditApartment(apartment);
            }

            return RedirectToAction("ViewApartments");
        }

        public ActionResult DeleteApartment(int id)
        {
            using (var model = new Model())
            {
                model.RemoveApartment(id);
                return RedirectToAction("ViewApartments");
            }
        }

        public ActionResult Sort(string id)
        {
            var apartments = TempData["modelApa"] as List<Apartment>;

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
                    return View("ViewApartments", apartments.OrderBy(a => a.TimeOfCheckOut.TimeOfDay).ToList());
                case "CheckoutDsc":
                    ViewBag.CheckoutAsc = new object();
                    return View("ViewApartments", apartments.OrderByDescending(a => a.TimeOfCheckOut.TimeOfDay).ToList());
                default:
                    break;
            }

            return RedirectToAction("ViewApartments");
        }

        public ActionResult Search(string apartmentType, string minNumberOfRooms, string maxNumberOfRooms, string numberOfGuests, string minPricePerNight, string maxPricePerNight, string registrationTime, string checkoutTime, string status, string amenity)
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
                apartments = apartments.Where(i => i.ApartmentType == type).ToList();
                isChanged = true;
                ViewBag.apartmentType = type;
            }

            if (!string.IsNullOrEmpty(minNumberOfRooms))
            {
                int rooms = int.Parse(minNumberOfRooms);
                apartments = apartments.Where(i => i.NumberOfRooms >= rooms).ToList();
                isChanged = true;
                ViewBag.numberOfRoomsMin = rooms;
            }

            if (!string.IsNullOrEmpty(maxNumberOfRooms))
            {
                int rooms = int.Parse(maxNumberOfRooms);
                apartments = apartments.Where(i => i.NumberOfRooms <= rooms).ToList();
                isChanged = true;
                ViewBag.numberOfRoomsMax = rooms;
            }

            if (!string.IsNullOrEmpty(numberOfGuests))
            {
                int guests = int.Parse(numberOfGuests);
                apartments = apartments.Where(i => i.NumberOfGuests == guests).ToList();
                isChanged = true;
                ViewBag.numberOfGuests = guests;
            }

            if (!string.IsNullOrEmpty(minPricePerNight))
            {
                int price = int.Parse(minPricePerNight);
                apartments = apartments.Where(i => i.PricePerNight >= price).ToList();
                isChanged = true;
                ViewBag.pricePerNightMin = price;
            }

            if (!string.IsNullOrEmpty(maxPricePerNight))
            {
                int price = int.Parse(maxPricePerNight);
                apartments = apartments.Where(i => i.PricePerNight <= price).ToList();
                isChanged = true;
                ViewBag.pricePerNightMax = price;
            }

            if (!string.IsNullOrEmpty(registrationTime))
            {
                var regTime = new DateTime(1, 1, 1, int.Parse(registrationTime.Split(':').First()), int.Parse(registrationTime.Split(':').Last()), 0);
                apartments = apartments.Where(i => i.TimeOfRegistration >= regTime).ToList();
                isChanged = true;
                ViewBag.registrationTime = registrationTime;
            }

            if (!string.IsNullOrEmpty(checkoutTime))
            {
                var outTime = new DateTime(1, 1, 1, int.Parse(checkoutTime.Split(':').First()), int.Parse(checkoutTime.Split(':').Last()), 0);
                apartments = apartments.Where(i => i.TimeOfCheckOut <= outTime).ToList();
                isChanged = true;
                ViewBag.checkoutTime = checkoutTime;
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status != "None")
                {
                    var stat = (ApartmentStatus)Enum.Parse(typeof(ApartmentStatus), status, true);
                    apartments = apartments.Where(i => i.Status == stat).ToList();
                    isChanged = true;
                    ViewBag.status = status;
                }
            }

            if (!string.IsNullOrEmpty(amenity))
            {
                using (var model = new Model())
                {
                    var ameni = model.GetAmenity(int.Parse(amenity));
                    apartments = apartments.Where(i => i.Amenities.Contains(ameni)).ToList();
                    isChanged = true;
                    ViewBag.amenity = ameni.Id;
                }
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

    }
}