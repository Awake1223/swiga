using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Infrastructure.Repositories
{
    public class AdminRepository
    {
        private readonly SwigaDbContext _context;

        public AdminRepository(SwigaDbContext context)
        {
            _context = context;
        }
    }
}
