using FluentValidation;
using StoreApi.DTOs;

namespace StoreApi.Validators
{
    public class UpdateOrderStatusDtoValidator:AbstractValidator<UpdateOrderStatusDto>
    {
        public UpdateOrderStatusDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("وضعیت سفارش نامعتبر است");
        }
    }
}
