using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HOT3.Models;
using HOT3.ViewModels;

namespace HOT3.Controllers
{
    public class CartController : Controller
    {
        private const string CartKey = "CART";
        private readonly HOT3Context _context;
        public CartController(HOT3Context context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var dict = HttpContext.Session.GetObject<Dictionary<int, int>>(CartKey) ?? new();
            var vm = new CartVM();

            if (dict.Any())
            {
                var ids = dict.Keys.ToList();
                var products = await _context.Products.Where(p => ids.Contains(p.ProductId)).ToListAsync();
                foreach (var p in products)
                {
                    vm.Items.Add(new CartItemVM { Product = p, Quantity = dict[p.ProductId] });
                }
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult Add(int id)
        {
            var dict = HttpContext.Session.GetObject<Dictionary<int,int>>(CartKey) ?? new();
            dict[id] = dict.ContainsKey(id) ? dict[id] + 1 : 1;
            HttpContext.Session.SetObject(CartKey, dict);
            TempData["Success"] = "Item added to cart";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var dict = HttpContext.Session.GetObject<Dictionary<int,int>>(CartKey) ?? new();
            if (dict.Remove(id))
            {
                HttpContext.Session.SetObject(CartKey, dict);
                TempData["Success"] = "Item removed from cart";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return await Index();
        }

        [HttpPost]
        public async Task<IActionResult> Purchase()
        {
            var dict = HttpContext.Session.GetObject<Dictionary<int,int>>(CartKey) ?? new();
            if (!dict.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var ids = dict.Keys.ToList();
            var products = await _context.Products.Where(p => ids.Contains(p.ProductId)).ToListAsync();

            var order = new Order();
            foreach (var p in products)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = p.ProductId,
                    Quantity = dict[p.ProductId],
                    UnitPrice = p.Price
                });
            }
            order.Total = order.Items.Sum(i => i.UnitPrice * i.Quantity);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartKey);
            TempData["Success"] = "Purchase complete!";
            return RedirectToAction(nameof(Index));
        }
    }
}
