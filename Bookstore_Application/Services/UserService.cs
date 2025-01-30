using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bookstore_Application.Constants;
using Bookstore_Application.Models;
using Bookstore_Application.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<AuthenticationModel> GetTokenAsync(TokenRequest model)
    {
        _logger.LogDebug("GetTokenAsync:: started");
        try
        {
            AuthenticationModel authenticationModel = new AuthenticationModel();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = "No user registered with this email";
                return authenticationModel;
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
               authenticationModel.IsAuthenticated = true;
               JwtSecurityToken securityToken = await CreateJwtToken(user);
               authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
               authenticationModel.Email = user.Email;
               authenticationModel.Username = user.UserName;
               var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
               authenticationModel.Roles = roleList.ToList();
               return authenticationModel;
            } 
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = "Invalid login attempt";
            return authenticationModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTokenAsync:: Failed");
            throw new Exception("An error occurred while getting the token", ex);
        }
    }

    private async Task<JwtSecurityToken> CreateJwtToken(IdentityUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
        }

        var claim = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("uid", user.UserName),
        }.Union(userClaims).Union(roleClaims);
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer:_jwt.Issuer,
            audience:_jwt.Audience,
            claims: claim,
            expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: credentials
        );
        return jwtSecurityToken;
    }
}