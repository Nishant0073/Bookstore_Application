namespace Bookstore_Application.Models;

public class Book
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Publisher { get; set; }
    public double Price { get; set; }
    public string CategoryId { get; set; }
    public int Stock { get; set; }
}