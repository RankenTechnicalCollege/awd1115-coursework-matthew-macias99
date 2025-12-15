namespace HungryOClockV2.Models.ViewModels
{
    public class RestaurantDetailsVM
    {
        public Restaurant Restaurant { get; set; }
        public IEnumerable<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public int MenuPage { get; set; }
        public int MenuTotalPages { get; set; }
    }
}
