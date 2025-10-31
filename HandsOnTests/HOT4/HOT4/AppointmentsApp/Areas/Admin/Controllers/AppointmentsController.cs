using AppointmentsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;
        public AppointmentsController(AppDbContext Context)
        {
            _context = Context;
        }

        //get /Admin/Appointments
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .OrderBy(a => a.Start)
                .ToListAsync();
            return View(appointments);
        }

        private async Task LoadCustomersAsync()
        {
            ViewBag.Customers = await _context.Customers
                .OrderBy(c => c.Username)
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.Username
                })
                .ToListAsync();
        }

        //get /Admin/Appointmnet/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var appointments = await _context.Appointments
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointments == null) return NotFound();

            await LoadCustomersAsync();
            return View(appointments);
        }

        //post /Admin/Appointments/Edit
        [HttpPost]
        public async Task <IActionResult> Edit(int id, AppointmentsApp.Models.Appointment appointment)
        {
            if (id != appointment.AppointmentId) return NotFound();

            async Task redisplay()
            {
                await LoadCustomersAsync();
                ViewData["Title"] = "Edit Appointment";
            }

            if (!ModelState.IsValid)
            {
                await redisplay();
                return View(appointment);
            }

            //future date time check
            if (appointment.Start <= DateTime.Now)
            {
                ModelState.AddModelError(nameof(appointment.Start), "Appointment must be set in the future!");
            }

            //exactly on the hour check
            if (appointment.Start.Minute != 0 || appointment.Start.Second != 0 || appointment.Start.Millisecond != 0)
            {
                ModelState.AddModelError(nameof(appointment.Start), "Appointments must start on the hour!");
            }

            //customer exists check
            bool customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == appointment.CustomerId);
            if (!customerExists)
            {
                ModelState.AddModelError(nameof(appointment.CustomerId), "Please select a valid customer");
            }

            //unique time slot check
            bool slotTaken = await _context.Appointments.AnyAsync(a => a.AppointmentId != appointment.AppointmentId && a.Start == appointment.Start);
            if (slotTaken)
            {
                ModelState.AddModelError(nameof(appointment.Start), $"The {appointment.Start:g} time slot is already taken");
            }

            if (!ModelState.IsValid)
            {
                await redisplay();
                return View(appointment);
            }

            //save appointment
            try
            {
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Appointments.AnyAsync(a => a.AppointmentId == appointment.AppointmentId))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        //get /Admin/Appointments/Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.Include(a => a.Customer).FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null) return NotFound();
            return View(appointment);
        }

        //post /Admin/Appointments/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
