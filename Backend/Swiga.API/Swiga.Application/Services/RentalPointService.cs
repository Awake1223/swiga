

using Swiga.Domain.Abstructions;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Repositories;

namespace Swiga.Application.Services
{
    public class RentalPointService : IRentalPointService
    {
        private readonly IRentalPointRepository _rentalPoint;

        public RentalPointService(IRentalPointRepository rentalPoint)
        {
            _rentalPoint = rentalPoint;
        }


        public async Task<List<RentalPointModel>> GetRentalPoint()
        {
            return await _rentalPoint.GetRentalPointAsync();
        }

        public async Task<Guid> CreateRentalPoint(RentalPointModel rentalPoint)
        {
            return await _rentalPoint.CreateRentalPointAsync(rentalPoint);
        }

        public async Task<Guid> UpdateRentalPoint(Guid id, string name, string address, string city, string phoneNumber, string email)
        {
            return await _rentalPoint.UpdateRentalPointAsync(id, name, address, city, phoneNumber, email);
        }

        public async Task<Guid> DeleteRentalPoint(Guid id)
        {
            return await _rentalPoint.DeleteRentalPointAsync(id);
        }
    }
}
