using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ContactManager.Models
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required, Display(Name = "First Name"), StringLength(40)]
        public string FirstName { get; set; } = string.Empty;

        [Required, Display(Name = "Last Name"), StringLength(40)]
        public string LastName { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        public string Slug => $"{FirstName}-{LastName}".Trim().ToLower().Replace(' ', '-');
    }
}
