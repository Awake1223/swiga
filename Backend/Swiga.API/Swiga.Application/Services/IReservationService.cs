using Swiga.Application.DTOs.Inventory;
using Swiga.Application.DTOs.Reservations;
using Swiga.Domain.Models;

namespace Swiga.Application.Services
{
    public interface IReservationService
    {
        Task<(bool Success, string Error)> CancelReservationAsync(Guid reservationId, string reason = "По желанию клиента");
        Task<(bool Success, string Error)> ConfirmReservationAsync(Guid reservationId);
        Task<(Guid? ReservationId, string error)> CreateReservationAsync(CreateReservationDto items);
        Task<List<AvailableInventoryDto>> GetAvailableInventoryAsync(Guid rentalPointId, DateTime startDate, DateTime endDate);
        Task<List<ReservationModel>> GetClientReservationsAsync(Guid clientId);
        Task<ReservationModel?> GetReservationByIdAsync(Guid reservationId);
    }
}