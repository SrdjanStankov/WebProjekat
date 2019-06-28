using ProjektniZadatak.Models;
using ProjektniZadatak.Models.Databse;
using System.Linq;
using System.Web.Mvc;

namespace ProjektniZadatak.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Add(int id)
        {
            TempData["ApartmentIdForComment"] = id;
            return View();
        }

        public ActionResult AddComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                WriteErrors();

                return View("Add");
            }

            using (var model = new Model())
            {
                comment.GuestThaWroteComment = Session["User"] as Guest;
                var apId = TempData["ApartmentIdForComment"];
                model.AddComment(comment, int.Parse(apId.ToString()));
                return RedirectToAction("ViewReservations", "Reservation");
            }
        }

        public ActionResult Show(int id)
        {
            using (var model = new Model())
            {
                return View(model.GetComments(id).ToList());
            }
        }

        public ActionResult AllowShowing(int id)
        {
            using (var model = new Model())
            {
                model.SetShowOfComment(id, true);
                var comment = model.GetComment(id);
                return RedirectToAction("Show", new { id = comment.Apartment.Id });
            }
        }

        public ActionResult NotAllowShowing(int id)
        {
            using (var model = new Model())
            {
                model.SetShowOfComment(id, false);
                var comment = model.GetComment(id);
                return RedirectToAction("Show", new { id = comment.Apartment.Id });
            }
        }

        public ActionResult ViewComments(int id)
        {
            using (var model = new Model())
            {
                return View(model.GetComments(id, true).ToList());
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