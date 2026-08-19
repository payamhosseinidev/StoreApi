using FluentValidation;
using StoreApi.DTOs;

namespace StoreApi.Validators
{
    public class LoginDtoValidator:AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x=>x.Username).NotEmpty().WithMessage("نام کاربری الزامی است"); ;
            RuleFor(x=>x.Password).NotEmpty().WithMessage("رمز عبور الزامی است");
        }
    }
}
