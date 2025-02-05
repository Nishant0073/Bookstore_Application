using Bookstore_Application.Models;
using Bookstore_Application.Models.ReponseModels;

namespace Bookstore_Application.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(string id);
    PaginatedList<Book> GetPaginatedItems(int pageNumber, int pageSize);
}