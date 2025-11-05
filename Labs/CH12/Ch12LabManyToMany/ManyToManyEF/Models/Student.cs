namespace ManyToManyEF.Models
{
    public class Student
    {
        public Student()
        {
            Courses = new List<Course>();
        }
        public int StudentId { get; set; }

        public string Name { get; set; }

        public string FinancialAidStatus { get; set; }

        public List<Course> Courses { get; set; }
    }
}
