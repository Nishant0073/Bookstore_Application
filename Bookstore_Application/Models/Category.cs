using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore_Application.Models;

public class Category
{
    [Key]
    public string CategoryId { get; set; }
    
    [Index(IsUnique = true)]
    public string CategoryName { get; set; }
    
    //Foreign key
    //public ICollection<Book> Books { get; set; }
}