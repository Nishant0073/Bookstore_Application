
namespace Bookstore_Application.Models;

public class Order
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
    
}