using HungryOClockV2.Data;
using HungryOClockV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HungryOClockV2.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        //get reservations for current user
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservations = await _context.Reservations
                .Include(r => r.Restaurant)
                .Where(r => r.UserId == userId)
                .OrderBy(r => r.ReservationDateTime)
                .ToListAsync();

            return View(reservations);
        }

        //get create resrvation
        [HttpGet]
        public async Task<IActionResult> Create(int restaurantId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);

            if (restaurant == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var reservation = new Reservation
            {
                RestaurantId = restaurantId,
                ReservationDateTime = DateTime.Now.AddHours(1), //one hour from now being default
                ContactEmail = user?.Email ?? ""
            };

            ViewBag.RestaurantName = restaurant.Name;
            return View(reservation);
        }

        //post create reservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == reservation.RestaurantId);

            ViewBag.RestaurantName = restaurant?.Name ?? "";

            if(reservation.ReservationDateTime <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(Reservation.ReservationDateTime), "Reservation time must be in the future");
            }

            if (string.IsNullOrWhiteSpace(reservation.ContactPhone))
            {
                ModelState.AddModelError(nameof(Reservation.ContactPhone), "Contact phone is required");
            }

            if (string.IsNullOrWhiteSpace(reservation.ContactEmail))
            {
                ModelState.AddModelError(nameof(Reservation.ContactEmail), "Contact email is required");
            }

            if (!ModelState.IsValid)
            {
                return View(reservation);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            reservation.UserId = userId;
            reservation.CreatedAt = DateTime.Now;

            _context.Reservations.Add(reservation);

            try
            {
                await _context.SaveChangesAsync();
                TempData["message"] = "Reservation created!";
                return RedirectToAction("Details", "Restaurant", new { id = reservation.RestaurantId });
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(nameof(Reservation.ReservationDateTime), "Time slot already booked! Try again!");
                return View(reservation);
            }
        }

        //get edit reservation
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Restaurant)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if(reservation == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (reservation.UserId != currentUserId)
            {
                return Forbid();
            }

            ViewBag.RestaurantName = reservation.Restaurant?.Name ?? "";
            return View(reservation);
        }

        //post edit reservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantId == reservation.RestaurantId);

            ViewBag.RestaurantName = restaurant?.Name ?? "";

            if (reservation.ReservationDateTime <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(Reservation.ReservationDateTime), "Reservation time must be in the future");
            }

            if (string.IsNullOrWhiteSpace(reservation.ContactPhone))
            {
                ModelState.AddModelError(nameof(Reservation.ContactPhone), "Contact phone is required");
            }

            if (string.IsNullOrWhiteSpace(reservation.ContactEmail))
            {
                ModelState.AddModelError(nameof(Reservation.ContactEmail), "Contact email is required");
            }

            if (!ModelState.IsValid)
            {
                return View(reservation);
            }

            var existing = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == reservation.ReservationId);

            if (existing == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (reservation.UserId != currentUserId)
            {
                return Forbid();
            }

            existing.ReservationDateTime = reservation.ReservationDateTime;
            existing.ContactPhone = reservation.ContactPhone;
            existing.ContactEmail = reservation.ContactEmail;
            existing.Accomodations = reservation.Accomodations;

            try
            {
                await _context.SaveChangesAsync();
                TempData["message"] = "Reservation Updated!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(nameof(Reservation.ReservationDateTime), "That time slot is booked! Try another!");
                return View(reservation);
            }
        }

        //post delete reservation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.ReservationId == id);

            if(reservation == null)
            {
                TempData["message"] = "Reservation not found";
                return RedirectToAction(nameof(Index));
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (reservation.UserId != currentUserId)
            {
                return Forbid();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            TempData["message"] = "Reservation cancelled";
            return RedirectToAction(nameof(Index));
        }

    }
}
