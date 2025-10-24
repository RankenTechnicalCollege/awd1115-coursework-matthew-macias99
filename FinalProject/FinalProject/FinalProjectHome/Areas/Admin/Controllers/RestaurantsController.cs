using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectHome.Models;

namespace FinalProjectHome.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RestaurantsController : Controller
    {
        private readonly FinalContext _context;
        public RestaurantsController(FinalContext context)
        {
            _context = context;
        }

        private static IEnumerable<SelectListItem> PriceLevelItem =>
            Enumerable.Range(1, 4).Select(i => new SelectListItem(new string('$', i), i.ToString()));
        private static IEnumerable<SelectListItem> RatingItems =>
            Enumerable.Range(1, 5).Select(i => new SelectListItem(i.ToString(), i.ToString()));
        private void PopulateDropDowns()
        {
            ViewBag.PriceLevels = PriceLevelItem;
            ViewBag.Ratings = RatingItems;
        }
        private static string Slugify(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            var s = text.Trim().ToLower();
            s = System.Text.RegularExpressions.Regex.Replace(s, @"\s+", "-");
            s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-z0-9\-]", "");
            s = System.Text.RegularExpressions.Regex.Replace(s, @"-+", "-");
            return s.Trim('-');
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int pageNumber = 1, int pageSize = 5)
        {
            var query = _context.Restaurants.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r => r.Name.Contains(search) || r.Category.Contains(search));
            }

            query = query.OrderBy(r => r.Name);

            var total = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var vm = new PagedResult<Restaurant>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = total,
                Search = search
            };

            return View(vm);
        }

        //GET create restaurant
        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropDowns();
            return View(new Restaurant { PriceLevel = 2, Rating = 4 });
        }

        //POST create restaurant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropDowns();
                return View(restaurant);
            }

            _context.Add(restaurant);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Restaurant {restaurant.Name} created successfully";
            return RedirectToAction(nameof(Index));
        }

        //GET edit restaurant
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string? slug)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return NotFound();

            var expected = Slugify(restaurant.Name);
            if (!string.Equals(slug, expected, StringComparison.Ordinal))
            {
                return RedirectToAction(nameof(Edit), new { area = "Admin", id = restaurant.RestaurantId, slug = expected });
            }

            PopulateDropDowns();
            return View(restaurant);
        }

        //POST edit restaurant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Restaurant restaurant)
        {
            if (id != restaurant.RestaurantId) return NotFound();
            if (!ModelState.IsValid)
            {
                PopulateDropDowns();
                return View(restaurant);
            }
            try
            {
                _context.Update(restaurant);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Restaurant {restaurant.Name} updated successfully";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Restaurants.AnyAsync(r => r.RestaurantId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //GET delete restaurant
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string? slug)
        {
            if (id == null) return NotFound();
            var restaurant = await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RestaurantId == id);
            if (restaurant == null) return NotFound();

            var expected = Slugify(restaurant.Name);
            if (!string.Equals(slug, expected, StringComparison.Ordinal))
            {
                return RedirectToAction(nameof(Delete), new { area = "Admin", id = restaurant.RestaurantId, slug = expected });
            }

            return View(restaurant);
        }

        //POST delete restaurant
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = $"Restaurant {restaurant?.Name} deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
