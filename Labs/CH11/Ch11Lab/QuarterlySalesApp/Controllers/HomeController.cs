using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarterlySalesApp.Models;
using QuarterlySalesApp.ViewModels;

namespace QuarterlySalesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            var vm = new HomeIndexVM
            {
                SelectedEmployeeId = id,
                Employees = await _context.Employees
                    .OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
                    .Select(e => new SelectListItem { Value = e.EmployeeId.ToString(), Text = e.FullName })
                    .ToListAsync()
            };

            var q = _context.Sales.Include(s => s.Employee).AsQueryable();
            if (id.HasValue) q = q.Where(s => s.EmployeeId == id.Value);

            vm.Sales = await q.OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Quarter)
                .ToListAsync();

            return View(vm);
        }
    }
}
