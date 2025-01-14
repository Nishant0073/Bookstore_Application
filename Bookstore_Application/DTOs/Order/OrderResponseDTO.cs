using Bookstore_Application.Models;

namespace Bookstore_Application.DTOs.Order;

public class OrderResponseDTO
{
    public string OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalPrice { get; set; }
    public string OrderStatus { get; set; } = "Pending";
    public ICollection<OrderItem> OrderItems { get; set; }
}