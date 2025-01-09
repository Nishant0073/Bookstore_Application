using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.Models;

public class OrderItem
{
    [Key]
    public string OrderItemId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    
    
    //Foreign Key for Book
    public string BookId { get; set; }
    public Book Book { get; set; }
    
    //Foreign Key for Order
    public string OrderId { get; set; }
    public Order Order { get; set; }
    
    
}