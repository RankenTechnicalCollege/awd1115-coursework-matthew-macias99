using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TripLog2.Models.DomainModels
{
    public class Trip
    {
        public Trip() => Activities = new HashSet<Activity>();
        public int TripId { get; set; }

        public int DestinationId { get; set; } //FK property

        [ValidateNever]
        public Destination Destination { get; set; } = null!;  //nav prop

        [Required(ErrorMessage = "Please enter the date your trip starts.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Please enter the date your trip ends.")]
        public DateTime? EndDate { get; set; }


        public int AccomodationId { get; set; } //FK property
        [ValidateNever]
        public Accomodation Accomodation { get; set; } = null!;

        //skip nav prop
        public ICollection<Activity> Activities { get; set; }
    }
}
