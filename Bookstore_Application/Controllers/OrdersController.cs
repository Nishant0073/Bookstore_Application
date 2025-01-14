using AutoMapper;
using Bookstore_Application.Models;
using Bookstore_Application.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController: ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private IMapper _mapper;
    private IRepository<Order> _repository;

    public OrdersController(ILogger<OrdersController> logger, IMapper mapper, IRepository<Order> repository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        _logger.LogDebug("OrderController::GetOrders :: Started");
        try
        {
            IEnumerable<Order> orders = await _repository.GetAllAsync();
            _logger.LogDebug("OrderController::GetOrders :: Finished");
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OrderController::GetOrders :: Error");
            var errorResponse = new ErrorResponse
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                InnerExceptionMessage = ex.InnerException?.Message
            };

            return BadRequest(errorResponse);
        }
    }
}