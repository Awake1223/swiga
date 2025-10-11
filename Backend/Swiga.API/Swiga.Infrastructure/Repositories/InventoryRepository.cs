
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;

namespace Swiga.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly SwigaDbContext _context;

        public InventoryRepository(SwigaDbContext context)
        {
            _context = context;
        }


        public async Task<List<InventoryModel>> Get()
        {
            var inventoryEntities = await _context.Inventories
                .AsNoTracking()
                .ToListAsync();

            var inventory = inventoryEntities
                .Select(i => InventoryModel.Create(i.Id, i.Name, i.Size, i.Gender, i.PricePerHour, i.Amount).InventoryModel)
                .ToList();

            return inventory;
        }

        public async Task<Guid> Create(InventoryModel inventory)
        {
            var inventoryEntity = new InventoryEntity
            {
                Id = inventory.Id,
                Name = inventory.Name,
                Size = inventory.Size,
                Gender = inventory.Gender,
                PricePerHour = inventory.PricePerHour,
                Amount = inventory.Amount,
            };

            await _context.Inventories.AddAsync(inventoryEntity);
            await _context.SaveChangesAsync();

            return inventoryEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name, int size, Gender gender, decimal pricePerHour, int amount)
        {
            await _context.Inventories
                .Where(i => i.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(i => i.Name, i => name)
                    .SetProperty(i => i.Size, i => size)
                    .SetProperty(i => i.Gender, i => gender)
                    .SetProperty(i => i.PricePerHour, i => pricePerHour)
                    .SetProperty(i => i.Amount, i => amount)
                    );

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Inventories
                .Where(i => i.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

    }
}
