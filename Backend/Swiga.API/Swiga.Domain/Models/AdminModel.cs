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

    }
}
