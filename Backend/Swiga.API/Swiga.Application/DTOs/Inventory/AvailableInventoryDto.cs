using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swiga.Domain.Models;

namespace Swiga.Application.DTOs.Inventory
{
    public class AvailableInventoryDto
    {
        public Guid InventoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Size { get; set; }
        public Gender Gender { get; set; }
        public decimal PricePerHour { get; set; }
        public int AvailableQuantity { get; set; }
        public int TotalQuantity { get; set; }
    }
}
