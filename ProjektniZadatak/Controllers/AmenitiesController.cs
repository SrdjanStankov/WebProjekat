﻿using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProjektniZadatak.Controllers
{
    public class AmenitiesController : Controller
    {
        // GET: Amenities
        public ActionResult Index()
        {
            using (var model = new Model())
            {
                return View(new List<Amenities>(model.GetAmenities()));
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateAmenity(Amenities amenities)
        {
            if (string.IsNullOrEmpty(amenities.Name))
            {
                WriteErrors();

                return View("Create");
            }

            using (var model = new Model())
            {
                model.AddAmenity(amenities);
                return RedirectToAction("Index");
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

        public ActionResult Edit(int id)
        {
            using (var model = new Model())
            {
                return View(model.GetAmenity(id));
            }
        }

        public ActionResult EditAmenity(Amenities amenities)
        {
            if (string.IsNullOrEmpty(amenities.Name))
            {
                WriteErrors();

                return View("Edit");
            }

            using (var model = new Model())
            {
                var ameni = model.Amenities.SingleOrDefault(s => s.Id == amenities.Id && s.IsDeleted == false);
                model.Entry(ameni).CurrentValues.SetValues(amenities);
                model.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int id)
        {
            using (var model = new Model())
            {
                model.RemoveAmenity(id);
                return RedirectToAction("Index");
            }
        }

    }
}