namespace CarInventory
{
    public class CarDTO
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

        public CarDTO() { }

        public CarDTO(Car car)
        {
            Id = car.Id;
            Make = car.Make;
            Model = car.Model;
            Trim = car.Trim;
            Color = car.Color;
            HorsePower = car.HorsePower;
            DriveTrain = car.DriveTrain;
            Price = car.Price;
            IsAvailable = car.IsAvailable;
        }
    }
}
