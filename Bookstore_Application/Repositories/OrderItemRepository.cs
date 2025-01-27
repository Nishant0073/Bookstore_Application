using Bookstore_Application.Data;
using Bookstore_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Application.Repositories;

public class OrderItemRepository: IRepository<OrderItem>
{
    private readonly DbSet<OrderItem>_orderItemDbSet;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderItemRepository> _logger;

    public OrderItemRepository(ApplicationDbContext context, ILogger<OrderItemRepository> logger)
    {
        _context = context;
        _logger = logger;
        _orderItemDbSet = context.Set<OrderItem>();
    }

    public Task<IEnumerable<OrderItem>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<OrderItem> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderItem> AddAsync(OrderItem entity)
    {
       _logger.LogInformation("OrderItemRepository.AddOrderItem :: started ");
       try
       {
           var lastOrderItem = await _orderItemDbSet
               .OrderByDescending(o => Convert.ToInt32(o.OrderItemId.Substring(3)))
               .FirstOrDefaultAsync();
           
           int num = int.Parse((lastOrderItem != null) ? lastOrderItem.OrderItemId.Substring(3) : "0") + 1;
           entity.OrderItemId = "OIK" + num.ToString();
           _context.Entry(lastOrderItem).State = EntityState.Detached;
           
           await _orderItemDbSet.AddAsync(entity);
           await _context.SaveChangesAsync();
           _logger.LogInformation("OrderItemRepository.AddOrderItem :: finished ");
           return entity;
       }
       catch (Exception e)
       {
           _logger.LogInformation("OrderItemRepository.AddOrderItem :: error : " + e.Message);
           throw;
       }
    }

    public Task<OrderItem> UpdateAsync(OrderItem entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public PaginatedList<Book> GetPaginatedItems(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }
}