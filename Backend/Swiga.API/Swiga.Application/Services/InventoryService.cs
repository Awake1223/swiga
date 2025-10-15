using Swiga.Domain.Abstructions;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Repositories;

namespace Swiga.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<InventoryModel>> GetAllInventory()
        {
            return await _inventoryRepository.GetInventoryAsync();
        }

        public async Task<Guid> CreateInventory(InventoryModel inventory)
        {
            return await _inventoryRepository.CreateInventoryAsync(inventory);
        }

        public async Task<Guid> UpdateInventory(Guid id, string name, int size, Gender gender, decimal pricePerHour, int amount)
        {
            return await _inventoryRepository.UpdateInventoryAsync(id, name, size, gender, pricePerHour, amount);
        }

        public async Task<Guid> DeleteInventory(Guid id)
        {
            return await _inventoryRepository.DeleteInventoryAsync(id);
        }
    }
}
