using Microsoft.EntityFrameworkCore;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;

namespace Swiga.Infrastructure
{
    public class SwigaDbContext : DbContext
    {

        public SwigaDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<ClientEntity> Clients  { get; set; }
        public DbSet<InventoryEntity> Inventories { get; set; }
        public DbSet<RentalPointModel> RentalPoints { get; set; }


    }
}
