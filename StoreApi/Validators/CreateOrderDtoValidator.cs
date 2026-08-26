using FluentValidation;
using StoreApi.DTOs;

namespace StoreApi.Validators
{
    public class CreateOrderDtoValidator:AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("سبد سفارش نمی تواند خالی باشد")
                .Must(items => items.Select(items => items.ProductId).Distinct().Count() == items.Count)
                .WithMessage("هر محصول فقط یکبار می تواند در سفارش ثبت شود");
            RuleForEach(x => x.Items)
                .SetValidator(new CreateOrderItemDtoValidator());
        }
    }
}
