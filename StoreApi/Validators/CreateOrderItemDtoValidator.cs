using FluentValidation;
using StoreApi.DTOs;

namespace StoreApi.Validators
{
    public class CreateOrderItemDtoValidator:AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("شناسه محصول معتبر نیست");
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("تعداد محصول باید بیشتر از صفر باشد");
        }
    }
}
