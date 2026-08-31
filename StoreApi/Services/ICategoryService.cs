using StoreApi.Common;
using StoreApi.DTOs;

namespace StoreApi.Services
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDto>>> GetAllAsync();
        Task<Result<CategoryDto>> GetByIdAsync(int id);
        Task<Result<CategoryDto>> CreateAsync(CreateCategoryDto dto);
        Task<Result<CategoryDto>> UpdateAsync(int id,UpdateCategoryDto dto);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
