using Microsoft.AspNetCore.Mvc.Rendering;

namespace HungryOClockV2.Models.ViewModels
{
    public class HomeIndexVM
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; } 

        public List<SelectListItem> Categories { get; set; } = new();
        public List<Restaurant> Restaurants { get; set; } = new();
    }
}
