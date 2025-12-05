using System;
using System.ComponentModel.DataAnnotations;

namespace HOT3.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        [Range(0, 1000)]
        public decimal UnitPrice { get; set; }
    }
}
