using HungryOClockV2.Data;
using HungryOClockV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HungryOClockV2.Controllers
{
    [Authorize(Roles = "Admin")]
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

        //get edit category
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //post edit category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                ModelState.AddModelError(nameof(Category.Name), "Name is required");
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var existing = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = category.Name.Trim();
            await _context.SaveChangesAsync();
            TempData["message"] = "Category updated";
            return RedirectToAction(nameof(Index));
        }

        //post delete category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (int id)
        {
            var category = await _context.Categories
                .Include(c => c.RestaurantCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if(category == null)
            {
                TempData["message"] = "Category Not Found";
                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            TempData["message"] = "Category deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
