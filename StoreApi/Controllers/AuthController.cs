using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreApi.Common;
using StoreApi.DTOs;
using StoreApi.Services;
using System.Security.Claims;

namespace StoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUser;

        public AuthController(IAuthService authService,ICurrentUserService currentUser)
        {
            _authService = authService;
            _currentUser = currentUser;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Result<string>>> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto);

            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<string>>> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new {
                UserId = _currentUser.UserId,
                Username = _currentUser.Username,
                Role = _currentUser.Role
            });
        }
    }
}
