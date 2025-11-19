using Microsoft.AspNetCore.Mvc.Rendering;

namespace HungryOClockV2.Models.ViewModels
{
    public class RestaurantCreateVM
    {
        public int RestaurantId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int PriceLevel { get; set; }

        public List<int> SelectedCategoryIds { get; set; } = new List<int>();

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
