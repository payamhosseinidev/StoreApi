using StoreApi.Common;
using StoreApi.DTOs;

namespace StoreApi.Services
{
    public interface IAuthService
    {
        Task<Result<string>> Register(RegisterDto dto);
        Task<Result<string>> Login(LoginDto dto);
    }
}
