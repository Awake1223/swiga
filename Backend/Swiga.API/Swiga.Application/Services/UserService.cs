

using Swiga.Domain.Models;
using Swiga.Infrastructure.Repositories;

namespace Swiga.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<Guid> CreateUser(UserModel user)
        {
            return await _userRepository.CreateUserAsync(user);
        }

        public async Task<Guid> UpdateUser(UserModel user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUser(Guid id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        //Task<Guid> CreateUserAsync(UserModel user); +
        //Task DeleteUserAsync(Guid id);  +
        //Task<List<AdminModel>> GetAllAdminsAsync();+
        //Task<List<ClientModel>> GetAllClientsAsync();+
        //Task<List<UserModel>> GetAllUsersAsync();+
        //Task<UserModel?> GetUserByEmailAsync(string email); +
        //Task<UserModel?> GetUserByIdAsync(Guid id);
        //Task<Guid> UpdateUserAsync(UserModel user);+

        public async Task<List<ClientModel>> GetAllClients()
        {
            return await _userRepository.GetAllClientsAsync();
        }

        public async Task<List<AdminModel>> GetAllAdmin()
        {
            return await _userRepository.GetAllAdminsAsync();
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<UserModel?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}
