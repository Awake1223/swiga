using Swiga.Domain.Models;

namespace Swiga.Domain.Abstructions
{
    public interface IInventoryRepository
    {
        Task<Guid> CreateInventoryAsync(InventoryModel inventory);
        Task<Guid> DeleteInventoryAsync(Guid id);
        Task<List<InventoryModel>> GetInventoryAsync();
        Task<Guid> UpdateInventoryAsync(Guid id, string name, int size, Gender gender, decimal pricePerHour, int amount);
    }
}