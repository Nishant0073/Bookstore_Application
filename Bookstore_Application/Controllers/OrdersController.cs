using AutoMapper;
using Bookstore_Application.DTOs.Order;
using Bookstore_Application.DTOs.OrderItems;
using Bookstore_Application.Models;
using Bookstore_Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private IMapper _mapper;
    private IRepository<Order> _repository;
    private IRepository<Book> _bookRepository;
    private IRepository<OrderItem> _orderItemRepository;

    public OrdersController(ILogger<OrdersController> logger, IMapper mapper, IRepository<Order> repository,IRepository<Book> bookRepository, IRepository<OrderItem> orderItemRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
        _bookRepository = bookRepository;
        _orderItemRepository = orderItemRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        _logger.LogDebug("OrderController::GetOrders :: Started");
        try
        {
            IEnumerable<Order> orders = await _repository.GetAllAsync();
            List<OrderResponseDTO> responseDtos = [];
            foreach (var order in orders)
            {
                responseDtos.Add(_mapper.Map<OrderResponseDTO>(order));
                Console.WriteLine(order.OrderItems);
            }

            _logger.LogDebug("OrderController::GetOrders :: Finished");
            return Ok(responseDtos);
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrders(string id)
    {
        _logger.LogDebug("OrderController::GetOrdersId :: started");
        try
        {
            Order order = await _repository.GetByIdAsync(id);
            OrderResponseDTO responseDto = _mapper.Map<OrderResponseDTO>(order);
            _logger.LogDebug("OrderController::GetOrdersId :: Finished");
            return Ok(responseDto);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "OrderController::GetOrdersId :: Error");
            var errorResponse = new ErrorResponse
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                InnerExceptionMessage = ex.InnerException?.Message
            };

            return BadRequest(errorResponse);
        }
    }

    [HttpPost]
    public async Task<IActionResult> PostOrder([FromBody] OrderPostDTO orderPostDto)
    {
        _logger.LogDebug("OrderController::CreateOrder :: Started");
        try
        {
            if (!ModelState.IsValid || !ValidateOrderDto(orderPostDto))
            {
                return BadRequest(ModelState);
            }

            double orderPrice = 0;
            foreach(OrderItemPostDTO order in  orderPostDto.OrderItems)
            {
                Book book = await _bookRepository.GetByIdAsync(order.BookId); 
                if (book == null)
                    throw new Exception("Book with id: " +order.BookId+ " not found");
                order.Price = book.Price * order.Quantity;
                orderPrice += order.Price;
            }
            orderPrice = Math.Round(orderPrice, 2);
            Order newOrder = new Order
            {
                UserId = "user1",
                OrderDate = DateTime.Now,
                TotalPrice = orderPrice,
            };
            
            Order currentOrder = await _repository.AddAsync(newOrder);
            _logger.LogDebug($"OrderController::CreateOrder current order:: {currentOrder.OrderId}");
            foreach (OrderItemPostDTO orderItemPostDto in orderPostDto.OrderItems )
            {
                OrderItem entity = _mapper.Map<OrderItem>(orderItemPostDto);
                entity.OrderId = currentOrder.OrderId;
                OrderItem orderItem = await _orderItemRepository.AddAsync(entity);
                currentOrder.OrderItems.Add(orderItem);
            }
            
            OrderResponseDTO responseDto = _mapper.Map<OrderResponseDTO>(currentOrder);
            _logger.LogDebug("OrderController::CreateOrder :: Finished");
            return Ok(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OrderController::CreateOrder :: Error");
            var errorResponse = new ErrorResponse
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                InnerExceptionMessage = ex.InnerException?.Message
            };

            return BadRequest(errorResponse); 
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(string id)
    {
        _logger.LogDebug("OrderController::DeleteOrder :: Started");
        try
        {
            await _repository.DeleteAsync(id);
            _logger.LogDebug("OrderController::DeleteOrder :: Finished");
            return Ok($"Order with id {id} successfully deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OrderController::DeleteOrder :: Error");
            var errorResponse = new ErrorResponse
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                InnerExceptionMessage = ex.InnerException?.Message
            };
 
            return BadRequest(errorResponse);
        }
    }
    public bool ValidateOrderDto(OrderPostDTO orderPostDto)
    {
        if (orderPostDto.OrderItems.Count == 0)
        {
            throw new Exception("There are no order items");
        }
        foreach(OrderItemPostDTO order in  orderPostDto.OrderItems)
        {
            if(order.Quantity==0)
                throw new Exception($"There are no quantity for order item {order.BookId}");
        }

        return true;
    }
}
