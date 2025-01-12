using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.DTOs.Category;

public class CategoryRequestDTO
{
    [Required(ErrorMessage = "Category Name is required")]
    [MaxLength(50, ErrorMessage = "Category Name cannot be more than 50 characters")]
    public string CategoryName { get; set; }
}