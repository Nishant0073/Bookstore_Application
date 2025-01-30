using System.ComponentModel.DataAnnotations;

namespace Bookstore_Application.Models;

public class RegistrationModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}