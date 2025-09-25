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
    }
}
