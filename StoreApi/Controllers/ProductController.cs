using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Services;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetProducts()
        {
            var products = _productService.GetAllProducts();
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Category = p.Category,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Stock = p.Stock,
                CreatedAt = p.CreatedAt
            });
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProduct(int id)
        {

            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id,UpdateProductDto dto)
        {
            var result = _productService.Update(id,dto);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var success = _productService.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPost]
        public IActionResult CreateProduct(CreateProductDto dto)
        {
            _productService.Add(dto);
            return Created();
        }
    }
}
