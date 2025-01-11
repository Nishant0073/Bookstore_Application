using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.DTOs.Category;

public class CategoryPutDTO
{
   
        [Required(ErrorMessage = "CategoryId is required")]
        public string CategoryId { get; set; }
        
        [Required(ErrorMessage = "Category Name is required")]
        [MaxLength(50, ErrorMessage = "Category Name cannot be more than 50 characters")]
        public string Name { get; set; }
}