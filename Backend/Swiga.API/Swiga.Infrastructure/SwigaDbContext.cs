using Microsoft.EntityFrameworkCore;
using Swiga.Domain.Models;

namespace Swiga.Infrastructure
{
    public class SwigaDbContext : DbContext
    {

        public SwigaDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<ClientModel> Clients  { get; set; }
        public DbSet<InventoryModel> Inventories { get; set; }
        public DbSet<RentalPointModel> RentalPoints { get; set; }
        
    
    

    }
}
