using Microsoft.EntityFrameworkCore;

namespace CarInventory
{
    public class CarDb : DbContext
    {
        public CarDb(DbContextOptions<CarDb> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
    }
}
