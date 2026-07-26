using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Mappers
{
    public class ProductMapper
    {
        public static ProductDto ToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                CreatedAt = product.CreatedAt
            };
        }

        public static Product ToEntity(CreateProductDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                Stock = dto.Stock,
                CreatedAt = DateTime.Now
            };
        }
        
        public static void UpdateEntity(Product product,UpdateProductDto dto)
        {
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Category = dto.Category;
            product.Price = dto.Price;
            product.ImageUrl = dto.ImageUrl;
            product.Stock = dto.Stock;
        }
    }
}
