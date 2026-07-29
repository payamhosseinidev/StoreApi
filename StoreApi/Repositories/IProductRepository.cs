using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task AddAsync(Product product);

        Task DeleteAsync(Product product);
        Task SaveChangesAsync();
    }
}
