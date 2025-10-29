using Microsoft.AspNetCore.Mvc.Rendering;
using QuarterlySalesApp.Models;

namespace QuarterlySalesApp.ViewModels
{
    public class HomeIndexVM
    {
        public int? SelectedEmployeeId { get; set; }
        public List<SelectListItem> Employees { get; set; } = new();
        public List<Sale> Sales { get; set; } = new();
    }
}
