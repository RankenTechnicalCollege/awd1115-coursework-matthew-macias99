using System.ComponentModel.DataAnnotations;

namespace Ch2Lab1.Models

{
    public class TipCalculatorModel
    {
        [Required(ErrorMessage = "Please enter a valid amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal? Amount { get; set; }

    }
}
