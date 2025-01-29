using Microsoft.AspNetCore.Identity;

namespace Bookstore_Application.Services;

public interface IUserService
{
    public Task<IdentityResult> CreateUserAsync(string email, string password);
}