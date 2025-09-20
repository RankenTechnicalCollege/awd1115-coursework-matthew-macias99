using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using FinalProjectHome.Models;

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

        //GET restaurants
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurants
                .AsNoTracking()
                .OrderBy(r => r.Name)
                .ToListAsync();
            return View(restaurants);
        }

        //GEt restaurant details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null) return NotFound();
            return View(restaurant);
        }

        //GET create restaurant
        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropDowns();
            return View(new Restaurant { PriceLevel = 2, Rating = 4});
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
            return RedirectToAction(nameof(Index));
        }

        //GET edit restaurant
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return NotFound();

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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var restaurant = await _context.Restaurants
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RestaurantId == id);
            if (restaurant == null) return NotFound();
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

            return RedirectToAction(nameof(Index));
        }
    }
}
