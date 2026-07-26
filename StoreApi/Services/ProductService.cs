using StoreApi.Data;
using StoreApi.DTOs;
using StoreApi.Mappers;
using StoreApi.Models;

namespace StoreApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public void Add(CreateProductDto dto)
        {
            var product = ProductMapper.ToEntity(dto);
            _context.Products.AddAsync(product);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<ProductDto> GetAllProducts()
        {
            return _context.Products
                .Select(p => new ProductDto
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Description = p.Description,
                     Category = p.Category,
                     Price = p.Price,
                     ImageUrl = p.ImageUrl,
                     Stock = p.Stock,
                     CreatedAt = p.CreatedAt
                })
            .ToList();
        }

        public ProductDto? GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return null;
            }
            return ProductMapper.ToDto(product);
        }

        public bool Update(int id,UpdateProductDto dto)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return false;
            }
            
            ProductMapper.UpdateEntity(product, dto);

            _context.SaveChanges();
            return true;
          
        }

    }
}
