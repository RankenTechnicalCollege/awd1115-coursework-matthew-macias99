namespace HungryOClockV2.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
