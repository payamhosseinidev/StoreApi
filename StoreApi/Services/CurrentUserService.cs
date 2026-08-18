using System.Security.Claims;

namespace StoreApi.Services
{
    public class CurrentUserService :ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int? UserId
        {
            get
            {
                var userId = _httpContextAccessor
                   .HttpContext?
                   .User
                   .FindFirstValue(ClaimTypes.NameIdentifier);

                if (int.TryParse(userId, out var id))
                    return id;

                return null;
            }
        }

        public string? Username =>
           _httpContextAccessor
               .HttpContext?
               .User
               .FindFirstValue(ClaimTypes.Name);

        public string? Role =>
            _httpContextAccessor
                .HttpContext?
                .User
                .FindFirstValue(ClaimTypes.Role);
    }
}
