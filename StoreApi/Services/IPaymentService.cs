using StoreApi.Common;
using StoreApi.DTOs;

namespace StoreApi.Services
{
    public interface IPaymentService
    {
        Task<Result<PaymentDto>> CreateAsync(CreatePaymentDto dto);
        Task<Result<PaymentDto>> GetByOrderIdAsync(int  orderId);
        Task<Result<PaginationDto<PaymentDto>>> GetPagedAsync(int page, int pageSize);
        Task<Result<PaymentDto>> ConfirmAsync(int orderId,ConfirmPaymentDto dto);
    }
}
