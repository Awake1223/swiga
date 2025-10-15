using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Swiga.Domain.Abstructions;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;

namespace Swiga.Infrastructure.Repositories
{
    public class RentalPointRepository : IRentalPointRepository
    {
        private readonly SwigaDbContext _context;

        public RentalPointRepository(SwigaDbContext context)
        {
            _context = context;
        }

        public async Task<List<RentalPointModel>> GetRentalPointAsync()
        {
            var rentalPointEntities = await _context.RentalPoints
                .AsNoTracking()
                .ToListAsync();

            var rentalPoints = rentalPointEntities
                .Select(r => new RentalPointModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    City = r.City,
                    PhoneNumber = r.PhoneNumber,
                    Email = r.Email
                })
                .ToList();

            return rentalPoints;

        }

        public async Task<Guid> CreateRentalPointAsync(RentalPointModel rentalPoint)
        {

            var rentalPointEntity = new RentalPointEntity
            {
                Id = rentalPoint.Id,
                Name = rentalPoint.Name,
                Address = rentalPoint.Address,
                City = rentalPoint.City,
                PhoneNumber = rentalPoint.PhoneNumber,
                Email = rentalPoint.Email,
            };

            await _context.RentalPoints.AddAsync(rentalPointEntity);
            await _context.SaveChangesAsync();



            return rentalPointEntity.Id;
        }

        public async Task<Guid> UpdateRentalPointAsync(Guid id, string name, string address, string city, string phoneNumber, string email)
        {
            await _context.RentalPoints
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(r => r.Name, r => name)
                    .SetProperty(r => r.Address, r => address)
                    .SetProperty(r => r.City, r => city)
                    .SetProperty(r => r.PhoneNumber, r => phoneNumber)
                    .SetProperty(r => r.Email, r => email));

            return id;

        }


        public async Task<Guid> DeleteRentalPointAsync(Guid id)
        {
            await _context.RentalPoints
                .Where(r => r.Id == id)
                .ExecuteDeleteAsync();

            return id;

        }
    }
}
