using AutoMapper;
using Bookstore_Application.DTOs.Category;
using Bookstore_Application.Models;

namespace Bookstore_Application;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryRequestDTO, Category>();
        CreateMap<Category, CategoryReponseDTO>();
    }
}