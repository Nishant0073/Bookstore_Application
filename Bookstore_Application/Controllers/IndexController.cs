using Microsoft.AspNetCore.Mvc;

namespace Bookstore_Application.Controllers;

[ApiController]
[Route("[controller]")]
public class IndexController : ControllerBase
{
    // GET
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Bookstore Application");
    }
}