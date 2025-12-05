using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HOT3.Models;

namespace HOT3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly HOT3Context _context;
        public ProductsController(HOT3Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.OrderBy(p => p.Name).ToListAsync();
            return View(products);
        }

        //create

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }
            p.Slug = string.IsNullOrWhiteSpace(p.Slug) ? SlugHelper.Slugify(p.Name) : SlugHelper.Slugify(p.Slug);
            _context.Products.Add(p);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"{p.Name} has been added!";
            return RedirectToAction(nameof(Index));
        }

        //edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }
            p.Slug = string.IsNullOrWhiteSpace(p.Slug) ? SlugHelper.Slugify(p.Name) : SlugHelper.Slugify(p.Slug);
            _context.Products.Update(p);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"{p.Name} has been updated!";
            return RedirectToAction(nameof(Index));
        }

        //delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return NotFound();

            _context.Products.Remove(p);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Deleted {p.Name}";
            return RedirectToAction(nameof(Index));
        }
    }
}
