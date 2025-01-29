using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.Models;

public class RegistrationModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}