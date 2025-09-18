using Microsoft.EntityFrameworkCore;

namespace HOT2.Models
{
    public class SalesOrderContext(DbContextOptions<SalesOrderContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "AeroFlo ATB Wheels", ProductDescShort = "", ProductDescLong = "", ProductImage = "", ProductPrice = 189, ProductQty = 40},
                new Product { ProductId = 2, ProductName = "Clear Shade 85-T Glasses", ProductDescShort = "", ProductDescLong = "", ProductImage = "", ProductPrice = 45, ProductQty = 14},
                new Product { ProductId = 3, ProductName = "Cosmic Elite Road Warrior Wheels", ProductDescShort = "", ProductDescLong = "", ProductImage = "", ProductPrice = 165, ProductQty = 22},
                new Product { ProductId = 4, ProductName = "Cycle-Doc Pro Repair Stand", ProductDescShort = "", ProductDescLong = "", ProductImage = "", ProductPrice = 166, ProductQty = 12 },
                new Product { ProductId = 5, ProductName = "Dog Ear Aero-Flow Floor Pump", ProductDescShort = "", ProductDescLong = "", ProductImage = "", ProductPrice = 5, ProductQty = 25 }
            );

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithOne(c => c.Product)
                .HasForeignKey<Category>(c => c.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().Property(c => c.ProductId).ValueGeneratedNever();

            modelBuilder.Entity<Category>().HasData(
                new Category { ProductId = 1, CategoryName = "Accessories" },
                new Category { ProductId = 2, CategoryName = "Bikes" },
                new Category { ProductId = 3, CategoryName = "Clothing" },
                new Category { ProductId = 4, CategoryName = "Components" },
                new Category { ProductId = 5, CategoryName = "Car racks" }
            );




        }
    }
}
