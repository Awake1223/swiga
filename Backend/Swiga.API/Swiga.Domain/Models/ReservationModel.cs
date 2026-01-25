using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Swiga.Domain.Models
{

    public class ReservationModel
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid RentalPointId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get;     set; }
        public string? CancellationReason { get; set; }

        public ClientModel ClientModel { get;  set; }
        public RentalPointModel RentalPoint { get;  set; }

        public List<ReservationItemModel> Items { get;  set; } = new();

        private ReservationModel(Guid id, Guid clientId, Guid rentalPointId, DateTime startDate, DateTime endDate)
        {
            Id = id;
            ClientId = clientId;
            RentalPointId = rentalPointId;
            StartDate = startDate;
            EndDate = endDate;
            Status = ReservationStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            TotalPrice = 0;

        }

        public static (ReservationModel? ReservationModel, string error) Create(
            Guid clientId,
            Guid rentalPointId,
            DateTime startDate,
            DateTime endDate)
        {
            var error = string.Empty;

            if (clientId == Guid.Empty)
                error = "Не указан клиент";
            else if (rentalPointId == Guid.Empty)
                error = "Не указан пункт проката";
            else if (startDate < DateTime.UtcNow.AddMinutes(-5)) // 5 минут "окна"
                error = "Дата начала не может быть в прошлом";
            else if (endDate <= startDate)
                error = "Дата окончания должна быть позже даты начала";
            else if ((endDate - startDate).TotalHours > 72)
                error = "Максимальный срок бронирования - 72 часа";
            else if ((endDate - startDate).TotalHours < 1)
                error = "Минимальный срок бронирования - 1 час";

            if (!string.IsNullOrEmpty(error))
            {
                return (null, error);
            }

            var reservation = new ReservationModel(
                Guid.NewGuid(),
                clientId,
                rentalPointId,
                startDate.ToUniversalTime(),
                endDate.ToUniversalTime());

            return (reservation, error);

        }

        public (bool succes, string error) AddItem(
            InventoryModel inventory,
            int quantity,
            decimal pricePerHour)
        {
            if (inventory.RentalPointId != RentalPointId)
                return (false, $"Инвентарь не принадлежит пункту проката {RentalPointId}");

            if (quantity <= 0)
                return (false, "Количество должно быть больше 0");


            var (item, error) = ReservationItemModel.Create(
                inventory.Id,
                inventory.Size,
                inventory.Gender,
                quantity,
                pricePerHour,
                StartDate,
                EndDate);


            if (!string.IsNullOrEmpty(error))
                return (false, error);

            // Добавляем к брони
            Items.Add(item);

            // Пересчитываем общую стоимость
            RecalculateTotalPrice();

            return (true, string.Empty);
        }

        private void RecalculateTotalPrice()
        {
            TotalPrice = Items.Sum(item => item.TotalPrice);
        }

        public (bool success, string error) Confirm()
        {
            if (Status != ReservationStatus.Pending)
                return (false, $"Невозможно подтвердить бронь в статусе {Status}");

            if (Items.Count == 0)
                return (false, "Нельзя подтвердить пустую бронь");

            Status = ReservationStatus.Confirmed;
            ConfirmedAt = DateTime.UtcNow;

            return (true, string.Empty);
        }

        public (bool success, string error) Cancel(string reason = "По желанию клиента")
        {
            if (Status == ReservationStatus.Cancelled)
                return (false, "Бронь уже отменена");

            if (Status == ReservationStatus.Completed)
                return (false, "Нельзя отменить завершенную бронь");

            Status = ReservationStatus.Cancelled;
            CancelledAt = DateTime.UtcNow;
            CancellationReason = reason;

            return (true, string.Empty);   
                
        }

        public (bool success, string error) Complete()
        {
            if(Status != ReservationStatus.Confirmed)
                return (false, $"Невозможно завершить бронь в статусе {Status}");

            Status = ReservationStatus.Completed;
            return (true, string.Empty);  
        }

        public enum ReservationStatus
        {
            Pending = 1,     // Ожидает подтверждения
            Confirmed = 2,   // Подтверждена
            Cancelled = 3,   // Отменена
            Completed = 4    // Завершена (инвентарь выдан)
        }
    }
}
