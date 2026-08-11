using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Common;
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
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var result = await _productService.GetAllProducts();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var result = await _productService.GetProductById(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<ProductDto>>> UpdateProduct(int id,UpdateProductDto dto)
        {
            var result = await _productService.Update(id,dto);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<bool>>> DeleteProduct(int id)
        {
            var result = await _productService.Delete(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<Result<ProductDto>>> CreateProduct(CreateProductDto dto)
        {
            var result = await _productService.Add(dto);
            return Ok(result);
        }
    }
}
