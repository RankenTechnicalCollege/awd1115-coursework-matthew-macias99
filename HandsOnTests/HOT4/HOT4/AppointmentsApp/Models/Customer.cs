using System.ComponentModel.DataAnnotations;

namespace AppointmentsApp.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, Phone]
        public string Phone { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public override string ToString() => $"{Username} ({Phone})";
    }
}
