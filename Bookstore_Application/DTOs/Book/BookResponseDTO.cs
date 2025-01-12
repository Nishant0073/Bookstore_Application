namespace Bookstore_Application.DTOs.Category;

public class BookResponseDTO
{
    public string BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }
    public int Stock { get; set; }
}