using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.DTOs.Category;

public class BookPutDTO
{
    
    [Required(ErrorMessage = "BookId is required")]
    public string BookId { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(50, ErrorMessage = "Title cannot be longer than 50 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Author is required")]
    [MaxLength(50, ErrorMessage = "Author cannot be longer than 50 characters")]
    public string Author { get; set; }

    [Required(ErrorMessage = "Price is required")]
    public double Price { get; set; }

    [Required(ErrorMessage = "CategoryId is required")]
    public string CategoryId { get; set; }
    public int Stock { get; set; }
}