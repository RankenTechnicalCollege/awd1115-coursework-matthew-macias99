using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarterlySalesApp.Models;

namespace QuarterlySalesApp.Controllers
{
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;
        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await LoadEmployees();
            return View(new Sale { Quarter = 1, Year = DateTime.Today.Year });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Sale m)
        {
            if (!ModelState.IsValid)
            {
                await LoadEmployees();
                return View(m);
            }

            //employee, year, and quarter must be uniquee
            bool duplicate = await _context.Sales.AnyAsync(s => s.EmployeeId == m.EmployeeId && s.Year == m.Year && s.Quarter == m.Quarter);

            if (duplicate)
            {
                ModelState.AddModelError("", $"Sales for this employee for {m.Year} Q{m.Quarter} are already in the database");
                await LoadEmployees();
                return View(m);
            }

            _context.Add(m);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home", new {id = m.EmployeeId});
        }

        private async Task LoadEmployees()
        {
            ViewBag.Employees = await _context.Employees
                .OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
                .Select(e => new SelectListItem { Value = e.EmployeeId.ToString(), Text = e.FullName})
                .ToListAsync();
        }
    }
}
