using System.ComponentModel.DataAnnotations;

namespace Swiga.API.Contracts
{
    public class CreateReservationRequest()
    {
        [Required(ErrorMessage = "ClientId обязателен")]
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = "RentalPointId обязателен")]
        public Guid RentalPointId { get; set; }

        [Required(ErrorMessage = "StartDate обязателен")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate обязателен")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Items обязательны")]
        [MinLength(1, ErrorMessage = "Должен быть хотя бы один предмет")]
        public List<ReservationItemRequest> Items { get; set; } = new();
    }
}
