using FluentValidation;
using StoreApi.DTOs;

namespace StoreApi.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator() 
        { 
            RuleFor(x=>x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithName("نام محصول").WithMessage("{PropertyName} الزامی است.")
                .MinimumLength(3).WithMessage("{PropertyName} باید حداقل {MinLength} کاراکتر باشد.")
                .MaximumLength(100).WithMessage("{PropertyName} باید حداکثر {MaxLength} کاراکتر باشد.");

            RuleFor(x => x.Price)
                .InclusiveBetween(1, 1000000000).WithName("قیمت")
                .WithMessage("{PropertyName} باید بین {From} تا {To} باشد");

            RuleFor(x=>x.Stock)
                .GreaterThanOrEqualTo(0).WithName("موجودی")
                 .WithMessage("{PropertyName} نمیتواند منفی باشد.");
        }
    }
}
