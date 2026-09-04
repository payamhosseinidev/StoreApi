using FluentValidation;
using StoreApi.Common;
using StoreApi.DTOs;
using StoreApi.Models;
using StoreApi.Repositories;

namespace StoreApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly RefreshTokenService _refreshTokenService;
        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IValidator<RegisterDto> registerValidator,
            IValidator<LoginDto> loginValidator,
            IRefreshTokenRepository refreshTokenRepository,
            RefreshTokenService refreshTokenService
            )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result<string>> Register(RegisterDto dto)
        {

            var validationResult = await _registerValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);

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

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return Result<string>.SuccessResult(
                "ثبت نام با موفقیت انجام شد",
                "کاربر با موفقیت ثبت شد"
                );

        }

        public async Task<Result<AuthResponseDto>> Login(LoginDto dto)
        {

            var validationResult = await _loginValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var user = await _userRepository.GetByUsernameAsync(dto.Username);

            if (user == null)
            {
                return Result<AuthResponseDto>.Failure(
                    "نام کاربری یا رمز عبور اشتباه است");
            }

            var passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    user.PasswordHash);

            if (!passwordValid)
            {
                return Result<AuthResponseDto>.Failure(
                    "نام کاربری یا رمز عبور اشتباه است");
            }

            var accessToken = _jwtService.GenerateToken(
                user.Id,
                user.Username,
                user.Role);

            var refreshToken = _refreshTokenService.GenerateToken();
            var refreshTokenHash = _refreshTokenService.HashToken(refreshToken);

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = refreshTokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Result<AuthResponseDto>.SuccessResult(
                response,
                "ورود با موفقیت انجام شد"
            );
        }

        public async Task<Result<AuthResponseDto>> RefreshAsync(string refreshToken)
        {
            var tokenHash = _refreshTokenService.HashToken(refreshToken);

            var storedToken = await _refreshTokenRepository.GetByHashAsync(tokenHash);

            if (storedToken is null)
                return Result<AuthResponseDto>.Failure("Refresh Token نامعتبر است");

            if(storedToken.RevokedAt is not null)
                return Result<AuthResponseDto>.Failure("Refresh Token قبلا استفاده شده است");

            if (storedToken.ExpiresAt <= DateTime.UtcNow)
                return Result<AuthResponseDto>.Failure("Refresh Token منقضی شده است");

            var user = storedToken.User;

            var newAccessToken = _jwtService.GenerateToken(
                user.Id,
                user.Username,
                user.Role
            );

            var newRefreshToken = _refreshTokenService.GenerateToken();

            var newRefreshTokenHash = _refreshTokenService.HashToken(newRefreshToken);

            //Revoke former RefreshToken
            storedToken.RevokedAt = DateTime.UtcNow;

            var newTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = newRefreshTokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };

            await _refreshTokenRepository.AddAsync(newTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return Result<AuthResponseDto>.SuccessResult(
                new AuthResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                },
                "Token با موفقیت Refresh شد"
                );
        }

        public async Task<Result<bool>> LogoutAsync(string refreshToken)
        {
            var tokenHash = _refreshTokenService.HashToken(refreshToken);
            var storedToken = await _refreshTokenRepository.GetByHashAsync(tokenHash);

            if (storedToken is null)
                return Result<bool>.Failure("Refresh Token نامعتبر است");

            if(storedToken.RevokedAt is not null)
                return Result<bool>.Failure("Refresh Token قبلاً باطل شده است");

            storedToken.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository.SaveChangesAsync();

            return Result<bool>.SuccessResult(
                true,
                 "با موفقیت Logout شدید"
            );
        }
    }
}
