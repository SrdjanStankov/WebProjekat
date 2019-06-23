using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System.Collections.Generic;
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

        public ActionResult Create(int apartmentId = 0)
        {
            return View();
        }
    }
}