using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOT3.Models;

namespace HOT3.Controllers
{
    public class HomeController : Controller
    {
        private readonly HOT3Context _context;
        public HomeController(HOT3Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? category)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(p => p.Category == category);
            }
            ViewBag.SelectedCategory = category;
            var products = await query.OrderBy(p => p.Name).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id, string? slug)
        {
            var p = await _context.Products.FindAsync(id);
            if (p ==null) return NotFound();
            return View(p);
        }
    }
}
