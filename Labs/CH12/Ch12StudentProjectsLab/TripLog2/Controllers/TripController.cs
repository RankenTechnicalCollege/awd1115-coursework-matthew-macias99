using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TripLog2.Models.DataAccess;
using TripLog2.Models.DomainModels;
using TripsLog.Migrations;

namespace TripLog2.Controllers
{
    public class TripController : Controller
    {

        private Repository<Trip> tripData { get; set; }
        private Repository<Destination> destinationData { get; set; }
        private Repository<Accomodation> accomodationData { get; set; }
        private Repository<Activity> activityData { get; set; }

        public TripController(TripLog2Context ctx)
        {
            tripData = new Repository<Trip>(ctx);
            destinationData = new Repository<Destination>(ctx);
            accomodationData = new Repository<Accomodation>(ctx);
            activityData = new Repository<Activity>(ctx);
        }

        //add pt1
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Destinations = destinationData.List(new QueryOptions<Destination> { OrderBy = d => d.Name })
                .Select(d => new SelectListItem { Value = d.DestinationId.ToString(), Text = d.Name })
                .ToList();

            ViewBag.Accomodations = accomodationData.List(new QueryOptions<Accomodation> { OrderBy = a => a.Name })
                .Select(a => new SelectListItem { Value = a.AccomodationId.ToString(), Text = a.Name })
                .ToList();

            return View(new Trip());
        }

        [HttpPost]
        public IActionResult Add(Trip trip)
        {
            if (trip.DestinationId == 0)
            {
                ModelState.AddModelError(nameof(Trip.DestinationId), "You must select a destination.");
            }
            if (trip.AccomodationId == 0)
            {
                ModelState.AddModelError(nameof(Trip.AccomodationId), "You must select an accomodation.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Destinations = destinationData.List(new QueryOptions<Destination> { OrderBy = d => d.Name })
                .Select(d => new SelectListItem { Value = d.DestinationId.ToString(), Text = d.Name })
                .ToList();

                ViewBag.Accomodations = accomodationData.List(new QueryOptions<Accomodation> { OrderBy = a => a.Name })
                    .Select(a => new SelectListItem { Value = a.AccomodationId.ToString(), Text = a.Name })
                    .ToList();
                return View(trip);
            }

            return RedirectToAction(nameof(AddActivities), new
            {
                d_id = trip.DestinationId,
                acc_id = trip.AccomodationId,
                start = trip.StartDate?.ToString("yyyy-MM-dd"),
                end = trip.EndDate?.ToString("yyyy-MM-dd")
            });
        }

        //add pt2
        [HttpGet]
        public IActionResult AddActivities(int d_id, int acc_id, DateTime? start, DateTime? end)
        {
            if(d_id == 0 || acc_id == 0 || start == null || end == null)
            {
                return RedirectToAction(nameof(Add));
            }

            ViewBag.Activities = activityData.List(new QueryOptions<Activity> { OrderBy = a => a.Name })
                .Select(a => new SelectListItem { Value = a.ActivityId.ToString(), Text = a.Name })
                .ToList();
            ViewBag.DestinationName = destinationData.Get(d_id)?.Name ?? "";

            return View(new AddActivitiesCarrier
            {
                DestinationId = d_id,
                AccomodationId = acc_id,
                StartDate = start,
                EndDate = end
            });
        }

        [HttpPost]
        public IActionResult AddActivities(AddActivitiesCarrier model, List<int> selectedActivityIds)
        {
            var trip = new Trip
            {
                DestinationId = model.DestinationId,
                AccomodationId = model.AccomodationId,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            if(selectedActivityIds?.Any() == true)
            {
                trip.Activities = activityData.List(new QueryOptions<Activity>
                {
                    Where = a => selectedActivityIds.Contains(a.ActivityId)
                }).ToList();
            }

            tripData.Insert(trip);
            tripData.Save();

            var dest = destinationData.Get(model.DestinationId);
            TempData["message"] = $"Trip to {dest?.Name ?? "destination"} added successfully.";
            return RedirectToAction("Index", "Home");
        }

        //delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var trip = tripData.Get(new QueryOptions<Trip>
            {
                Where = t => t.TripId == id,
                Includes = "Activities"
            });

            if(trip == null)
            {
                TempData["message"] = "Trip not found.";
                return RedirectToAction("Index", "Home");
            }

            tripData.Delete(trip);
            try
            {
                tripData.Save();
                TempData["message"] = "Trip deleted.";
            }
            catch
            {
                TempData["message"] = "Trip could not be deleted.";
            }
            return RedirectToAction("Index", "Home");
        }

        public class AddActivitiesCarrier
        {
            public int DestinationId { get; set; }
            public int AccomodationId { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
