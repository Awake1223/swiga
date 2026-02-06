namespace Swiga.API.Contracts.Reservation
{
    public class ReservationItemResponse
    {
        public Guid InventoryId { get; set; }
        public string InventoryName { get; set; } = string.Empty;
        public int Size { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerHour { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
