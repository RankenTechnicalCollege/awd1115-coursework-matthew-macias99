using HungryOClockV2.Data;
using HungryOClockV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HungryOClockV2.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get category
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            return View(categories);
        }

        //get category create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //post category create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError(nameof(category.Name), "Name is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["message"] = "Category added!";
            return RedirectToAction(nameof(Index));
        }
    }
}
