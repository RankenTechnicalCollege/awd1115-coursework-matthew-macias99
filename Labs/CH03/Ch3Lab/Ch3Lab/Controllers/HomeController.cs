using Ch3Lab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ch3Lab.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.DiscountAmount = 0;
            ViewBag.Total = 0;
            return View();
        }

        [HttpPost]
        public IActionResult Index(PriceQuoteModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.DiscountAmount = model.DiscountAmount;
                ViewBag.Total = model.Total;
            }
            else
            {
                ViewBag.DiscountAmount = 0;
                ViewBag.Total = 0;
            }

            return View(model);
        }
    }
}
