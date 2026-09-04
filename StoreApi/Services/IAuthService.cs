using StoreApi.Common;
using StoreApi.DTOs;

namespace StoreApi.Services
{
    public interface IAuthService
    {
        Task<Result<string>> Register(RegisterDto dto);
        Task<Result<AuthResponseDto>> Login(LoginDto dto);
        Task<Result<AuthResponseDto>> RefreshAsync(string refreshToken);
        Task<Result<bool>> LogoutAsync(string refreshToken);
    }
}
