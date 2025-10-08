using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectHome.Models;
using FinalProjectHome.ViewModels;

namespace FinalProjectHome.Controllers
{
    public class ReservationController : Controller
    {
        private readonly FinalContext _context;
        public ReservationController(FinalContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var items = await _context.Reservations
                .Include(r => r.Restaurant)
                .OrderByDescending(r => r.ReservationId)
                .ToListAsync();
            return View(items);
        }

        //step 1 select date, time, and restaurant
        public IActionResult CreateStep1()
        {
            var vm = new ReservationStep1VM
            {
                Date = DateTime.Today,
                Time = new TimeSpan(18, 0, 0),
                Restaurants = _context.Restaurants
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem { Value = r.RestaurantId.ToString(), Text = r.Name })
                    .ToList()
            };

            ViewBag.SubHeader = "Choose a restaurant and time";
            return View(vm);
        }

        [HttpPost]
        public IActionResult CreateStep1(ReservationStep1VM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Restaurants = _context.Restaurants
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem { Value = r.RestaurantId.ToString(), Text = r.Name })
                    .ToList();
                ViewBag.SubHeader = "Choose a restaurant and time";
                return View(vm);
            }

            TempData["RestaurantId"] = vm.RestaurantId;
            TempData["Date"] = vm.Date;
            TempData["Time"] = vm.Time.ToString();
            TempData.Keep();

            return RedirectToAction(nameof(CreateStep2));
        }

        //step 2 party size
        public IActionResult CreateStep2()
        {
            TempData.Keep();
            ViewBag.SubHeader = "Party size";
            return View(new ReservationStep2VM());
        }
        [HttpPost]
        public IActionResult CreateStep2(ReservationStep2VM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData.Keep();
                ViewBag.SubHeader = "Party size";
                return View(vm);
            }

            TempData["PartySize"] = vm.PartySize;
            TempData.Keep();

            return RedirectToAction(nameof(CreateStep3));
        }

        //step 3 contact info + notes
        public IActionResult CreateStep3()
        {
            TempData.Keep();
            ViewBag.SubHeader = "Contact & accommodations";
            return View(new ReservationStep3VM());
        }

        [HttpPost]
        public async Task<IActionResult> CreateStep3(ReservationStep3VM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData.Keep();
                ViewBag.SubHeader = "Contact & accommodations";
                return View(vm);
            }

            var reservation = new Reservation
            {
                RestaurantId = Convert.ToInt32(TempData["RestaurantId"]),
                Date = Convert.ToDateTime(TempData["Date"]),
                Time = TimeSpan.Parse((string)TempData["Time"]),
                PartySize = Convert.ToInt32(TempData["PartySize"]),
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Notes = vm.Notes
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            TempData.Clear();
            TempData["Success"] = "Reservation created successfully!";  
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cancel()
        {
            TempData.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}
