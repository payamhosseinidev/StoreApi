using AutoMapper;
using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Mapping
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category,CategoryDto>();
        }
    }
}
