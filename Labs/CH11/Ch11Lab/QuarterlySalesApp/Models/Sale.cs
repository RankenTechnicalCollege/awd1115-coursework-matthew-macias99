using System.ComponentModel.DataAnnotations;

namespace QuarterlySalesApp.Models
{
    public class Sale
    {
        public int SaleId { get; set; }

        [Required, Range(1, 4, ErrorMessage = "Quarter must be between 1 and 4")]
        public int Quarter {  get; set; }

        [Required, Range(2001, 9999, ErrorMessage = "Year must be after 2000")]
        public int Year { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0. If you made $0 in sales you probably shouldn't work here")]
        public decimal Amount { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
