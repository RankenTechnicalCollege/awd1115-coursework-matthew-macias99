using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HungryOClockV2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
