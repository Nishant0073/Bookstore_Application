namespace Bookstore_Application.DTOs.OrderItems;

public class OrderItemResponseDTO
{
    public string OrderItemId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string BookId { get; set; }
}