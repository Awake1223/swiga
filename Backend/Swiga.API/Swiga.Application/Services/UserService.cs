
using Swiga.Domain.Abstructions;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Repositories;


namespace Swiga.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;


        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<Guid> CreateUser(UserModel user)
        {

            var existing = await _userRepository.GetUserByEmailAsync(user.Email);

            if (existing != null)
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            user.Password = _passwordHasher.HashPassword(user.Password);

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
