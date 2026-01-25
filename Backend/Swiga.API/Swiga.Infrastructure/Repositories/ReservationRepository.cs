using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;

namespace Swiga.Infrastructure.Repositories
{
    public class ReservationRepository
    {
        private readonly SwigaDbContext _context;

        public ReservationRepository(SwigaDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateReservationAsync(ReservationModel reservation)
        {

            var reservationEntity = new ReservationEntity
            {
                Id = reservation.Id,
                ClientId = reservation.ClientId,
                RentalPointId = reservation.RentalPointId,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Status = (int)reservation.Status,
                TotalPrice = reservation.TotalPrice,
                CreatedAt = reservation.CreatedAt,
                ConfirmedAt = reservation.ConfirmedAt,
                CancelledAt = reservation.CancelledAt,
                CancellationReason = reservation.CancellationReason
            };

            if (reservation.Items != null && reservation.Items.Any())
            {
                reservationEntity.Items = reservation.Items.Select(item => new ReservationItemEntity
                {
                    Id = item.Id,
                    ReservationId = reservation.Id,
                    InventoryId = item.InventoryId,
                    Size = item.Size,
                    Gender = (int)item.Gender,
                    Quantity = item.Quantity,
                    PricePerHour = item.PricePerHour,
                    TotalPrice = item.TotalPrice
                }).ToList();
            }

            await _context.Reservations.AddAsync(reservationEntity);

            await _context.SaveChangesAsync();

            return reservationEntity.Id;
        }
    }
}
