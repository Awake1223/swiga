using Swiga.Domain.Models;

namespace Swiga.Application.Services.Security
{
    public interface IJwtTokenService
    {
        (string token, DateTime expiresAtUtc) Create(UserModel user);
    }
}