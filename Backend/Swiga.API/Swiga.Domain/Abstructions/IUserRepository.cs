using Swiga.Domain.Models;

namespace Swiga.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> CreateUserAsync(UserModel user);
        Task DeleteUserAsync(Guid id);
        Task<List<AdminModel>> GetAllAdminsAsync();
        Task<List<ClientModel>> GetAllClientsAsync();
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task<UserModel?> GetUserByIdAsync(Guid id);
        Task<Guid> UpdateUserAsync(UserModel user);
    }
}