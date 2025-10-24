using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinalProjectHome.Models;
using System.Text.RegularExpressions;

namespace FinalProjectHome.Controllers
{
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

        //GET restaurant list
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
            var items = await query.Skip((pageNumber -1) * pageSize)
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

        //GEt restaurant details (/restaurants/details/{id}/{slug})
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string? slug)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null) return NotFound();

            var expected = Slugify(restaurant.Name);
            if (!string.Equals(slug, expected, StringComparison.Ordinal))
            {
                return RedirectToRoute("restaurant-details", new { id = restaurant.RestaurantId, slug = expected });
            }
            return View(restaurant);
        }

        
    }
}
