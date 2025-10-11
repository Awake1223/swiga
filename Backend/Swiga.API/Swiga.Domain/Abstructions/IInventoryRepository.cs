using Swiga.Domain.Models;

namespace Swiga.Infrastructure.Repositories
{
    public interface IInventoryRepository
    {
        Task<Guid> Create(InventoryModel inventory);
        Task<Guid> Delete(Guid id);
        Task<List<InventoryModel>> Get();
        Task<Guid> Update(Guid id, string name, int size, Gender gender, decimal pricePerHour, int amount);
    }
}