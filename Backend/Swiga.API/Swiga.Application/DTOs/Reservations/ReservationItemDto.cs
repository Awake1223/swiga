using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Application.DTOs.Reservations
{
    public class ReservationItemDto
    {
        public Guid InventoryId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerHour { get; set; }
    }
}
