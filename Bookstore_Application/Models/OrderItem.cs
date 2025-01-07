namespace Bookstore_Application.Models;

public class OrderItem
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public int Quantity { get; set; }
    public string BookId { get; set; }
    public double Price { get; set; }
}