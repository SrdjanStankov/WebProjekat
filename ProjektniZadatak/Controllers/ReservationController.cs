using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProjektniZadatak.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewReservations()
        {
            using (var model = new Model())
            {
                if (Session["User"] is Guest)
                {
                    return View(new List<Reservation>(model.GetReservations((Session["User"] as User).Username)));
                }
                else if (Session["User"] is Host)
                {
                    return View(new List<Reservation>(model.GetReservations(Session["User"] as Host)));
                }
                else if (Session["User"] is Administrator)
                {
                    return View(new List<Reservation>(model.GetReservations()));
                }
                return View();
            }
        }

        public ActionResult Create(int id)
        {
            List<CustomDate> availableDates;
            DateTime minCustomDate;
            DateTime maxCustomDate;
            List<DateTime> disabledDates;

            using (var mod = new Model())
            {
                availableDates = new List<CustomDate>(mod.GetAvailableDates(id));
                minCustomDate = mod.FindMinDate(availableDates).Date;
                maxCustomDate = mod.FindMaxDate(availableDates).Date;
                disabledDates = mod.GetDatesNotActiveInRange(minCustomDate, maxCustomDate, availableDates);
            }

            ViewBag.apartmentId = id;
            ViewBag.minCustomDate = minCustomDate;
            ViewBag.maxCustomDate = maxCustomDate;
            ViewBag.dates = disabledDates;

            return View();
        }

        public ActionResult CheckReservation(string apartmentId, string registrationTime, int numberOfNights)
        {
            Apartment apartment;
            using (var model = new Model())
            {
                apartment = model.GetApartment(int.Parse(apartmentId));
                string[] regtimes = registrationTime.Split('-');
                var registrationStartDate = new DateTime(int.Parse(regtimes.Last()), int.Parse(regtimes[1]), int.Parse(regtimes.First()));
                var registration = new DateTime(int.Parse(regtimes.Last()), int.Parse(regtimes[1]), int.Parse(regtimes.First()));
                var length = apartment.AvailableDates.LastOrDefault().Date;
                var availableDates = apartment.AvailableDates.Select(s => s.Date);

                for (int i = 0; i < numberOfNights; i++, registration = registration.AddDays(1))
                {
                    bool found = false;
                    foreach (var item in availableDates)
                    {
                        if (item.Year == registration.Year && item.Month == registration.Month && item.Day == registration.Day)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        TempData["Days"] = "Unable to reserve apartment for given dates";
                        return RedirectToAction("Create", new { id = apartment.Id });
                    }
                }

                // create reservation
                var res = new Reservation(apartment, registrationStartDate, numberOfNights, apartment.PricePerNight * numberOfNights, Session["User"] as Guest, ReservationStatus.Created);
                model.AddReservation(res);
                return RedirectToAction("ViewApartments", "Apartment");
            }
        }

        public ActionResult AbortReservation(int id)
        {
            using (var model = new Model())
            {
                var reser = model.GetReservation(id);
                if (reser.Status == ReservationStatus.Accepted || reser.Status == ReservationStatus.Created)
                {
                    model.ChangeReservationStatus(id, ReservationStatus.Cancellation);
                }
            }
            return RedirectToAction("ViewReservations");
        }

        public ActionResult Accept(int id)
        {
            using (var model = new Model())
            {
                var reser = model.GetReservation(id);

                if (reser.Status == ReservationStatus.Created)
                {
                    model.ChangeReservationStatus(id, ReservationStatus.Accepted);
                }
            }

            return RedirectToAction("ViewReservations");
        }

        public ActionResult Reject(int id)
        {
            using (var model = new Model())
            {
                var reser = model.GetReservation(id);

                if (reser.Status == ReservationStatus.Created || reser.Status == ReservationStatus.Accepted)
                {
                    model.ChangeReservationStatus(id, ReservationStatus.Rejected);
                }
            }

            return RedirectToAction("ViewReservations");
        }

        public ActionResult End(int id)
        {
            using (var model = new Model())
            {
                var reser = model.GetReservation(id);

                model.ChangeReservationStatus(id, ReservationStatus.Completed);
            }

            return RedirectToAction("ViewReservations");
        }

        public ActionResult Search(string Username)
        {
            using (var model = new Model())
            {
                List<Reservation> reservations;
                if (Session["User"] is Host)
                {
                    reservations = new List<Reservation>(model.GetReservations(Session["User"] as Host));
                }
                else if (Session["User"] is Administrator)
                {
                    reservations = new List<Reservation>(model.GetReservations());
                }
                else (Session["User"] is Guest)
                {
                    reservations = new List<Reservation>(model.GetReservations((Session["User"] as User).Username));
                }

                if (!string.IsNullOrEmpty(Username))
                {
                    reservations = reservations.Where(r => r.Guest.Username == Username).ToList();
                }

                return View("ViewReservations", reservations);
            }
        }
    }
}