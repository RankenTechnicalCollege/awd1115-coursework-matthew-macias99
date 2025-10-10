using System.ComponentModel.DataAnnotations;

namespace HOT3.Models
{
    public class Product
    {
        public int ProductId { get; set; }  

        [Required, StringLength(70)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Range(0, 1000)]
        public decimal Price { get; set; }

        [Required, StringLength(200)]
        public string ImageFileName { get; set; } = "placeholder.png";

        [Required, StringLength(100)]
        public string Slug { get; set; } = string.Empty;

    }
}
