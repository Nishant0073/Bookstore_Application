using Bookstore_Application.Constants;
using Bookstore_Application.Settings;
using Microsoft.AspNetCore.Identity;

namespace Bookstore_Application.Services;

public class UserService: IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserService> _logger;
    private readonly JWT _jwt;

    public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, JWT jwt,ILogger<UserService> logger)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwt = jwt;
    }

    
    public async Task<object> CreateUserAsync(string email, string password)
    {
        _logger.LogDebug("CreateUserAsync:: started");
        try
        {
            var user = new IdentityUser()
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(email);
            if (userWithSameEmail != null)
            {
                return new { Success = false, Message = $"Email {email} is already in use" };
            }

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Authorization.Roles.User.ToString());
            }

            _logger.LogDebug("CreateUserAsync:: Succeeded");
            return new 
            { 
                Success = result.Succeeded, 
                Errors = result.Errors.Select(e => e.Description).ToList() 
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateUserAsync:: Failed");
            return new { Success = false, Message = "An error occurred while creating the user." };
        }
    }

}