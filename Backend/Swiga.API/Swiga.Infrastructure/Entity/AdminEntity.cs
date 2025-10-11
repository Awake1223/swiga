using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Swiga.Infrastructure.Entity
{
    [Table("Admins")]
    public class AdminEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [ForeignKey("RentalPoint")]
        public Guid RentalPointId { get; set; }
        public RentalPointEntity RentalPoint { get; set; }
    }
}
