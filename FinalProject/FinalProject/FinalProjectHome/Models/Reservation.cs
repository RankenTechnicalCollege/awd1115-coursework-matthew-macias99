using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProjectHome.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public Restaurant? Restaurant { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        [Required, Range(1, 20)]
        public int PartySize { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
