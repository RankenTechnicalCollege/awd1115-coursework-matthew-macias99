using ContactManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Models
{
    public class ContactManagerContext(DbContextOptions<ContactManagerContext> options) : DbContext(options)
    {
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Family" },
                new Category { CategoryId = 2, Name = "Friend" },
                new Category { CategoryId = 3, Name = "Work" }
            );

            modelBuilder.Entity(ContactSeed());

            static Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Contact>> ContactSeed()
                => e => e.HasData(
                    new Contact
                    {
                        ContactId = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        Phone = "555-123-4567",
                        Email = "john@doe.com",
                        CategoryId = 1,
                        DateAdded = DateTime.Now
                    },
                    new Contact
                    {
                        ContactId = 2,
                        FirstName = "Danny",
                        LastName = "Joe",
                        Phone = "555-987-6543",
                        Email = "danny@joe.com",
                        CategoryId = 2,
                        DateAdded = DateTime.Now
                    },
                    new Contact
                    {
                        ContactId = 3,
                        FirstName = "Elizabeth",
                        LastName = "Queen",
                        Phone = "123-555-4567",
                        Email = "liz@queen.com",
                        CategoryId = 3,
                        DateAdded = DateTime.Now
                    }
                    );
        }
    }

}
