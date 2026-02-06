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

        Task<(bool success, string error)> ChangePasswordAsync(Guid userId,string currentPassword,string newPassword);

        Task<(bool success, string error)> DeleteUserAsync(Guid userId,string password);

        Task<bool> CheckPasswordAsync(Guid userId, string password);
    }
}