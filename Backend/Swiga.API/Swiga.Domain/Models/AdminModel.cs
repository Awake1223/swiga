using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Domain.Models
{
    public class AdminModel : UserModel
    {
        public string FullName { get; set; }
        public Guid RentalPointId { get; set; }
        public RentalPointModel RentalPoint { get; set; }

        private AdminModel(string fullName, Guid rentalPointId, string email, string phoneNumber, string password)
                   : base(Guid.NewGuid(), email, phoneNumber, password, DateTime.UtcNow, Role.Admin)  // ← Вызов родителя
        {
            FullName = fullName;
            RentalPointId = rentalPointId;
        }

        public static AdminModel Create(string fullName, Guid rentalPointId, string email, string phoneNumber, string password)
        {
            return new AdminModel(fullName, rentalPointId, email, phoneNumber, password);
        }

    }
}
