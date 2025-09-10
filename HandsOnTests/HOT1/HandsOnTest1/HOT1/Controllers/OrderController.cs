using Microsoft.AspNetCore.Mvc;
using HOT1.Models;

namespace HOT1.Controllers
{
    public class OrderController : Controller
    {
        private readonly Dictionary<string, decimal> _codes = new(StringComparer.OrdinalIgnoreCase)
        {
            ["6175"] = 0.30m,
            ["1390"] = 0.20m,
            ["BB88"] = 0.10m
        };

        [HttpGet]
        public IActionResult Index()
        {
            return View(new OrderFormModel());
        }

        [HttpPost]
        public IActionResult Index(OrderFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var qty = model.Quantity!.Value;
            model.Subtotal = qty * model.UnitPrice;

            if (!string.IsNullOrWhiteSpace(model.DiscountCode))
            {
                if (_codes.TryGetValue(model.DiscountCode.Trim(), out var pct))
                {
                    model.DiscountPercentage = pct;
                }
                else
                {
                    model.DiscountPercentage = 0;
                    model.DiscountError = "Invalid discount code.";
                }
            }

            model.DiscountAmount = Math.Round(model.Subtotal * model.DiscountPercentage, 2);
            var discounted = model.Subtotal - model.DiscountAmount;

            model.Tax = Math.Round(discounted * 0.08m, 2);
            model.Total = Math.Round(discounted + model.Tax, 2);

            return View(model);
        }
    }
}
