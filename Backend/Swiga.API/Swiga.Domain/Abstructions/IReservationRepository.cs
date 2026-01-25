using Swiga.Domain.Models;

namespace Swiga.Infrastructure.Repositories
{
    public interface IReservationRepository
    {
        Task<Guid> CreateReservationAsync(ReservationModel reservation);
        Task<int> GetBookedQuantityAsync(Guid inventoryId, Guid rentalPointId, DateTime startDate, DateTime endDate);
        Task<List<ReservationModel>> GetReservationByClinetIdAsync(Guid clientId);
        Task<ReservationModel?> GetReservationByIdAsync(Guid reservationId);
        Task UpdateReservationAsync(ReservationModel reservation);
    }
}