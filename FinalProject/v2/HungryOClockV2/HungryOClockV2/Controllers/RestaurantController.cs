using HungryOClockV2.Data;
using HungryOClockV2.Models;
using HungryOClockV2.Models.ViewModels;
using HungryOClockV2.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HungryOClockV2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get restaurant
        [AllowAnonymous]
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

            if (ModelState.IsValid)
            {
                restaurant.Slug = SlugHelper.GenerateSlug(model.Name);
            }

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            TempData["message"] = "Restaurant created successfully!";
            return RedirectToAction(nameof(Index));
        }

        //details
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(string slug, int menuPage = 1)
        {
            const int menuPageSize = 3;
            
            if (string.IsNullOrWhiteSpace(slug))
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(r => r.RestaurantCategories)
                .ThenInclude(rc => rc.Category)
                .Include(r => r.MenuItems)
                .Include(r => r.Reviews)
                .ThenInclude(rv => rv.User)
                .FirstOrDefaultAsync(r => r.Slug == slug);

            if(restaurant == null)
            {
                return NotFound();
            }

            var totalMenuItems = await _context.MenuItems
                .Where(mi => mi.RestaurantId == restaurant.RestaurantId)
                .CountAsync();

            var menuItems = await _context.MenuItems
                .Where(mi => mi.RestaurantId == restaurant.RestaurantId)
                .OrderBy(mi => mi.Name)
                .Skip((menuPage - 1) * menuPageSize)
                .Take(menuPageSize)
                .ToListAsync();

            var vm = new RestaurantDetailsVM
            {
                Restaurant = restaurant,
                MenuItems = menuItems,
                MenuPage = menuPage,
                MenuTotalPages = (int)Math.Ceiling(totalMenuItems / (double)menuPageSize)
            };

            return View(vm);
        }

        //get edit restaurant
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var restaurant = await _context.Restaurants.Include(r=>r.RestaurantCategories).FirstOrDefaultAsync(r=>r.RestaurantId == id);

            if(restaurant == null)
            {
                return NotFound();
            }

            var vm = new RestaurantCreateVM{
                RestaurantId = restaurant.RestaurantId,
                Name = restaurant.Name,
                PriceLevel = restaurant.PriceLevel,
                SelectedCategoryIds = restaurant.RestaurantCategories.Select(rc => rc.CategoryId).ToList()
            };

            vm.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = vm.SelectedCategoryIds.Contains(c.CategoryId)
                }).ToListAsync();

            return View(vm);
        }

        //post edit restaurant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestaurantCreateVM model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError(nameof(model.Name), "Name is required");
            }

            if(model.PriceLevel < 1 || model.PriceLevel > 4)
            {
                ModelState.AddModelError(nameof(model.PriceLevel), "Price level must be between 1 and 4");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await _context.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.Name,
                        Selected = model.SelectedCategoryIds.Contains(c.CategoryId)
                    }).ToListAsync();

                return View(model);
            }

            var restaurant = await _context.Restaurants.Include(r => r.RestaurantCategories).FirstOrDefaultAsync(r => r.RestaurantId ==  model.RestaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            restaurant.Name = model.Name.Trim();
            restaurant.PriceLevel = model.PriceLevel;

            var selectedIds = model.SelectedCategoryIds.Distinct().ToList();
            var existingIds = restaurant.RestaurantCategories.Select(rc => rc.CategoryId).ToList();

            var toRemove = restaurant.RestaurantCategories.Where(rc => !selectedIds.Contains(rc.CategoryId)).ToList();

            foreach (var rc in toRemove)
            {
                _context.RestaurantCategories.Remove(rc);
            }

            var toAddIds = selectedIds.Where(id => !existingIds.Contains(id)).ToList();

            foreach(var catId in toAddIds)
            {
                restaurant.RestaurantCategories.Add(new RestaurantCategory
                {
                    CategoryId = catId,
                    RestaurantId = restaurant.RestaurantId
                });
            }

            if (ModelState.IsValid)
            {
                restaurant.Slug = SlugHelper.GenerateSlug(model.Name);
                _context.Update(restaurant);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            TempData["message"] = "Restaurant updated!";
            return RedirectToAction(nameof(Index));
        }


        //post delete restaurant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.RestaurantCategories)
                .Include(r => r.MenuItems)
                .Include(r => r.Reviews)
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                TempData["message"] = "Restaurant not found";
                return RedirectToAction(nameof(Index));
            }

            if (restaurant.Reservations.Any())
            {
                TempData["message"] = "Cannont delete restaurant with reservations";
                return RedirectToAction(nameof(Index));
            }

            _context.RestaurantCategories.RemoveRange(restaurant.RestaurantCategories);
            _context.MenuItems.RemoveRange(restaurant.MenuItems);
            _context.Reviews.RemoveRange(restaurant.Reviews);
            _context.Reservations.RemoveRange(restaurant.Reservations);

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            TempData["message"] = $"{restaurant.Name} deleted successfully";
            return RedirectToAction(nameof(Index));
        }


    }
}
