using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.DTOs;
using StoreApi.Services;

namespace StoreApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController:ControllerBase
    {
        private readonly IPaymentService _service;
        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if(!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _service.GetByOrderIdAsync(orderId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaged(int page = 1,int pagSize = 10)
        {
            var result = await _service.GetPagedAsync(page, pagSize);

            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("confirm/{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm(int orderId,ConfirmPaymentDto dto)
        {
            var result = await _service.ConfirmAsync(orderId, dto);

            if(!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
