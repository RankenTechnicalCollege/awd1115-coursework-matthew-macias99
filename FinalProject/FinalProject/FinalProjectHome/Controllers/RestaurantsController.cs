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
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurants
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .ToListAsync();
            return View(restaurants);
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
