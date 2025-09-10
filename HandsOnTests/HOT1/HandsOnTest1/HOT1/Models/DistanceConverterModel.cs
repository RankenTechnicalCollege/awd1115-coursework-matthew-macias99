using System.ComponentModel.DataAnnotations;

namespace HOT1.Models
{
    public class DistanceConverterModel
    {
        [Required(ErrorMessage = "Distance is required.")]
        [Range(1,500, ErrorMessage = "Distance must be between 1 and 500 inches.")]
        public double? Inches { get; set; }

        public double? Centimeters { get; set; }

        public string? ResultMessage
        {
            get
            {
                if (Inches.HasValue && Centimeters.HasValue)
                {
                    return $"{Inches.Value:F2} inches is {Centimeters.Value:F2} centimeters.";
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
