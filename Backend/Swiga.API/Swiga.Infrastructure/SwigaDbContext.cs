using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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
        public DbSet<RentalPointEntity> RentalPoints { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<ReservationItemEntity> ReservationItems { get; set; }

        public class SwigaDbContextFactory : IDesignTimeDbContextFactory<SwigaDbContext>
        {
            public SwigaDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<SwigaDbContext>();
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=SwigaDb;Username=postgres;Password=postgres123");

                return new SwigaDbContext(optionsBuilder.Options);
            }
        }
    }
}
