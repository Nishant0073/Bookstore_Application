using AutoMapper;
using Bookstore_Application.Data;
using Bookstore_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore_Application.Repositories;

public class BookRepository:IRepository<Book>
{
    private ApplicationDbContext _context { get; set; }
    private readonly ILogger<BookRepository> _logger;
    private readonly DbSet<Book> _booksDbSet;

    public BookRepository(ApplicationDbContext context, ILogger<BookRepository> logger)
    {
        _context = context;
        _logger = logger;
        _booksDbSet = context.Set<Book>();
    }
    
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        _logger.LogDebug("BookRepository.GetAllAsync :: Started");
        try
        {
            IEnumerable<Book> result = await _booksDbSet.Include(b => b.Category).ToListAsync();
            _logger.LogDebug("BookRepository.GetAllAsync :: Finished");
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError("BookRepository.GetAllAsync :: Error", e.Message);
            throw new Exception($"BookRepository.GetAllAsync :: Error: {e.Message}");
        }
    }

    public async Task<Book> GetByIdAsync(string id)
    {
        _logger.LogDebug("BookRepository.GetByIdAsync:: Started");
        try
        {
            Book?  entity = await _booksDbSet.Include(b => b.Category).FirstOrDefaultAsync(book => book.BookId == id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"BookRepository.GetByIdAsync:: Book with id {id} not found");
            }
            return entity;
        }
        catch (Exception e)
        {
            throw new Exception("BookRepository.GetByIdAsync An error occured while getting book", e);
        }
    }
    public async Task<Book> AddAsync(Book entity)
    {
        _logger.LogDebug("BookRepository.AddAsync:: Started");
        try
        {
            var  existingBook = await _booksDbSet.FirstOrDefaultAsync( c => c.Title == entity.Title);
            if (existingBook != null)
            {
                _logger.LogError($"BookRepository.AddAsync:: Error {existingBook .Title} already exists");
                throw new InvalidOperationException($"A Book with the title '{entity.Title}' already exists.");
            }

            var  lastBook = await _booksDbSet
                .OrderByDescending(c => c.BookId)
                .FirstOrDefaultAsync();

            int nextId = lastBook != null
                ? int.Parse(lastBook.BookId.Substring(2)) + 1
                : 1;
            entity.BookId = "BK"+nextId; 
          
            await _booksDbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogDebug("BookRepository.AddAsync:: Finished");
            Book book =  await _booksDbSet.Include(b => b.Category).FirstOrDefaultAsync(book => book.BookId == entity.BookId);
            return book;
        }
        catch (Exception e)
        {
            throw new Exception("BookRepository:AddAsync  Error occured while adding new book", e);
        }
    }

    public async Task<Book> UpdateAsync(Book entity)
    {
        _logger.LogDebug("BookRepository.UpdateAsync:: Started");
        try
        {
            var  existingBook = await _booksDbSet.FirstOrDefaultAsync( c => c.Title== entity.Title);
            if (existingBook!= null && existingBook.BookId != entity.BookId)
            {
                _logger.LogError($"BookCategoryRepository.AddAsync:: Error {existingBook.Title} already exists");
                throw new InvalidOperationException($"A book with the name '{entity.Title}' already exists.");
            }
            _context.Entry(existingBook).State = EntityState.Detached;
            Book? book = await _booksDbSet.Include(b => b.Category).FirstOrDefaultAsync(book => book.BookId == entity.BookId);
            if (book== null)
            {
                _logger.LogError($"BookCategoryRepository.UpdateAsync:: Error {entity.BookId} not found");
                throw new Exception($"BookCategoryRepository.UpdateAsync:: Book with id {entity.BookId} not found"); }
            if(!string.IsNullOrEmpty(entity.Title))
                book.Title = entity.Title;
            if(!string.IsNullOrEmpty(entity.Author))
                book.Author = entity.Author;
            if(entity.CategoryId != null)
                book.CategoryId = entity.CategoryId;
            if(entity.Price != null)
                book.Price = entity.Price;
            if(entity.BookId!=null)
                book.BookId = entity.BookId;
            
            _booksDbSet.Update(book); 
            await _context.SaveChangesAsync();
            _logger.LogDebug("BookRepository.UpdateAsync:: Finished");
            return book;
        }
        catch (Exception e)
        {
            _logger.LogError("BookRepository.UpdateAsync:: Error", e.Message);
            throw new Exception($"Error occured while updating book", e);
        }
    }

    public async Task DeleteAsync(string id)
    {
        _logger.LogDebug("BookRepository.DeleteAsync:: Started");
        try
        {
            Book? entity = _booksDbSet.FirstOrDefault(b => b.BookId == id);
            if (entity == null)
            {
                _logger.LogError($"BookRepository.DeleteAsync:: Book with id {id} not found");
                throw new Exception($"BookRepository.DeleteAsync:: Book with id {id} not found");
            }
            _booksDbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("BookRepository.DeleteAsync:: Error", e.Message);
            throw new Exception($"Error occured while deleting book", e);
        }
    }
}