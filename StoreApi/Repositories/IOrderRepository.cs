using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(int id);
        Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        Task<List<Order>> GetAllByUserIdAsync(int userId);
        Task<List<Order>> GetAllAsync();

        Task SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

    }
}
