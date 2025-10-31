using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointmentsApp.Models;

namespace AppointmentsApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        //get /Customer
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.OrderBy(c => c.Username).ToListAsync();
            return View(customers);
        }

        //get /Customer/Add
        [HttpGet]
        public IActionResult Add() => View(new Customer());

        //post /Customer/Add
        [HttpPost]
        public async Task<IActionResult> Add(Customer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool usernameTaken = await _context.Customers.AnyAsync(c => c.Username.ToLower() == model.Username.ToLower());

            if (usernameTaken)
            {
                ModelState.AddModelError(nameof(model.Username), "Username already taken :(");
                return View(model);
            }

            _context.Customers.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
