using Microsoft.AspNetCore.Mvc;
using Ch2Lab1.Models;

namespace Ch2Lab1.Controllers
{
    public class TipCalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Tip15 = 0m;
            ViewBag.Tip20 = 0m;
            ViewBag.Tip25 = 0m;
            return View(new TipCalculatorModel());
        }

        [HttpPost]
        public IActionResult Index(TipCalculatorModel model)
        {
            var amount = model?.Amount ?? 0m;

            ViewBag.Tip15 = amount * 0.15m;
            ViewBag.Tip20 = amount * 0.20m;
            ViewBag.Tip25 = amount * 0.25m;

            return View(model);  
        }
    }
}
