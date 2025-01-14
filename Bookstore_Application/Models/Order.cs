
namespace Bookstore_Application.Models;

public class Order
{
    public string OrderId { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
    public string OrderStatus { get; set; } = "Pending";
    
    public ICollection<OrderItem> OrderItems { get; set; }
    
}