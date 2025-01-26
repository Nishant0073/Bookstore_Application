using Bookstore_Application.Data;
using Bookstore_Application.Models;
using Bookstore_Application.Repositories;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IRepository<Order>
{
    private readonly DbSet<Order>_orderDbSet;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(ApplicationDbContext context, ILogger<OrderRepository> logger)
    {
       _context = context;
       _logger = logger;
       _orderDbSet = context.Set<Order>();
    }


    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        _logger.LogDebug("OrderRepository.GetAllAsync :: Started");
        try
        {
            IEnumerable<Order> orders = await _orderDbSet.Include(o => o.OrderItems).ToListAsync();
            _logger.LogDebug("OrderRepository.GetAllAsync :: Finished");
            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogDebug("OrderRepository.GetAllAsync :: Failed");
            throw new Exception($"OrderRepository.GetAllAsync :: Error: {ex.Message}", ex.InnerException);
        }
    }

    public async Task<Order> GetByIdAsync(string id)
    {
        _logger.LogDebug("OrderRepository.GetByIdAsync :: Started");
        try
        {
            Order? order = await _orderDbSet.Include(o => o.OrderItems).FirstOrDefaultAsync(order => order.OrderId == id);
            if (order == null)
            {
                _logger.LogDebug("OrderRepository.GetByIdAsync :: Not found");
                throw new Exception($"OrderRepository.GetByIdAsync :: Not found");
            }
            return order;
        }
        catch (Exception ex)
        {
            throw new Exception($"OrderRepository.GetByIdAsync :: Error: {ex.Message}", ex.InnerException);
        }
    }

    public async Task<Order> AddAsync(Order entity)
    {
        _logger.LogDebug("OrderRepository.AddAsync :: Started");
        try
        {
            var lastOrder = _orderDbSet
                .OrderByDescending(o => Convert.ToInt32(o.OrderId.Substring(2)))
                .FirstOrDefault();


            int newOrderId = lastOrder!= null ? int.Parse(lastOrder.OrderId.Substring(2)) + 1 : 0;
            
            entity.OrderId = "OK" + newOrderId;
            await _orderDbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogDebug("OrderRepository.AddAsync :: Finished");
            return entity;
        }
        catch (Exception e)
        {
            throw new Exception($"OrderRepository.AddAsync :: Error: {e.Message}", e.InnerException);
        }
    }

    public async Task<Order> UpdateAsync(Order entity)
    {
        _logger.LogDebug("OrderRepository.UpdateAsync :: Started");
        try
        {
            var existingOrder = _orderDbSet.Find(entity.OrderId);
            if (existingOrder == null)
            {
                _logger.LogDebug("OrderRepository.UpdateAsync :: Not found");
                throw new Exception($"OrderRepository.UpdateAsync :: Not found");
            }

            if (existingOrder.TotalPrice != entity.TotalPrice)
            {
                existingOrder.TotalPrice = entity.TotalPrice;
            }

            _orderDbSet.Update(existingOrder);
            await _context.SaveChangesAsync();
            _logger.LogDebug("OrderRepository.UpdateAsync :: Finished");
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogDebug("OrderRepository.UpdateAsync :: Error: {ex.Message}", ex.InnerException);
            throw new Exception($"OrderRepository.UpdateAsync :: Error: {ex.Message}", ex.InnerException);
        }
    }

    public async Task DeleteAsync(string id)
    {
        _logger.LogDebug("OrderRepository.DeleteAsync :: Started");
        try
        {
            var order = await _orderDbSet.FindAsync(id);
            if (order == null)
            {
                _logger.LogDebug("OrderRepository.DeleteAsync :: Not found");
                throw new Exception($"OrderRepository.DeleteAsync :: Not found");
            }

            _orderDbSet.Remove(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogDebug("OrderRepository.DeleteAsync :: Error: {ex.Message}", ex.InnerException);
            throw new Exception($"OrderRepository.DeleteAsync :: Error: {ex.Message}", ex.InnerException);
        }
    }
}