using Swiga.Domain.Models;

namespace Swiga.API.Contracts
{
    public record InventoryRequest(
        string Name,
        int Size,
        Gender Gender,
        decimal PricePerHour,
        int Amount);

}
