using Microsoft.AspNetCore.Mvc;
using TripsLog.Models;
using TripsLog.ViewModels;

namespace TripsLog.Controllers
{
    public class TripController : Controller
    {
        private readonly TripContext _context;

        public TripController(TripContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var trips = _context.Trips.ToList();
            ViewBag.SubHeader = "";
            return View(trips);
        }

        //page one destination and dates
        public IActionResult AddPage1()
        {
            ViewBag.SubHeader = "Add Trip Destination and Dates";
            return View();  
        }

        [HttpPost]
        public IActionResult AddPage1(TripDestinationViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Destination"] = model.Destination;
                TempData["StartDate"] = model.StartDate;
                TempData["EndDate"] = model.EndDate;
                return RedirectToAction("AddPage2");
            }
            return View(model);
        }

        //page two accommodation and contact info
        public IActionResult AddPage2()
        {
            ViewBag.SubHeader = $"Add info for {TempData["Destination"]}";
            TempData.Keep();
            return View();
        }

        [HttpPost]
        public IActionResult AddPage2(TripAccommodationViewModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["Accommodation"] = model.Accommodation;
                TempData["AccomodationPhone"] = model.AccommodationPhone;
                TempData["AccomodationEmail"] = model.AccommodationEmail;
                TempData.Keep();
                return RedirectToAction("AddPage3");
            }
            TempData.Keep();
            return View(model);
        }

        //page three activities
        public IActionResult AddPage3()
        {
            ViewBag.SubHeader = $"Add things to do in {TempData["Destination"]}";
            TempData.Keep();
            return View();
        }
        [HttpPost]
        public IActionResult AddPage3(TripActivitiesViewModel model)
        {
            var trip = new Trip
            {
                Destination = TempData["Destination"]?.ToString() ?? "",
                StartDate = Convert.ToDateTime(TempData["StartDate"]),
                EndDate = Convert.ToDateTime(TempData["EndDate"]),
                Accommodation = TempData["Accommodation"]?.ToString() ?? "",
                AccommodationPhone = TempData["AccommodationPhone"]?.ToString(),
                AccommodationEmail = TempData["AccommodationEmail"]?.ToString(),
                Activity1 = model.Activity1,
                Activity2 = model.Activity2,
                Activity3 = model.Activity3
            };

            _context.Trips.Add(trip);
            _context.SaveChanges();
            TempData.Clear();

            TempData["Message"] = $"Trip to {trip.Destination} added!";
            return RedirectToAction("Index");
        }

        public IActionResult Cancel()
        {
            TempData.Clear();
            return RedirectToAction("Index");
        }
    }
}
