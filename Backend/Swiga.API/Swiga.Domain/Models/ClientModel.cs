using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models
{
    public class ClientModel : UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PassportData { get; set; }
        public string? DriverLicense { get; set; }


        // Конструктор ClientModel
        public ClientModel(string firstName, string lastName, string email, string phoneNumber, string password)
            : base(Guid.NewGuid(), email, phoneNumber, password, DateTime.UtcNow, Role.Client) // ← вызов базового конструктора
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static ClientModel Create(string firstName, string lastName, string email, string phoneNumber, string password)
        {
            return new ClientModel(firstName, lastName, email, phoneNumber, password);
        }
    }
}

