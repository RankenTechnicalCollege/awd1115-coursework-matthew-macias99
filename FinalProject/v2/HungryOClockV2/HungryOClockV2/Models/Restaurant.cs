namespace HungryOClockV2.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;

        //1-4 rating system
        public int PriceLevel { get; set; }

        //comes from reviews. can be null if no reviews yet
        public decimal? AverageRating { get; set; }

        public string Slug { get; set; }

        //nav
        public ICollection<RestaurantCategory> RestaurantCategories { get; set; } = new List<RestaurantCategory>();
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
