using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HungryOClockV2.Models;

namespace HungryOClockV2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //view users
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var adminIds = admins.Select(u => u.Id).ToHashSet();

            ViewBag.AdminIds = adminIds;
            return View(users);
        }

        //delete users
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        //add user to admin
        [HttpPost]
        public async Task<IActionResult> AddToAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            return RedirectToAction("Index");
        }

        //remove user from admin role
        [HttpPost]
        public async Task<IActionResult> RemoveFromAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
            return RedirectToAction("Index");
        }
    }
}
