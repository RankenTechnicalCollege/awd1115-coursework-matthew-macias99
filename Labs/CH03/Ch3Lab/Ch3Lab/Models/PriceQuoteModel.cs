using System.ComponentModel.DataAnnotations;

namespace Ch3Lab.Models
{
    public class PriceQuoteModel
    {
        [Required(ErrorMessage = "Please enter a valid subtotal")]
        public decimal? Subtotal { get; set; }

        [Required(ErrorMessage = "Please enter a valid percent")]
        [Range(0, 100, ErrorMessage = "Percent must be between 0-100")]
        public decimal? DiscountPercent { get; set; }

        public decimal? DiscountAmount
        {
            get
            {
                return Subtotal * DiscountPercent / 100;
            }
        }

        public decimal? Total
        {
            get
            {
                return Subtotal - DiscountAmount;
            }
        }
    }
}
