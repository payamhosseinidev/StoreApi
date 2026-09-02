using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByOrderIdAsync(int orderId);
        Task AddAsync(Payment payment);
        Task<(IEnumerable<Payment> Payments,int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task SaveChangesAsync();
    }
}
