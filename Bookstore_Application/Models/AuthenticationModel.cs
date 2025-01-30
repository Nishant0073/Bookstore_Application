namespace Bookstore_Application.Models;

public class AuthenticationModel
{
    public string Message { get; set; }
    public bool IsAuthenticated { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public string Token { get; set; }
}