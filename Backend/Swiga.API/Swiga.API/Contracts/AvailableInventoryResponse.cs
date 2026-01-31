namespace Swiga.API.Contracts
{
    public class AvailableInventoryResponse
    {
        public Guid InventoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Size { get; set; }
        public string Gender { get; set; } = string.Empty;
        public decimal PricePerHour { get; set; }
        public int AvailableQuantity { get; set; }
        public int TotalQuantity { get; set; }
    }
}
