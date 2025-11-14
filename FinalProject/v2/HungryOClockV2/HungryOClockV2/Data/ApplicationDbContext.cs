using HungryOClockV2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HungryOClockV2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<RestaurantCategory> RestaurantCategories { get; set; } = null!;
        public DbSet<MenuItem> MenuItems { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RestaurantCategory>().HasKey(rc => new { rc.RestaurantId, rc.CategoryId });

            builder.Entity<RestaurantCategory>()
                .HasOne(rc => rc.Restaurant)
                .WithMany(r => r.RestaurantCategories)
                .HasForeignKey(rc => rc.RestaurantId);

            builder.Entity<RestaurantCategory>()
                .HasOne(rc => rc.Category)
                .WithMany(c => c.RestaurantCategories)
                .HasForeignKey(rc => rc.CategoryId);

            builder.Entity<Reservation>()
                .HasIndex(r => new {r.RestaurantId, r.ReservationDateTime })
                .IsUnique();
        }
    }
}
