using HungryOClockV2.Data;
using HungryOClockV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HungryOClockV2.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get menu item create
        [HttpGet]
        public async Task<IActionResult> Create(int restaurantId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            ViewBag.RestuarantName = restaurant.Name;

            var menuItem = new MenuItem
            {
                RestaurantId = restaurantId
            };

            return View(menuItem);
        }

        //post menu item create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            if (string.IsNullOrWhiteSpace(menuItem.Name))
            {
                ModelState.AddModelError(nameof(MenuItem.Name), "Name is required");
            }

            if(menuItem.Price <= 0)
            {
                ModelState.AddModelError(nameof(MenuItem.Price), "Price must be greater than 0");
            }

            if (!ModelState.IsValid)
            {
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == menuItem.RestaurantId);
                ViewBag.RestaurantName = restaurant?.Name ?? "";
                return View(menuItem);
            }

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
            TempData["message"] = "Menu item added!";
            return RedirectToAction("Details", "Restaurant", new {id = menuItem.RestaurantId});
        }

        //get edit menu item
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await _context.MenuItems.Include(mi => mi.Restaurant).FirstOrDefaultAsync(mi => mi.MenuItemId == id);

            if(menuItem == null)
            {
                return NotFound();

            }

            ViewBag.RestaurantName = menuItem.Restaurant?.Name ?? "";
            return View(menuItem);
        }

        //post edit menu item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuItem menuItem)
        {
            if (string.IsNullOrWhiteSpace(menuItem.Name))
            {
                ModelState.AddModelError(nameof(MenuItem.Name), "Name is required");
            }

            if(menuItem.Price <= 0)
            {
                ModelState.AddModelError(nameof(MenuItem.Price), "Price must be greater than zero");
            }

            if (!ModelState.IsValid)
            {
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == menuItem.RestaurantId);
                ViewBag.RestaurantName = restaurant?.Name ?? "";
                return View(menuItem);
            }

            var existing = await _context.MenuItems.FirstOrDefaultAsync(mi => mi.MenuItemId == menuItem.MenuItemId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = menuItem.Name;
            existing.Price = menuItem.Price;
            existing.Description = menuItem.Description;
            existing.ImageUrl = menuItem.ImageUrl;

            await _context.SaveChangesAsync();
            TempData["message"] = "Menu item updated!";
            return RedirectToAction("Details", "Restaurant", new { id = existing.RestaurantId });
        }

        //post delete menu item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _context.MenuItems.FirstOrDefaultAsync(mi => mi.MenuItemId == id);

            if (menuItem == null)
            {
                TempData["message"] = "Menu item not found";
                return RedirectToAction("Index", "Restaurant");
            }

            var restaurantId = menuItem.RestaurantId;

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            TempData["message"] = "Menu item deleted";
            return RedirectToAction("Details", "Restaurant", new {id = restaurantId});
        }
       
    }
}
