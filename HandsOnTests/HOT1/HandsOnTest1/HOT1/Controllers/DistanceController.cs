using Microsoft.AspNetCore.Mvc;
using HOT1.Models;

namespace HOT1.Controllers
{
    public class DistanceController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new DistanceConverterModel());
        }

        [HttpPost]
        public IActionResult Index(DistanceConverterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Centimeters = Math.Round(model.Inches!.Value * 2.54, 2);
            return View(model);
        }
    }
}
