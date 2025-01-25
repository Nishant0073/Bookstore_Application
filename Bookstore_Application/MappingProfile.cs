using AutoMapper;
using Bookstore_Application.DTOs.Category;
using Bookstore_Application.DTOs.Order;
using Bookstore_Application.DTOs.OrderItems;
using Bookstore_Application.Models;

namespace Bookstore_Application;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        //Category
        CreateMap<CategoryRequestDTO, Category>();
        CreateMap<Category, CategoryReponseDTO>();
        CreateMap<CategoryPutDTO, Category>();
        
        //Book
        CreateMap<BookPostDTO, Book>();
        CreateMap<Book, BookResponseDTO>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.CategoryName));
        CreateMap<BookPutDTO, Book>();
       
        //Order
        CreateMap<Order,OrderResponseDTO>();
        
        //OrderItem
        CreateMap<OrderItemPostDTO, OrderItem>();
        CreateMap<OrderItem, OrderItemResponseDTO>();
        
    }
}