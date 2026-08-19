using FluentValidation;
using StoreApi.Common;
using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        public AuthService(IUserRepository repository,IJwtService jwtService, IValidator<RegisterDto> registerValidator,
    IValidator<LoginDto> loginValidator)
        {
            _repository = repository;
            _jwtService = jwtService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        public async Task<Result<string>> Register(RegisterDto dto)
        {

            var validationResult = await _registerValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _repository.GetByUsernameAsync(dto.Username);

            if (existingUser != null)
            {
                return Result<string>.Failure(
                        "این نام کاربری قبلا ثبت شده است"
                );
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                Role = "User"
            };

            await _repository.AddAsync(user);
            await _repository.SaveChangesAsync();

            return Result<string>.SuccessResult(
                "ثبت نام با موفقیت انجام شد",
                "کاربر با موفقیت ثبت شد"
                );

        }

        public async Task<Result<string>> Login(LoginDto dto)
        {

            var validationResult = await _loginValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user =
                await _repository.GetByUsernameAsync(dto.Username);

            if (user == null)
            {
                return Result<string>.Failure(
                    "نام کاربری یا رمز عبور اشتباه است");
            }

            var passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    user.PasswordHash);

            if (!passwordValid)
            {
                return Result<string>.Failure(
                    "نام کاربری یا رمز عبور اشتباه است");
            }

            var token = _jwtService.GenerateToken(
                user.Id,
                user.Username,
                user.Role);

            return Result<string>.SuccessResult(
                token,
                "ورود با موفقیت انجام شد"
            );
        }
    }
}
