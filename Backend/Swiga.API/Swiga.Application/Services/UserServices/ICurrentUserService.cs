
namespace Swiga.Application.Services.UserServices
{
    public interface ICurrentUserService
    {
        Guid? GetCurrentUserId();
        string? GetCurrentUserRole();
        bool IsAuthenticated();
        bool IsInRole(string role);
    }
}