using Bookstore_Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Bookstore_Application.Services;

public interface IUserService
{
    public Task<object> CreateUserAsync(string email, string password);
    Task<AuthenticationModel> GetTokenAsync(TokenRequest model);
}