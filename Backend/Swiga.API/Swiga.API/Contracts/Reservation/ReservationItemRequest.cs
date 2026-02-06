using System.ComponentModel.DataAnnotations;

namespace Swiga.API.Contracts.Reservation
{
    public class ReservationItemRequest()
    {
        [Required(ErrorMessage = "InventoryId обязателен")]
        public Guid InventoryId { get; set; }

        [Required(ErrorMessage = "Quantity обязателен")]
        [Range(1, 10, ErrorMessage = "Количество должно быть от 1 до 10")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "PricePerHour обязателен")]
        [Range(0.01, 10000, ErrorMessage = "Цена должна быть от 0.01 до 10000")]
        public decimal PricePerHour { get; set; }
    }
}
