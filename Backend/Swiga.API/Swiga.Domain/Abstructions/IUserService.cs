using Swiga.Domain.Models;

namespace Swiga.Application.Services
{
    public interface IUserService
    {
        Task<Guid> CreateUser(UserModel user);
        Task DeleteUser(Guid id);
        Task<List<AdminModel>> GetAllAdmin();
        Task<List<ClientModel>> GetAllClients();
        Task<UserModel?> GetUserByEmailAsync(string email);
        Task<UserModel?> GetUserByIdAsync(Guid id);
        Task<List<UserModel>> GetUsers();
        Task<Guid> UpdateUser(UserModel user);
    }
}