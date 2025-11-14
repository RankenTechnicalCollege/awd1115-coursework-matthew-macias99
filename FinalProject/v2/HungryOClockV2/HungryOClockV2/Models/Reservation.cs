namespace HungryOClockV2.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public DateTime ReservationDateTime { get; set; }
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string? Accomodations { get; set; }

        public DateTime CreatedAt { get; set; }
        }
}
