using Bookstore_Application.Models;
using Bookstore_Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace Bookstore_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: Controller
{
    private readonly IUserService _userService;
    ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        this._logger = logger;
        this._userService = userService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody]RegistrationModel registrationModel)
    {
        _logger.LogInformation("UserController::RegisterAsync:: Started");
        
        try
        {
            var result = await _userService.CreateUserAsync(registrationModel.Email, registrationModel.Password);
            
            _logger.LogInformation("UserController::RegisterAsync:: End");
            return Ok(result);
            
        }
        catch (Exception e)
        {
            
            _logger.LogInformation("UserController::RegisterAsync:: Error");
            var errorResponse = new ErrorResponse
            {
                Message = e.Message,
                StackTrace = e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message
            };

            return StatusCode(500, errorResponse); // Or log errorResponse
        }
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetTokenTask(TokenRequest tokenRequest)
    {
        _logger.LogInformation("UserController::GetTokenTask:: Started");
        try
        {
            var result = await _userService.GetTokenAsync(tokenRequest);
            _logger.LogInformation("UserController::GetTokenTask:: End");
            return Ok(result);
        }
        catch (Exception e)
        {
           
            var errorResponse = new ErrorResponse
            {
                Message = e.Message,
                StackTrace = e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message
            };

            return StatusCode(500, errorResponse); // Or log errorResponse
        }
    }
    
}