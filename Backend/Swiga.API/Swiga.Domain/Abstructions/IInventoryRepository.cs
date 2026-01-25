using Swiga.Domain.Models;

namespace Swiga.Infrastructure.Repositories
{
    public interface IInventoryRepository
    {
        Task<Guid> CreateInventoryAsync(InventoryModel inventory);
        Task<Guid> DeleteInventoryAsync(Guid id);
        Task<List<InventoryModel>> GetInventoryAsync();
        Task<InventoryModel?> GetInventoryByIdAsync(Guid inventoryId);
        Task<List<InventoryModel>> GetInventoryByRentalPointAsync(Guid rentalPointId);
        Task<Guid> UpdateInventoryAsync(Guid id, string name, int size, Gender gender, decimal pricePerHour, int amount);
    }
}