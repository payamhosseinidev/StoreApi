using FluentValidation;
using StoreApi.DTOs;

namespace StoreApi.Validators
{
    public class CreateCategoryValidator:AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("نام دسته‌بندی الزامی است.")
                .MinimumLength(2)
                .WithMessage("نام دسته‌بندی باید حداقل 2 کاراکتر باشد.")
                .MaximumLength(100)
                .WithMessage("نام دسته‌بندی نمی‌تواند بیشتر از 100 کاراکتر باشد.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("توضیحات نمی‌تواند بیشتر از 500 کاراکتر باشد.");
        }
    }
}
