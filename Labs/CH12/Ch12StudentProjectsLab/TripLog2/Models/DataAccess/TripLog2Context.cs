using Microsoft.EntityFrameworkCore;
using TripLog2.Models.DataAccess.Configuration;
using TripLog2.Models.DomainModels;

namespace TripLog2.Models.DataAccess
{
    public class TripLog2Context : DbContext
    {
        public TripLog2Context(DbContextOptions<TripLog2Context> options)
            : base(options)
        {
        }
        public DbSet<Trip> Trips { get; set; } = null!;

        public DbSet<Destination> Destinations { get; set; } = null!;

        public DbSet<Accomodation> Accomodations { get; set; } = null!;

        public DbSet<Activity> Activities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TripConfig());
        }
    }
}
