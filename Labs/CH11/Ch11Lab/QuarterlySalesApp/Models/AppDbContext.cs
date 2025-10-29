using Microsoft.EntityFrameworkCore;

namespace QuarterlySalesApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Sale> Sales => Set<Sale>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                EmployeeId = 1,
                FirstName = "Mister",
                LastName = "Boss",
                DOB = new DateTime(1955,12,25),
                DateOfHire = new DateTime(1995,1,1),
                ManagerId = null
            });

            modelBuilder.Entity<Sale>().HasData(
                new Sale { SaleId = 1, EmployeeId = 1, Year = 2002, Quarter = 1, Amount = 4578m}
            );
        }
    }
}
