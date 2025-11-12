using System.ComponentModel.DataAnnotations;

namespace TripLog2.Models.DomainModels
{
    public class Destination
    {
        public Destination() => Trips = new HashSet<Trip>();
        public int DestinationId { get; set; } //PK

        [Required(ErrorMessage = "Please enter the destination name.")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Trip> Trips { get; set; } 
    }
}
