using Microsoft.AspNetCore.Mvc;

namespace Ch7Lab.Areas.Help.Controllers
{
    [Area("Help")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Help";
            return View();
        }
    }
}
