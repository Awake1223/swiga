using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Application.DTOs.Reservations
{
    public class CreateReservationDto
    {
        public Guid ClientId { get; set; }
        public Guid RentalPointId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ReservationItemDto> Items { get; set; } = new();
    }
}

