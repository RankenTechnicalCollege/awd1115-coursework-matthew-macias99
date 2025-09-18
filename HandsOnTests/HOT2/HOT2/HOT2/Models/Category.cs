using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace HOT2.Models
{
    public class Category
    {
        
        [Key, ForeignKey("Product")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Category Name is Required"), MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public Product Product { get; set; } = null!;
    }
}
