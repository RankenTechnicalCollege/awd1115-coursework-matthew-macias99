using System.ComponentModel.DataAnnotations;

namespace AppointmentsApp.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        public DateTime End => Start.AddHours(1);

        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
