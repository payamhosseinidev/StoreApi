using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            ProductFilterDto? filter
            ); 

        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        Task DeleteAsync(Product product);
        Task SaveChangesAsync();
    }
}
