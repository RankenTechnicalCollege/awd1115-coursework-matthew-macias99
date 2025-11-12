using Microsoft.AspNetCore.Mvc;
using TripLog2.Models.DataAccess;
using TripLog2.Models.DomainModels;

namespace TripLog2.Controllers
{
    public class DestinationController : Controller
    {
        private Repository<Destination> data { get; set; }

        public DestinationController(TripLog2Context ctw)
        {
            data = new Repository<Destination>(ctw);
        }
        public IActionResult Index()
        {
            var destinations = data.List(new QueryOptions<Destination>
            {
                OrderBy = d => d.Name
            });
            return View(destinations);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Destination());
        }

        [HttpPost]
        public IActionResult Add(Destination model)
        {
            if (ModelState.IsValid)
            {
                data.Insert(model);
                data.Save();
                TempData["message"] = $"{model.Name} added.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var destination = data.Get(id);
            data.Delete(destination);
            try
            {
                data.Save();
                TempData["message"] = $"{destination.Name} deleted.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = $"Unable to delete {destination.Name} because it's associated with a trip.";
                return View("Index");
            }
        }
    }
}
