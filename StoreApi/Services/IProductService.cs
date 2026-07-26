using Microsoft.AspNetCore.Http.HttpResults;
using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Services
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAllProducts();
        ProductDto? GetProductById(int id);
        void Add(CreateProductDto dto);
        bool Update(int id,UpdateProductDto product);
        bool Delete(int id);
    }
}
