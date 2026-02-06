namespace Swiga.API.Contracts.Reservation
{
    public class CreateReservationResponse
    {
        public Guid ReservationId { get; set; }
        public string Message { get; set; } = "Бронирование создано успешно";
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
