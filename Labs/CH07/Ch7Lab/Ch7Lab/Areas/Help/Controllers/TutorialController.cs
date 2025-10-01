using Microsoft.AspNetCore.Mvc;

namespace Ch7Lab.Areas.Help.Controllers
{
    [Area("Help")]
    public class TutorialController : Controller
    {
        public IActionResult Index(string? id)
        {
            var viewName = (id ?? "").ToLowerInvariant() switch
            {
                "page2" => "Page2",
                "page3" => "Page3",
                _ => "Page1"
            };
            ViewData["Title"] = $"Tutorial {viewName.Replace("Page", "Page ")}";
            return View(viewName);
        }
    }
}
