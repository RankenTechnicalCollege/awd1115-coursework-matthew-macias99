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
        [HttpGet("product/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productToDelete = await _context.Products.FindAsync(id);
            return View(productToDelete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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

        //GET add
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = BuildCategorySelectList();
            var model = new Product() { Category = new Category() };
            return View("AddEdit", model);  
        }

        //GET edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);

            if (product is null) return NotFound();

            product.Category ??= new Category { ProductId = product.ProductId };
            ViewBag.Categories = BuildCategorySelectList(product.Category.CategoryName);
            return View("AddEdit", product);
        }

        //POST save (add or edit)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Product product)
        {
            var catName = product.Category?.CategoryName;
            product.Category = null;


            if (!ModelState.IsValid)
            {
                var selected = product.Category?.CategoryName;
                ViewBag.Categories = BuildCategorySelectList(selected);
                product.Category ??= new Category();
                return View("AddEdit", product);    
            }

            if (product.ProductId == 0)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var categoryName = product.Category?.CategoryName?.Trim();
                if (!string.IsNullOrEmpty(categoryName))
                {
                    _context.Categories.Add(new Category
                    {
                        ProductId = product.ProductId,
                        CategoryName = categoryName
                    });
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                _context.Products.Update(product);

                var exisitingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.ProductId == product.ProductId);
                var categoryName = product.Category?.CategoryName?.Trim();

                if(exisitingCategory == null && !string.IsNullOrWhiteSpace(categoryName))
                {
                    _context.Categories.Add(new Category
                    {
                        ProductId = product.ProductId,
                        CategoryName = categoryName
                    });
                }else if(exisitingCategory != null)
                {
                    exisitingCategory.CategoryName = categoryName ?? exisitingCategory.CategoryName;
                    _context.Categories.Update(exisitingCategory);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(List));
        }

    }
}
