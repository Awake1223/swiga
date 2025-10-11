using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swiga.Domain.Models;

namespace Swiga.Infrastructure.Entity
{
    public class InventoryEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Size { get; set; }

        [Required]
        public Gender Gender { get; set; }  // Храним как string

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerHour { get; set; }

        public int Amount { get; set; }

        [ForeignKey("RentalPoint")]
        public Guid RentalPointId { get; set; }
        public RentalPointEntity RentalPoint { get; set; }
    }
}
