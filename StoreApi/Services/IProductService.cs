using Microsoft.AspNetCore.Http.HttpResults;
using StoreApi.Models;

namespace StoreApi.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts();
        Product? GetProductById(int id);
        void Add(Product product);
        bool Update(Product product);
        bool Delete(int id);
    }
}
