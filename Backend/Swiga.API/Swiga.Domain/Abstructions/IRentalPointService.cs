using Swiga.Domain.Models;

namespace Swiga.Application.Services
{
    public interface IRentalPointService
    {
        Task<Guid> CreateRentalPoint(RentalPointModel rentalPoint);
        Task<Guid> DeleteRentalPoint(Guid id);
        Task<List<RentalPointModel>> GetRentalPoint();
        Task<Guid> UpdateRentalPoint(Guid id, string name, string address, string city, string phoneNumber, string email);
    }
}