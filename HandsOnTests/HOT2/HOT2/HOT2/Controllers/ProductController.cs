using HOT2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HOT2.Controllers
{
    public class ProductController : Controller
    {
        private readonly SalesOrderContext _context;

        public ProductController(SalesOrderContext context)
        {
            _context = context;
        }

        private static readonly string[] CategoryNames =
        [
            "Accessories", "Bikes", "Clothing", "Components", "Car racks"
        ];

        private static SelectList BuildCategorySelectList(string? selected = null) =>
            new SelectList(CategoryNames.Select(n => new { Value = n, Text = n }),
                   "Value", "Text", selected);

        //product list
        [Route("product/list")]
        public async Task<IActionResult> List()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products);
        }

        //delete product
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var productToDelete = await _context.Products.FindAsync(id);
            return View(productToDelete);
        }

        [HttpPost]
        [Route("product/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productToDelete = await _context.Products.FindAsync(id);
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("List", "Product");
        }

        //add & edit product
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            if(id == 0)
            {
                ViewBag.Operation = "Add";
                var model = new Product
                {
                    Category = new Category()
                };
                ViewBag.Categories = BuildCategorySelectList();
                return View(model);
            }
            ViewBag.Operation = "Edit";
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Category ??= new Category { ProductId = product.ProductId};
            ViewBag.Categories = BuildCategorySelectList(product.Category.CategoryName);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEdit(Product product)
        {
            ViewBag.Operation = product.ProductId == 0 ? "Add" : "Edit";

             if (!ModelState.IsValid)
            {
                var selected = product.Category?.CategoryName;
                ViewBag.Categories = BuildCategorySelectList(selected);
                product.Category ??= new Category();
                return View(product);
            }
             if (product.ProductId == 0)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var categoryName = product.Category?.CategoryName;
                if(!string.IsNullOrWhiteSpace(categoryName))
                {
                    var category = new Category
                    {
                        ProductId = product.ProductId,
                        CategoryName = categoryName.Trim()
                    };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                _context.Products.Update(product);

                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.ProductId == product.ProductId);
                var categoryName = product.Category?.CategoryName?.Trim();

                if(existingCategory == null && !string.IsNullOrWhiteSpace(categoryName))
                {
                    _context.Categories.Add(new Category
                    {
                        ProductId = product.ProductId,
                        CategoryName = categoryName!.Trim()
                    });
                }else if(existingCategory != null)
                {
                    existingCategory.CategoryName = categoryName ?? existingCategory.CategoryName;
                    _context.Categories.Update(existingCategory);
                }
                await _context.SaveChangesAsync();
            }
                return RedirectToAction("List", "Product");
        }

    }
}
