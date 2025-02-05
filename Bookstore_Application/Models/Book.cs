using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.Models;

public class Book
{
    [Key]
    public string BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public double Price { get; set; }
    public string CategoryId { get; set; }
    public int Stock { get; set; }
   //Reference 
    public Category Category { get; set; }
}