using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models.Auth
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Имя обязательно")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string? PhoneNumber { get; set; }
    }
}
