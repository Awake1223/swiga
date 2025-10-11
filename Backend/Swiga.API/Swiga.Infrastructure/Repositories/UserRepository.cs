using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Swiga.Domain.Models;

namespace Swiga.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly SwigaDbContext _context;

        public UserRepository(SwigaDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserModel>> Get()
        {
            
        }

        public async Task<AdminModel> Create()
        {

        }

        public async Task<Guid> Update()
        {

        }

        public async Task<Guid> Delete()
        {

        }

    }
}
