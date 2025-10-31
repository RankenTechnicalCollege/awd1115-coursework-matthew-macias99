using Microsoft.EntityFrameworkCore;
using AppointmentsApp.Models;

namespace AppointmentsApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>().HasIndex(a => a.Start).IsUnique();

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, Username = "test_dummy", Phone = "555-555-5555"}
            );

            var seededAppointment = DateTime.Today.AddDays(1).AddHours(9);
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { AppointmentId = 1, CustomerId = 1, Start = seededAppointment }    
            );
        }
    }
}
