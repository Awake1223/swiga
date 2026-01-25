using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Infrastructure.Entity
{
    public class ReservationEntity
    {
        public Guid Id { get; set; }

        // Внешние ключи
        public Guid ClientId { get; set; }
        public Guid RentalPointId { get; set; }

        // Основные данные
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; } // Enum как int
        public decimal TotalPrice { get; set; }

        // Даты
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }

        // Навигационные свойства
        public virtual UserEntity Client { get; set; } // Клиент
        public virtual RentalPointEntity RentalPoint { get; set; }
        public virtual ICollection<ReservationItemEntity> Items { get; set; } = new List<ReservationItemEntity>();

    }
}
