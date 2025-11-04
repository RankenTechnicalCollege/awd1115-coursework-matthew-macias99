using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RankenClassSchedule.Models.DomainModels;

namespace RankenClassSchedule.Models.Configuration
{
    public class TeacherConfig : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> entity)
        {
            entity.HasData(
                new Teacher {  TeacherId = 1, FirstName = "Mister", LastName = "Teacher"},
                new Teacher { TeacherId = 2, FirstName = "Lester", LastName = "Reacher" },
                new Teacher { TeacherId = 3, FirstName = "Chester", LastName = "Feaster" },
                new Teacher { TeacherId = 4, FirstName = "Miss", LastName = "Professor" },
                new Teacher { TeacherId = 5, FirstName = "Liz", LastName = "Wizz" }
                );
        }
    }
}
