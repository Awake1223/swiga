

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Swiga.Application.Services.UserServices
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;

            if (Guid.TryParse(userIdClaim.Value, out Guid userId))
                return userId;

            return null;
        }

        public string? GetCurrentUserRole()
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.Role);

            return roleClaim?.Value;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }

        public bool IsInRole(string role)
        {
            var userRole = GetCurrentUserRole();

            return !string.IsNullOrEmpty(userRole) && userRole.Equals(role, StringComparison.OrdinalIgnoreCase);
        }
    }

}
