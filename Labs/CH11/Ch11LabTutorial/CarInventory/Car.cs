namespace CarInventory
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Trim { get; set; }
        public string Color { get; set; }
        public int HorsePower { get; set; }
        public string DriveTrain { get; set; }
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }
        public string? Secret { get; set; }
        
        

    }
}
