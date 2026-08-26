using AutoMapper;
using FluentValidation;
using StoreApi.Common;
using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Services
{
    public class OrderService:IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<CreateOrderDto> _validator;
        private readonly IValidator<UpdateOrderStatusDto> _statusValidator;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository repository, 
            ICurrentUserService currentUserService,
            IValidator<CreateOrderDto> validator,
            IValidator<UpdateOrderStatusDto> statusValidator,
            IMapper mapper)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _statusValidator = statusValidator;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> CreateAsync(CreateOrderDto dto)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                return Result<OrderDto>.Failure("کاربر احراز هویت نشده است.");
            }

            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(
                    ", ",
                    validationResult.Errors.Select(x => x.ErrorMessage)
                );
                return Result<OrderDto>.Failure(errors);
            }

            //Add Distinct() To prevent duplicated queries
            var productIds = dto.Items
                .Select(x => x.ProductId)
                .Distinct()
                .ToList();

            var products = await _repository.GetProductsByIdsAsync(productIds);

            //Check if all Products exist
            if(products.Count != productIds.Count)
            {
                return Result<OrderDto>.Failure("یک یا چند محصول پیدا نشد");
            }

            //Check if Stock exists for every item
            foreach(var item in dto.Items)
            {
                var product = products.First(p=>p.Id == item.ProductId);
                if (product.Stock < item.Quantity)
                {
                    return Result<OrderDto>.Failure(
                         $"موجودی محصول {product.Name} کافی نیست."
                    );
                }
            }
            
            //Create Order
            var order = new Order
            {
                UserId = userId.Value,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };
            //Find product from database for each item user has sent, then add it to OrderItem 
            foreach (var item in dto.Items)
            {
                var product = products
                    .First(p => p.Id == item.ProductId);

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                order.Items.Add(orderItem);
                // Product is tracked by the DbContext,
                // so EF Core detects the Stock change on SaveChangesAsync().
                product.Stock -= item.Quantity;
            }

            await _repository.AddAsync(order);
            await _repository.SaveChangesAsync();

            var orderDto = _mapper.Map<OrderDto>(order);

            return Result<OrderDto>.SuccessResult(
                orderDto,
                "سفارش با موفقیت ایجاد شد"
            );
        }

        public async Task<Result<List<OrderDto>>> GetAllAsync()
        {
            List<Order> orders;

            if(_currentUserService.Role == "Admin")
            {
                orders = await _repository.GetAllAsync();
            }
            else
            {
                var userId = _currentUserService.UserId;

                if(userId is null)
                {
                    return Result<List<OrderDto>>.Failure(
                        "کاربر احراز هویت نشده است"
                    );
                }

                orders = await _repository.GetAllByUserIdAsync(userId.Value);
            }

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            return Result<List<OrderDto>>.SuccessResult(
                orderDtos,
                "سفارش ها با موفقیت دریافت شدند"
            );
        }

        public async Task<Result<OrderDto>> GetByIdAsync(int id)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                return Result<OrderDto>.Failure("کاربر احراز هویت نشده است.");

            var order = await _repository.GetByIdAsync(id);
            if(order is null)
                return Result<OrderDto>.Failure("سفارش پیدا نشد.");

            if(order.UserId !=  userId.Value)
                return Result<OrderDto>.Failure("شما به این سفارش دسترسی ندارید.");

            var orderDto = _mapper.Map<OrderDto>(order);

            return Result<OrderDto>.SuccessResult(
                orderDto,
                "سفارش با موفقیت دریافت شد."
            );

        }

        public async Task<Result<OrderDto>> CancelAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);
            if (order is null)
                return Result<OrderDto>.Failure("سفارش پیدا نشد");

            if (_currentUserService.Role != "Admin" && order.UserId != _currentUserService.UserId)
                return Result<OrderDto>.Failure("شما به این سفارش دسترسی ندارید");

            if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Paid)
                return Result<OrderDto>.Failure($"سفارش با وضعیت {order.Status} قابل لغو نیست.");

            foreach(var item in order.Items)
            {
                item.Product.Stock += item.Quantity;
            }

            order.Status = OrderStatus.Cancelled;

            await _repository.SaveChangesAsync();

            var orderDto = _mapper.Map<OrderDto>(order);

            return Result<OrderDto>.SuccessResult(
                orderDto,
                "سفارش با موفقیت لغو شد"
            );
        }

        public async Task<Result<OrderDto>> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
        {
            var validationResult = await _statusValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(
                    ", ",
                    validationResult.Errors.Select(x => x.ErrorMessage)
                );
                return Result<OrderDto>.Failure(errors);
            }

            var order = await _repository.GetByIdAsync(id);

            if (order is null)
                return Result<OrderDto>.Failure("سفارش پیدا نشد");

            if (!IsValidStatusTransition(order.Status, dto.Status))
            {
                return Result<OrderDto>.Failure(
                    $"تغییر وضعیت از {order.Status} به {dto.Status} مجاز نیست."
                );
            }

            order.Status = dto.Status;
            await _repository.SaveChangesAsync();

            var ordrDto = _mapper.Map<OrderDto>(order);

            return Result<OrderDto>.SuccessResult(
                ordrDto,
                "وضعیت سفارش با موفقیت تغییر کرد"
            );
        }
        private bool IsValidStatusTransition(OrderStatus currentStatus,OrderStatus newStatus)
        {
            return currentStatus switch
            {
                OrderStatus.Pending =>
                    newStatus == OrderStatus.Paid ||
                    newStatus == OrderStatus.Cancelled,

                OrderStatus.Paid =>
                    newStatus == OrderStatus.Processing ||
                    newStatus == OrderStatus.Cancelled,

                OrderStatus.Processing =>
                    newStatus == OrderStatus.Shipped ||
                    newStatus == OrderStatus.Cancelled,

                OrderStatus.Shipped =>
                    newStatus == OrderStatus.Delivered,

                OrderStatus.Delivered => false,

                OrderStatus.Cancelled => false,

                _ => false
            };
        }
    }
}
