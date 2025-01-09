using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.Models;

public class Category
{
    [Key]
    public string CategoryId { get; set; }
    public string Name { get; set; }
    
    //Foreign key
    public string BookId { get; set; }
    public Book Book { get; set; }
}