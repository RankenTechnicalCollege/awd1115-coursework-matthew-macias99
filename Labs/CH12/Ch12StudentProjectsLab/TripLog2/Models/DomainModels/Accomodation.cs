using System.ComponentModel.DataAnnotations;

namespace TripLog2.Models.DomainModels
{
    public class Accomodation
    {
        public Accomodation() => Trips = new HashSet<Trip>();
        public int AccomodationId { get; set; } //PK

        [Required(ErrorMessage = "Please enter the accommodation name.")]
        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public ICollection<Trip> Trips { get; set; }

        public bool HasPhone => !string.IsNullOrEmpty(Phone);

        public bool HasEmail => !string.IsNullOrEmpty(Email);

    }
}
