

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;


namespace Swiga.Infrastructure.Configurations
{
    public class RentalPointConfiguration : IEntityTypeConfiguration<RentalPointEntity>
    {
        public void Configure (EntityTypeBuilder<RentalPointEntity> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name); 
            builder.Property(r => r.Address);
            builder.Property(r => r.City);
            builder.Property(r => r.PhoneNumber);
            builder.Property(r => r.Email);
        }
    }
}
