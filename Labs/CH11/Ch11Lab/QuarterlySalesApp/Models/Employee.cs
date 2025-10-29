using System.ComponentModel.DataAnnotations;

namespace QuarterlySalesApp.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required, StringLength(40)]
        public string FirstName { get; set; } = "";

        [Required, StringLength(40)]
        public string LastName { get; set; } = "";

        [Required, DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime DateOfHire { get; set; }

        public int? ManagerId { get; set; }

        public Employee? Manager { get; set; }

        public ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public string FullName => $"{FirstName} {LastName}";


    }
}
