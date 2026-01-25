using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;
using static Swiga.Domain.Models.ReservationModel;


namespace Swiga.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
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

        public async Task<ReservationModel?> GetReservationByIdAsync(Guid reservationId)
        {
            var entity = await _context.Reservations
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (entity == null) return null;

            var (reservation, error) = ReservationModel.Create(
                entity.ClientId,
                entity.RentalPointId,
                entity.StartDate,
                entity.EndDate);


            if (!string.IsNullOrEmpty(error) || reservation == null)
            {
                return null;
            }

            reservation.Id = entity.Id;
            reservation.Status = (ReservationStatus)entity.Status;
            reservation.TotalPrice = entity.TotalPrice;
            reservation.CreatedAt = entity.CreatedAt;
            reservation.ConfirmedAt = entity.ConfirmedAt;
            reservation.CancelledAt = entity.CancelledAt;
            reservation.CancellationReason = entity.CancellationReason;

            if (entity.Items != null && entity.Items.Any())
            {
                foreach (var itemEntity in entity.Items)
                {
                    var inventory = await _context.Inventories
                        .FirstOrDefaultAsync(i => i.Id == itemEntity.InventoryId);

                    if (inventory != null)
                    {
                        var (inventoryModel, inventoryError) = InventoryModel.Create(
                            inventory.Id,
                            inventory.Name,
                            inventory.Size,
                            (Gender)inventory.Gender, // <- Кастинг Gender
                            inventory.PricePerHour,
                            inventory.Amount,
                            inventory.RentalPointId);

                        if (inventoryModel != null)
                        {
                            var (success, itemError) = reservation.AddItem(
                                inventoryModel,
                                itemEntity.Quantity,
                                itemEntity.PricePerHour);

                            if (!success)
                            {
                                // Логируем ошибку, но продолжаем
                                Console.WriteLine($"Error adding item to reservation: {itemError}");
                            }
                        }
                    }
                }
            }
            return reservation;
        }


        public async Task UpdateReservationAsync(ReservationModel reservation)
        {
            var entity = await _context.Reservations
                .Include(r => r.Items)
                .FirstOrDefaultAsync(r => r.Id == reservation.Id);

            if (entity == null)
                throw new ArgumentException($"Reservation with id {reservation.Id} not found");

            entity.Status = (int)reservation.Status;
            entity.TotalPrice = reservation.TotalPrice;
            entity.ConfirmedAt = reservation.ConfirmedAt;
            entity.CancelledAt = reservation.CancelledAt;
            entity.CancellationReason = reservation.CancellationReason;

            if (reservation.Items != null && reservation.Items.Any())
            {
                _context.ReservationItems.RemoveRange(entity.Items);

                var newItems = reservation.Items.Select(item => new ReservationItemEntity
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

                await _context.ReservationItems.AddRangeAsync(newItems);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReservationModel>> GetReservationByClinetIdAsync(Guid clientId)
        {
            var entites = await _context.Reservations
                .Where(r => r.ClientId == clientId)
                .Include(r => r.Items)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var result = new List<ReservationModel>();

            foreach (var entity in entites)
            {
                var (reservation, error) = ReservationModel.Create(
                    entity.ClientId,
                    entity.RentalPointId,
                    entity.StartDate,
                    entity.EndDate);


                if (!string.IsNullOrEmpty(error) || reservation == null)
                {
                    continue;
                }

                reservation.Id = entity.Id;
                reservation.Status = (ReservationModel.ReservationStatus)entity.Status;
                reservation.TotalPrice = entity.TotalPrice;
                reservation.CreatedAt = entity.CreatedAt;
                reservation.ConfirmedAt = entity.ConfirmedAt;
                reservation.CancelledAt = entity.CancelledAt;
                reservation.CancellationReason = entity.CancellationReason;

                result.Add(reservation);
            }

            return result;

        }

        public async Task<int> GetBookedQuantityAsync(Guid inventoryId, Guid rentalPointId, DateTime startDate, DateTime endDate)
        {
            var bookedQuantity = await _context.ReservationItems
                .Join(_context.Reservations,
                    item => item.ReservationId,
                    reservation => reservation.Id,
                    (item, reservation) => new { Item = item, Reservation = reservation })
                .Where(x => x.Item.InventoryId == inventoryId &&
                           x.Reservation.RentalPointId == rentalPointId &&
                           x.Reservation.Status != (int)ReservationStatus.Cancelled && // Не учитываем отмененные
                                                                                       // Проверяем пересечение периодов
                           !(x.Reservation.EndDate <= startDate || x.Reservation.StartDate >= endDate))
                .SumAsync(x => (int?)x.Item.Quantity) ?? 0;

            return bookedQuantity;
        }
    }

}
