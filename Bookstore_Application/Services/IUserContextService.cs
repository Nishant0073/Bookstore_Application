namespace Bookstore_Application.Services;

public interface IUserContextService
{
    string UserId { get; }
    string Role { get; }
}