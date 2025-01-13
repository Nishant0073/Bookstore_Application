using AutoMapper;
using Bookstore_Application.DTOs.Category;
using Bookstore_Application.Models;
using Bookstore_Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore_Application.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BooksController: ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private IMapper _mapper;
    private IRepository<Book> _bookRepository;

    public BooksController(ILogger<BooksController> logger, IMapper mapper, IRepository<Book> bookRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _bookRepository = bookRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        _logger.LogDebug("BookController.GetBooks :: Started");
        try
        {
            IEnumerable<Book>  books = await _bookRepository.GetAllAsync();
            IEnumerable<BookResponseDTO> booksDto = _mapper.Map<IEnumerable<BookResponseDTO>>(books);
           
            _logger.LogDebug("BookController.GetBooks :: Finished");
            return Ok(booksDto);
        }
        catch (Exception ex)
        {
            _logger.LogError("BookController.GetBooks :: Error");
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
    public    async Task<IActionResult> GetBook(string id)
    {
        _logger.LogDebug("BookController.GetBook :: Started");
        try
        {
            Book book = await _bookRepository.GetByIdAsync(id);
            BookResponseDTO bookDto = _mapper.Map<BookResponseDTO>(book);
            return Ok(bookDto);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError("BookController.GetBook :: NotFound");
            return NotFound(ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError("BookController.GetBook :: Failed");
            var errorResponse = new ErrorResponse
            {
                Message = e.Message,
                StackTrace = e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message
            };

            return StatusCode(500, errorResponse); // Or log errorResponse
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> PostBook([FromBody] BookPostDTO bookPostDto)
    {
        _logger.LogDebug("BookController.PostBook :: Started");
        
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("BookController.PostBook :: Bad Request");
                return BadRequest(ModelState);
            }
            var book = _mapper.Map<Book>(bookPostDto); 
           
            Book responseCategory = await _bookRepository.AddAsync(book); 
            
            BookResponseDTO bookResponseDto = _mapper.Map<BookResponseDTO>(responseCategory);
             
            _logger.LogDebug("BookController.PostBook :: Success");
            return Created(bookResponseDto.BookId,bookResponseDto);
        }
        catch (Exception e)
        {
            _logger.LogError("BookController.PostBook :: Failed");
            var errorResponse = new ErrorResponse
            {
                Message = e.Message,
                StackTrace = e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message
            };
            return BadRequest(errorResponse);
        }
    }
    
    [HttpPut]
    public async Task<IActionResult> PutBook([FromBody] BookPutDTO bookPutDto)
    {
        _logger.LogDebug("BookController.PutBook :: Started ");
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("BookController.PutBook :: Bad Request");
                return BadRequest(ModelState);
            }
            var book = _mapper.Map<Book>(bookPutDto);
            Book responseBook = await _bookRepository.UpdateAsync(book);
            BookResponseDTO bookResponseDto = _mapper.Map<BookResponseDTO>(responseBook);
            _logger.LogDebug("BookController.PutBook :: Success");
            return Ok(bookResponseDto);
        }
        catch (Exception e)
        {
            _logger.LogError("BookController.PutBook :: Failed");
            var errorResponse = new ErrorResponse
            {
                Message = e.Message,
                StackTrace = e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message
            };
            return BadRequest(errorResponse); 
        }
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        _logger.LogDebug("BookController.DeleteBook :: Started ");
        try
        {
            await _bookRepository.DeleteAsync(id);
            _logger.LogDebug("BookController.DeleteBook :: Success");
            return Ok($"Book with id {id} deleted!");
        }
        catch (Exception e)
        {
            _logger.LogError("BookController.DeleteBook :: Failed");
            var errorResponse = new ErrorResponse
            {
                Message = e.Message,
                StackTrace = e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message
            };
            return BadRequest(errorResponse); 
        }
    }

    
}
