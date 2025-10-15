using Swiga.Domain.Models;

namespace Swiga.Domain.Abstructions
{
    public interface IInventoryService
    {
        Task<Guid> CreateInventory(InventoryModel inventory);
        Task<Guid> DeleteInventory(Guid id);
        Task<List<InventoryModel>> GetAllInventory();
        Task<Guid> UpdateInventory(Guid id, string name, int size, Gender gender, decimal pricePerHour, int amount);
    }
}