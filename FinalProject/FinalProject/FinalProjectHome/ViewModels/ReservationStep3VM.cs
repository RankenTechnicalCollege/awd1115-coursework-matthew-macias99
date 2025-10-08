using System.ComponentModel.DataAnnotations;

namespace FinalProjectHome.ViewModels
{
    public class ReservationStep3VM
    {
        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
