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

        [HttpGet("paged")]
        public async Task<ActionResult<Result<PaginationDto<ProductDto>>>> GetProductsPaged(
            int page = 1,
            int pageSize = 3,
            string? search = null,
            string? category = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string? sortBy = null,
            string? sortOrder = null
            )
        {
            var filter = new ProductFilterDto
            {
                Search = search,
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy,
                SortOrder = sortOrder
            };
            var result = await _productService.GetProductsPaged(page,pageSize, filter);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
