using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Swiga.Application.DTOs.Inventory;
using Swiga.Application.DTOs.Reservations;
using Swiga.Domain.Abstructions;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Repositories;

namespace Swiga.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IRentalPointRepository _rentalPointRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IUserRepository userRepository,
            IInventoryRepository inventoryRepository,
            IRentalPointRepository rentalPointRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _inventoryRepository = inventoryRepository;
            _rentalPointRepository = rentalPointRepository;
        }

        public async Task<(Guid? ReservationId, string error)> CreateReservationAsync(CreateReservationDto dto) 
        {
            try
            {
                if (dto == null)
                    return (null, "Запрос не может быть пустым");

                if (dto.Items == null || !dto.Items.Any())
                    return (null, "Должен быть хотя бы один предмет");

                var clientExist = await _userRepository.ClientExistAsync(dto.ClientId);
                if (!clientExist)
                    return (null, "Клиент не найден");

                var rentalPoints = await _rentalPointRepository.GetRentalPointAsync();
                var rentalPoint = rentalPoints.FirstOrDefault(rp => rp.Id == dto.RentalPointId);

                if (rentalPoint == null)
                    return (null, "Пункт проката не найден");

                var startDateUtc = dto.StartDate.ToUniversalTime();
                var endDateUtc = dto.EndDate.ToUniversalTime();
                var nowUtc = DateTime.UtcNow;

                if (startDateUtc < nowUtc.AddHours(-1))
                    return (null, "Дата начала не может быть в прошлом");

                if (endDateUtc <= startDateUtc)
                    return (null, "Дата окончания должна быть позже даты начала");

                var durationHours = (endDateUtc - startDateUtc).TotalHours;
                if (durationHours > 72)
                    return (null, "Максимальный срок бронирования 72 часа");

                if (durationHours < 1)
                    return (null, "Минимальный срок бронирования - 1 час");

                foreach (var itemDto in dto.Items)
                {
                    var inventory = await _inventoryRepository.GetInventoryByIdAsync(itemDto.InventoryId);

                    if (inventory == null)
                        return (null, $"Инвентарь с ID {itemDto.InventoryId} не найден");

                    if (inventory.RentalPointId != dto.RentalPointId)
                        return (null, $"Инвентарь '{inventory.Name}' не принадлежит выбранному пункту проката");

                    var bookedQuantity = await _reservationRepository.GetBookedQuantityAsync(
                        itemDto.InventoryId,
                        dto.RentalPointId,
                        startDateUtc,
                        endDateUtc);

                    var availableQuantity = inventory.Amount - bookedQuantity;

                    if (availableQuantity < itemDto.Quantity)
                    {
                        return (null,
                          $"Недостаточно инвентаря '{inventory.Name}' размера {inventory.Size}. " +
                          $"Доступно: {availableQuantity}, Запрошено: {itemDto.Quantity}");
                    }

                    if (itemDto.PricePerHour <= 0)
                        return (null, $"Цена для '{inventory.Name}' должна быть больше 0");

                    var priceDifference = Math.Abs(itemDto.PricePerHour - inventory.PricePerHour);

                    if (priceDifference > inventory.PricePerHour * 0.5m)
                        return (null, $"Цена для '{inventory.Name}' указана некорректно");
                }

                var (reservation, error) = ReservationModel.Create(
                    dto.ClientId,
                    dto.RentalPointId,
                    dto.StartDate,
                    dto.EndDate);

                if (!string.IsNullOrEmpty(error) || reservation == null)
                    return (null, error ?? "Ошибка создания бронирования");

                foreach (var itemDto in dto.Items)
                {
                    var inventory = await _inventoryRepository.GetInventoryByIdAsync(itemDto.InventoryId);

                    if (inventory == null)
                        return (null, $"Инвентарь {itemDto.InventoryId} не найден при добавлении");

                    var (success, addError) = reservation.AddItem(
                        inventory,
                        itemDto.Quantity,
                        itemDto.PricePerHour);

                    if (!success)
                        return (null, addError);
                }

                var reservationId = await _reservationRepository.CreateReservationAsync(reservation);

                return (reservationId, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateReservationAsync: {ex.Message}");
                return (null, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        public async Task<ReservationModel?> GetReservationByIdAsync(Guid reservationId)
        {
            try
            {
                return await _reservationRepository.GetReservationByIdAsync(reservationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetReservationByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<List<ReservationModel>> GetClientReservationsAsync(Guid clientId)
        {
            try
            {
                return await _reservationRepository.GetReservationByClinetIdAsync(clientId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClientReservationsAsync: {ex.Message}");
                return new List<ReservationModel>();

            }
        }

        public async Task<(bool Success, string Error)> ConfirmReservationAsync(Guid reservationId)
        {
            try
            {
                var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);

                if (reservation == null)
                    return (false, "Бронь не найдена");

                var (success, error) = reservation.Confirm();

                if (!success)
                    return (false, error);


                await _reservationRepository.UpdateReservationAsync(reservation);

                return (true, string.Empty);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error in ConfirmReservationAsync: {ex.Message}");
                return (false, $"Внутренняя ошибка: {ex.Message}");
            }
        }


        public async Task<(bool Success, string Error)> CancelReservationAsync(
            Guid reservationId, string reason = "По желанию клиента")
        {
            try
            {
                var reservation = await _reservationRepository.GetReservationByIdAsync(reservationId);
                if (reservation == null)
                    return (false, "Бронь не найдена");

                var (success, error) = reservation.Cancel(reason);
                if (!success)
                    return (false, error);

                await _reservationRepository.UpdateReservationAsync(reservation);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CancelReservationAsync: {ex.Message}");
                return (false, $"Внутренняя ошибка: {ex.Message}");
            }
        }


        public async Task<List<AvailableInventoryDto>> GetAvailableInventoryAsync(Guid rentalPointId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var startDateUtc = startDate.ToUniversalTime();
                var endDateUtc = endDate.ToUniversalTime();

                if (endDateUtc <= startDateUtc)
                    return new List<AvailableInventoryDto>();

                if ((endDateUtc - startDateUtc).TotalHours > 72)
                    return new List<AvailableInventoryDto>();

                var result = new List<AvailableInventoryDto>();

                var allInventory = await _inventoryRepository.GetInventoryByRentalPointAsync(rentalPointId);

                foreach (var inventory in allInventory)
                {
                    var bookedQuantity = await _reservationRepository.GetBookedQuantityAsync(
                        inventory.Id,
                        rentalPointId,
                        startDateUtc,
                        endDateUtc);

                    var availableQuantity = inventory.Amount - bookedQuantity;

                    if (availableQuantity > 0)
                    {
                        result.Add(new AvailableInventoryDto
                        {
                            InventoryId = inventory.Id,
                            Name = inventory.Name,
                            Size = inventory.Size,
                            Gender = inventory.Gender,
                            PricePerHour = inventory.PricePerHour,
                            AvailableQuantity = availableQuantity,
                            TotalQuantity = inventory.Amount,
                        });
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableInventoryAsync: {ex.Message}");
                return new List<AvailableInventoryDto>();
            }
        }

    }
}
