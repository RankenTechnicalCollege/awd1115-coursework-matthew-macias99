using Microsoft.EntityFrameworkCore;
using HOT3.Models;

namespace HOT3.Models
{
    public class HOT3Context : DbContext
    {
        public HOT3Context(DbContextOptions<HOT3Context> opts) : base(opts) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Classic T-Shirt",
                    Brand = "T-Shirt Factory",
                    Category = "Tops",
                    Price = 19.99m,
                    ImageFileName = "tshirt.jpg",
                    Slug = "classic-t-shirt"
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Sweatshirt",
                    Brand = "Cozy Co.",
                    Category = "Tops",
                    Price = 39.99m,
                    ImageFileName = "sweatshirt.jpg",
                    Slug = "sweatshirt"
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Pants",
                    Brand = "Pant Co.",
                    Category = "Bottoms",
                    Price = 49.99m,
                    ImageFileName = "pants.jpg",
                    Slug = "pants"
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Everyday Shorts",
                    Brand = "Pant Co.",
                    Category = "Bottoms",
                    Price = 29.99m,
                    ImageFileName = "shorts.jpg",
                    Slug = "everyday-shorts"
                },
                new Product
                {
                    ProductId = 5,
                    Name = "Socks (6-Pack)",
                    Brand = "Nike",
                    Category = "Underwear",
                    Price = 12.99m,
                    ImageFileName = "socks.jpg",
                    Slug = "socks-6-pack"
                }
            );
        }
    }
}
