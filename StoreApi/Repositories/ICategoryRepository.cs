using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task AddAsync(Category category);
        Task DeleteAsync(Category category);
        Task SaveChangesAsync();
    }
}
