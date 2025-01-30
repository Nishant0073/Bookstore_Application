using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.Models;

public class TokenRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [PasswordPropertyText]
    public string Password { get; set; }
}