using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarterlySalesApp.Models;

namespace QuarterlySalesApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            await LoadManagers();
            return View(new Employee());
        }

        [HttpPost]
        public async Task<IActionResult> Add(Employee m)
        {
            if (!ModelState.IsValid)
            {
                await LoadManagers();
                return View(m);
            }

            //date of birth check
            if (m.DOB >= DateTime.Today)
            {
                ModelState.AddModelError(nameof(m.DOB), "Date of birth must be in the past");
            }

            //date of hire checks
            if (m.DateOfHire >= DateTime.Today)
            {
                ModelState.AddModelError(nameof(m.DateOfHire), "Date of hire must be in the past");
            }

            if (m.DateOfHire < new DateTime(1995, 1, 1))
            {
                ModelState.AddModelError(nameof(m.DateOfHire), "Date of hire can't be before 1/1/1995");
            }

            //same person can't exist twice
            bool exists = await _context.Employees.AnyAsync(e => e.FirstName == m.FirstName && e.LastName == m.LastName && e.DOB == m.DOB);
            if (exists)
            {
                ModelState.AddModelError("", "An employee with the same name and date of birth cannot be added twice");
            }

            //employee and manager cant be the same person
            if (m.ManagerId.HasValue)
            {
                var manager = await _context.Employees.FindAsync(m.ManagerId.Value);

                if(manager != null && manager.FirstName == m.FirstName && manager.LastName == m.LastName && manager.DOB == m.DOB)
                {
                    ModelState.AddModelError(nameof(m.ManagerId), "Manager and employee can't be the same person");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadManagers();
                return View(m);
            }

            _context.Add(m);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task LoadManagers()
        {
            ViewBag.Managers = await _context.Employees
                .OrderBy(e => e.LastName).ThenBy(e => e.FirstName)
                .Select(e => new SelectListItem { Value = e.EmployeeId.ToString(), Text = e.FullName})
                .ToListAsync();
            ViewBag.Managers.Insert(0, new SelectListItem { Value = "", Text = "(none)" });
        }
    }
}
