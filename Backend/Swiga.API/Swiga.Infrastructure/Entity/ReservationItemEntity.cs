using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Infrastructure.Entity
{
    public class ReservationItemEntity
    {
        public Guid Id { get; set; }

        // Внешние ключи
        public Guid ReservationId { get; set; }
        public Guid InventoryId { get; set; }

        // Данные позиции
        public int Size { get; set; }
        public int Gender { get; set; } // Enum как int
        public int Quantity { get; set; }
        public decimal PricePerHour { get; set; }
        public decimal TotalPrice { get; set; }

        // Навигационные свойства
        public virtual ReservationEntity Reservation { get; set; }
        public virtual InventoryEntity Inventory { get; set; }
    }
}
