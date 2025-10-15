using Swiga.Domain.Models;

namespace Swiga.Domain.Abstructions
{
    public interface IRentalPointRepository
    {
        Task<Guid> CreateRentalPointAsync(RentalPointModel rentalPoint);
        Task<Guid> DeleteRentalPointAsync(Guid id);
        Task<List<RentalPointModel>> GetRentalPointAsync();
        Task<Guid> UpdateRentalPointAsync(Guid id, string name, string address, string city, string phoneNumber, string email);
    }
}