using Microsoft.AspNetCore.Mvc;

namespace Ch2Lab1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Name = "John Doe";
            ViewBag.Salary = 1000000;
            return View();
        }
    }
}
