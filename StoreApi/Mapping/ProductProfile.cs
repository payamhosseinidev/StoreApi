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

            CreateMap<Product, ProductDto>();

        }
    }
}
