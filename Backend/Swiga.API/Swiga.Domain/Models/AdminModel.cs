using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models
{
    public class AdminModel : UserModel
    {
       // public string FullName { get; set; }
        public Guid RentalPointId { get; set; }
        public RentalPointModel RentalPoint { get; set; }

        private AdminModel(string firstName, string lastName, Guid rentalPointId, string email, string phoneNumber, string password)
            : base(Guid.NewGuid(), firstName, lastName, email, phoneNumber, password, DateTime.UtcNow, Role.Admin) 
        {
            RentalPointId = rentalPointId;
        }

        public static AdminModel Create(string firstName, string lastName, Guid rentalPointId, string email, string phoneNumber, string password)
        {
            return new AdminModel(firstName, lastName, rentalPointId, email, phoneNumber, password);
        }

        // НОВЫЙ метод для хешированных паролей
        public static AdminModel CreateWithHashedPassword(string firstName, string lastName,
            Guid rentalPointId, string email, string phoneNumber, string hashedPassword)
        {
            return new AdminModel(
                firstName,
                lastName,
                rentalPointId,
                email,
                phoneNumber,
                hashedPassword);
        }

    }
}
