namespace HungryOClockV2.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<RestaurantCategory> RestaurantCategories { get; set; } = new List<RestaurantCategory>();
    }

    public class RestaurantCategory
    {
        public int RestaurantId { get; set; }
        public int CategoryId { get; set; } 
        public Restaurant? Restaurant { get; set; }
        public Category? Category { get; set; }
    }
}
