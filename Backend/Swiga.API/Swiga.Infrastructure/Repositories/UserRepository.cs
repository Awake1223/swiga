
using Microsoft.EntityFrameworkCore;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;

namespace Swiga.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly SwigaDbContext _context;

        public UserRepository(SwigaDbContext context)
        {
            _context = context;
        }

        // GET - Получить всех пользователей
        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var entities = await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToListAsync();

            return entities.Select(ToModel).ToList();
        }

        // GET - Получить всех клиентов
        public async Task<List<ClientModel>> GetAllClientsAsync()
        {
            var entities = await _context.Users
                .AsNoTracking()
                .Where(u => u.Role == (int)Role.Client)
                .OrderBy(u => u.Id)
                .ToListAsync();

            return entities.Select(ToClientModel).ToList();
        }

        // GET - Получить всех админов
        public async Task<List<AdminModel>> GetAllAdminsAsync()
        {
            var entities = await _context.Users
                .AsNoTracking()
                .Where(u => u.Role == (int)Role.Admin)
                .OrderBy(u => u.Id)
                .ToListAsync();

            return entities.Select(ToAdminModel).ToList();
        }

        // GET - Найти пользователя по ID
        public async Task<UserModel?> GetUserByIdAsync(Guid id)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            return entity != null ? ToModel(entity) : null;
        }

        // GET - Найти пользователя по email
        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            var entity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            return entity != null ? ToModel(entity) : null;
        }

        // CREATE
        public async Task<Guid> CreateAsync(UserModel user)
        {
            var entity = ToEntity(user);
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        // UPDATE  
        public async Task<Guid> UpdateAsync(UserModel user)
        {
            var entity = ToEntity(user);
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        // DELETE
        public async Task DeleteAsync(Guid id)
        {
            await _context.Users
                .Where(u => u.Id == id)
                .ExecuteDeleteAsync();
        }

        // Маппинг Entity → Model
        private UserModel ToModel(UserEntity entity)
        {
            return entity.Role switch
            {
                (int)Role.Client => ToClientModel(entity),
                (int)Role.Admin => ToAdminModel(entity),
                _ => throw new InvalidOperationException($"Unknown role: {entity.Role}")
            };
        }

        private ClientModel ToClientModel(UserEntity entity)
        {
            var client = ClientModel.Create(
                entity.FirstName?? string.Empty,
                entity.LastName ?? string.Empty,
                entity.Email,
                entity.PhoneNumber,
                entity.Password
            );

            // Устанавливаем остальные свойства через рефлексию или методы
            SetBaseProperties(client, entity);
            // client.DateOfBirth = entity.DateOfBirth; // если есть
            // client.PassportData = entity.PassportData; // если есть

            return client;
        }

        private AdminModel ToAdminModel(UserEntity entity)
        {
            var admin = AdminModel.Create(
                entity.FullName ?? string.Empty,
                entity.RentalPointId ?? Guid.Empty,
                entity.Email,
                entity.PhoneNumber,
                entity.Password
            );

            SetBaseProperties(admin, entity);
            return admin;
        }

        private void SetBaseProperties(UserModel model, UserEntity entity)
        {
            // Устанавливаем базовые свойства
            // Временное решение - в идеале сделать свойства с public set
            model.Id = entity.Id;
            model.CreatedAt = entity.CreatedAt;
            // Role уже установлен в Create методе
        }

        // Маппинг Model → Entity
        private UserEntity ToEntity(UserModel model)
        {
            var entity = new UserEntity
            {
                Id = model.Id,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                CreatedAt = model.CreatedAt,
                Role = (int)model.Role
            };

            // Заполняем специфичные поля
            if (model is ClientModel client)
            {
                entity.FirstName = client.FirstName;
                entity.LastName = client.LastName;
                // entity.DateOfBirth = client.DateOfBirth;
                // entity.PassportData = client.PassportData;
            }
            else if (model is AdminModel admin)
            {
                entity.FullName = admin.FullName;
                entity.RentalPointId = admin.RentalPointId;
            }

            return entity;
        }
    }
}
