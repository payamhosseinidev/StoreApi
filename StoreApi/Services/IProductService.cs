using StoreApi.Common;
using StoreApi.DTOs;

namespace StoreApi.Services
{
    public interface IProductService
    {
        Task<Result<IEnumerable<ProductDto>>> GetAllProducts();
        Task<Result<ProductDto>> GetProductById(int id);
        Task<Result<ProductDto>> Add(CreateProductDto dto);
        Task<Result<ProductDto>> Update(int id,UpdateProductDto product);
        Task<Result<bool>> Delete(int id);
    }
}
