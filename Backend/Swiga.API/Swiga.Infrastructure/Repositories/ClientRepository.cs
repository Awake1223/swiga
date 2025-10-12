using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swiga.Infrastructure.Repositories
{
    public class ClientRepository
    {
        private readonly SwigaDbContext _context;

        public ClientRepository(SwigaDbContext context)
        {
            _context = context;
        }
    }
}
