

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;


namespace Swiga.Infrastructure.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<InventoryEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryEntity> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Name).IsRequired();
            builder.Property(i => i.Size).IsRequired();
            builder.Property(i => i.PricePerHour).IsRequired();
            builder.Property(i => i.Amount).IsRequired();
            builder.Property(i => i.Gender)
                .IsRequired()
                .HasConversion<string>();

             builder.HasOne(i => i.RentalPoint)
                .WithMany(rp => rp.Inventories) // Убедитесь, что в RentalPointEntity есть это свойство
                .HasForeignKey(i => i.RentalPointId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
