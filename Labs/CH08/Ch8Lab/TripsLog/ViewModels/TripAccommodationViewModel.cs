using System;
using System.ComponentModel.DataAnnotations;

namespace TripsLog.ViewModels
{
    public class TripAccommodationViewModel
    {
        [Required]
        public string Accommodation { get; set; } = string.Empty;

        public string? AccommodationPhone { get; set; } = string.Empty;

        [EmailAddress]
        public string? AccommodationEmail { get; set; } = string.Empty;
    }
}
