namespace Bookstore_Application.Models;

public class ErrorResponse
{
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public string InnerExceptionMessage { get; set; }
}