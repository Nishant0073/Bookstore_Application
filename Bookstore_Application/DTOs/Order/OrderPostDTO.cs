using Bookstore_Application.DTOs.OrderItems;

namespace Bookstore_Application.DTOs.Order;

public class OrderPostDTO
{
    public double TotalPrice { get; set; } = 0;
    public List<OrderItemPostDTO> OrderItems { get; set; } = new List<OrderItemPostDTO>();
}