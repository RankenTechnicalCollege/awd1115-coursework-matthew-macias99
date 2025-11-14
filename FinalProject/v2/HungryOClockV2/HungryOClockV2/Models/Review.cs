namespace HungryOClockV2.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
