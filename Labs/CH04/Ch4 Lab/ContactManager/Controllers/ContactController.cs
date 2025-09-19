using ContactManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Controllers
{
    public class ContactController(ContactManagerContext db) : Controller
    {
        private SelectList CategoryList(int? selected = null) =>
            new(db.Categories.OrderBy(c => c.Name).ToList(), "CategoryId", "Name", selected);

        //list of contacts
        public async Task<IActionResult> Index()
        {
            var contacts = await db.Contacts
                .Include(c => c.Category)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
            return View(contacts);
        }

        public async Task<IActionResult> Details(int id, string? slug)
        {
            var contacts = await db.Contacts.Include(c => c.Category).FirstOrDefaultAsync(c => c.ContactId == id);

            if (contacts is null) return NotFound();

            if(string.IsNullOrWhiteSpace(slug) || slug != contacts.Slug)
            {
                return RedirectToAction(nameof(Details), new { id, slug = contacts.Slug });
            }

            return View(contacts);
        }

        //GET create a contact
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = CategoryList();
            return View("AddEdit", new Contact());
        }

        //GET edit a contact
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await db.Contacts.FindAsync(id);
            if (contact is null) return NotFound();

            ViewBag.Categories = CategoryList(contact.CategoryId);
            return View("AddEdit", contact);
        }

        //POST create or edit a contact
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Contact contact)
        {
            if (contact.ContactId == 0)
            {
                contact.DateAdded = DateTime.Now;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = CategoryList(contact.CategoryId);
                return View("AddEdit", contact);
            }

            if (contact.ContactId == 0)
            {
                db.Contacts.Add(contact);
            }
            else
            {
                db.Contacts.Update(contact);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET delete confirm
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var contact = await db.Contacts.Include(c => c.Category).FirstOrDefaultAsync(c => c.ContactId == id);
            if (contact is null) return NotFound();
            return View(contact);
        }

        //POST delete a contact
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await db.Contacts.FindAsync(id);
            if(contact is not null)
            {
                db.Contacts.Remove(contact);
                await db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
