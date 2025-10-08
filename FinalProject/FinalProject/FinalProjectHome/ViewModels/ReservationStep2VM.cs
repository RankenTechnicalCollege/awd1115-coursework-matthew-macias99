using System.ComponentModel.DataAnnotations;

namespace FinalProjectHome.ViewModels
{
    public class ReservationStep2VM
    {
        [Range(1, 20)]
        public int PartySize { get; set; } = 2;
    }
}
