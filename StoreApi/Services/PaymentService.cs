using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using StoreApi.Common;
using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public PaymentService(
            IPaymentRepository paymentRepository
            ,IOrderRepository orderRepository
            ,ICurrentUserService currentUserService
            , IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<Result<PaymentDto>> CreateAsync(CreatePaymentDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(dto.OrderId);
            if (order is null)
                return Result<PaymentDto>.Failure("سفارش مورد نظر پیدا نشد");

            if (_currentUserService.Role != "Admin" && order.UserId != _currentUserService.UserId)
                return Result<PaymentDto>.Failure("شما به این سفارش دسترسی ندارید");

            var existingPayment = await _paymentRepository.GetByOrderIdAsync(dto.OrderId);
            if(existingPayment is not null)
                return Result<PaymentDto>.Failure("برای این سفارش قبلا پرداخت ثبت شده است");

            var amount = order.Items.Sum(item => item.Quantity * item.UnitPrice);

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = amount,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            var paymentDto = _mapper.Map<PaymentDto>(payment);
            return Result<PaymentDto>.SuccessResult(paymentDto, "پرداخت با موفقیت ایجاد شد");
        }

        public async Task<Result<PaymentDto>> GetByOrderIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order is null)
                return Result<PaymentDto>.Failure("سفارش مورد نظر پیدا نشد");

            if (_currentUserService.Role != "Admin" &&
                   order.UserId != _currentUserService.UserId)
            {
                return Result<PaymentDto>.Failure(
                    "شما به این سفارش دسترسی ندارید");
            }

            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

            if (payment is null)
                return Result<PaymentDto>.Failure("پرداختی برای این سفارش پیدا نشد");

            var paymentDto = _mapper.Map<PaymentDto>(payment);

            return Result<PaymentDto>.SuccessResult(paymentDto, "اطلاعات پرداخت با موفقیت دریافت شد");
        }

        public async Task<Result<PaginationDto<PaymentDto>>> GetPagedAsync(int page,int pageSize)
        {
            if (page < 1)
                return Result<PaginationDto<PaymentDto>>.Failure("شماره صفحه باید بزرگتر از یک باشد");

            if(pageSize<1 || pageSize>100)
                return Result<PaginationDto<PaymentDto>>.Failure("تعداد پرداخت‌ها در هر صفحه باید بین 1 تا 100 باشد");

            var (payments, totalCount) = await _paymentRepository.GetPagedAsync(page, pageSize);
            
            var paymentDtos = _mapper.Map<IEnumerable<PaymentDto>>(payments);

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var pagination = new PaginationDto<PaymentDto>
            {
                Items = paymentDtos,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return Result<PaginationDto<PaymentDto>>.SuccessResult(
                pagination,
                "پرداخت ها با موفقیت دریافت شدند"
            );
        }
        public async Task<Result<PaymentDto>> ConfirmAsync(int orderId, ConfirmPaymentDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order is null)
                return Result<PaymentDto>.Failure("سفارش مورد نظر پیدا نشد");

            var payment = order.Payment;

            if (payment is null)
                return Result<PaymentDto>.Failure("پرداخت مورد نظر پیدا نشد");         

            if (payment.Status != PaymentStatus.Pending)
                return Result<PaymentDto>.Failure("این پرداخت قبلاً تعیین تکلیف شده است");

            await _orderRepository.BeginTransactionAsync();

            try
            {
                if (dto.IsSuccessful)
                {
                    payment.Status = PaymentStatus.Successful;
                    order.Status = OrderStatus.Paid;
                }
                else
                {
                    payment.Status = PaymentStatus.Failed;
                }
                await _orderRepository.SaveChangesAsync();
                await _orderRepository.CommitTransactionAsync();

                var paymentDto = _mapper.Map<PaymentDto>(payment);

                return Result<PaymentDto>.SuccessResult(
                    paymentDto,
                    dto.IsSuccessful? "پرداخت با موفقیت تأیید شد" : "پرداخت ناموفق بود"
                );
            }
            catch
            {
                await _orderRepository.RollbackTransactionAsync();
                throw;
            }
        }


    }
}
