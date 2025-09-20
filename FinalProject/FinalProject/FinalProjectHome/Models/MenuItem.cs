using System.ComponentModel.DataAnnotations;

namespace FinalProjectHome.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        
        [Required, StringLength(80)]
        public string Name { get; set; } = "";

        public decimal Price { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
    }
}
