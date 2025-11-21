using HungryOClockV2.Data;
using HungryOClockV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HungryOClockV2.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get create review
        [HttpGet]
        public async Task<IActionResult> Create(int restaurantId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            ViewBag.RestaurantName = restaurant.Name;

            var review = new Review
            {
                RestaurantId = restaurantId,
                Rating = 4 //default rating
            };

            return View(review);
        }

        //post create review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (string.IsNullOrWhiteSpace(review.Title))
            {
                ModelState.AddModelError(nameof(Review.Title), "Title is required");
            }

            if (string.IsNullOrWhiteSpace(review.Content))
            {
                ModelState.AddModelError(nameof(Review.Content), "You must include some form of text description");
            }

            if (review.Rating < 1 || review.Rating > 4)
            {
                ModelState.AddModelError(nameof(Review.Rating), "Rating must be between 1 and 4.");
            }

            if (!ModelState.IsValid)
            {
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == review.RestaurantId);
                ViewBag.RestaurantName = restaurant?.Name ?? "";
                return View(review);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            review.UserId = userId;
            review.CreatedAt = DateTime.Now;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            await UpdateRestaurantAverageRating(review.RestaurantId);
            TempData["message"] = "Review Added";
            return RedirectToAction("Details", "Restaurant", new { id = review.RestaurantId });
        }

        //get edit review
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var review = await _context.Reviews.Include(r => r.Restaurant).FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(review.UserId != currentUserId)
            {
                return Forbid();
            }

            ViewBag.RestaurantName = review.Restaurant?.Name ?? "";
            return View(review);
        }

        //post edit review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Review review)
        {
            if (string.IsNullOrWhiteSpace(review.Title))
            {
                ModelState.AddModelError(nameof(Review.Title), "Title is required");
            }

            if (string.IsNullOrWhiteSpace(review.Content))
            {
                ModelState.AddModelError(nameof(Review.Content), "You must include some form of text description");
            }

            if (review.Rating < 1 || review.Rating > 4)
            {
                ModelState.AddModelError(nameof(Review.Rating), "Rating must be between 1 and 4.");
            }

            if (!ModelState.IsValid)
            {
                var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == review.RestaurantId);
                ViewBag.RestaurantName = restaurant?.Name ?? "";
                return View(review);
            }

            var existing = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == review.ReviewId);

            if(existing == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (review.UserId != currentUserId)
            {
                return Forbid();
            }

            existing.Title = review.Title;
            existing.Content = review.Content;
            existing.Rating = review.Rating;

            await _context.SaveChangesAsync();
            await UpdateRestaurantAverageRating(existing.RestaurantId);

            TempData["message"] = "Review Updated!";
            return RedirectToAction("Details", "Restaurant", new { id = existing.RestaurantId });
        }

        //post delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == id);

            if(review == null)
            {
                TempData["message"] = "Review not found";
                return RedirectToAction("Index", "Restaurant");
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (review.UserId != currentUserId)
            {
                return Forbid();
            }

            var restaurantId = review.RestaurantId;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            await UpdateRestaurantAverageRating(restaurantId);

            TempData["message"] = "Review deleted!";
            return RedirectToAction("Details", "Restaurant", new { id = restaurantId });
        }

        //recompute average rating
        private async Task UpdateRestaurantAverageRating(int restaurantId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.RestaurantId == restaurantId)
                .Select(r => r.Rating)
                .ToListAsync();

            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);

            if (restaurant == null) return;

            if(ratings.Count == 0)
            {
                restaurant.AverageRating = null;
            }
            else
            {
                restaurant.AverageRating = (decimal?)ratings.Average();
            }

            await _context.SaveChangesAsync();
        }
    }
}
