using Swiga.Domain.Models;

namespace Swiga.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ClientExistAsync(Guid clientId);
        Task<Guid> CreateUserAsync(UserModel user);
        Task DeleteUserAsync(Guid id);
        Task<List<AdminModel>> GetAllAdminsAsync();
        Task<List<ClientModel>> GetAllClientsAsync();
        Task<List<UserModel>> GetAllUsersAsync();
        Task<ClientModel?> GetClientByIdAsync(Guid clientId);
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task<UserModel?> GetUserByIdAsync(Guid id);
        Task<Guid> UpdateUserAsync(UserModel user);
    }
}