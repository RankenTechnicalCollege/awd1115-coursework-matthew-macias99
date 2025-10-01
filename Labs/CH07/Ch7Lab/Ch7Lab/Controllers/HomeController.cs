using System.Diagnostics;
using Ch7Lab.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ch7Lab.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "About";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Title"] = "Contact";

            var contact = new Dictionary<string, string>
            {
                ["Phone"] = "314-555-5555",
                ["Email"] = "matthew@email.com",
                ["Website"] = "www.matthew.com"
            };

            return View(contact);
        }
    }
}
