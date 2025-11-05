using System.ComponentModel.DataAnnotations;

namespace ManyToManyEF.Models
{
    public class Course
    {
        public Course()
        {
            Students = new List<Student>();
        }
        public int CourseId { get; set; }

        [Required]
        public string Title { get; set; }

        public List<Student> Students { get; set; }

    }
}
