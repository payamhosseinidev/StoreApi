using AutoMapper;
using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>();

            CreateMap<UpdateProductDto, Product>();

            CreateMap<Product, ProductDto>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty)
                );

        }
    }
}
