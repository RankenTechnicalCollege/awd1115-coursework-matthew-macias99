using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RankenClassSchedule.Models.DomainModels;

namespace RankenClassSchedule.Models.Configuration
{
    public class ClassConfig : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> entity)
        {
            entity.HasOne(c => c.Teacher).WithMany(t => t.Classes).OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new Class { ClassId = 1, Title = "Intro To C#", Number = 100, TeacherId = 1, DayId = 1, MilitaryTime = "0800"},
                new Class { ClassId = 2, Title = "Intro To Web Dev", Number = 101, TeacherId = 1, DayId = 2, MilitaryTime = "0800" },
                new Class { ClassId = 3, Title = "Intro To MERN", Number = 102, TeacherId = 1, DayId = 3, MilitaryTime = "0800" },
                new Class { ClassId = 4, Title = "Intro To .NET MVC Core", Number = 103, TeacherId = 1, DayId = 4, MilitaryTime = "0800" },
                new Class { ClassId = 5, Title = "Intro To Desktop Support", Number = 104, TeacherId = 2, DayId = 1, MilitaryTime = "0800" },
                new Class { ClassId = 6, Title = "Intro To Hardware", Number = 105, TeacherId = 3, DayId = 2, MilitaryTime = "0800" },
                new Class { ClassId = 7, Title = "Intro To IT Admin", Number = 106, TeacherId = 4, DayId = 1, MilitaryTime = "0800" },
                new Class { ClassId = 8, Title = "Intro To Networking", Number = 107, TeacherId = 5, DayId = 1, MilitaryTime = "0800" }
                );
        }
    }
}
