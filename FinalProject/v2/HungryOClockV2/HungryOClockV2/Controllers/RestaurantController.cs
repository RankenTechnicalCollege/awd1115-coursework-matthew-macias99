using HungryOClockV2.Data;
using HungryOClockV2.Models;
using HungryOClockV2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HungryOClockV2.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get restaurant
        public async Task<IActionResult> Index()
        {
            var restaurants = await _context.Restaurants
                .Include(r => r.RestaurantCategories)
                .ThenInclude(rc => rc.Category)
                .OrderBy(r => r.Name)
                .ToListAsync();
            return View(restaurants);
        }

        //get create restaurant
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new RestaurantCreateVM
            {
                Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToListAsync()
            };

            return View(vm);
        }

        //post create restaurant
        [HttpPost]
        public async Task<IActionResult> Create(RestaurantCreateVM model)
        {
            if(string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "Restaurant name is required.");
            }

            if(model.PriceLevel < 1 || model.PriceLevel > 4)
            {
                ModelState.AddModelError(nameof(model.PriceLevel), "Price level must be between 1 and 4.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await _context.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.Name
                    })
                    .ToListAsync();
                return View(model);
            }

            var restaurant = new Restaurant
            {
                Name = model.Name.Trim(),
                PriceLevel = model.PriceLevel
            };

            foreach(var catId in model.SelectedCategoryIds.Distinct())
            {
                restaurant.RestaurantCategories.Add(new RestaurantCategory
                {
                    CategoryId = catId
                });
            }

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            TempData["message"] = "Restaurant created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.RestaurantCategories)
                .ThenInclude(rc => rc.Category)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if(restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }
    }
}
