using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            var totalCount = await query.CountAsync();

            var products = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (products, totalCount);
        }
    }
}
