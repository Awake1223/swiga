namespace Swiga.API.Contracts.Reservation
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid RentalPointId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string? CancellationReason { get; set; }
        public List<ReservationItemResponse> Items { get; set; } = new();
    }
}
