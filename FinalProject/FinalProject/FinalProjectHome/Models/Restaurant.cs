using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FinalProjectHome.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }

        [Required, StringLength(80, MinimumLength = 2)]
        public string Name { get; set; } = "";

        [Required, StringLength(40)]
        public string Category { get; set; } = "";

        [Range(1, 4)]
        public int PriceLevel { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public ICollection<RestaurantTag> RestaurantTags { get; set; } = new List<RestaurantTag>();
    }
}
