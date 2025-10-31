using AppointmentsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsApp.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        //get /Appointment
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .OrderBy(a => a.Start)
                .ToListAsync();
            return View(appointments);
        }

        //get /Appointment/Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Customers = await _context.Customers
                .OrderBy(c => c.Username)
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.Username
                })
                .ToListAsync();
            return View(new Appointment
            {
                Start = DateTime.Today.AddDays(1).AddHours(9)
            });
        }
        
        //post /Appointment/Add
        [HttpPost]
        public async Task<IActionResult> Add(Appointment appointment)
        {
            async Task reloadCustomers()
            {
                ViewBag.Customers = await _context.Customers
                    .OrderBy(c => c.Username)
                    .Select(c => new SelectListItem { Value = c.CustomerId.ToString(), Text = c.Username })
                    .ToListAsync();
            }

            if (!ModelState.IsValid)
            {
                await reloadCustomers();
                return View(appointment);
            }

            //future date time
            if(appointment.Start <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(appointment.Start), "Appointment must be in the future!");
            }

            //exactly on the hour mark
            if(appointment.Start.Minute != 0 || appointment.Start.Second != 0 || appointment.Start.Millisecond != 0)
            {
                ModelState.AddModelError(nameof(appointment.Start), "Appointments must start exactly on the hour!");
            }

            //make sure customer is real
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == appointment.CustomerId);
            if (!customerExists)
            {
                ModelState.AddModelError(nameof(appointment.CustomerId), "Please select a valid customer");
            }

            //unique time slot
            bool slotTaken = await _context.Appointments.AnyAsync(a => a.Start == appointment.Start);
            if (slotTaken)
            {
                ModelState.AddModelError(nameof(appointment.Start), $"The {appointment.Start:g} time slot is already booked up!");
            }

            if (!ModelState.IsValid)
            {
                await reloadCustomers();
                return View(appointment);
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
