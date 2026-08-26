using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.DTOs;
using StoreApi.Services;
using System.Security.Claims;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController:ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {

            var result = await _service.CreateAsync(dto);

            if (!result.Success)
                return BadRequest(result);
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if(!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _service.CancelAsync(id);
            if(!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [Authorize(Roles ="Admin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id,[FromBody] UpdateOrderStatusDto dto)
        {
            var result = await _service.UpdateStatusAsync(id, dto);

            if(!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
