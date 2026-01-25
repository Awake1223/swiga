using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models
{
    public class ReservationItemModel
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public Guid InventoryId { get; set; }
        public int Size { get; private set; }
        public Gender Gender { get; private set; }
        public int Quantity { get; private set; }
        public decimal PricePerHour { get; private set; }
        public decimal TotalPrice { get; private set; }


        public InventoryModel Inventory { get; set; }


        private ReservationItemModel(Guid id, Guid inventoryId, int size, Gender gender,
           int quantity, decimal pricePerHour, decimal totalPrice)
        {
            Id = id;
            InventoryId = inventoryId;
            Size = size;
            Gender = gender;
            Quantity = quantity;
            PricePerHour = pricePerHour;
            TotalPrice = totalPrice;
        }

        public static (ReservationItemModel? ReservationItemModel, string error) Create(
            Guid inventoryId,
            int size,
            Gender gender,
            int quantity,
            decimal pricePerHour,
            DateTime startDate,
            DateTime endDate)
        {
            var error = string.Empty;

            // Валидация
            if (inventoryId == Guid.Empty)
                error = "Не указан инвентарь";
            else if (quantity <= 0)
                error = "Количество должно быть больше 0";
            else if (pricePerHour <= 0)
                error = "Цена должна быть больше 0";
            else if (startDate >= endDate)
                error = "Некорректный период бронирования";

            if (!string.IsNullOrEmpty(error))
                return (null, error);

            // Расчет стоимости
            var hours = (decimal)(endDate - startDate).TotalHours;
            var totalPrice = hours * pricePerHour * quantity;

            var item = new ReservationItemModel(
                Guid.NewGuid(),
                inventoryId,
                size,
                gender,
                quantity,
                pricePerHour,
                totalPrice);

            return (item, error);
        }
    }
}

