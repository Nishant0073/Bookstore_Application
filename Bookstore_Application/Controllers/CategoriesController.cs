using AutoMapper;
using Bookstore_Application.Data;
using Bookstore_Application.DTOs.Category;
using Bookstore_Application.Models;
using Bookstore_Application.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Bookstore_Application.Controllers;

[ApiController]
[Route($"/api/[controller]")]
[Authorize(Roles = "Admin")]
public class CategoriesController: ControllerBase
{
    private readonly ILogger<CategoriesController> _logger;
    private readonly IRepository<Category> _categoryRepo;
    private readonly IMapper _mapper;
    public CategoriesController( ILogger<CategoriesController> logger, IRepository<Category> categoryRepo, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _categoryRepo = categoryRepo;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories()
    {
        _logger.LogDebug("CategoriesController.GetCategories :: Started");
        try
        {
            IEnumerable<Category> categories = await _categoryRepo.GetAllAsync();
            _logger.LogDebug("CategoriesController.GetCategories :: Finished");
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError("CategoriesController.GetCategories :: Finished");
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
    [AllowAnonymous]
    public    async Task<IActionResult> GetCategory(string id)
    {
        _logger.LogDebug("CategoriesController.GetCategory :: Started");
        try
        {
            Category category = await _categoryRepo.GetByIdAsync(id);
            return Ok(category);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError("CategoriesController.GetCategory :: NotFound");
            return NotFound(ex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError("CategoriesController.GetCategory :: Failed");
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
    public async Task<IActionResult> PostCategory([FromBody] CategoryRequestDTO categoryRequestDTO)
    {
        _logger.LogDebug("CategoriesController.PostCategory :: Started");
        
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CategoriesController.PostCategory :: Bad Request");
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(categoryRequestDTO); 
           
            Category responseCategory = await _categoryRepo.AddAsync(category); 
            
            CategoryReponseDTO responseCategoryDto = _mapper.Map<CategoryReponseDTO>(responseCategory);
             
            _logger.LogDebug("CategoriesController.PostCategory :: Finished");
            return Created(responseCategory.CategoryId, responseCategoryDto);
        }
        catch (Exception e)
        {
            _logger.LogError("CategoriesController.PostCategory :: Failed");
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
    public async Task<IActionResult> PutCategory([FromBody] CategoryPutDTO categoryPutDTO)
    {
        _logger.LogDebug("CategoriesController.PutCategory :: Started");
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CategoriesController.PutCategory :: Bad Request");
                return BadRequest(ModelState);
            }
            var category = _mapper.Map<Category>(categoryPutDTO);
            Category responseCategory = await _categoryRepo.UpdateAsync(category);
            CategoryReponseDTO responseCategoryDto = _mapper.Map<CategoryReponseDTO>(responseCategory);
            _logger.LogDebug("CategoriesController.PutCategory :: Finished");
            return Ok(responseCategoryDto);
        }
        catch (Exception e)
        {
            _logger.LogError("CategoriesController.PutCategory :: Failed");
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
    public async Task<IActionResult> DeleteCategory(string id)
    {
        _logger.LogDebug("CategoriesController.DeleteCategory :: Started");
        try
        {
            await _categoryRepo.DeleteAsync(id);
            return Ok($"Category with CategoryId {id} deleted");
        }
        catch (Exception e)
        {
           _logger.LogError("CategoriesController.DeleteCategory :: Failed");
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