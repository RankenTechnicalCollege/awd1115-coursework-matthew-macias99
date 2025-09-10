using System.ComponentModel.DataAnnotations;

namespace HOT1.Models
{
    public class OrderFormModel
    {
        [Required(ErrorMessage = "Please enter a quantity.")]
        [Range(1, 100000, ErrorMessage = "Quantity must be at least 1")]
        public int? Quantity { get; set; }

        public string? DiscountCode { get; set; }

        public decimal UnitPrice { get; set; } = 15m;
        public decimal Subtotal { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public string? DiscountError { get; set; }
        }
}
