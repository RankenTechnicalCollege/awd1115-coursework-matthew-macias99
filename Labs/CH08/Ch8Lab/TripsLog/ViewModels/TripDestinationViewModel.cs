using System;
using System.ComponentModel.DataAnnotations;

namespace TripsLog.ViewModels
{
    public class TripDestinationViewModel
    {

        [Required]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

    }
}
