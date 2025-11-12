using Microsoft.AspNetCore.Mvc;
using TripLog2.Models.DataAccess;
using TripLog2.Models.DomainModels;

namespace TripLog2.Controllers
{
    public class AccomodationController : Controller
    {
        private Repository<Accomodation> data { get; set; }
        public AccomodationController(TripLog2Context ctx)
        {
            data = new Repository<Accomodation>(ctx);
        }
        public IActionResult Index()
        {
            var accomodations = data.List(new QueryOptions<Accomodation>
            {
                Where = a => a.AccomodationId > 0,
                OrderBy = a => a.Name
            });
            return View(accomodations);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var accomodation = data.Get(id);
            data.Delete(accomodation);
            try
            {
                data.Save();
                TempData["message"] = $"{accomodation.Name} deleted.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = $"{accomodation.Name} cannot be deleted because it is associated with a trip.";
                var accomodations = data.List(new QueryOptions<Accomodation>
                {
                    Where = a => a.AccomodationId > 0,
                    OrderBy = a => a.Name
                });
                return View("Index", accomodations);
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Accomodation());
        }

        [HttpPost]
        public IActionResult Add(Accomodation accomodation)
        {
            if(ModelState.IsValid)
            {
                data.Insert(accomodation);
                data.Save();
                TempData["message"] = $"{accomodation.Name} added.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(accomodation);
            }
        }
    }
}
