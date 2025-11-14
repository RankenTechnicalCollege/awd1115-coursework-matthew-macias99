using System.Diagnostics;
using HungryOClockV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace HungryOClockV2.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }
    }
}
