using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProjectHome.ViewModels
{
    public class ReservationStep1VM
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        public IEnumerable<SelectListItem> Restaurants { get; set; } = new List<SelectListItem>();
    }
}
