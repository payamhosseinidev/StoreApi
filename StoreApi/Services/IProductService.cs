using Microsoft.AspNetCore.Http.HttpResults;
using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto?> GetProductById(int id);
        Task Add(CreateProductDto dto);
        Task<bool> Update(int id,UpdateProductDto product);
        Task<bool> Delete(int id);
    }
}
