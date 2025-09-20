using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FinalProjectHome.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        [Required, StringLength(40)]
        public string Name { get; set; } = "";

        public ICollection<RestaurantTag> RestaurantTags { get; set; } = new List<RestaurantTag>();
    }
}
