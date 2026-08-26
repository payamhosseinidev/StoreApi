using StoreApi.Common;
using StoreApi.DTOs;

namespace StoreApi.Services
{
    public interface IOrderService
    {
        Task<Result<OrderDto>> CreateAsync(CreateOrderDto dto);
        Task<Result<OrderDto>> GetByIdAsync(int id);
        Task<Result<List<OrderDto>>> GetAllAsync();
        Task<Result<OrderDto>> CancelAsync(int id);
        Task<Result<OrderDto>> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
    }
}
