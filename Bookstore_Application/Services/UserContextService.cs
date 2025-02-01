using System.Security.Claims;

namespace Bookstore_Application.Services;

public class UserContextService: IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string UserId => _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string Role => _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value;
}