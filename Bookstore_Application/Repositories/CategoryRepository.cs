using Bookstore_Application.Data;
using Bookstore_Application.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Security;

namespace Bookstore_Application.Repositories;

public class CategoryRepository : IRepository<Category>
{
    private readonly Microsoft.EntityFrameworkCore.DbSet<Category> _dbSet;
    private readonly ILogger<CategoryRepository> _logger;
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext dbContext, ILogger<CategoryRepository> logger)
    {
        _dbSet = dbContext.Set<Category>();
        _context = dbContext;
        _logger = logger;
    }
    
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        _logger.LogDebug("CategoryRepository.GetAllAsync:: Started");
        try
        {
            IEnumerable<Category> result = await _dbSet.ToListAsync();
            _logger.LogDebug("CategoryRepository.GetAllAsync:: Finished");
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CategoryRepository.GetAllAsync:: Failed");
            throw new Exception("An error occured while getting all categories",e);
        }
    }

    public async Task<Category> GetByIdAsync(string id)
    {
        _logger.LogDebug("CategoryRepository.GetByIdAsync:: Started");
        try
        {
          Category?  entity = await _dbSet.FindAsync(id);
          if (entity == null)
          {
              throw new KeyNotFoundException($"Category with id {id} not found");
          }

          return entity;
        }
        catch (Exception e)
        {
            throw new Exception("An error occured while getting category", e);
        }
    }

    public async Task<Category> AddAsync(Category entity)
    {
        _logger.LogDebug("CategoryRepository.AddAsync:: Started");
        try
        {
            var  existingCategory = await _context.Categories.FirstOrDefaultAsync( c => c.CategoryName == entity.CategoryName );
            if (existingCategory != null)
            {
                _logger.LogError($"CategoryRepository.AddAsync:: Error {existingCategory.CategoryName} already exists");
                throw new InvalidOperationException($"A category with the name '{entity.CategoryName}' already exists.");
            }

            // Generate ID in CK+int format
            var lastCategory = await _context.Categories
                .OrderByDescending(c => Convert.ToInt32(c.CategoryId.Substring(2)))
                .FirstOrDefaultAsync();

            int nextId = lastCategory != null
                ? int.Parse(lastCategory.CategoryId.Substring(2)) + 1
                : 1;
           entity.CategoryId = "CK"+nextId; 
           
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogDebug("CategoryRepository.AddAsync:: Finished");
            return entity;
        }
        catch (Exception e)
        {
            throw new Exception("Error occured while adding new category", e);
        }
    }

    public async Task<Category> UpdateAsync(Category entity)
    {
        _logger.LogDebug("CategoryRepository.UpdateAsync:: Started");
        try
        {
            var  existingCategory = await _context.Categories.FirstOrDefaultAsync( c => c.CategoryName == entity.CategoryName );
            if (existingCategory != null)
            {
                _logger.LogError($"CategoryRepository.AddAsync:: Error {existingCategory.CategoryName} already exists");
                throw new InvalidOperationException($"A category with the name '{entity.CategoryName}' already exists.");
            }

            Category? category = await  _dbSet.FindAsync(entity.CategoryId);
            if (category == null)
            {
                _logger.LogError($"CategoryRepository.UpdateAsync:: Error {entity.CategoryId} not found");
                throw new Exception($"Category with id {entity.CategoryId} not found"); }
            
            
            category.CategoryName = entity.CategoryName;
            _context.Update(category);
            await _context.SaveChangesAsync();
            _logger.LogDebug("CategoryRepository.UpdateAsync:: Finished");
            return category;
        }
        catch (Exception e)
        {
            _logger.LogError($"CategoryRepository.UpdateAsync:: Error {entity.CategoryId} not found", e);
            throw new Exception($"Error occured while updating category", e);
        }
    }

    public async Task DeleteAsync(string id)
    {
        _logger.LogDebug("CategoryRepository.DeleteAsync:: Started");
        try
        {

            Category? category = await  _dbSet.FindAsync(id);
            if (category == null)
            {
                _logger.LogError($"CategoryRepository.UpdateAsync:: Error {id} not found");
                throw new KeyNotFoundException($"Category with id {id} not found");
            }
            _dbSet.Remove(category);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError($"CategoryRepository.DeleteAsync:: Error {id} not found", e);
            throw new KeyNotFoundException($"Error occured while deleting category", e);
        }
    }

    public PaginatedList<Book> GetPaginatedItems(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }
}