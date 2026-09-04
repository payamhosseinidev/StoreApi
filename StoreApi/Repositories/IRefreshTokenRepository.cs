using StoreApi.DTOs;
using StoreApi.Models;

namespace StoreApi.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetByHashAsync(string tokenHash); 
        Task SaveChangesAsync();
    }
}
