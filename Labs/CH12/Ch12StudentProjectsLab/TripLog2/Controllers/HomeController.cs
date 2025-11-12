using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TripLog2.Models;
using TripLog2.Models.DataAccess;
using TripLog2.Models.DomainModels;

namespace TripLog2.Controllers
{
    public class HomeController : Controller
    {
        private Repository<Trip> data { get; set; }

        public HomeController(TripLog2Context ctx)
        {
            data = new Repository<Trip>(ctx);
        }
        public IActionResult Index()
        {
            var options = new QueryOptions<Trip>
            {
                Includes = "Destination, Accomodation, Activities",
                OrderBy = t => t.StartDate!
            };

            var trips = data.List(options);
            return View(trips);
        }
    }
}
