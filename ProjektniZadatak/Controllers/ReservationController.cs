using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
            int apartmentId = ViewBag.apartmentId;
            List<CustomDate> availableDates;
            DateTime minCustomDate;
            DateTime maxCustomDate;
            List<DateTime> disabledDates;

            using (var mod = new Model())
            {
                availableDates = new List<CustomDate>(mod.GetAvailableDates(apartmentId));
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
    }
}