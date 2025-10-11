using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Infrastructure.Entity
{
    [Table("Clients")]
    public class ClientEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [MaxLength(100)]
        public string? PassportData { get; set; }

        [MaxLength(50)]
        public string? DriverLicense { get; set; }
    }
}
