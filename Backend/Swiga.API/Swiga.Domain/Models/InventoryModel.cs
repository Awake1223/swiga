using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models
{
    public class InventoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public Gender Gender { get; set; }
        public decimal PricePerHour { get; set; }
        public int Amount { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
