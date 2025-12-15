using System.Diagnostics;
using HungryOClockV2.Data;
using HungryOClockV2.Models;
using HungryOClockV2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HungryOClockV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchTerm, int? categoryId, int page = 1)
        {
            const int pageSize = 9;
            
            var query = _context.Restaurants
                .Include(r => r.RestaurantCategories)
                .ThenInclude(rc => rc.Category)
                .Include(r => r.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim().ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(term));
            }

            if(categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(r => r.RestaurantCategories.Any(rc => rc.CategoryId == categoryId.Value));
            }

            var totalRestaurants = await query.CountAsync();

            var restaurants = await query
                .OrderBy(r => r.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = (categoryId.HasValue && categoryId.Value == c.CategoryId)
                })
                .ToListAsync();

            categories.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "All Categories"
            });

            var vm = new HomeIndexVM
            {
                SearchTerm = searchTerm,
                CategoryId = categoryId,
                Categories = categories,
                Restaurants = restaurants,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalRestaurants / (double)pageSize)
            };

            return View(vm);
        }
    }
}
