using FluentValidation;
using StoreApi.DTOs;

namespace StoreApi.Validators
{
    public class RegisterDtoValidator: AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("نام کاربری الزامی است")
                .MinimumLength(3)
                .WithMessage("نام کاربری باید حداقل 3 کاراکتر باشد")
                .MaximumLength(50)
                .WithMessage("نام کاربری نمی‌تواند بیشتر از 50 کاراکتر باشد");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("رمز عبور الزامی است")
                .MinimumLength(6)
                .WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد")
                .MaximumLength(100)
                .WithMessage("رمز عبور نمی‌تواند بیشتر از 100 کاراکتر باشد");
        }
    }
}
