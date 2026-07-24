using StoreApi.Models;

namespace StoreApi.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new()
            {
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "Gaming Laptop",
                    Category = "Electronics",
                    Price = 45000000,
                    ImageUrl = "laptop.jpg",
                    Stock=5,
                    CreatedAt = DateTime.Now,
                },
                new Product
                {
                    Id = 2,
                    Name = "iPhone",
                    Description = "Apple Mobile",
                    Category = "Electronics",
                    Price = 80000000,
                    ImageUrl = "iphone.jpg",
                    Stock=3,
                    CreatedAt = DateTime.Now,
                },
                 new Product
                {
                    Id = 3,
                    Name = "smart tv",
                    Description = "television",
                    Category = "Electronics",
                    Price = 92000000,
                    ImageUrl = "smarttv.jpg",
                    Stock=7,
                    CreatedAt = DateTime.Now,
                }

            };
        public void Add(Product product)
        {
            _products.Add(product);
        }

        public bool Delete(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return false;
            }
            _products.Remove(product);
            return true;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _products;
        }

        public Product? GetProductById(int id)
        {
            var product = _products.FirstOrDefault(i => i.Id == id);
            return product;
        }

        public bool Update(Product product)
        {
            var existingProduct = _products.FirstOrDefault(i=>i.Id== product.Id);
            if (existingProduct == null)
            {
                return false;
            }
            
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Category = product.Category;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.ImageUrl = product.ImageUrl;
            return true;
          
        }
    }
}
