using Microsoft.AspNetCore.Mvc;
using Ch2Lab1.Models;

namespace Ch2Lab1.Controllers
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
