using Swiga.Domain.Models;

namespace Swiga.API.Contracts.Inventory
{
    public record InventoryResponse(
        Guid Id,
        string Name,
        int Size,
        Gender Gender, 
        decimal PricePerHour,
        int Amount,
        Guid RentalPointId);

}
