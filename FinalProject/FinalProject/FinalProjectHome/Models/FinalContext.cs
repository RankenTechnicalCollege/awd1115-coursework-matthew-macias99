using FinalProjectHome.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectHome.Models
{
    public class FinalContext: DbContext
    {
        public FinalContext(DbContextOptions<FinalContext> options) : base(options) { }

        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<RestaurantTag> RestaurantTags => Set<RestaurantTag>();

        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RestaurantTag>().HasKey(rt => new {rt.RestaurantId, rt.TagId });

            modelBuilder.Entity<RestaurantTag>()
                .HasOne(rt => rt.Restaurant)
                .WithMany(r => r.RestaurantTags)
                .HasForeignKey(rt => rt.RestaurantId);

            modelBuilder.Entity<RestaurantTag>()
                .HasOne(rt => rt.Tag)
                .WithMany(t => t.RestaurantTags)
                .HasForeignKey(rt => rt.TagId);

            modelBuilder.Entity<MenuItem>()
                .HasOne(mi => mi.Restaurant)
                .WithMany(r => r.MenuItems)
                .HasForeignKey(mi => mi.RestaurantId);
        }
    }
}
