using Microsoft.AspNetCore.Mvc;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly List<Product> products = new()
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

        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
           
            var product = products.FirstOrDefault(i=>i.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
