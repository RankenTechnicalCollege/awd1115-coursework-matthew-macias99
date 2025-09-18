using System.ComponentModel.DataAnnotations;

namespace HOT2.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is Required")]
        public string ProductName { get; set; }
        public string ProductDescShort { get; set; }
        public string ProductDescLong { get; set; }
        public string ProductImage { get; set; }

        [Range(1,100000, ErrorMessage = "Price must be between 1 and 100000")]
        public decimal ProductPrice { get; set; }

        [Range(1,1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        public int ProductQty { get; set; }

        public Category? Category { get; set; }

        public string Slug => ProductName?.Replace(" ", "-").ToLower();

    }
}
