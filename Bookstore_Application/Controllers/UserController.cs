using Bookstore_Application.Models;
using Bookstore_Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace Bookstore_Application.Controllers;

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
    public async Task<IActionResult> RegisterAsync(RegistrationModel registrationModel)
    {
        _logger.LogInformation("UserController::RegisterAsync:: Started");
        
        try
        {
            var result = _userService.CreateUserAsync(registrationModel.Email, registrationModel.Password);
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
    
}