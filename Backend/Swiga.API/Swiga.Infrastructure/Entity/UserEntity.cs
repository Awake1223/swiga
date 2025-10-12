using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Infrastructure.Entity
{
    public class UserEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int Role { get; set; } // Храним как int для БД

        public string FirstName { get; set; }
        public string LastName { get; set; }  
        public string FullName { get; set; }
        public Guid? RentalPointId { get; set; }
    }
}
