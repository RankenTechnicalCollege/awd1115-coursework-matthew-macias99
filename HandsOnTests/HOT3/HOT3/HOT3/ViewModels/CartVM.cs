using HOT3.Models;

namespace HOT3.ViewModels
{
    public class CartItemVM
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal LineTotal => Product.Price * Quantity;
    }

    public class CartVM
    {
        public List<CartItemVM> Items { get; set; } = new();
        public int TotalQuantity => Items.Sum(i => i.Quantity);
        public decimal TotalPrice => Items.Sum(i => i.LineTotal);
    }
}

