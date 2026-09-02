using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p=>p.OrderId == orderId);
        }

        public async Task<(IEnumerable<Payment> Payments, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize)
        {
            var query = _context.Payments
                .AsNoTracking()
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var payments = await query
                .OrderByDescending(p=>p.CreatedAt)
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (payments,totalCount);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
