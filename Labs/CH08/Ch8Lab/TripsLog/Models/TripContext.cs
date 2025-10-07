using Microsoft.EntityFrameworkCore;

namespace TripsLog.Models
{
    public class TripContext : DbContext
    {
        public TripContext(DbContextOptions<TripContext> options) : base(options)
        {
        }
        public DbSet<Trip> Trips => Set<Trip>();
    }
}
