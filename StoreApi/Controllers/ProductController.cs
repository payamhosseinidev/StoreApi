using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {

            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id,Product product)
        {
            if(id != product.Id)
            {
                return BadRequest();
            }
            var result = _productService.Update(product);
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
        public IActionResult CreateProduct(Product product)
        {
            _productService.Add(product);
            return CreatedAtAction(
                nameof(GetProduct),
                new { id = product.Id },
                product
            );
        }
    }
}
